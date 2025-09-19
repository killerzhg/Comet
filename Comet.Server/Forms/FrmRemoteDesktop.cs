using Comet.Common.Enums;
using Comet.Common.Helpers;
using Comet.Common.Messages;
using Comet.Server.Helper;
using Comet.Server.Messages;
using Comet.Server.Networking;
using Comet.Server.Utilities;
using Open.Nat;
using RFTEST.Function;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Comet.Server.Controls; // 新增：使用 RapidPictureBox
using System.Runtime.InteropServices;

namespace Comet.Server.Forms
{
    public partial class FrmRemoteDesktop : Form
    {

        static bool win7 = false;
        private readonly AudioHandler _audioHandler;
        /// <summary>
        /// States whether remote mouse input is enabled.
        /// </summary>
        private bool _enableMouseInput;

        /// <summary>
        /// States whether remote keyboard input is enabled.
        /// </summary>
        private bool _enableKeyboardInput;

        /// <summary>
        /// A list of pressed keys for synchronization between key down & -up events.
        /// </summary>
        private readonly List<Keys> _keysPressed;

        /// <summary>
        /// The client which can be used for the remote desktop.
        /// </summary>
        private readonly Client _connectClient;

        /// <summary>
        /// The message handler for handling the communication with the client.
        /// </summary>
        private readonly RemoteDesktopHandler _remoteDesktopHandler;

        /// <summary>
        /// Holds the opened remote desktop form for each client.
        /// </summary>
        private static readonly Dictionary<Client, FrmRemoteDesktop> OpenedForms = new Dictionary<Client, FrmRemoteDesktop>();

