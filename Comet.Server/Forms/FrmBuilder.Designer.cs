using Comet.Server.Controls;

namespace Comet.Server.Forms
{
    partial class FrmBuilder
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmBuilder));
            this.btnBuild = new System.Windows.Forms.Button();
            this.tooltip = new System.Windows.Forms.ToolTip(this.components);
            this.picUAC2 = new System.Windows.Forms.PictureBox();
            this.picUAC1 = new System.Windows.Forms.PictureBox();
            this.rbSystem = new System.Windows.Forms.RadioButton();
            this.rbProgramFiles = new System.Windows.Forms.RadioButton();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.removeHostToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.builderTabs = new Comet.Server.Controls.DotNetBarTabControl();
            this.generalPage = new System.Windows.Forms.TabPage();
            this.label3 = new System.Windows.Forms.Label();
            this.chkUnattendedMode = new System.Windows.Forms.CheckBox();
            this.line2 = new Comet.Server.Controls.Line();
            this.label2 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.line6 = new Comet.Server.Controls.Line();
            this.label8 = new System.Windows.Forms.Label();
            this.txtTag = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.lblTag = new System.Windows.Forms.Label();
            this.txtMutex = new System.Windows.Forms.TextBox();
            this.btnMutex = new System.Windows.Forms.Button();
            this.line5 = new Comet.Server.Controls.Line();
            this.lblMutex = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.connectionPage = new System.Windows.Forms.TabPage();
            this.numericUpDownPort = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownDelay = new System.Windows.Forms.NumericUpDown();
            this.line3 = new Comet.Server.Controls.Line();
            this.label4 = new System.Windows.Forms.Label();
            this.line1 = new Comet.Server.Controls.Line();
            this.label1 = new System.Windows.Forms.Label();
            this.lstHosts = new System.Windows.Forms.ListBox();
            this.btnAddHost = new System.Windows.Forms.Button();
            this.lblMS = new System.Windows.Forms.Label();
            this.lblHost = new System.Windows.Forms.Label();
            this.txtHost = new System.Windows.Forms.TextBox();
            this.lblDelay = new System.Windows.Forms.Label();
            this.lblPort = new System.Windows.Forms.Label();
            this.installationPage = new System.Windows.Forms.TabPage();
            this.chkHideSubDirectory = new System.Windows.Forms.CheckBox();
            this.line7 = new Comet.Server.Controls.Line();
            this.label10 = new System.Windows.Forms.Label();
            this.line4 = new Comet.Server.Controls.Line();
            this.label5 = new System.Windows.Forms.Label();
            this.chkInstall = new System.Windows.Forms.CheckBox();
            this.lblInstallName = new System.Windows.Forms.Label();
            this.txtInstallName = new System.Windows.Forms.TextBox();
            this.txtRegistryKeyName = new System.Windows.Forms.TextBox();
            this.lblExtension = new System.Windows.Forms.Label();
            this.lblRegistryKeyName = new System.Windows.Forms.Label();
            this.chkStartup = new System.Windows.Forms.CheckBox();
            this.rbAppdata = new System.Windows.Forms.RadioButton();
            this.chkHide = new System.Windows.Forms.CheckBox();
            this.lblInstallDirectory = new System.Windows.Forms.Label();
            this.lblInstallSubDirectory = new System.Windows.Forms.Label();
            this.lblPreviewPath = new System.Windows.Forms.Label();
            this.txtInstallSubDirectory = new System.Windows.Forms.TextBox();
            this.txtPreviewPath = new System.Windows.Forms.TextBox();
            this.assemblyPage = new System.Windows.Forms.TabPage();
            this.iconPreview = new System.Windows.Forms.PictureBox();
            this.btnBrowseIcon = new System.Windows.Forms.Button();
            this.txtIconPath = new System.Windows.Forms.TextBox();
            this.line8 = new Comet.Server.Controls.Line();
            this.label11 = new System.Windows.Forms.Label();
            this.chkChangeAsmInfo = new System.Windows.Forms.CheckBox();
            this.txtFileVersion = new System.Windows.Forms.TextBox();
            this.line9 = new Comet.Server.Controls.Line();
            this.lblProductName = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.chkChangeIcon = new System.Windows.Forms.CheckBox();
            this.lblFileVersion = new System.Windows.Forms.Label();
            this.txtProductName = new System.Windows.Forms.TextBox();
            this.txtProductVersion = new System.Windows.Forms.TextBox();
            this.lblDescription = new System.Windows.Forms.Label();
            this.lblProductVersion = new System.Windows.Forms.Label();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.txtOriginalFilename = new System.Windows.Forms.TextBox();
            this.lblCompanyName = new System.Windows.Forms.Label();
            this.lblOriginalFilename = new System.Windows.Forms.Label();
            this.txtCompanyName = new System.Windows.Forms.TextBox();
            this.txtTrademarks = new System.Windows.Forms.TextBox();
            this.lblCopyright = new System.Windows.Forms.Label();
            this.lblTrademarks = new System.Windows.Forms.Label();
            this.txtCopyright = new System.Windows.Forms.TextBox();
            this.monitoringTab = new System.Windows.Forms.TabPage();
            this.chkHideLogDirectory = new System.Windows.Forms.CheckBox();
            this.txtLogDirectoryName = new System.Windows.Forms.TextBox();
            this.lblLogDirectory = new System.Windows.Forms.Label();
            this.line10 = new Comet.Server.Controls.Line();
            this.label14 = new System.Windows.Forms.Label();
            this.chkKeylogger = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.picUAC2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picUAC1)).BeginInit();
            this.contextMenuStrip.SuspendLayout();
            this.builderTabs.SuspendLayout();
            this.generalPage.SuspendLayout();
            this.connectionPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPort)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDelay)).BeginInit();
            this.installationPage.SuspendLayout();
            this.assemblyPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iconPreview)).BeginInit();
            this.monitoringTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnBuild
            // 
            this.btnBuild.Location = new System.Drawing.Point(804, 780);
            this.btnBuild.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.btnBuild.Name = "btnBuild";
            this.btnBuild.Size = new System.Drawing.Size(242, 46);
            this.btnBuild.TabIndex = 1;
            this.btnBuild.Text = "Build Client";
            this.btnBuild.UseVisualStyleBackColor = true;
            this.btnBuild.Click += new System.EventHandler(this.btnBuild_Click);
            // 
            // picUAC2
            // 
            this.picUAC2.Image = global::Comet.Server.Properties.Resources.uac_shield;
            this.picUAC2.Location = new System.Drawing.Point(726, 176);
            this.picUAC2.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.picUAC2.Name = "picUAC2";
            this.picUAC2.Size = new System.Drawing.Size(16, 20);
            this.picUAC2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picUAC2.TabIndex = 32;
            this.picUAC2.TabStop = false;
            this.tooltip.SetToolTip(this.picUAC2, "Administrator Privileges are required to install the client in System.");
            // 
            // picUAC1
            // 
            this.picUAC1.Image = global::Comet.Server.Properties.Resources.uac_shield;
            this.picUAC1.Location = new System.Drawing.Point(726, 136);
            this.picUAC1.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.picUAC1.Name = "picUAC1";
            this.picUAC1.Size = new System.Drawing.Size(16, 20);
            this.picUAC1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picUAC1.TabIndex = 31;
            this.picUAC1.TabStop = false;
            this.tooltip.SetToolTip(this.picUAC1, "Administrator Privileges are required to install the client in Program Files.");
            // 
            // rbSystem
            // 
            this.rbSystem.AutoSize = true;
            this.rbSystem.Location = new System.Drawing.Point(482, 182);
            this.rbSystem.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.rbSystem.Name = "rbSystem";
            this.rbSystem.Size = new System.Drawing.Size(113, 34);
            this.rbSystem.TabIndex = 5;
            this.rbSystem.TabStop = true;
            this.rbSystem.Text = "System";
            this.tooltip.SetToolTip(this.rbSystem, "Administrator Privileges are required to install the client in System.");
            this.rbSystem.UseVisualStyleBackColor = true;
            this.rbSystem.CheckedChanged += new System.EventHandler(this.HasChangedSettingAndFilePath);
            // 
            // rbProgramFiles
            // 
            this.rbProgramFiles.AutoSize = true;
            this.rbProgramFiles.Location = new System.Drawing.Point(482, 136);
            this.rbProgramFiles.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.rbProgramFiles.Name = "rbProgramFiles";
            this.rbProgramFiles.Size = new System.Drawing.Size(176, 34);
            this.rbProgramFiles.TabIndex = 4;
            this.rbProgramFiles.TabStop = true;
            this.rbProgramFiles.Text = "Program Files";
            this.tooltip.SetToolTip(this.rbProgramFiles, "Administrator Privileges are required to install the client in Program Files.");
            this.rbProgramFiles.UseVisualStyleBackColor = true;
            this.rbProgramFiles.CheckedChanged += new System.EventHandler(this.HasChangedSettingAndFilePath);
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.removeHostToolStripMenuItem,
            this.clearToolStripMenuItem});
            this.contextMenuStrip.Name = "ctxtMenuHosts";
            this.contextMenuStrip.Size = new System.Drawing.Size(256, 84);
            // 
            // removeHostToolStripMenuItem
            // 
            this.removeHostToolStripMenuItem.Image = global::Comet.Server.Properties.Resources.delete;
            this.removeHostToolStripMenuItem.Name = "removeHostToolStripMenuItem";
            this.removeHostToolStripMenuItem.Size = new System.Drawing.Size(255, 40);
            this.removeHostToolStripMenuItem.Text = "Remove host";
            this.removeHostToolStripMenuItem.Click += new System.EventHandler(this.removeHostToolStripMenuItem_Click);
            // 
            // clearToolStripMenuItem
            // 
            this.clearToolStripMenuItem.Image = global::Comet.Server.Properties.Resources.broom;
            this.clearToolStripMenuItem.Name = "clearToolStripMenuItem";
            this.clearToolStripMenuItem.Size = new System.Drawing.Size(255, 40);
            this.clearToolStripMenuItem.Text = "Clear all";
            this.clearToolStripMenuItem.Click += new System.EventHandler(this.clearToolStripMenuItem_Click);
            // 
            // builderTabs
            // 
            this.builderTabs.Alignment = System.Windows.Forms.TabAlignment.Left;
            this.builderTabs.Controls.Add(this.generalPage);
            this.builderTabs.Controls.Add(this.connectionPage);
            this.builderTabs.Controls.Add(this.installationPage);
            this.builderTabs.Controls.Add(this.assemblyPage);
            this.builderTabs.Controls.Add(this.monitoringTab);
            this.builderTabs.Dock = System.Windows.Forms.DockStyle.Top;
            this.builderTabs.ItemSize = new System.Drawing.Size(60, 136);
            this.builderTabs.Location = new System.Drawing.Point(0, 0);
            this.builderTabs.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.builderTabs.Multiline = true;
            this.builderTabs.Name = "builderTabs";
            this.builderTabs.SelectedIndex = 0;
            this.builderTabs.Size = new System.Drawing.Size(1070, 768);
            this.builderTabs.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.builderTabs.TabIndex = 0;
            // 
            // generalPage
            // 
            this.generalPage.BackColor = System.Drawing.SystemColors.Control;
            this.generalPage.Controls.Add(this.label3);
            this.generalPage.Controls.Add(this.chkUnattendedMode);
            this.generalPage.Controls.Add(this.line2);
            this.generalPage.Controls.Add(this.label2);
            this.generalPage.Controls.Add(this.label9);
            this.generalPage.Controls.Add(this.line6);
            this.generalPage.Controls.Add(this.label8);
            this.generalPage.Controls.Add(this.txtTag);
            this.generalPage.Controls.Add(this.label7);
            this.generalPage.Controls.Add(this.lblTag);
            this.generalPage.Controls.Add(this.txtMutex);
            this.generalPage.Controls.Add(this.btnMutex);
            this.generalPage.Controls.Add(this.line5);
            this.generalPage.Controls.Add(this.lblMutex);
            this.generalPage.Controls.Add(this.label6);
            this.generalPage.Location = new System.Drawing.Point(140, 4);
            this.generalPage.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.generalPage.Name = "generalPage";
            this.generalPage.Padding = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.generalPage.Size = new System.Drawing.Size(926, 760);
            this.generalPage.TabIndex = 4;
            this.generalPage.Text = "Basic Settings";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(34, 428);
            this.label3.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(661, 60);
            this.label3.TabIndex = 24;
            this.label3.Text = "Activating the unattended mode allows remote control of the client\r\nwithout user " +
    "interaction.";
            // 
            // chkUnattendedMode
            // 
            this.chkUnattendedMode.AutoSize = true;
            this.chkUnattendedMode.Location = new System.Drawing.Point(40, 504);
            this.chkUnattendedMode.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.chkUnattendedMode.Name = "chkUnattendedMode";
            this.chkUnattendedMode.Size = new System.Drawing.Size(289, 34);
            this.chkUnattendedMode.TabIndex = 23;
            this.chkUnattendedMode.Text = "Enable unattended mode";
            this.chkUnattendedMode.UseVisualStyleBackColor = true;
            this.chkUnattendedMode.CheckedChanged += new System.EventHandler(this.HasChangedSetting);
            // 
            // line2
            // 
            this.line2.LineAlignment = Comet.Server.Controls.Line.Alignment.Horizontal;
            this.line2.Location = new System.Drawing.Point(230, 392);
            this.line2.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.line2.Name = "line2";
            this.line2.Size = new System.Drawing.Size(540, 26);
            this.line2.TabIndex = 22;
            this.line2.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 392);
            this.label2.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(190, 30);
            this.label2.TabIndex = 21;
            this.label2.Text = "Unattended mode";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(34, 188);
            this.label9.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(682, 60);
            this.label9.TabIndex = 5;
            this.label9.Text = "A unique mutex ensures that only one instance of the client is running\r\non the sa" +
    "me system.";
            // 
            // line6
            // 
            this.line6.LineAlignment = Comet.Server.Controls.Line.Alignment.Horizontal;
            this.line6.Location = new System.Drawing.Point(170, 156);
            this.line6.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.line6.Name = "line6";
            this.line6.Size = new System.Drawing.Size(600, 26);
            this.line6.TabIndex = 20;
            this.line6.TabStop = false;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(12, 156);
            this.label8.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(153, 30);
            this.label8.TabIndex = 4;
            this.label8.Text = "Process Mutex";
            // 
            // txtTag
            // 
            this.txtTag.Location = new System.Drawing.Point(260, 80);
            this.txtTag.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.txtTag.Name = "txtTag";
            this.txtTag.Size = new System.Drawing.Size(506, 37);
            this.txtTag.TabIndex = 3;
            this.txtTag.TextChanged += new System.EventHandler(this.HasChangedSetting);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(34, 40);
            this.label7.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(432, 30);
            this.label7.TabIndex = 1;
            this.label7.Text = "You can choose a tag to identify your client.";
            // 
            // lblTag
            // 
            this.lblTag.AutoSize = true;
            this.lblTag.Location = new System.Drawing.Point(34, 86);
            this.lblTag.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblTag.Name = "lblTag";
            this.lblTag.Size = new System.Drawing.Size(113, 30);
            this.lblTag.TabIndex = 2;
            this.lblTag.Text = "Client Tag:";
            // 
            // txtMutex
            // 
            this.txtMutex.Location = new System.Drawing.Point(260, 260);
            this.txtMutex.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.txtMutex.MaxLength = 64;
            this.txtMutex.Name = "txtMutex";
            this.txtMutex.Size = new System.Drawing.Size(502, 37);
            this.txtMutex.TabIndex = 7;
            this.txtMutex.TextChanged += new System.EventHandler(this.HasChangedSetting);
            // 
            // btnMutex
            // 
            this.btnMutex.Location = new System.Drawing.Point(524, 316);
            this.btnMutex.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.btnMutex.Name = "btnMutex";
            this.btnMutex.Size = new System.Drawing.Size(242, 46);
            this.btnMutex.TabIndex = 8;
            this.btnMutex.Text = "Random Mutex";
            this.btnMutex.UseVisualStyleBackColor = true;
            this.btnMutex.Click += new System.EventHandler(this.btnMutex_Click);
            // 
            // line5
            // 
            this.line5.LineAlignment = Comet.Server.Controls.Line.Alignment.Horizontal;
            this.line5.Location = new System.Drawing.Point(224, 10);
            this.line5.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.line5.Name = "line5";
            this.line5.Size = new System.Drawing.Size(542, 26);
            this.line5.TabIndex = 15;
            this.line5.TabStop = false;
            // 
            // lblMutex
            // 
            this.lblMutex.AutoSize = true;
            this.lblMutex.Location = new System.Drawing.Point(34, 266);
            this.lblMutex.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblMutex.Name = "lblMutex";
            this.lblMutex.Size = new System.Drawing.Size(79, 30);
            this.lblMutex.TabIndex = 6;
            this.lblMutex.Text = "Mutex:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 10);
            this.label6.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(199, 30);
            this.label6.TabIndex = 0;
            this.label6.Text = "Client Identification";
            // 
            // connectionPage
            // 
            this.connectionPage.BackColor = System.Drawing.SystemColors.Control;
            this.connectionPage.Controls.Add(this.numericUpDownPort);
            this.connectionPage.Controls.Add(this.numericUpDownDelay);
            this.connectionPage.Controls.Add(this.line3);
            this.connectionPage.Controls.Add(this.label4);
            this.connectionPage.Controls.Add(this.line1);
            this.connectionPage.Controls.Add(this.label1);
            this.connectionPage.Controls.Add(this.lstHosts);
            this.connectionPage.Controls.Add(this.btnAddHost);
            this.connectionPage.Controls.Add(this.lblMS);
            this.connectionPage.Controls.Add(this.lblHost);
            this.connectionPage.Controls.Add(this.txtHost);
            this.connectionPage.Controls.Add(this.lblDelay);
            this.connectionPage.Controls.Add(this.lblPort);
            this.connectionPage.Location = new System.Drawing.Point(140, 4);
            this.connectionPage.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.connectionPage.Name = "connectionPage";
            this.connectionPage.Padding = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.connectionPage.Size = new System.Drawing.Size(926, 760);
            this.connectionPage.TabIndex = 0;
            this.connectionPage.Text = "Connection Settings";
            // 
            // numericUpDownPort
            // 
            this.numericUpDownPort.Location = new System.Drawing.Point(508, 102);
            this.numericUpDownPort.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.numericUpDownPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numericUpDownPort.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownPort.Name = "numericUpDownPort";
            this.numericUpDownPort.Size = new System.Drawing.Size(258, 37);
            this.numericUpDownPort.TabIndex = 3;
            this.numericUpDownPort.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // numericUpDownDelay
            // 
            this.numericUpDownDelay.Location = new System.Drawing.Point(552, 356);
            this.numericUpDownDelay.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.numericUpDownDelay.Maximum = new decimal(new int[] {
            600000,
            0,
            0,
            0});
            this.numericUpDownDelay.Name = "numericUpDownDelay";
            this.numericUpDownDelay.Size = new System.Drawing.Size(160, 37);
            this.numericUpDownDelay.TabIndex = 10;
            this.numericUpDownDelay.Value = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.numericUpDownDelay.ValueChanged += new System.EventHandler(this.HasChangedSetting);
            // 
            // line3
            // 
            this.line3.LineAlignment = Comet.Server.Controls.Line.Alignment.Horizontal;
            this.line3.Location = new System.Drawing.Point(190, 318);
            this.line3.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.line3.Name = "line3";
            this.line3.Size = new System.Drawing.Size(580, 26);
            this.line3.TabIndex = 18;
            this.line3.TabStop = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 318);
            this.label4.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(173, 30);
            this.label4.TabIndex = 17;
            this.label4.Text = "Reconnect Delay";
            // 
            // line1
            // 
            this.line1.LineAlignment = Comet.Server.Controls.Line.Alignment.Horizontal;
            this.line1.Location = new System.Drawing.Point(208, 10);
            this.line1.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.line1.Name = "line1";
            this.line1.Size = new System.Drawing.Size(562, 26);
            this.line1.TabIndex = 13;
            this.line1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 10);
            this.label1.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(183, 30);
            this.label1.TabIndex = 14;
            this.label1.Text = "Connection Hosts";
            // 
            // lstHosts
            // 
            this.lstHosts.ContextMenuStrip = this.contextMenuStrip;
            this.lstHosts.FormattingEnabled = true;
            this.lstHosts.ItemHeight = 30;
            this.lstHosts.Location = new System.Drawing.Point(40, 42);
            this.lstHosts.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.lstHosts.Name = "lstHosts";
            this.lstHosts.Size = new System.Drawing.Size(294, 214);
            this.lstHosts.TabIndex = 5;
            this.lstHosts.TabStop = false;
            // 
            // btnAddHost
            // 
            this.btnAddHost.Location = new System.Drawing.Point(508, 156);
            this.btnAddHost.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.btnAddHost.Name = "btnAddHost";
            this.btnAddHost.Size = new System.Drawing.Size(258, 44);
            this.btnAddHost.TabIndex = 4;
            this.btnAddHost.Text = "Add Host";
            this.btnAddHost.UseVisualStyleBackColor = true;
            this.btnAddHost.Click += new System.EventHandler(this.btnAddHost_Click);
            // 
            // lblMS
            // 
            this.lblMS.AutoSize = true;
            this.lblMS.Location = new System.Drawing.Point(712, 366);
            this.lblMS.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblMS.Name = "lblMS";
            this.lblMS.Size = new System.Drawing.Size(41, 30);
            this.lblMS.TabIndex = 11;
            this.lblMS.Text = "ms";
            // 
            // lblHost
            // 
            this.lblHost.AutoSize = true;
            this.lblHost.Location = new System.Drawing.Point(350, 50);
            this.lblHost.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblHost.Name = "lblHost";
            this.lblHost.Size = new System.Drawing.Size(144, 30);
            this.lblHost.TabIndex = 0;
            this.lblHost.Text = "IP/Hostname:";
            // 
            // txtHost
            // 
            this.txtHost.Location = new System.Drawing.Point(508, 44);
            this.txtHost.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.txtHost.Name = "txtHost";
            this.txtHost.Size = new System.Drawing.Size(254, 37);
            this.txtHost.TabIndex = 1;
            // 
            // lblDelay
            // 
            this.lblDelay.AutoSize = true;
            this.lblDelay.Location = new System.Drawing.Point(34, 364);
            this.lblDelay.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblDelay.Name = "lblDelay";
            this.lblDelay.Size = new System.Drawing.Size(376, 30);
            this.lblDelay.TabIndex = 9;
            this.lblDelay.Text = "Time to wait between reconnect tries:";
            // 
            // lblPort
            // 
            this.lblPort.AutoSize = true;
            this.lblPort.Location = new System.Drawing.Point(350, 106);
            this.lblPort.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new System.Drawing.Size(57, 30);
            this.lblPort.TabIndex = 2;
            this.lblPort.Text = "Port:";
            // 
            // installationPage
            // 
            this.installationPage.BackColor = System.Drawing.SystemColors.Control;
            this.installationPage.Controls.Add(this.chkHideSubDirectory);
            this.installationPage.Controls.Add(this.line7);
            this.installationPage.Controls.Add(this.label10);
            this.installationPage.Controls.Add(this.line4);
            this.installationPage.Controls.Add(this.label5);
            this.installationPage.Controls.Add(this.picUAC2);
            this.installationPage.Controls.Add(this.picUAC1);
            this.installationPage.Controls.Add(this.chkInstall);
            this.installationPage.Controls.Add(this.rbSystem);
            this.installationPage.Controls.Add(this.lblInstallName);
            this.installationPage.Controls.Add(this.rbProgramFiles);
            this.installationPage.Controls.Add(this.txtInstallName);
            this.installationPage.Controls.Add(this.txtRegistryKeyName);
            this.installationPage.Controls.Add(this.lblExtension);
            this.installationPage.Controls.Add(this.lblRegistryKeyName);
            this.installationPage.Controls.Add(this.chkStartup);
            this.installationPage.Controls.Add(this.rbAppdata);
            this.installationPage.Controls.Add(this.chkHide);
            this.installationPage.Controls.Add(this.lblInstallDirectory);
            this.installationPage.Controls.Add(this.lblInstallSubDirectory);
            this.installationPage.Controls.Add(this.lblPreviewPath);
            this.installationPage.Controls.Add(this.txtInstallSubDirectory);
            this.installationPage.Controls.Add(this.txtPreviewPath);
            this.installationPage.Location = new System.Drawing.Point(140, 4);
            this.installationPage.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.installationPage.Name = "installationPage";
            this.installationPage.Padding = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.installationPage.Size = new System.Drawing.Size(926, 760);
            this.installationPage.TabIndex = 1;
            this.installationPage.Text = "Installation Settings";
            // 
            // chkHideSubDirectory
            // 
            this.chkHideSubDirectory.AutoSize = true;
            this.chkHideSubDirectory.Location = new System.Drawing.Point(372, 370);
            this.chkHideSubDirectory.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.chkHideSubDirectory.Name = "chkHideSubDirectory";
            this.chkHideSubDirectory.Size = new System.Drawing.Size(338, 34);
            this.chkHideSubDirectory.TabIndex = 37;
            this.chkHideSubDirectory.Text = "Set subdir attributes to hidden";
            this.chkHideSubDirectory.UseVisualStyleBackColor = true;
            // 
            // line7
            // 
            this.line7.LineAlignment = Comet.Server.Controls.Line.Alignment.Horizontal;
            this.line7.Location = new System.Drawing.Point(120, 548);
            this.line7.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.line7.Name = "line7";
            this.line7.Size = new System.Drawing.Size(646, 26);
            this.line7.TabIndex = 36;
            this.line7.TabStop = false;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(14, 548);
            this.label10.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(101, 30);
            this.label10.TabIndex = 14;
            this.label10.Text = "Autostart";
            // 
            // line4
            // 
            this.line4.LineAlignment = Comet.Server.Controls.Line.Alignment.Horizontal;
            this.line4.Location = new System.Drawing.Point(234, 10);
            this.line4.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.line4.Name = "line4";
            this.line4.Size = new System.Drawing.Size(532, 26);
            this.line4.TabIndex = 34;
            this.line4.TabStop = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 10);
            this.label5.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(203, 30);
            this.label5.TabIndex = 0;
            this.label5.Text = "Installation Location";
            // 
            // chkInstall
            // 
            this.chkInstall.AutoSize = true;
            this.chkInstall.Location = new System.Drawing.Point(40, 42);
            this.chkInstall.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.chkInstall.Name = "chkInstall";
            this.chkInstall.Size = new System.Drawing.Size(161, 34);
            this.chkInstall.TabIndex = 1;
            this.chkInstall.Text = "Install Client";
            this.chkInstall.UseVisualStyleBackColor = true;
            this.chkInstall.CheckedChanged += new System.EventHandler(this.chkInstall_CheckedChanged);
            // 
            // lblInstallName
            // 
            this.lblInstallName.AutoSize = true;
            this.lblInstallName.Location = new System.Drawing.Point(34, 312);
            this.lblInstallName.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblInstallName.Name = "lblInstallName";
            this.lblInstallName.Size = new System.Drawing.Size(137, 30);
            this.lblInstallName.TabIndex = 8;
            this.lblInstallName.Text = "Install Name:";
            // 
            // txtInstallName
            // 
            this.txtInstallName.Location = new System.Drawing.Point(364, 306);
            this.txtInstallName.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.txtInstallName.Name = "txtInstallName";
            this.txtInstallName.Size = new System.Drawing.Size(336, 37);
            this.txtInstallName.TabIndex = 9;
            this.txtInstallName.TextChanged += new System.EventHandler(this.HasChangedSettingAndFilePath);
            this.txtInstallName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtInstallname_KeyPress);
            // 
            // txtRegistryKeyName
            // 
            this.txtRegistryKeyName.Location = new System.Drawing.Point(364, 648);
            this.txtRegistryKeyName.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.txtRegistryKeyName.Name = "txtRegistryKeyName";
            this.txtRegistryKeyName.Size = new System.Drawing.Size(398, 37);
            this.txtRegistryKeyName.TabIndex = 17;
            this.txtRegistryKeyName.TextChanged += new System.EventHandler(this.HasChangedSetting);
            // 
            // lblExtension
            // 
            this.lblExtension.AutoSize = true;
            this.lblExtension.Location = new System.Drawing.Point(704, 318);
            this.lblExtension.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblExtension.Name = "lblExtension";
            this.lblExtension.Size = new System.Drawing.Size(52, 30);
            this.lblExtension.TabIndex = 10;
            this.lblExtension.Text = ".exe";
            // 
            // lblRegistryKeyName
            // 
            this.lblRegistryKeyName.AutoSize = true;
            this.lblRegistryKeyName.Location = new System.Drawing.Point(34, 654);
            this.lblRegistryKeyName.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblRegistryKeyName.Name = "lblRegistryKeyName";
            this.lblRegistryKeyName.Size = new System.Drawing.Size(151, 30);
            this.lblRegistryKeyName.TabIndex = 16;
            this.lblRegistryKeyName.Text = "Startup Name:";
            // 
            // chkStartup
            // 
            this.chkStartup.AutoSize = true;
            this.chkStartup.Location = new System.Drawing.Point(40, 596);
            this.chkStartup.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.chkStartup.Name = "chkStartup";
            this.chkStartup.Size = new System.Drawing.Size(395, 34);
            this.chkStartup.TabIndex = 15;
            this.chkStartup.Text = "Run Client when the computer starts";
            this.chkStartup.UseVisualStyleBackColor = true;
            this.chkStartup.CheckedChanged += new System.EventHandler(this.chkStartup_CheckedChanged);
            // 
            // rbAppdata
            // 
            this.rbAppdata.AutoSize = true;
            this.rbAppdata.Checked = true;
            this.rbAppdata.Location = new System.Drawing.Point(482, 90);
            this.rbAppdata.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.rbAppdata.Name = "rbAppdata";
            this.rbAppdata.Size = new System.Drawing.Size(252, 34);
            this.rbAppdata.TabIndex = 3;
            this.rbAppdata.TabStop = true;
            this.rbAppdata.Text = "User Application Data";
            this.rbAppdata.UseVisualStyleBackColor = true;
            this.rbAppdata.CheckedChanged += new System.EventHandler(this.HasChangedSettingAndFilePath);
            // 
            // chkHide
            // 
            this.chkHide.AutoSize = true;
            this.chkHide.Location = new System.Drawing.Point(40, 370);
            this.chkHide.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.chkHide.Name = "chkHide";
            this.chkHide.Size = new System.Drawing.Size(307, 34);
            this.chkHide.TabIndex = 11;
            this.chkHide.Text = "Set file attributes to hidden";
            this.chkHide.UseVisualStyleBackColor = true;
            this.chkHide.CheckedChanged += new System.EventHandler(this.HasChangedSetting);
            // 
            // lblInstallDirectory
            // 
            this.lblInstallDirectory.AutoSize = true;
            this.lblInstallDirectory.Location = new System.Drawing.Point(34, 94);
            this.lblInstallDirectory.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblInstallDirectory.Name = "lblInstallDirectory";
            this.lblInstallDirectory.Size = new System.Drawing.Size(168, 30);
            this.lblInstallDirectory.TabIndex = 2;
            this.lblInstallDirectory.Text = "Install Directory:";
            // 
            // lblInstallSubDirectory
            // 
            this.lblInstallSubDirectory.AutoSize = true;
            this.lblInstallSubDirectory.Location = new System.Drawing.Point(34, 252);
            this.lblInstallSubDirectory.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblInstallSubDirectory.Name = "lblInstallSubDirectory";
            this.lblInstallSubDirectory.Size = new System.Drawing.Size(203, 30);
            this.lblInstallSubDirectory.TabIndex = 6;
            this.lblInstallSubDirectory.Text = "Install Subdirectory:";
            // 
            // lblPreviewPath
            // 
            this.lblPreviewPath.AutoSize = true;
            this.lblPreviewPath.Location = new System.Drawing.Point(34, 436);
            this.lblPreviewPath.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblPreviewPath.Name = "lblPreviewPath";
            this.lblPreviewPath.Size = new System.Drawing.Size(290, 30);
            this.lblPreviewPath.TabIndex = 12;
            this.lblPreviewPath.Text = "Installation Location Preview:";
            // 
            // txtInstallSubDirectory
            // 
            this.txtInstallSubDirectory.Location = new System.Drawing.Point(364, 246);
            this.txtInstallSubDirectory.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.txtInstallSubDirectory.Name = "txtInstallSubDirectory";
            this.txtInstallSubDirectory.Size = new System.Drawing.Size(398, 37);
            this.txtInstallSubDirectory.TabIndex = 7;
            this.txtInstallSubDirectory.TextChanged += new System.EventHandler(this.HasChangedSettingAndFilePath);
            this.txtInstallSubDirectory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtInstallsub_KeyPress);
            // 
            // txtPreviewPath
            // 
            this.txtPreviewPath.Location = new System.Drawing.Point(40, 468);
            this.txtPreviewPath.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.txtPreviewPath.Name = "txtPreviewPath";
            this.txtPreviewPath.ReadOnly = true;
            this.txtPreviewPath.Size = new System.Drawing.Size(722, 37);
            this.txtPreviewPath.TabIndex = 13;
            this.txtPreviewPath.TabStop = false;
            // 
            // assemblyPage
            // 
            this.assemblyPage.BackColor = System.Drawing.SystemColors.Control;
            this.assemblyPage.Controls.Add(this.iconPreview);
            this.assemblyPage.Controls.Add(this.btnBrowseIcon);
            this.assemblyPage.Controls.Add(this.txtIconPath);
            this.assemblyPage.Controls.Add(this.line8);
            this.assemblyPage.Controls.Add(this.label11);
            this.assemblyPage.Controls.Add(this.chkChangeAsmInfo);
            this.assemblyPage.Controls.Add(this.txtFileVersion);
            this.assemblyPage.Controls.Add(this.line9);
            this.assemblyPage.Controls.Add(this.lblProductName);
            this.assemblyPage.Controls.Add(this.label12);
            this.assemblyPage.Controls.Add(this.chkChangeIcon);
            this.assemblyPage.Controls.Add(this.lblFileVersion);
            this.assemblyPage.Controls.Add(this.txtProductName);
            this.assemblyPage.Controls.Add(this.txtProductVersion);
            this.assemblyPage.Controls.Add(this.lblDescription);
            this.assemblyPage.Controls.Add(this.lblProductVersion);
            this.assemblyPage.Controls.Add(this.txtDescription);
            this.assemblyPage.Controls.Add(this.txtOriginalFilename);
            this.assemblyPage.Controls.Add(this.lblCompanyName);
            this.assemblyPage.Controls.Add(this.lblOriginalFilename);
            this.assemblyPage.Controls.Add(this.txtCompanyName);
            this.assemblyPage.Controls.Add(this.txtTrademarks);
            this.assemblyPage.Controls.Add(this.lblCopyright);
            this.assemblyPage.Controls.Add(this.lblTrademarks);
            this.assemblyPage.Controls.Add(this.txtCopyright);
            this.assemblyPage.Location = new System.Drawing.Point(140, 4);
            this.assemblyPage.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.assemblyPage.Name = "assemblyPage";
            this.assemblyPage.Size = new System.Drawing.Size(926, 760);
            this.assemblyPage.TabIndex = 2;
            this.assemblyPage.Text = "Assembly Settings";
            // 
            // iconPreview
            // 
            this.iconPreview.Location = new System.Drawing.Point(638, 604);
            this.iconPreview.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.iconPreview.Name = "iconPreview";
            this.iconPreview.Size = new System.Drawing.Size(128, 128);
            this.iconPreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.iconPreview.TabIndex = 42;
            this.iconPreview.TabStop = false;
            // 
            // btnBrowseIcon
            // 
            this.btnBrowseIcon.Location = new System.Drawing.Point(354, 686);
            this.btnBrowseIcon.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.btnBrowseIcon.Name = "btnBrowseIcon";
            this.btnBrowseIcon.Size = new System.Drawing.Size(250, 46);
            this.btnBrowseIcon.TabIndex = 41;
            this.btnBrowseIcon.Text = "Browse...";
            this.btnBrowseIcon.UseVisualStyleBackColor = true;
            this.btnBrowseIcon.Click += new System.EventHandler(this.btnBrowseIcon_Click);
            // 
            // txtIconPath
            // 
            this.txtIconPath.Location = new System.Drawing.Point(40, 630);
            this.txtIconPath.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.txtIconPath.Name = "txtIconPath";
            this.txtIconPath.Size = new System.Drawing.Size(560, 37);
            this.txtIconPath.TabIndex = 39;
            // 
            // line8
            // 
            this.line8.LineAlignment = Comet.Server.Controls.Line.Alignment.Horizontal;
            this.line8.Location = new System.Drawing.Point(244, 10);
            this.line8.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.line8.Name = "line8";
            this.line8.Size = new System.Drawing.Size(522, 26);
            this.line8.TabIndex = 36;
            this.line8.TabStop = false;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(12, 10);
            this.label11.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(224, 30);
            this.label11.TabIndex = 35;
            this.label11.Text = "Assembly Information";
            // 
            // chkChangeAsmInfo
            // 
            this.chkChangeAsmInfo.AutoSize = true;
            this.chkChangeAsmInfo.Location = new System.Drawing.Point(40, 42);
            this.chkChangeAsmInfo.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.chkChangeAsmInfo.Name = "chkChangeAsmInfo";
            this.chkChangeAsmInfo.Size = new System.Drawing.Size(336, 34);
            this.chkChangeAsmInfo.TabIndex = 0;
            this.chkChangeAsmInfo.Text = "Change Assembly Information";
            this.chkChangeAsmInfo.UseVisualStyleBackColor = true;
            this.chkChangeAsmInfo.CheckedChanged += new System.EventHandler(this.chkChangeAsmInfo_CheckedChanged);
            // 
            // txtFileVersion
            // 
            this.txtFileVersion.Location = new System.Drawing.Point(364, 480);
            this.txtFileVersion.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.txtFileVersion.Name = "txtFileVersion";
            this.txtFileVersion.Size = new System.Drawing.Size(398, 37);
            this.txtFileVersion.TabIndex = 16;
            this.txtFileVersion.TextChanged += new System.EventHandler(this.HasChangedSetting);
            // 
            // line9
            // 
            this.line9.LineAlignment = Comet.Server.Controls.Line.Alignment.Horizontal;
            this.line9.Location = new System.Drawing.Point(166, 552);
            this.line9.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.line9.Name = "line9";
            this.line9.Size = new System.Drawing.Size(600, 26);
            this.line9.TabIndex = 38;
            this.line9.TabStop = false;
            // 
            // lblProductName
            // 
            this.lblProductName.AutoSize = true;
            this.lblProductName.Location = new System.Drawing.Point(34, 94);
            this.lblProductName.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblProductName.Name = "lblProductName";
            this.lblProductName.Size = new System.Drawing.Size(157, 30);
            this.lblProductName.TabIndex = 1;
            this.lblProductName.Text = "Product Name:";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(12, 552);
            this.label12.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(152, 30);
            this.label12.TabIndex = 0;
            this.label12.Text = "Assembly Icon";
            // 
            // chkChangeIcon
            // 
            this.chkChangeIcon.AutoSize = true;
            this.chkChangeIcon.Location = new System.Drawing.Point(40, 588);
            this.chkChangeIcon.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.chkChangeIcon.Name = "chkChangeIcon";
            this.chkChangeIcon.Size = new System.Drawing.Size(264, 34);
            this.chkChangeIcon.TabIndex = 2;
            this.chkChangeIcon.Text = "Change Assembly Icon";
            this.chkChangeIcon.UseVisualStyleBackColor = true;
            this.chkChangeIcon.CheckedChanged += new System.EventHandler(this.chkChangeIcon_CheckedChanged);
            // 
            // lblFileVersion
            // 
            this.lblFileVersion.AutoSize = true;
            this.lblFileVersion.Location = new System.Drawing.Point(34, 486);
            this.lblFileVersion.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblFileVersion.Name = "lblFileVersion";
            this.lblFileVersion.Size = new System.Drawing.Size(129, 30);
            this.lblFileVersion.TabIndex = 15;
            this.lblFileVersion.Text = "File Version:";
            // 
            // txtProductName
            // 
            this.txtProductName.Location = new System.Drawing.Point(364, 88);
            this.txtProductName.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.txtProductName.Name = "txtProductName";
            this.txtProductName.Size = new System.Drawing.Size(398, 37);
            this.txtProductName.TabIndex = 2;
            this.txtProductName.TextChanged += new System.EventHandler(this.HasChangedSetting);
            // 
            // txtProductVersion
            // 
            this.txtProductVersion.Location = new System.Drawing.Point(364, 424);
            this.txtProductVersion.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.txtProductVersion.Name = "txtProductVersion";
            this.txtProductVersion.Size = new System.Drawing.Size(398, 37);
            this.txtProductVersion.TabIndex = 14;
            this.txtProductVersion.TextChanged += new System.EventHandler(this.HasChangedSetting);
            // 
            // lblDescription
            // 
            this.lblDescription.AutoSize = true;
            this.lblDescription.Location = new System.Drawing.Point(34, 150);
            this.lblDescription.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(127, 30);
            this.lblDescription.TabIndex = 3;
            this.lblDescription.Text = "Description:";
            // 
            // lblProductVersion
            // 
            this.lblProductVersion.AutoSize = true;
            this.lblProductVersion.Location = new System.Drawing.Point(34, 430);
            this.lblProductVersion.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblProductVersion.Name = "lblProductVersion";
            this.lblProductVersion.Size = new System.Drawing.Size(171, 30);
            this.lblProductVersion.TabIndex = 13;
            this.lblProductVersion.Text = "Product Version:";
            // 
            // txtDescription
            // 
            this.txtDescription.Location = new System.Drawing.Point(364, 144);
            this.txtDescription.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(398, 37);
            this.txtDescription.TabIndex = 4;
            this.txtDescription.TextChanged += new System.EventHandler(this.HasChangedSetting);
            // 
            // txtOriginalFilename
            // 
            this.txtOriginalFilename.Location = new System.Drawing.Point(364, 368);
            this.txtOriginalFilename.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.txtOriginalFilename.Name = "txtOriginalFilename";
            this.txtOriginalFilename.Size = new System.Drawing.Size(398, 37);
            this.txtOriginalFilename.TabIndex = 12;
            this.txtOriginalFilename.TextChanged += new System.EventHandler(this.HasChangedSetting);
            // 
            // lblCompanyName
            // 
            this.lblCompanyName.AutoSize = true;
            this.lblCompanyName.Location = new System.Drawing.Point(34, 206);
            this.lblCompanyName.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblCompanyName.Name = "lblCompanyName";
            this.lblCompanyName.Size = new System.Drawing.Size(175, 30);
            this.lblCompanyName.TabIndex = 5;
            this.lblCompanyName.Text = "Company Name:";
            // 
            // lblOriginalFilename
            // 
            this.lblOriginalFilename.AutoSize = true;
            this.lblOriginalFilename.Location = new System.Drawing.Point(34, 374);
            this.lblOriginalFilename.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblOriginalFilename.Name = "lblOriginalFilename";
            this.lblOriginalFilename.Size = new System.Drawing.Size(187, 30);
            this.lblOriginalFilename.TabIndex = 11;
            this.lblOriginalFilename.Text = "Original Filename:";
            // 
            // txtCompanyName
            // 
            this.txtCompanyName.Location = new System.Drawing.Point(364, 200);
            this.txtCompanyName.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.txtCompanyName.Name = "txtCompanyName";
            this.txtCompanyName.Size = new System.Drawing.Size(398, 37);
            this.txtCompanyName.TabIndex = 6;
            this.txtCompanyName.TextChanged += new System.EventHandler(this.HasChangedSetting);
            // 
            // txtTrademarks
            // 
            this.txtTrademarks.Location = new System.Drawing.Point(364, 312);
            this.txtTrademarks.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.txtTrademarks.Name = "txtTrademarks";
            this.txtTrademarks.Size = new System.Drawing.Size(398, 37);
            this.txtTrademarks.TabIndex = 10;
            this.txtTrademarks.TextChanged += new System.EventHandler(this.HasChangedSetting);
            // 
            // lblCopyright
            // 
            this.lblCopyright.AutoSize = true;
            this.lblCopyright.Location = new System.Drawing.Point(34, 262);
            this.lblCopyright.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblCopyright.Name = "lblCopyright";
            this.lblCopyright.Size = new System.Drawing.Size(114, 30);
            this.lblCopyright.TabIndex = 7;
            this.lblCopyright.Text = "Copyright:";
            // 
            // lblTrademarks
            // 
            this.lblTrademarks.AutoSize = true;
            this.lblTrademarks.Location = new System.Drawing.Point(34, 318);
            this.lblTrademarks.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblTrademarks.Name = "lblTrademarks";
            this.lblTrademarks.Size = new System.Drawing.Size(130, 30);
            this.lblTrademarks.TabIndex = 9;
            this.lblTrademarks.Text = "Trademarks:";
            // 
            // txtCopyright
            // 
            this.txtCopyright.Location = new System.Drawing.Point(364, 256);
            this.txtCopyright.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.txtCopyright.Name = "txtCopyright";
            this.txtCopyright.Size = new System.Drawing.Size(398, 37);
            this.txtCopyright.TabIndex = 8;
            this.txtCopyright.TextChanged += new System.EventHandler(this.HasChangedSetting);
            // 
            // monitoringTab
            // 
            this.monitoringTab.BackColor = System.Drawing.SystemColors.Control;
            this.monitoringTab.Controls.Add(this.chkHideLogDirectory);
            this.monitoringTab.Controls.Add(this.txtLogDirectoryName);
            this.monitoringTab.Controls.Add(this.lblLogDirectory);
            this.monitoringTab.Controls.Add(this.line10);
            this.monitoringTab.Controls.Add(this.label14);
            this.monitoringTab.Controls.Add(this.chkKeylogger);
            this.monitoringTab.Location = new System.Drawing.Point(140, 4);
            this.monitoringTab.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.monitoringTab.Name = "monitoringTab";
            this.monitoringTab.Size = new System.Drawing.Size(926, 760);
            this.monitoringTab.TabIndex = 3;
            this.monitoringTab.Text = "Monitoring Settings";
            // 
            // chkHideLogDirectory
            // 
            this.chkHideLogDirectory.AutoSize = true;
            this.chkHideLogDirectory.Location = new System.Drawing.Point(40, 144);
            this.chkHideLogDirectory.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.chkHideLogDirectory.Name = "chkHideLogDirectory";
            this.chkHideLogDirectory.Size = new System.Drawing.Size(365, 34);
            this.chkHideLogDirectory.TabIndex = 7;
            this.chkHideLogDirectory.Text = "Set directory attributes to hidden";
            this.chkHideLogDirectory.UseVisualStyleBackColor = true;
            this.chkHideLogDirectory.CheckedChanged += new System.EventHandler(this.HasChangedSetting);
            // 
            // txtLogDirectoryName
            // 
            this.txtLogDirectoryName.Location = new System.Drawing.Point(524, 88);
            this.txtLogDirectoryName.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.txtLogDirectoryName.Name = "txtLogDirectoryName";
            this.txtLogDirectoryName.Size = new System.Drawing.Size(232, 37);
            this.txtLogDirectoryName.TabIndex = 6;
            this.txtLogDirectoryName.TextChanged += new System.EventHandler(this.HasChangedSetting);
            this.txtLogDirectoryName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtLogDirectoryName_KeyPress);
            // 
            // lblLogDirectory
            // 
            this.lblLogDirectory.AutoSize = true;
            this.lblLogDirectory.Location = new System.Drawing.Point(34, 94);
            this.lblLogDirectory.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblLogDirectory.Name = "lblLogDirectory";
            this.lblLogDirectory.Size = new System.Drawing.Size(213, 30);
            this.lblLogDirectory.TabIndex = 5;
            this.lblLogDirectory.Text = "Log Directory Name:";
            // 
            // line10
            // 
            this.line10.LineAlignment = Comet.Server.Controls.Line.Alignment.Horizontal;
            this.line10.Location = new System.Drawing.Point(156, 10);
            this.line10.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.line10.Name = "line10";
            this.line10.Size = new System.Drawing.Size(604, 26);
            this.line10.TabIndex = 41;
            this.line10.TabStop = false;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(12, 10);
            this.label14.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(121, 30);
            this.label14.TabIndex = 3;
            this.label14.Text = "Monitoring";
            // 
            // chkKeylogger
            // 
            this.chkKeylogger.AutoSize = true;
            this.chkKeylogger.Location = new System.Drawing.Point(40, 42);
            this.chkKeylogger.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.chkKeylogger.Name = "chkKeylogger";
            this.chkKeylogger.Size = new System.Drawing.Size(287, 34);
            this.chkKeylogger.TabIndex = 4;
            this.chkKeylogger.Text = "Enable keyboard logging";
            this.chkKeylogger.UseVisualStyleBackColor = true;
            this.chkKeylogger.CheckedChanged += new System.EventHandler(this.chkKeylogger_CheckedChanged);
            // 
            // FrmBuilder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(192F, 192F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1070, 848);
            this.Controls.Add(this.builderTabs);
            this.Controls.Add(this.btnBuild);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmBuilder";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Client Builder";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmBuilder_FormClosing);
            this.Load += new System.EventHandler(this.FrmBuilder_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picUAC2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picUAC1)).EndInit();
            this.contextMenuStrip.ResumeLayout(false);
            this.builderTabs.ResumeLayout(false);
            this.generalPage.ResumeLayout(false);
            this.generalPage.PerformLayout();
            this.connectionPage.ResumeLayout(false);
            this.connectionPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPort)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDelay)).EndInit();
            this.installationPage.ResumeLayout(false);
            this.installationPage.PerformLayout();
            this.assemblyPage.ResumeLayout(false);
            this.assemblyPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iconPreview)).EndInit();
            this.monitoringTab.ResumeLayout(false);
            this.monitoringTab.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label lblPort;
        private System.Windows.Forms.TextBox txtHost;
        private System.Windows.Forms.Label lblHost;
        private System.Windows.Forms.Label lblDelay;
        private System.Windows.Forms.CheckBox chkInstall;
        private System.Windows.Forms.TextBox txtInstallName;
        private System.Windows.Forms.Label lblInstallName;
        private System.Windows.Forms.TextBox txtMutex;
        private System.Windows.Forms.Label lblMutex;
        private System.Windows.Forms.Label lblExtension;
        private System.Windows.Forms.Label lblInstallDirectory;
        private System.Windows.Forms.RadioButton rbAppdata;
        private System.Windows.Forms.TextBox txtInstallSubDirectory;
        private System.Windows.Forms.Label lblInstallSubDirectory;
        private System.Windows.Forms.Label lblPreviewPath;
        private System.Windows.Forms.TextBox txtPreviewPath;
        private System.Windows.Forms.Button btnMutex;
        private System.Windows.Forms.CheckBox chkHide;
        private System.Windows.Forms.TextBox txtRegistryKeyName;
        private System.Windows.Forms.Label lblRegistryKeyName;
        private System.Windows.Forms.CheckBox chkStartup;
        private System.Windows.Forms.Button btnBuild;
        private System.Windows.Forms.Label lblMS;
        private System.Windows.Forms.RadioButton rbSystem;
        private System.Windows.Forms.RadioButton rbProgramFiles;
        private System.Windows.Forms.PictureBox picUAC1;
        private System.Windows.Forms.PictureBox picUAC2;
        private System.Windows.Forms.ToolTip tooltip;
        private System.Windows.Forms.CheckBox chkChangeIcon;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.TextBox txtProductName;
        private System.Windows.Forms.Label lblProductName;
        private System.Windows.Forms.TextBox txtOriginalFilename;
        private System.Windows.Forms.Label lblOriginalFilename;
        private System.Windows.Forms.TextBox txtTrademarks;
        private System.Windows.Forms.Label lblTrademarks;
        private System.Windows.Forms.TextBox txtCopyright;
        private System.Windows.Forms.Label lblCopyright;
        private System.Windows.Forms.TextBox txtCompanyName;
        private System.Windows.Forms.Label lblCompanyName;
        private System.Windows.Forms.TextBox txtFileVersion;
        private System.Windows.Forms.Label lblFileVersion;
        private System.Windows.Forms.TextBox txtProductVersion;
        private System.Windows.Forms.Label lblProductVersion;
        private System.Windows.Forms.CheckBox chkChangeAsmInfo;
        private System.Windows.Forms.CheckBox chkKeylogger;
        private Controls.DotNetBarTabControl builderTabs;
        private System.Windows.Forms.TabPage connectionPage;
        private System.Windows.Forms.TabPage installationPage;
        private System.Windows.Forms.TabPage assemblyPage;
        private System.Windows.Forms.TabPage monitoringTab;
        private System.Windows.Forms.ListBox lstHosts;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.Button btnAddHost;
        private System.Windows.Forms.ToolStripMenuItem removeHostToolStripMenuItem;
        private Controls.Line line1;
        private System.Windows.Forms.Label label1;
        private Controls.Line line3;
        private System.Windows.Forms.Label label4;
        private Controls.Line line4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TabPage generalPage;
        private System.Windows.Forms.TextBox txtTag;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lblTag;
        private Controls.Line line5;
        private System.Windows.Forms.Label label6;
        private Controls.Line line6;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private Controls.Line line7;
        private System.Windows.Forms.Label label10;
        private Controls.Line line8;
        private System.Windows.Forms.Label label11;
        private Controls.Line line9;
        private System.Windows.Forms.Label label12;
        private Controls.Line line10;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.ToolStripMenuItem clearToolStripMenuItem;
        private System.Windows.Forms.Button btnBrowseIcon;
        private System.Windows.Forms.TextBox txtIconPath;
        private System.Windows.Forms.PictureBox iconPreview;
        private System.Windows.Forms.Label lblLogDirectory;
        private System.Windows.Forms.TextBox txtLogDirectoryName;
        private System.Windows.Forms.CheckBox chkHideLogDirectory;
        private System.Windows.Forms.NumericUpDown numericUpDownDelay;
        private System.Windows.Forms.NumericUpDown numericUpDownPort;
        private System.Windows.Forms.CheckBox chkHideSubDirectory;
        private System.Windows.Forms.CheckBox chkUnattendedMode;
        private Line line2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}
