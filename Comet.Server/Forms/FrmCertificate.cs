﻿using Comet.Server.Helper;
using System;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;
using Comet.Server.Models;

namespace Comet.Server.Forms
{
    public partial class FrmCertificate : Form
    {
        private X509Certificate2 _certificate;

        public FrmCertificate()
        {
            InitializeComponent();
        }

        private void SetCertificate(X509Certificate2 certificate)
        {
            _certificate = certificate;
            txtDetails.Text = _certificate.ToString(false);
            //btnSave.Enabled = true;
            SaveCertificate();
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            SetCertificate(CertificateHelper.CreateCertificateAuthority("Comet Server CA", 4096));
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.CheckFileExists = true;
                ofd.Filter = "*.p12|*.p12";
                ofd.Multiselect = false;
                ofd.InitialDirectory = Application.StartupPath;
                if (ofd.ShowDialog(this) == DialogResult.OK)
                {
                    try
                    {
                        SetCertificate(new X509Certificate2(ofd.FileName, "", X509KeyStorageFlags.Exportable));
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(this, $"Error importing the certificate:\n{ex.Message}", "Save error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void SaveCertificate()
        {
            try
            {
                if (_certificate == null)
                    throw new ArgumentNullException();

                if (!_certificate.HasPrivateKey)
                    throw new ArgumentException();

                File.WriteAllBytes(Settings.CertificatePath, _certificate.Export(X509ContentType.Pkcs12));

                MessageBox.Show(this,
                    "请立即备份证书。丢失证书将导致丢失所有客户端!",
                    "Certificate backup", MessageBoxButtons.OK, MessageBoxIcon.Information);

                //string argument = "/select, \"" + Settings.CertificatePath + "\"";
                //Process.Start("explorer.exe", argument);

                this.DialogResult = DialogResult.OK;
                Close();
            }
            catch (ArgumentNullException)
            {
                MessageBox.Show(this, "Please create or import a certificate first.", "Save error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (ArgumentException)
            {
                MessageBox.Show(this,
                    "The imported certificate has no associated private key. Please import a different certificate.",
                    "Save error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception)
            {
                MessageBox.Show(this,
                    "There was an error saving the certificate, please make sure you have write access to the Comet directory.",
                    "Save error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
