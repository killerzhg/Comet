using Comet.Server.Controls;

namespace Comet.Server.Forms
{
    partial class FrmRemoteDesktop
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmRemoteDesktop));
            this.btnStart = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.barQuality = new System.Windows.Forms.TrackBar();
            this.lblQuality = new System.Windows.Forms.Label();
            this.lblQualityShow = new System.Windows.Forms.Label();
            this.btnMouse = new System.Windows.Forms.Button();
            this.panelTop = new System.Windows.Forms.Panel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnHide = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.cbMonitors = new System.Windows.Forms.ComboBox();
            this.btnShow = new System.Windows.Forms.Button();
            this.toolTipButtons = new System.Windows.Forms.ToolTip(this.components);
            this.picDesktop = new Comet.Server.Controls.RapidPictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.barQuality)).BeginInit();
            this.panelTop.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picDesktop)).BeginInit();
            this.SuspendLayout();
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(22, 13);
            this.btnStart.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(102, 34);
            this.btnStart.TabIndex = 1;
            this.btnStart.TabStop = false;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnStop
            // 
            this.btnStop.Enabled = false;
            this.btnStop.Location = new System.Drawing.Point(124, 13);
            this.btnStop.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(102, 34);
            this.btnStop.TabIndex = 2;
            this.btnStop.TabStop = false;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // barQuality
            // 
            this.barQuality.Location = new System.Drawing.Point(310, 48);
            this.barQuality.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.barQuality.Maximum = 100;
            this.barQuality.Minimum = 1;
            this.barQuality.Name = "barQuality";
            this.barQuality.Size = new System.Drawing.Size(114, 69);
            this.barQuality.TabIndex = 3;
            this.barQuality.TabStop = false;
            this.barQuality.Value = 75;
            this.barQuality.Scroll += new System.EventHandler(this.barQuality_Scroll);
            // 
            // lblQuality
            // 
            this.lblQuality.AutoSize = true;
            this.lblQuality.Location = new System.Drawing.Point(250, 56);
            this.lblQuality.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblQuality.Name = "lblQuality";
            this.lblQuality.Size = new System.Drawing.Size(68, 23);
            this.lblQuality.TabIndex = 4;
            this.lblQuality.Text = "Quality:";
            // 
            // lblQualityShow
            // 
            this.lblQualityShow.AutoSize = true;
            this.lblQualityShow.Location = new System.Drawing.Point(422, 56);
            this.lblQualityShow.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblQualityShow.Name = "lblQualityShow";
            this.lblQualityShow.Size = new System.Drawing.Size(77, 23);
            this.lblQualityShow.TabIndex = 5;
            this.lblQualityShow.Text = "75 (high)";
            // 
            // btnMouse
            // 
            this.btnMouse.Image = global::Comet.Server.Properties.Resources.mouse_delete;
            this.btnMouse.Location = new System.Drawing.Point(218, 4);
            this.btnMouse.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnMouse.Name = "btnMouse";
            this.btnMouse.Size = new System.Drawing.Size(102, 34);
            this.btnMouse.TabIndex = 6;
            this.btnMouse.TabStop = false;
            this.toolTipButtons.SetToolTip(this.btnMouse, "Enable mouse input.");
            this.btnMouse.UseVisualStyleBackColor = true;
            this.btnMouse.Click += new System.EventHandler(this.btnMouse_Click);
            // 
            // panelTop
            // 
            this.panelTop.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelTop.Controls.Add(this.flowLayoutPanel1);
            this.panelTop.Controls.Add(this.cbMonitors);
            this.panelTop.Controls.Add(this.lblQualityShow);
            this.panelTop.Controls.Add(this.btnStart);
            this.panelTop.Controls.Add(this.btnStop);
            this.panelTop.Controls.Add(this.lblQuality);
            this.panelTop.Controls.Add(this.barQuality);
            this.panelTop.Location = new System.Drawing.Point(284, -2);
            this.panelTop.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(590, 90);
            this.panelTop.TabIndex = 7;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.btnHide);
            this.flowLayoutPanel1.Controls.Add(this.checkBox1);
            this.flowLayoutPanel1.Controls.Add(this.btnMouse);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(245, 8);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(358, 46);
            this.flowLayoutPanel1.TabIndex = 10;
            // 
            // btnHide
            // 
            this.btnHide.Location = new System.Drawing.Point(4, 4);
            this.btnHide.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnHide.Name = "btnHide";
            this.btnHide.Size = new System.Drawing.Size(102, 34);
            this.btnHide.TabIndex = 7;
            this.btnHide.TabStop = false;
            this.btnHide.Text = "Hide";
            this.btnHide.UseVisualStyleBackColor = true;
            this.btnHide.Click += new System.EventHandler(this.btnHide_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Location = new System.Drawing.Point(112, 2);
            this.checkBox1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Padding = new System.Windows.Forms.Padding(15, 8, 4, 4);
            this.checkBox1.Size = new System.Drawing.Size(100, 39);
            this.checkBox1.TabIndex = 8;
            this.checkBox1.Text = "Audio";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // cbMonitors
            // 
            this.cbMonitors.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMonitors.FormattingEnabled = true;
            this.cbMonitors.Location = new System.Drawing.Point(22, 50);
            this.cbMonitors.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbMonitors.Name = "cbMonitors";
            this.cbMonitors.Size = new System.Drawing.Size(204, 31);
            this.cbMonitors.TabIndex = 8;
            this.cbMonitors.TabStop = false;
            // 
            // btnShow
            // 
            this.btnShow.Location = new System.Drawing.Point(882, 0);
            this.btnShow.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnShow.Name = "btnShow";
            this.btnShow.Size = new System.Drawing.Size(82, 28);
            this.btnShow.TabIndex = 8;
            this.btnShow.TabStop = false;
            this.btnShow.Text = "Show";
            this.btnShow.UseVisualStyleBackColor = true;
            this.btnShow.Visible = false;
            this.btnShow.Click += new System.EventHandler(this.btnShow_Click);
            // 
            // picDesktop
            // 
            this.picDesktop.BackColor = System.Drawing.Color.Black;
            this.picDesktop.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picDesktop.Cursor = System.Windows.Forms.Cursors.Default;
            this.picDesktop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picDesktop.GetImageSafe = null;
            this.picDesktop.Location = new System.Drawing.Point(0, 0);
            this.picDesktop.Margin = new System.Windows.Forms.Padding(4);
            this.picDesktop.Name = "picDesktop";
            this.picDesktop.Running = false;
            this.picDesktop.Size = new System.Drawing.Size(1176, 842);
            this.picDesktop.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picDesktop.TabIndex = 0;
            this.picDesktop.TabStop = false;
            this.picDesktop.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picDesktop_MouseDown);
            this.picDesktop.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picDesktop_MouseMove);
            this.picDesktop.MouseUp += new System.Windows.Forms.MouseEventHandler(this.picDesktop_MouseUp);
            // 
            // FrmRemoteDesktop
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(1176, 842);
            this.Controls.Add(this.btnShow);
            this.Controls.Add(this.panelTop);
            this.Controls.Add(this.picDesktop);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MinimumSize = new System.Drawing.Size(949, 688);
            this.Name = "FrmRemoteDesktop";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Remote Desktop []";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmRemoteDesktop_FormClosing);
            this.Load += new System.EventHandler(this.FrmRemoteDesktop_Load);
            this.Resize += new System.EventHandler(this.FrmRemoteDesktop_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.barQuality)).EndInit();
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picDesktop)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.TrackBar barQuality;
        private System.Windows.Forms.Label lblQuality;
        private System.Windows.Forms.Label lblQualityShow;
        private System.Windows.Forms.Button btnMouse;
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Button btnHide;
        private System.Windows.Forms.Button btnShow;
        private System.Windows.Forms.ComboBox cbMonitors;
        private System.Windows.Forms.ToolTip toolTipButtons;
        private Controls.RapidPictureBox picDesktop;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.CheckBox checkBox1;
    }
}