        /// <summary>
        /// Creates a new remote desktop form for the client or gets the current open form, if there exists one already.
        /// </summary>
        /// <param name="client">The client used for the remote desktop form.</param>
        /// <returns>
        /// Returns a new remote desktop form for the client if there is none currently open, otherwise creates a new one.
        /// </returns>
        public static FrmRemoteDesktop CreateNewOrGetExisting(Client client,bool isWin7)
        {
            win7 = isWin7;
            if (OpenedForms.ContainsKey(client))
            {
                return OpenedForms[client];
            }
            FrmRemoteDesktop r = new FrmRemoteDesktop(client);
            r.Disposed += (sender, args) => OpenedForms.Remove(client);
            OpenedForms.Add(client, r);
            var useAudio = Configs.GetConfig("UseAudio");
            if (!string.IsNullOrEmpty(useAudio))
            {
                r.checkBox1.Checked = bool.Parse(useAudio); 
            }
            return r;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FrmRemoteDesktop"/> class using the given client.
        /// </summary>
        /// <param name="client">The client used for the remote desktop form.</param>
        public FrmRemoteDesktop(Client client)
        {
            _connectClient = client;
            _remoteDesktopHandler = new RemoteDesktopHandler(client);
            _keysPressed = new List<Keys>();
            _audioHandler = new AudioHandler(client);
            MessageHandler.Register(_audioHandler);
            RegisterMessageHandler();
            InitializeComponent();
            picDesktop.MouseWheel += OnMouseWheelMove;
        }

        /// <summary>
        /// Called whenever a client disconnects.
        /// </summary>
        /// <param name="client">The client which disconnected.</param>
        /// <param name="connected">True if the client connected, false if disconnected</param>
        private void ClientDisconnected(Client client, bool connected)
        {
            if (!connected)
            {
                this.Invoke((MethodInvoker)this.Close);
            }
        }

        /// <summary>
        /// Registers the remote desktop message handler for client communication.
        /// </summary>
        private void RegisterMessageHandler()
        {
            _connectClient.ClientState += ClientDisconnected;
            _remoteDesktopHandler.DisplaysChanged += DisplaysChanged;
            _remoteDesktopHandler.ProgressChanged += UpdateImage;
            MessageHandler.Register(_remoteDesktopHandler);
        }

        /// <summary>
        /// Unregisters the remote desktop message handler.
        /// </summary>
        private void UnregisterMessageHandler()
        {
            MessageHandler.Unregister(_remoteDesktopHandler);
            _remoteDesktopHandler.DisplaysChanged -= DisplaysChanged;
            _remoteDesktopHandler.ProgressChanged -= UpdateImage;
            _connectClient.ClientState -= ClientDisconnected;
        }

        /// <summary>
        /// Subscribes to local mouse and keyboard events for remote desktop input.
        /// </summary>
        private void SubscribeEvents()
        {
            // 如需窗体级按键先处理
            this.KeyPreview = true;
            InstallKeyboardHook();
        }

        /// <summary>
        /// Unsubscribes from local mouse and keyboard events.
        /// </summary>
        private void UnsubscribeEvents()
        {
            UninstallKeyboardHook();
        }

        // =================== Win32 全局键盘钩子实现 ===================

        // 常量与结构
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_KEYUP = 0x0101;
        private const int WM_SYSKEYDOWN = 0x0104;
        private const int WM_SYSKEYUP = 0x0105;

        [StructLayout(LayoutKind.Sequential)]
        private struct KBDLLHOOKSTRUCT
        {
            public uint vkCode;
            public uint scanCode;
            public uint flags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        // P/Invoke
        private delegate IntPtr HookProc(int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        private IntPtr _kbHook = IntPtr.Zero;
        private HookProc _kbProc; // 防止被GC

        private void InstallKeyboardHook()
        {
            if (_kbHook != IntPtr.Zero) return;
            _kbProc = KeyboardHookCallback;

            using (var curProcess = System.Diagnostics.Process.GetCurrentProcess())
            using (var curModule = curProcess.MainModule)
            {
                IntPtr hMod = GetModuleHandle(curModule.ModuleName);
                _kbHook = SetWindowsHookEx(WH_KEYBOARD_LL, _kbProc, hMod, 0);
            }
        }

        private void UninstallKeyboardHook()
        {
            if (_kbHook == IntPtr.Zero) return;
            UnhookWindowsHookEx(_kbHook);
            _kbHook = IntPtr.Zero;
            _kbProc = null;
        }

        private IntPtr KeyboardHookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && lParam != IntPtr.Zero)
            {
                var info = (KBDLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(KBDLLHOOKSTRUCT));
                var key = (Keys)info.vkCode;

                switch ((int)wParam)
                {
                    case WM_KEYDOWN:
                    case WM_SYSKEYDOWN:
                        if (ProcessKeyDownFromHook(key))
                            return (IntPtr)1; // 拦截本地
                        break;

                    case WM_KEYUP:
                    case WM_SYSKEYUP:
                        if (ProcessKeyUpFromHook(key))
                            return (IntPtr)1; // 拦截本地
                        break;
                }
            }
            return CallNextHookEx(_kbHook, nCode, wParam, lParam);
        }

        // 将现有 OnKeyDown/OnKeyUp 逻辑抽取为可供钩子回调复用的方法
        private bool ProcessKeyDownFromHook(Keys key)
        {
            // 不满足条件则放行
            if (picDesktop.Image == null || !_enableKeyboardInput || !this.ContainsFocus || !picDesktop.Focused)
                return false;

            //if (IsLockKey(key))
            //    return false;

            //if (_keysPressed.Contains(key))
                //return true; 

            _keysPressed.Add(key);

            _remoteDesktopHandler.SendKeyboardEvent((byte)key, true);
            return true; // 非锁定键拦截本地
        }

        private bool ProcessKeyUpFromHook(Keys key)
        {
            if (picDesktop.Image == null || !_enableKeyboardInput || !this.ContainsFocus || !picDesktop.Focused)
                return false;

            //if (IsLockKey(key))
            //    return false;

            _keysPressed.Remove(key);
            _remoteDesktopHandler.SendKeyboardEvent((byte)key, false);
            return true;
        }

        /// <summary>
        /// Starts the remote desktop stream and begin to receive desktop frames.
        /// </summary>
        private void StartStream()
        {
            ToggleConfigurationControls(true);

            ////// 仅启动时设置一次本地期望分辨率（而不是在 Resize 里反复设置）
            ////if (_remoteDesktopHandler.LocalResolution == Size.Empty)
            ////    _remoteDesktopHandler.LocalResolution = picDesktop.Size;

            picDesktop.Start();
            // Subscribe to the new frame counter.
            picDesktop.SetFrameUpdatedEvent(frameCounter_FrameUpdated);

            this.ActiveControl = picDesktop;

            _remoteDesktopHandler.BeginReceiveFrames(barQuality.Value, cbMonitors.SelectedIndex);
        }

        /// <summary>
        /// Stops the remote desktop stream.
        /// </summary>
        private void StopStream()
        {
            ToggleConfigurationControls(false);

            picDesktop.Stop();
            // Unsubscribe from the frame counter. It will be re-created when starting again.
            picDesktop.UnsetFrameUpdatedEvent(frameCounter_FrameUpdated);

            this.ActiveControl = picDesktop;

            _remoteDesktopHandler.EndReceiveFrames();
        }

        /// <summary>
        /// Toggles the activatability of configuration controls in the status/configuration panel.
        /// </summary>
        /// <param name="started">When set to <code>true</code> the configuration controls get enabled, otherwise they get disabled.</param>
        private void ToggleConfigurationControls(bool started)
        {
            btnStart.Enabled = !started;
            btnStop.Enabled = started;
            barQuality.Enabled = !started;
            cbMonitors.Enabled = !started;
        }

        /// <summary>
        /// Toggles the visibility of the status/configuration panel.
        /// </summary>
        /// <param name="visible">Decides if the panel should be visible.</param>
        private void TogglePanelVisibility(bool visible)
        {
            panelTop.Visible = visible;
            btnShow.Visible = !visible;
            this.ActiveControl = picDesktop;
        }

        /// <summary>
        /// Called whenever the remote displays changed.
        /// </summary>
        /// <param name="sender">The message handler which raised the event.</param>
        /// <param name="displays">The currently available displays.</param>
        private void DisplaysChanged(object sender, int displays)
        {
            cbMonitors.Items.Clear();
            for (int i = 0; i < displays; i++)
                cbMonitors.Items.Add($"Display {i + 1}");
            cbMonitors.SelectedIndex = 0;
            StartScreen();
            TogglePanelVisibility(false);
        }

        /// <summary>
        /// Updates the current desktop image by drawing it to the desktop picturebox.
        /// </summary>
        /// <param name="sender">The message handler which raised the event.</param>
        /// <param name="bmp">The new desktop image to draw.</param>
        private void UpdateImage(object sender, Bitmap bmp)
        {
            picDesktop.UpdateImage(bmp, false);
        }

        private void FrmRemoteDesktop_Load(object sender, EventArgs e)
        {
            this.Text = WindowHelper.GetWindowTitle("Remote Desktop", _connectClient);

            OnResize(EventArgs.Empty); // 触发调整大小事件以对齐控件

            _remoteDesktopHandler.RefreshDisplays();
        }

        /// <summary>
        /// Updates the title with the current frames per second.
        /// </summary>
        /// <param name="e">The new frames per second.</param>
        private void frameCounter_FrameUpdated(FrameUpdatedEventArgs e)
        {
            this.Text = string.Format("{0} - FPS: {1}", WindowHelper.GetWindowTitle("Remote Desktop", _connectClient), e.CurrentFramesPerSecond.ToString("0.00"));
        }

        private void FrmRemoteDesktop_FormClosing(object sender, FormClosingEventArgs e)
        {
            // all cleanup logic goes here
            UnsubscribeEvents();
            if (_remoteDesktopHandler.IsStarted) StopStream();
            UnregisterMessageHandler();
            _remoteDesktopHandler.Dispose();
            picDesktop.Image?.Dispose();
            _audioHandler?.Stop();
        }

        private void FrmRemoteDesktop_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
                return;

            //_remoteDesktopHandler.LocalResolution = picDesktop.Size;
            panelTop.Left = (this.Width - panelTop.Width) / 2;
            btnShow.Left = (this.Width - btnShow.Width) / 2;
           // btnHide.Left = (panelTop.Width - btnHide.Width) / 2;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            StartScreen();
        }

