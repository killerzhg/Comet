using Mono.Cecil.Cil;
using Quasar.Common.Messages;
using Quasar.Common.Networking;
using Quasar.Common.Video.Codecs;
using Quasar.Server.Networking;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quasar.Server.Messages
{
    internal class RemoteWebcamHandler : MessageProcessorBase<Bitmap>, IDisposable
    {
        public RemoteWebcamHandler(Client client) : base(true)
        {
            _client = client;
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
                _client.Send(new GetDesktop { CreateNew = true, Quality = quality, DisplayIndex = display });
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
        /// The video stream codec used to decode received frames.
        /// </summary>
        private UnsafeStreamCodec _codec;

        /// <summary>
        /// Represents the method that will handle display changes.
        /// </summary>
        /// <param name="sender">The message processor which raised the event.</param>
        /// <param name="value">All currently available displays.</param>
        public delegate void DisplaysChangedEventHandler(object sender, int value);

        /// <summary>
        /// Raised when a display changed.
        /// </summary>
        /// <remarks>
        /// Handlers registered with this event will be invoked on the 
        /// <see cref="System.Threading.SynchronizationContext"/> chosen when the instance was constructed.
        /// </remarks>
        public event DisplaysChangedEventHandler DisplaysChanged;

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

        public override bool CanExecute(IMessage message)
        {
            throw new NotImplementedException();
        }

        public override bool CanExecuteFrom(ISender sender)
        {
            throw new NotImplementedException();
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

        public override void Execute(ISender sender, IMessage message)
        {
            throw new NotImplementedException();
        }
    }
}
