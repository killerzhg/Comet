namespace Quasar.Server.Forms
{
    partial class FrmRemoteWebcam
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmRemoteWebcam));
            this.cbResolutions = new System.Windows.Forms.ComboBox();
            this.panelTop = new System.Windows.Forms.Panel();
            this.cbWebcams = new System.Windows.Forms.ComboBox();
            this.btnHide = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnShow = new System.Windows.Forms.Button();
            this.picWebcam = new Quasar.Server.Controls.RapidPictureBox();
            this.panelTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picWebcam)).BeginInit();
            this.SuspendLayout();
            // 
            // cbResolutions
            // 
            this.cbResolutions.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbResolutions.FormattingEnabled = true;
            this.cbResolutions.Location = new System.Drawing.Point(19, 71);
            this.cbResolutions.Margin = new System.Windows.Forms.Padding(4);
            this.cbResolutions.Name = "cbResolutions";
            this.cbResolutions.Size = new System.Drawing.Size(185, 23);
            this.cbResolutions.TabIndex = 9;
            this.cbResolutions.TabStop = false;
            // 
            // panelTop
            // 
            this.panelTop.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelTop.Controls.Add(this.cbResolutions);
            this.panelTop.Controls.Add(this.cbWebcams);
            this.panelTop.Controls.Add(this.btnHide);
            this.panelTop.Controls.Add(this.btnStart);
            this.panelTop.Controls.Add(this.btnStop);
            this.panelTop.Location = new System.Drawing.Point(302, 0);
            this.panelTop.Margin = new System.Windows.Forms.Padding(4);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(226, 136);
            this.panelTop.TabIndex = 11;
            // 
            // cbWebcams
            // 
            this.cbWebcams.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbWebcams.FormattingEnabled = true;
            this.cbWebcams.Location = new System.Drawing.Point(19, 38);
            this.cbWebcams.Margin = new System.Windows.Forms.Padding(4);
            this.cbWebcams.Name = "cbWebcams";
            this.cbWebcams.Size = new System.Drawing.Size(185, 23);
            this.cbWebcams.TabIndex = 8;
            this.cbWebcams.TabStop = false;
            // 
            // btnHide
            // 
            this.btnHide.Location = new System.Drawing.Point(71, 105);
            this.btnHide.Margin = new System.Windows.Forms.Padding(4);
            this.btnHide.Name = "btnHide";
            this.btnHide.Size = new System.Drawing.Size(68, 24);
            this.btnHide.TabIndex = 7;
            this.btnHide.TabStop = false;
            this.btnHide.Text = "Hide";
            this.btnHide.UseVisualStyleBackColor = true;
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(19, 6);
            this.btnStart.Margin = new System.Windows.Forms.Padding(4);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(85, 29);
            this.btnStart.TabIndex = 1;
            this.btnStart.TabStop = false;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnStop
            // 
            this.btnStop.Enabled = false;
            this.btnStop.Location = new System.Drawing.Point(120, 6);
            this.btnStop.Margin = new System.Windows.Forms.Padding(4);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(85, 29);
            this.btnStop.TabIndex = 2;
            this.btnStop.TabStop = false;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            // 
            // btnShow
            // 
            this.btnShow.Location = new System.Drawing.Point(375, 144);
            this.btnShow.Margin = new System.Windows.Forms.Padding(4);
            this.btnShow.Name = "btnShow";
            this.btnShow.Size = new System.Drawing.Size(68, 24);
            this.btnShow.TabIndex = 12;
            this.btnShow.TabStop = false;
            this.btnShow.Text = "Show";
            this.btnShow.UseVisualStyleBackColor = true;
            this.btnShow.Visible = false;
            // 
            // picWebcam
            // 
            this.picWebcam.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picWebcam.GetImageSafe = null;
            this.picWebcam.Location = new System.Drawing.Point(0, 0);
            this.picWebcam.Name = "picWebcam";
            this.picWebcam.Running = false;
            this.picWebcam.Size = new System.Drawing.Size(800, 450);
            this.picWebcam.TabIndex = 0;
            this.picWebcam.TabStop = false;
            // 
            // FrmRemoteWebcam
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.panelTop);
            this.Controls.Add(this.btnShow);
            this.Controls.Add(this.picWebcam);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmRemoteWebcam";
            this.Text = "FrmRemoteWebcam";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmRemoteWebcam_FormClosing);
            this.Load += new System.EventHandler(this.FrmRemoteWebcam_Load);
            this.panelTop.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picWebcam)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.RapidPictureBox picWebcam;
        private System.Windows.Forms.ComboBox cbResolutions;
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.ComboBox cbWebcams;
        private System.Windows.Forms.Button btnHide;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnShow;
    }
}