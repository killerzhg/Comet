using Comet.Common.Messages;
using Comet.Common.Video;
using Comet.Server.Helper;
using Comet.Server.Messages;
using Comet.Server.Networking;
using Comet.Server.Utilities;
using Microsoft.Extensions.ObjectPool;
using Mono.Cecil.Cil;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace Comet.Server.Forms
{
    public partial class FrmRemoteWebcam : Form
    {
        /// <summary>
        /// 可用于任务管理器的客户端程序。
        /// </summary>
        private readonly Client _connectClient;
        Dictionary<string, List<Resolution>> _webcams = null;
        private readonly CascadeClassifier cascadeClassifier;
        //Record
        private VideoWriter videoWriter;
        bool record = false;
        bool faceDetection = false;
        Stopwatch stopwatch = new Stopwatch();
        private readonly object queueLock = new object();
        private Task videoWriteTask;
        private CancellationTokenSource cancellationTokenSource;
        // 添加帧率统计和动态调整
        private DateTime recordStartTime;
        private int frameCount = 0;
        private double actualFps = 0;
        private readonly object fpsLock = new object();

        public bool IsStarted { get; private set; }

        /// <summary>
        /// The message handler for handling the communication with the client.
        /// </summary>
        private readonly RemoteWebcamHandlerS _remoteWebcamHandler;

        public FrmRemoteWebcam(Client client)
        {
            _connectClient = client;
            _remoteWebcamHandler = new RemoteWebcamHandlerS(client);
            RegisterMessageHandler();
            cascadeClassifier = new CascadeClassifier("haarcascade_frontalface_default.xml");
            InitializeComponent();
        }

        private void RegisterMessageHandler()
        {
            _connectClient.ClientState += ClientDisconnected;
            _remoteWebcamHandler.DisplaysChanged += DisplaysChanged;
            _remoteWebcamHandler.ProgressChanged += UpdateImageAsync;
            MessageHandler.Register(_remoteWebcamHandler);
        }

        /// <summary>
        /// Unregisters the remote desktop message handler.
        /// </summary>
        private void UnregisterMessageHandler()
        {
            MessageHandler.Unregister(_remoteWebcamHandler);
            _remoteWebcamHandler.DisplaysChanged -= DisplaysChanged;
            _remoteWebcamHandler.ProgressChanged -= UpdateImageAsync;
            _connectClient.ClientState -= ClientDisconnected;
        }

        private void DisplaysChanged(object sender, Dictionary<string, List<Resolution>> webcams)
        {
            cbWebcams.Items.Clear();
           _webcams=webcams;
            foreach (var webcam in webcams.Keys)
            {
                cbWebcams.Items.Add(webcam);
            }
            cbWebcams.SelectedIndex = 0;
        }

        private async void UpdateImageAsync(object sender, Bitmap bmp)
        {
            if (WindowState != FormWindowState.Minimized)
            {
                if (faceDetection)
                {
                    FaceDetection(bmp);
                }
                else
                {
                    picWebcam.UpdateImage(bmp, true);
                }
            }

            if (record)
            {
                int imgWidth = bmp.Size.Width;
                int imgHeight = bmp.Size.Height;

                if (videoWriter == null)
                {
                    InitializeVideoWriter(imgWidth, imgHeight);
                }

                // 异步处理帧，避免阻塞UI
                await ProcessFrameAsync(bmp, imgWidth, imgHeight);
            }
        }



        private void InitializeVideoWriter(int width, int height)
        {
            string mp4_file = Path.Combine(
            Directory.GetCurrentDirectory(),
            @"Record\Webcam",
            DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".mp4");
            // 先用较低的帧率，后续动态调整
            videoWriter = new VideoWriter(mp4_file, FourCC.XVID, 10, new OpenCvSharp.Size(width, height));

            recordStartTime = DateTime.Now;
            frameCount = 0;

            cancellationTokenSource = new CancellationTokenSource();
            videoWriteTask = Task.Run(() => VideoWriteWorker(cancellationTokenSource.Token));
        }

        private async Task ProcessFrameAsync(Bitmap bmp, int imgWidth, int imgHeight)
        {
            await Task.Run(() =>
            {
                try
                {
                    using (var frame = ImageToMat(bmp))
                    {
                        if (frame.Width != imgWidth || frame.Height != imgHeight)
                        {
                            Cv2.Resize(frame, frame, new OpenCvSharp.Size(imgWidth, imgHeight));
                        }

                        // 计算实际帧率
                        lock (fpsLock)
                        {
                            frameCount++;
                            var elapsed = (DateTime.Now - recordStartTime).TotalSeconds;
                            if (elapsed > 0)
                            {
                                actualFps = frameCount / elapsed;
                            }
                        }

                        // 使用更大的队列，并添加时间戳
                        lock (queueLock)
                        {
                            // 增加队列大小，减少丢帧
                            if (frameQueue.Count < 100)
                            {
                                var timestampedFrame = new TimestampedFrame
                                {
                                    Frame = frame.Clone(),
                                    Timestamp = DateTime.Now
                                };
                                frameQueue.Enqueue(timestampedFrame);
                            }
                            else
                            {
                                Log($"Frame dropped, queue full. Actual FPS: {actualFps:F2}");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log($"ProcessFrame error: {ex.Message}");
                }
                finally
                {
                    bmp?.Dispose();
                }
            });
        }

        // 修改队列为带时间戳的帧
        private readonly Queue<TimestampedFrame> frameQueue = new Queue<TimestampedFrame>();

        // 创建带时间戳的帧结构
        private class TimestampedFrame
        {
            public Mat Frame { get; set; }
            public DateTime Timestamp { get; set; }
        }

        private void VideoWriteWorker(CancellationToken cancellationToken)
        {
            DateTime lastFrameTime = DateTime.Now;

            while (!cancellationToken.IsCancellationRequested)
            {
                TimestampedFrame frameToWrite = null;

                lock (queueLock)
                {
                    if (frameQueue.Count > 0)
                    {
                        frameToWrite = frameQueue.Dequeue();
                    }
                }

                if (frameToWrite != null)
                {
                    try
                    {
                        stopwatch.Restart();

                        // 计算帧间隔，确保写入频率正确
                        var timeSinceLastFrame = (frameToWrite.Timestamp - lastFrameTime).TotalMilliseconds;

                        // 如果间隔太短，可能需要跳过一些帧来匹配目标帧率
                        if (timeSinceLastFrame >= 50) // 20fps = 50ms间隔
                        {
                            videoWriter.Write(frameToWrite.Frame);
                            lastFrameTime = frameToWrite.Timestamp;

                            stopwatch.Stop();
                            Log($"Write time: {stopwatch.ElapsedMilliseconds}ms, Queue size: {frameQueue.Count}, Actual FPS: {actualFps:F2}");
                        }
                        else
                        {
                            Log($"Frame skipped, interval too short: {timeSinceLastFrame}ms");
                        }
                    }
                    catch (Exception ex)
                    {
                        Log($"VideoWrite error: {ex.Message}");
                    }
                    finally
                    {
                        frameToWrite.Frame?.Dispose();
                    }
                }
                else
                {
                    Thread.Sleep(10); // 增加休眠时间，减少CPU占用
                }
            }
        }

        // 停止录制时的清理
        private void StopRecording()
        {
            record = false;

            Log($"Recording stopped. Total frames: {frameCount}, Duration: {(DateTime.Now - recordStartTime).TotalSeconds:F2}s, Average FPS: {actualFps:F2}");

            if (cancellationTokenSource != null)
            {
                cancellationTokenSource.Cancel();
                videoWriteTask?.Wait(10000); // 增加等待时间
            }

            // 清空队列时写入剩余帧
            lock (queueLock)
            {
                Log($"Flushing remaining {frameQueue.Count} frames");
                while (frameQueue.Count > 0)
                {
                    var frameToWrite = frameQueue.Dequeue();
                    try
                    {
                        videoWriter?.Write(frameToWrite.Frame);
                    }
                    finally
                    {
                        frameToWrite.Frame?.Dispose();
                    }
                }
            }

            videoWriter?.Release();
            videoWriter = null;

            cancellationTokenSource?.Dispose();
            cancellationTokenSource = null;
        }

        // 进一步优化：使用内存池
        private readonly ObjectPool<Mat> matPool = new DefaultObjectPool<Mat>(new MatPoolPolicy());

        private class MatPoolPolicy : IPooledObjectPolicy<Mat>
        {
            public Mat Create() => new Mat();

            public bool Return(Mat obj)
            {
                if (obj != null && !obj.IsDisposed)
                {
                    obj.SetTo(Scalar.All(0)); // 清空内容
                    return true;
                }
                return false;
            }
        }
        public static void Log(string message)
        {
            try
            {
                using (StreamWriter sw = File.AppendText("1.log"))
                {
                    sw.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}\r\n");
                }
            }
            catch (Exception ex)
            {
                // 处理日志写入失败的情况
                Console.WriteLine($"无法写入日志: {ex.Message}");
            }
        }
        private Mat ImageToMat(System.Drawing.Image img)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                ms.Seek(0, SeekOrigin.Begin);

                return Mat.ImDecode(ms.ToArray(), ImreadModes.Color);
            }
        }

        private void ClientDisconnected(Client client, bool connected)
        {
            if (!connected)
            {
                this.Invoke((MethodInvoker)this.Close);
            }
        }

        private void FrmRemoteWebcam_Load(object sender, EventArgs e)
        {
            this.Text = WindowHelper.GetWindowTitle("Remote Webcam", _connectClient);
            _remoteWebcamHandler.RefreshDisplays();
            _remoteWebcamHandler.LocalResolution = picWebcam.Size;
            OnResize(EventArgs.Empty); // 触发调整大小事件以对齐控件
            if (!Directory.Exists(@"Record\Webcam"))
            {
                Directory.CreateDirectory(@"Record\Webcam");
            }
        }
        // <summary>
        /// Holds the opened Webcam form for each client.
        /// </summary>
        private static readonly Dictionary<Client, FrmRemoteWebcam> OpenedForms = new Dictionary<Client, FrmRemoteWebcam>();
        public static FrmRemoteWebcam CreateNewOrGetExisting(Client client)
        {
            if (OpenedForms.ContainsKey(client))
            {
                return OpenedForms[client];
            }
            FrmRemoteWebcam f = new FrmRemoteWebcam(client);
            f.Disposed += (sender, args) => OpenedForms.Remove(client);
            OpenedForms.Add(client, f);
            return f;
        }
        private void recordCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked)
            {
                record = true; 
            }
            else
            {
                record = false;
                StopRecording();
            }
        }

        private async void FrmRemoteWebcam_FormClosing(object sender, FormClosingEventArgs e)
        {
            record = false;
            // all cleanup logic goes here
            if (_remoteWebcamHandler.IsStarted) StopStream();
            UnregisterMessageHandler();
            _remoteWebcamHandler.Dispose();
            picWebcam.Image?.Dispose();
            _connectClient.Send(new DoWebcamStop());
            StopRecording();
        }

        /// <summary>
        /// Stops the remote Webcam stream.
        /// </summary>
        private void StopStream()
        {
            ToggleConfigurationControls(false);
            picWebcam.Stop();
            // Unsubscribe from the frame counter. It will be re-created when starting again.
            picWebcam.UnsetFrameUpdatedEvent(frameCounter_FrameUpdated);
            this.ActiveControl = picWebcam;
            _remoteWebcamHandler.EndReceiveFrames();
        }

        /// <summary>
        /// Updates the title with the current frames per second.
        /// </summary>
        /// <param name="e">The new frames per second.</param>
        private void frameCounter_FrameUpdated(FrameUpdatedEventArgs e)
        {
            this.Text = string.Format("{0} - FPS: {1}", WindowHelper.GetWindowTitle("Remote Webcam", _connectClient), e.CurrentFramesPerSecond.ToString("0"));//"0"不保留小数点
        }

        /// <summary>
        /// Toggles the activatability of configuration controls in the status/configuration panel.
        /// </summary>
        /// <param name="started">When set to <code>true</code> the configuration controls get enabled, otherwise they get disabled.</param>
        private void ToggleConfigurationControls(bool started)
        {
            btnStart.Enabled = !started;
            btnStop.Enabled = started;
            //barQuality.Enabled = !started;
            cbWebcams.Enabled = !started;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (cbWebcams.Items.Count == 0)
            {
                MessageBox.Show("No webcam detected.\nPlease wait till the client sends a list with available webcams.",
                    "Starting failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            ToggleControls(false);
            picWebcam.Start();
            picWebcam.SetFrameUpdatedEvent(frameCounter_FrameUpdated);
            this.ActiveControl = picWebcam;
            _connectClient.Send(new GetWebcam
            {
                Webcam = cbWebcams.SelectedIndex,
                Resolution = cbResolutions.SelectedIndex
            });
            recordCheckBox.Enabled = true;
            
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            ToggleControls(true);
            this.ActiveControl = picWebcam;
            _connectClient.Send(new DoWebcamStop());
            recordCheckBox.Enabled = false;
            faceDetection = false;
        }

        public void ToggleControls(bool state)
        {
            IsStarted = !state;

            cbWebcams.Enabled = cbResolutions.Enabled = btnStart.Enabled = state;
            btnStop.Enabled = !state;
        }

        private void cbWebcams_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbResolutions.Invoke((MethodInvoker)delegate
            {
                cbResolutions.Items.Clear();
                foreach (var resolution in this._webcams.ElementAt(cbWebcams.SelectedIndex).Value)
                {
                    cbResolutions.Items.Add(resolution.ToString());
                }
                cbResolutions.SelectedIndex = 0;
            });
        }

        private void FrmRemoteWebcam_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
                return;
            panelTop.Left = (this.Width / 2) - (panelTop.Width / 2);

            //btnHide.Left = (panelTop.Width / 2) - (btnHide.Width / 2);

            btnShow.Location = new System.Drawing.Point(377, 0);
            btnShow.Left = (this.Width / 2) - (btnShow.Width / 2);
            //_remoteWebcamHandler.LocalResolution = picWebcam.Size;
        }

        private void btnHide_Click(object sender, EventArgs e)
        {
            HideWin();
        }

        void HideWin() 
        {
            panelTop.Visible = false;
            btnShow.Visible = true;
            btnHide.Visible = false;
            this.ActiveControl = picWebcam;
        }

        private void btnShow_Click(object sender, EventArgs e)
        {
            panelTop.Visible = true;
            btnShow.Visible = false;
            btnHide.Visible = true;
            this.ActiveControl = picWebcam;
        }

        void FaceDetection(Bitmap detectBitmap)
        {
            if (detectBitmap != null)
            {
                using (var detectMat = BitmapConverter.ToMat(detectBitmap))
                {
                    // 检测人脸
                    var rects = cascadeClassifier.DetectMultiScale(
                        detectMat,
                        1.1,
                        5,
                        HaarDetectionTypes.ScaleImage,
                        new OpenCvSharp.Size(30, 30)
                    );
                    // 绘制所有检测到的人脸
                    foreach (var rect in rects)
                    {
                        Cv2.Rectangle(detectMat, rect, Scalar.Red, 2);
                    }
                    picWebcam.UpdateImage(BitmapConverter.ToBitmap(detectMat), true);
                }
            }
        }

        private void faceDetectioncheckBox_CheckedChanged(object sender, EventArgs e)
        {
            faceDetection = ((CheckBox)sender).Checked;
        }
    }
}
