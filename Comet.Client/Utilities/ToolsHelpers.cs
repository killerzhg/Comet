using System;
using System.Diagnostics;
using System.Management;
using System.Runtime.InteropServices;
using System.Windows.Forms;

/// <summary>
/// SYSTEM进程也能获取用户名
/// </summary>
public static class ToolsHelpers
{
    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern uint WTSGetActiveConsoleSessionId();

    [DllImport("Wtsapi32.dll", EntryPoint = "WTSQueryUserToken")]
    public static extern bool WTSQueryUserToken(UInt32 sessionId, out IntPtr phToken);

    [DllImport("advapi32.DLL")]
    public static extern bool ImpersonateLoggedOnUser(IntPtr hToken); //handle to token for logged-on user

    [DllImport("advapi32.DLL")]
    public static extern bool RevertToSelf(); //停止模拟
    public static bool SimulateCurrentUserLogin() 
    {
        uint dwConsoleSessionId = WTSGetActiveConsoleSessionId();
        IntPtr userTokenPtr = new IntPtr();
        if (WTSQueryUserToken(dwConsoleSessionId, out userTokenPtr))
        {
            if (ImpersonateLoggedOnUser(userTokenPtr))
            {
                return true;
            }
            return false;
        }
        return false;
    }
    public static string GetExplorerUserName()
    {
        Process[] p = Process.GetProcessesByName("explorer");
        if (p != null)
        {
            return getProcessOwnerName(p[0].Id);
        }
        return "";
    }

    /// <summary>
    /// 通过pid获取用户名
    /// </summary>
    /// <param name="processId"></param>
    /// <returns></returns>
    public static string getProcessOwnerName(int processId)
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
}