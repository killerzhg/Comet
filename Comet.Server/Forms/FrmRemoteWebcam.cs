using Comet.Common.Messages;
using Comet.Common.Video;
using Comet.Server.Helper;
using Comet.Server.Messages;
using Comet.Server.Networking;
using Comet.Server.Utilities;
using Mono.Cecil.Cil;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlTypes;
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
        private SemaphoreSlim fileLock;
        private bool isProcessing = false;
        bool record = false;
        bool faceDetection = false;

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
                picWebcam.Image = new Bitmap(bmp, picWebcam.Width, picWebcam.Height);
            }
            if (record)
            {
                int imgWidth = bmp.Size.Width;
                int imgHeight = bmp.Size.Height;
                if (videoWriter == null)
                {
                    string mp4_file = Path.Combine(new string[]
                    {
                            Directory.GetCurrentDirectory(),
                            @"Record\Webcam",
                            DateTime.Now.ToString("yyyyMMdd_HHmmss")+".mp4",
                    });

                    videoWriter = new VideoWriter(mp4_file, FourCC.MP4V, 10, new OpenCvSharp.Size(imgWidth, imgHeight));
                    fileLock = new SemaphoreSlim(1, 1);
                }

                recordTask = Task.Run(async () =>
                {
                    await fileLock.WaitAsync();
                    try
                    {
                        System.Drawing.Image imgCopy = null;
                        try
                        {
                            lock (bmp)
                            {
                                imgCopy = new Bitmap(bmp);
                            }

                            try
                            {
                                if (imgCopy == null)
                                    return;

                                using (var frame = ImageToMat(imgCopy))
                                {
                                    if (frame.Width != imgWidth || frame.Height != imgHeight)
                                        Cv2.Resize(frame, frame, new OpenCvSharp.Size(imgWidth, imgHeight));

                                    Cv2.PutText(
                                        frame,
                                        "*",
                                        new OpenCvSharp.Point(30, 30),
                                        HersheyFonts.HersheySimplex,
                                        1.0,
                                        Scalar.White,
                                        2,
                                        LineTypes.AntiAlias
                                    );

                                    if (videoWriter.IsOpened())
                                    {
                                        Invoke(new Action(() =>
                                        {
                                            videoWriter.Write(frame);
                                        }));
                                    }
                                    else
                                    {

                                    }
                                }
                            }
                            catch (Exception ex)
                            {

                            }
                            finally
                            {
                                if (imgCopy != null)
                                    imgCopy.Dispose();
                            }
                        }
                        catch (InvalidOperationException ex)
                        {

                        }
                    }
                    catch
                    {

                    }
                    finally
                    {
                        fileLock.Release();
                    }
                    
                });
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
        private async void recordCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            var checkBox = sender as CheckBox;
            if (checkBox != null)
            {
                if (checkBox.Checked)
                {
                    // 选中时的逻辑
                    record = true;
                }
                else
                {
                    // 未选中时的逻辑
                    record = false;
                    if (videoWriter != null)
                    {
                        if (videoWriter.IsOpened())
                        {
                            videoWriter.Release();
                            if (videoWriter != null)
                            {
                                //等待录制任务完成
                                if (recordTask != null)
                                {
                                    try
                                    {
                                        await recordTask;
                                        videoWriter = null;
                                    }
                                    catch { }
                                }
                            }
                        }
                    }
                }
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

            if (recordCheckBox.Checked && videoWriter != null)
            {
                if (videoWriter.IsOpened())
                {
                    videoWriter.Release();
                    if (videoWriter != null)
                    {
                        //等待录制任务完成
                        if (recordTask != null)
                        {
                            try
                            {
                                await recordTask;
                                videoWriter = null;
                            }
                            catch { }
                        }
                    }
                }
            }
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
            this.Text = string.Format("{0} - FPS: {1}", WindowHelper.GetWindowTitle("Remote Webcam", _connectClient), e.CurrentFramesPerSecond.ToString("0.00"));
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

                    // 将检测结果显示到pictureBox1
                    picWebcam.Image = BitmapConverter.ToBitmap(detectMat);
                }
            }
        }

        private void faceDetectioncheckBox_CheckedChanged(object sender, EventArgs e)
        {
            faceDetection = ((CheckBox)sender).Checked;
        }
    }
}
