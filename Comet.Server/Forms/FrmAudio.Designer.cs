namespace Comet.Server.Forms
{
    partial class FrmAudio
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmAudio));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.OutDevice = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.waveInDeviceName = new System.Windows.Forms.ComboBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.labRuntime = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.sendataLen = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.recvdataLen = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tip = new System.Windows.Forms.Label();
            this.startListen = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.OutDevice);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.waveInDeviceName);
            this.groupBox1.Controls.Add(this.checkBox2);
            this.groupBox1.Controls.Add(this.labRuntime);
            this.groupBox1.Controls.Add(this.progressBar1);
            this.groupBox1.Controls.Add(this.sendataLen);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.checkBox1);
            this.groupBox1.Controls.Add(this.recvdataLen);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.tip);
            this.groupBox1.Location = new System.Drawing.Point(13, 13);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(630, 252);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            // 
            // OutDevice
            // 
            this.OutDevice.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.OutDevice.FormattingEnabled = true;
            this.OutDevice.Location = new System.Drawing.Point(150, 85);
            this.OutDevice.Margin = new System.Windows.Forms.Padding(4);
            this.OutDevice.Name = "OutDevice";
            this.OutDevice.Size = new System.Drawing.Size(461, 23);
            this.OutDevice.TabIndex = 20;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.Color.Red;
            this.label6.Location = new System.Drawing.Point(23, 88);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(87, 15);
            this.label6.TabIndex = 19;
            this.label6.Text = "Listen on:";
            // 
            // waveInDeviceName
            // 
            this.waveInDeviceName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.waveInDeviceName.FormattingEnabled = true;
            this.waveInDeviceName.Location = new System.Drawing.Point(150, 37);
            this.waveInDeviceName.Margin = new System.Windows.Forms.Padding(4);
            this.waveInDeviceName.Name = "waveInDeviceName";
            this.waveInDeviceName.Size = new System.Drawing.Size(461, 23);
            this.waveInDeviceName.TabIndex = 18;
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Enabled = false;
            this.checkBox2.Location = new System.Drawing.Point(150, 205);
            this.checkBox2.Margin = new System.Windows.Forms.Padding(4);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(119, 19);
            this.checkBox2.TabIndex = 11;
            this.checkBox2.Text = "录制远程声音";
            this.checkBox2.UseVisualStyleBackColor = true;
            this.checkBox2.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
            // 
            // labRuntime
            // 
            this.labRuntime.AutoSize = true;
            this.labRuntime.Location = new System.Drawing.Point(463, 165);
            this.labRuntime.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labRuntime.Name = "labRuntime";
            this.labRuntime.Size = new System.Drawing.Size(148, 15);
            this.labRuntime.TabIndex = 10;
            this.labRuntime.Text = "已运行:00.00.00.00";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(150, 131);
            this.progressBar1.Margin = new System.Windows.Forms.Padding(4);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(461, 11);
            this.progressBar1.TabIndex = 7;
            // 
            // sendataLen
            // 
            this.sendataLen.AutoSize = true;
            this.sendataLen.Location = new System.Drawing.Point(326, 165);
            this.sendataLen.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.sendataLen.Name = "sendataLen";
            this.sendataLen.Size = new System.Drawing.Size(39, 15);
            this.sendataLen.TabIndex = 6;
            this.sendataLen.Text = "0 KB";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(258, 165);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 15);
            this.label3.TabIndex = 5;
            this.label3.Text = "已发送:";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Enabled = false;
            this.checkBox1.Location = new System.Drawing.Point(522, 205);
            this.checkBox1.Margin = new System.Windows.Forms.Padding(4);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(89, 19);
            this.checkBox1.TabIndex = 3;
            this.checkBox1.Text = "语音通话";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // recvdataLen
            // 
            this.recvdataLen.AutoSize = true;
            this.recvdataLen.Location = new System.Drawing.Point(211, 165);
            this.recvdataLen.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.recvdataLen.Name = "recvdataLen";
            this.recvdataLen.Size = new System.Drawing.Size(39, 15);
            this.recvdataLen.TabIndex = 2;
            this.recvdataLen.Text = "0 KB";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(147, 165);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 15);
            this.label2.TabIndex = 1;
            this.label2.Text = "已接收:";
            // 
            // tip
            // 
            this.tip.AutoSize = true;
            this.tip.ForeColor = System.Drawing.Color.Red;
            this.tip.Location = new System.Drawing.Point(23, 40);
            this.tip.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.tip.Name = "tip";
            this.tip.Size = new System.Drawing.Size(119, 15);
            this.tip.TabIndex = 0;
            this.tip.Text = "Remote Device:";
            // 
            // startListen
            // 
            this.startListen.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.startListen.Location = new System.Drawing.Point(470, 286);
            this.startListen.Name = "startListen";
            this.startListen.Size = new System.Drawing.Size(173, 48);
            this.startListen.TabIndex = 2;
            this.startListen.Text = "Start listening";
            this.startListen.UseVisualStyleBackColor = true;
            this.startListen.Click += new System.EventHandler(this.startListen_Click);
            // 
            // FrmAudio
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(656, 352);
            this.Controls.Add(this.startListen);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FrmAudio";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Remote Audio";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmAudio_FormClosing);
            this.Load += new System.EventHandler(this.FrmAudio_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.Label labRuntime;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label sendataLen;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Label recvdataLen;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label tip;
        private System.Windows.Forms.ComboBox waveInDeviceName;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox OutDevice;
        private System.Windows.Forms.Button startListen;
    }
}