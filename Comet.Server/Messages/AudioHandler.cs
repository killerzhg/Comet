using Comet.Common.Messages;
using Comet.Common.Networking;
using Comet.Server.Forms;
using Comet.Server.Networking;
using Concentus.Structs;
using Mono.Cecil.Cil;
using NAudio.Codecs;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Comet.Server.Messages
{
    internal class AudioHandler : MessageProcessorBase<string>, IDisposable
    {
        internal WaveFormat format;
        internal WaveOut WaveOut = new WaveOut();
        internal BufferedWaveProvider BufferedWaveProvider;
        internal WaveFileWriter WaveFileWriter;
        private static WaveInEvent microphone;
        bool isStart = false;

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
        void Execute(ISender client, GetAudioResponse message)
        {
            if (message.Buffer.Length > 0 && message.BytesRecorded > 0 && isStart)
            {
                byte[] decoded;

                if (message.IsSystem)
                {
                    var _decoded = new short[960 * format.Channels]; // 960帧 * 通道数
                    int samplesDecoded = decoder.Decode(message.Buffer, 0, message.Buffer.Length, _decoded, 0, 960, false);
                    int bytesDecoded = samplesDecoded * format.Channels * 2; // 2字节/采样
                    var byteBuffer = new byte[bytesDecoded];
                    Buffer.BlockCopy(_decoded, 0, byteBuffer, 0, bytesDecoded);
                    BufferedWaveProvider?.AddSamples(byteBuffer, 0, bytesDecoded);
                    WaveFileWriter?.Write(byteBuffer, 0, bytesDecoded);
                    WaveFileWriter?.Flush();
                }
                else
                {
                    decoded = DecodeMicro(message.Buffer, 0, message.Buffer.Length);
                    BufferedWaveProvider?.AddSamples(decoded, 0, decoded.Length);
                    WaveFileWriter?.Write(decoded, 0, decoded.Length);
                    WaveFileWriter?.Flush();
                }
            }
        }

        void Execute(ISender client, GetAudioNames message)
        {
            if (message.WaveInDeviceName?.Count > 0)
            {
                OnDisplaysChanged(message);
            }
        }
        OpusDecoder decoder;
        public void StartListen(int index, int isSystem)
        {
            if (isStart==false && format == null)
            {
                isStart = true; 
                _client.Send(new GetAudioResponse
                {
                    IsSystem = isSystem == 1,
                });
                format = isSystem == 1 ? new WaveFormat(48000, 16, 2) : new WaveFormat(16000, 1);
                BufferedWaveProvider = new BufferedWaveProvider(format);
                decoder = new OpusDecoder(format.SampleRate, format.Channels);
                WaveOut.DeviceNumber = index;
                WaveOut.Init(BufferedWaveProvider);
                WaveOut.Play();
            }
        }

        public void Stop() 
        {
            isStart=false;
            _client.Send(new DoAudioStop());
            WaveOut?.Stop();
            this.WaveFileWriter?.Close();
            this.BufferedWaveProvider?.ClearBuffer();
            microphone?.StopRecording();
            format = null;
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
