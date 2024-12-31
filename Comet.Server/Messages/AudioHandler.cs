using Comet.Common.Messages;
using Comet.Common.Networking;
using Comet.Server.Forms;
using Comet.Server.Networking;
using Mono.Cecil.Cil;
using NAudio.Codecs;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using System.ServiceModel.Channels;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Comet.Server.Messages
{
    internal class AudioHandler : MessageProcessorBase<string>, IDisposable
    {
        internal WaveFormat format;
        internal WaveOut WaveOut = new WaveOut();
        internal BufferedWaveProvider BufferedWaveProvider;
        internal WaveFileWriter WaveFileWriter;
        private static WaveInEvent microphone;

        private readonly G722CodecState decoderState = new G722CodecState(64000, G722Flags.None);
        private readonly G722Codec codec = new G722Codec();

        /// <summary>
        /// 在lock语句中用于同步UI线程和线程池之间对<see cref="_codec"/>的访问。
        /// </summary>
        private readonly object _syncLock = new object();
        /// <summary>
        /// 与此启动管理器处理程序相关联的客户端。
        /// </summary>
        public readonly Client _client;

        /// <summary>
        /// States if the client is currently streaming desktop frames.
        /// </summary>
        public bool IsStarted { get; set; }

        /// <summary>
        /// 表示将处理显示更改的回调函数。
        /// </summary>
        /// <param name="sender">The message processor which raised the event.</param>
        /// <param name="value">All currently available displays.</param>
        public delegate void DisplaysChangedEventHandler(object sender, GetAudioNames value);

        public event DisplaysChangedEventHandler DisplaysChanged;

        public AudioHandler(Client client) : base(true)
        {
            _client=client;
        }
        public void RefreshDisplays()
        {
            _client.Send(new GetAudioNames());
        }
        public override bool CanExecute(IMessage message)=> message is GetAudioNames || 
                                                            message is GetAudioResponse ||
                                                            message is SendMicrophoneInit;

        public override bool CanExecuteFrom(ISender sender) => _client.Equals(sender);

        public override void Execute(ISender sender, IMessage message)
        {
            switch (message)
            {
                case GetAudioNames d:
                    Execute(sender, d);
                    break;
                case GetAudioResponse m:
                    Execute(sender, m);
                    break;
                case SendMicrophoneInit m:
                    Execute(sender, m);
                    break;
            }
        }
        void Execute(ISender client, SendMicrophoneInit message)
        {
            microphone?.StopRecording();
            microphone = new WaveInEvent();
            microphone.WaveFormat = new WaveFormat(16000, 1);
            microphone.DeviceNumber = 0;
            microphone.DataAvailable += WaveDataAvailable;
            microphone.RecordingStopped += MicWaveStop;
            microphone.StartRecording();
        }
        void WaveDataAvailable(object sender, WaveInEventArgs e)
        {
            _client.Send(new SendMicrophoneData
            {
                Buffer = e.Buffer,
                BytesRecorded = e.BytesRecorded,
            });
        }
        void MicWaveStop(object sender, StoppedEventArgs e)
        {
            microphone.DataAvailable -= null;
            microphone.RecordingStopped -= null;
            microphone?.Dispose();
        }

        void Execute(ISender client, GetAudioNames message)
        {
            if (message.WaveInDeviceName?.Count > 0)
            {
                OnDisplaysChanged(message);
            }
        }

        public void StartListen(int index, int isSystem)
        {
            _client.Send(new GetAudioResponse
            {
                IsSystem= isSystem==1,
            });
            
            format = isSystem == 1 ? WaveFormat.CreateIeeeFloatWaveFormat(48000, 2) : new WaveFormat(16000, 1);
            BufferedWaveProvider = new BufferedWaveProvider(format);
            WaveOut.DeviceNumber = index;
            WaveOut.Init(BufferedWaveProvider);
            WaveOut.Play();
        }

        public void Stop() 
        {
            _client.Send(new DoAudioStop());
            WaveOut?.Stop();
            this.WaveFileWriter?.Close();
            this.BufferedWaveProvider?.ClearBuffer();
            microphone?.StopRecording();
        }

        void Execute(ISender client, GetAudioResponse message)
        {
            if (message.Buffer.Length > 0 && message.BytesRecorded > 0)
            {
                byte[] decoded;
                
                if (message.IsSystem)
                {
                    decoded = DecodeSys(message.Buffer, 0, message.Buffer.Length);
                }
                else
                {
                    decoded = DecodeMicro(message.Buffer, 0, message.Buffer.Length);
                }
                
                try
                {
                    BufferedWaveProvider?.AddSamples(decoded, 0, message.BytesRecorded);
                    WaveFileWriter?.Write(decoded, 0, decoded.Length);
                    WaveFileWriter?.Flush();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
            
        }
        byte[] DecodeSys(byte[] data, int offset, int length)
        {
            var decoded = new byte[length];
            Array.Copy(data, offset, decoded, 0, length);
            return decoded;
        }
        byte[] DecodeMicro(byte[] data, int offset, int length)
        {
            if (offset != 0)
            {
                throw new ArgumentException("G722 does not yet support non-zero offsets");
            }
            int decodedLength = length * 4;
            var outputBuffer = new byte[decodedLength];
            var wb = new WaveBuffer(outputBuffer);
            int decoded = codec.Decode(decoderState, wb.ShortBuffer, data, length);
            Debug.Assert(decodedLength == decoded * 2);  // because decoded is a number of samples
            return outputBuffer;
        }

        /// <summary>
        /// Reports changed displays.
        /// </summary>
        /// <param name="value">All currently available displays.</param>
        private void OnDisplaysChanged(GetAudioNames value)
        {
            SynchronizationContext.Post(val =>
            {
                DisplaysChangedEventHandler handler = DisplaysChanged;
                handler?.Invoke(this, (GetAudioNames)val);
            }, value);
        }

        public void Dispose()
        {
            Stop();
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                lock (_syncLock)
                {
                    this.WaveFileWriter?.Close();
                    this.BufferedWaveProvider?.ClearBuffer();
                    IsStarted = false;
                }
            }
        }
    }
}