        void StartScreen() 
        {
            if (cbMonitors.Items.Count == 0)
            {
                MessageBox.Show("No remote display detected.\nPlease wait till the client sends a list with available displays.",
                    "Starting failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            SubscribeEvents();
            StartStream();
            if (checkBox1.Checked)
            {
                _audioHandler?.StartListen(0, true, win7);
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            UnsubscribeEvents();
            StopStream();
        }

        #region Remote Desktop Input

        private void picDesktop_MouseDown(object sender, MouseEventArgs e)
        {
            if (picDesktop.Image != null && _enableMouseInput && this.ContainsFocus)
            {
                MouseAction action = MouseAction.None;

                if (e.Button == MouseButtons.Left)
                    action = MouseAction.LeftDown;
                if (e.Button == MouseButtons.Right)
                    action = MouseAction.RightDown;

                int selectedDisplayIndex = cbMonitors.SelectedIndex;

                // 将控件坐标转换为图像像素坐标（与 message.Resolution 对齐）
                Point imgPt = (picDesktop as RapidPictureBox)?.TranslateToImage(e.Location) ?? e.Location;

                _remoteDesktopHandler.SendMouseEvent(action, true, imgPt.X, imgPt.Y, selectedDisplayIndex);
            }
        }

        private void picDesktop_MouseUp(object sender, MouseEventArgs e)
        {
            if (picDesktop.Image != null && _enableMouseInput && this.ContainsFocus)
            {
                MouseAction action = MouseAction.None;

                if (e.Button == MouseButtons.Left)
                    action = MouseAction.LeftUp;
                if (e.Button == MouseButtons.Right)
                    action = MouseAction.RightUp;

                int selectedDisplayIndex = cbMonitors.SelectedIndex;

                Point imgPt = (picDesktop as RapidPictureBox)?.TranslateToImage(e.Location) ?? e.Location;

                _remoteDesktopHandler.SendMouseEvent(action, false, imgPt.X, imgPt.Y, selectedDisplayIndex);
            }
        }

        private void picDesktop_MouseMove(object sender, MouseEventArgs e)
        {
            if (picDesktop.Image != null && _enableMouseInput && this.ContainsFocus)
            {
                int selectedDisplayIndex = cbMonitors.SelectedIndex;

                Point imgPt = (picDesktop as RapidPictureBox)?.TranslateToImage(e.Location) ?? e.Location;

                _remoteDesktopHandler.SendMouseEvent(MouseAction.MoveCursor, false, imgPt.X, imgPt.Y, selectedDisplayIndex);
            }
        }

        private void OnMouseWheelMove(object sender, MouseEventArgs e)
        {
            if (picDesktop.Image != null && _enableMouseInput && this.ContainsFocus)
            {
                int deltaUnit = SystemInformation.MouseWheelScrollDelta; // 通常是120
                int steps = Math.Max(1, Math.Abs(e.Delta) / Math.Max(1, deltaUnit));

                var action = e.Delta > 0 ? MouseAction.ScrollUp : MouseAction.ScrollDown;

                for (int i = 0; i < steps; i++)
                {
                    _remoteDesktopHandler.SendMouseEvent(action, false, 0, 0, cbMonitors.SelectedIndex);
                }
            }
        }

        private bool IsLockKey(Keys key)
        {
            return ((key & Keys.CapsLock) == Keys.CapsLock)
                   || ((key & Keys.NumLock) == Keys.NumLock)
                   || ((key & Keys.Scroll) == Keys.Scroll);
        }

        #endregion

        #region Remote Desktop Configuration

        private void barQuality_Scroll(object sender, EventArgs e)
        {
            int value = barQuality.Value;
            lblQualityShow.Text = value.ToString();

            if (value < 25)
                lblQualityShow.Text += " (low)";
            else if (value >= 85)
                lblQualityShow.Text += " (best)";
            else if (value >= 75)
                lblQualityShow.Text += " (high)";
            else if (value >= 25)
                lblQualityShow.Text += " (mid)";

            this.ActiveControl = picDesktop;
        }

        private void btnMouse_Click(object sender, EventArgs e)
        {
            if (_enableMouseInput)
            {
                //this.picDesktop.Cursor = Cursors.Default;
                btnMouse.Image = Properties.Resources.mouse_delete;
                toolTipButtons.SetToolTip(btnMouse, "Enable mouse input.");
                _enableMouseInput = false;
                _enableKeyboardInput = false;
            }
            else
            {
                //this.picDesktop.Cursor = Cursors.Hand;
                btnMouse.Image = Properties.Resources.mouse_add;
                toolTipButtons.SetToolTip(btnMouse, "Disable mouse input.");
                _enableMouseInput = true;
                _enableKeyboardInput = true;
            }
            TogglePanelVisibility(false);
            this.ActiveControl = picDesktop;
        }

        #endregion

        private void btnHide_Click(object sender, EventArgs e)
        {
            TogglePanelVisibility(false);
        }

        private void btnShow_Click(object sender, EventArgs e)
        {
            TogglePanelVisibility(true);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Configs.SetConfig("UseAudio", checkBox1.Checked.ToString());
            if (checkBox1.Checked)
            {
                _audioHandler?.StartListen(0, true,win7);
            }
            else
            {
                _audioHandler?.Stop();
            }
        }

        private void picDesktop_MouseEnter(object sender, EventArgs e)
        {
            if (_enableMouseInput)
                _remoteDesktopHandler.SendMouseEvent(MouseAction.Enter, false, 0, 0, cbMonitors.SelectedIndex);
        }

        private void picDesktop_MouseLeave(object sender, EventArgs e)
        {
            if (_enableMouseInput)
                _remoteDesktopHandler.SendMouseEvent(MouseAction.Leave, false, 0, 0, cbMonitors.SelectedIndex);
        }
    }
}
