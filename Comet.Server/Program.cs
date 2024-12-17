using Comet.Server.Forms;
using System;
using System.Net;
using System.Windows.Forms;
//DotFuscator 壳 默认情况下禁用字符串加密。在 Feature -> Settings -> Options，可以通过将 Disable String Encryption 设置为 No
namespace Comet.Server
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            //DateTime now = DateTime.Now.Date;
            //if (now.Month > 3)
            //{
            //    Application.Exit();
            //    return;
            //} 
            // enable TLS 1.2
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FrmMain());
        }
    }
}
