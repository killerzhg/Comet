namespace Comet.Server.Forms
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
            this.btnStop = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnHide = new System.Windows.Forms.Button();
            this.cbWebcams = new System.Windows.Forms.ComboBox();
            this.cbResolutions = new System.Windows.Forms.ComboBox();
            this.panelTop = new System.Windows.Forms.Panel();
            this.recordCheckBox = new System.Windows.Forms.CheckBox();
            this.btnShow = new System.Windows.Forms.Button();
            this.faceDetectioncheckBox = new System.Windows.Forms.CheckBox();
            this.picWebcam = new Comet.Server.Controls.RapidPictureBox();
            this.panelTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picWebcam)).BeginInit();
            this.SuspendLayout();
            // 
            // btnStop
            // 
            this.btnStop.Enabled = false;
            this.btnStop.Location = new System.Drawing.Point(144, 7);
            this.btnStop.Margin = new System.Windows.Forms.Padding(5);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(102, 35);
            this.btnStop.TabIndex = 2;
            this.btnStop.TabStop = false;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(23, 7);
            this.btnStart.Margin = new System.Windows.Forms.Padding(5);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(102, 35);
            this.btnStart.TabIndex = 1;
            this.btnStart.TabStop = false;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnHide
            // 
            this.btnHide.Location = new System.Drawing.Point(254, 83);
            this.btnHide.Margin = new System.Windows.Forms.Padding(5);
            this.btnHide.Name = "btnHide";
            this.btnHide.Size = new System.Drawing.Size(82, 29);
            this.btnHide.TabIndex = 7;
            this.btnHide.TabStop = false;
            this.btnHide.Text = "Hide";
            this.btnHide.UseVisualStyleBackColor = true;
            this.btnHide.Click += new System.EventHandler(this.btnHide_Click);
            // 
            // cbWebcams
            // 
            this.cbWebcams.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbWebcams.FormattingEnabled = true;
            this.cbWebcams.Location = new System.Drawing.Point(23, 46);
            this.cbWebcams.Margin = new System.Windows.Forms.Padding(5);
            this.cbWebcams.Name = "cbWebcams";
            this.cbWebcams.Size = new System.Drawing.Size(221, 26);
            this.cbWebcams.TabIndex = 8;
            this.cbWebcams.TabStop = false;
            this.cbWebcams.SelectedIndexChanged += new System.EventHandler(this.cbWebcams_SelectedIndexChanged);
            // 
            // cbResolutions
            // 
            this.cbResolutions.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbResolutions.FormattingEnabled = true;
            this.cbResolutions.Location = new System.Drawing.Point(23, 85);
            this.cbResolutions.Margin = new System.Windows.Forms.Padding(5);
            this.cbResolutions.Name = "cbResolutions";
            this.cbResolutions.Size = new System.Drawing.Size(221, 26);
            this.cbResolutions.TabIndex = 9;
            this.cbResolutions.TabStop = false;
            // 
            // panelTop
            // 
            this.panelTop.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelTop.Controls.Add(this.faceDetectioncheckBox);
            this.panelTop.Controls.Add(this.recordCheckBox);
            this.panelTop.Controls.Add(this.cbResolutions);
            this.panelTop.Controls.Add(this.cbWebcams);
            this.panelTop.Controls.Add(this.btnHide);
            this.panelTop.Controls.Add(this.btnStart);
            this.panelTop.Controls.Add(this.btnStop);
            this.panelTop.Location = new System.Drawing.Point(397, 0);
            this.panelTop.Margin = new System.Windows.Forms.Padding(5);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(408, 133);
            this.panelTop.TabIndex = 11;
            // 
            // recordCheckBox
            // 
            this.recordCheckBox.AutoSize = true;
            this.recordCheckBox.Enabled = false;
            this.recordCheckBox.Location = new System.Drawing.Point(254, 50);
            this.recordCheckBox.Name = "recordCheckBox";
            this.recordCheckBox.Size = new System.Drawing.Size(88, 22);
            this.recordCheckBox.TabIndex = 10;
            this.recordCheckBox.Text = "Record";
            this.recordCheckBox.UseVisualStyleBackColor = true;
            this.recordCheckBox.CheckedChanged += new System.EventHandler(this.recordCheckBox_CheckedChanged);
            // 
            // btnShow
            // 
            this.btnShow.Location = new System.Drawing.Point(562, 134);
            this.btnShow.Margin = new System.Windows.Forms.Padding(5);
            this.btnShow.Name = "btnShow";
            this.btnShow.Size = new System.Drawing.Size(82, 29);
            this.btnShow.TabIndex = 12;
            this.btnShow.TabStop = false;
            this.btnShow.Text = "Show";
            this.btnShow.UseVisualStyleBackColor = true;
            this.btnShow.Visible = false;
            this.btnShow.Click += new System.EventHandler(this.btnShow_Click);
            // 
            // faceDetectioncheckBox
            // 
            this.faceDetectioncheckBox.AutoSize = true;
            this.faceDetectioncheckBox.Location = new System.Drawing.Point(254, 14);
            this.faceDetectioncheckBox.Name = "faceDetectioncheckBox";
            this.faceDetectioncheckBox.Size = new System.Drawing.Size(151, 22);
            this.faceDetectioncheckBox.TabIndex = 11;
            this.faceDetectioncheckBox.Text = "FaceDetection";
            this.faceDetectioncheckBox.UseVisualStyleBackColor = true;
            this.faceDetectioncheckBox.CheckedChanged += new System.EventHandler(this.faceDetectioncheckBox_CheckedChanged);
            // 
            // picWebcam
            // 
            this.picWebcam.BackColor = System.Drawing.Color.Black;
            this.picWebcam.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picWebcam.GetImageSafe = null;
            this.picWebcam.Location = new System.Drawing.Point(0, 0);
            this.picWebcam.Margin = new System.Windows.Forms.Padding(4);
            this.picWebcam.Name = "picWebcam";
            this.picWebcam.Running = false;
            this.picWebcam.Size = new System.Drawing.Size(1190, 842);
            this.picWebcam.TabIndex = 13;
            this.picWebcam.TabStop = false;
            // 
            // FrmRemoteWebcam
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(1190, 842);
            this.Controls.Add(this.panelTop);
            this.Controls.Add(this.btnShow);
            this.Controls.Add(this.picWebcam);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MinimumSize = new System.Drawing.Size(711, 454);
            this.Name = "FrmRemoteWebcam";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FrmRemoteWebcam";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmRemoteWebcam_FormClosing);
            this.Load += new System.EventHandler(this.FrmRemoteWebcam_Load);
            this.Resize += new System.EventHandler(this.FrmRemoteWebcam_Resize);
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picWebcam)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnHide;
        private System.Windows.Forms.ComboBox cbWebcams;
        private System.Windows.Forms.ComboBox cbResolutions;
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Button btnShow;
        private Controls.RapidPictureBox picWebcam;
        private System.Windows.Forms.CheckBox recordCheckBox;
        private System.Windows.Forms.CheckBox faceDetectioncheckBox;
    }
}