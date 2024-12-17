using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    //新进程以普通用户权限运行
    //调用示例：Class3.OpenProcessWithParent(procrss.Handle, Environment.CurrentDirectory + "\\start.bat", "");
    class Class3
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool InitializeProcThreadAttributeList(IntPtr lpAttributeList, int dwAttributeCount, int dwFlags, ref IntPtr lpSize);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool UpdateProcThreadAttribute(
        IntPtr lpAttributeList,
        uint dwFlags,
        uint Attribute,
        IntPtr lpValue,
        IntPtr cbSize,
        IntPtr lpPreviousValue,
        IntPtr lpReturnSize);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        struct STARTUPINFOEX
        {
            public STARTUPINFO StartupInfo;
            public IntPtr lpAttributeList;
        }
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        struct STARTUPINFO
        {
            public Int32 cb;
            public string lpReserved;
            public string lpDesktop;
            public string lpTitle;
            public Int32 dwX;
            public Int32 dwY;
            public Int32 dwXSize;
            public Int32 dwYSize;
            public Int32 dwXCountChars;
            public Int32 dwYCountChars;
            public Int32 dwFillAttribute;
            public Int32 dwFlags;
            public Int16 wShowWindow;
            public Int16 cbReserved2;
            public IntPtr lpReserved2;
            public IntPtr hStdInput;
            public IntPtr hStdOutput;
            public IntPtr hStdError;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct PROCESS_INFORMATION
        {
            internal IntPtr hProcess;
            internal IntPtr hThread;
            internal int dwProcessId;
            internal int dwThreadId;
        }

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern Boolean CreateProcessAsUser(
                                                    IntPtr hToken,
                                                    String lpApplicationName,
                                                    String lpCommandLine,
                                                    IntPtr lpProcessAttributes,
                                                    IntPtr lpThreadAttributes,
                                                    Boolean bInheritHandles,
                                                    UInt32 dwCreationFlags,
                                                    IntPtr lpEnvironment,
                                                    String lpCurrentDirectory,
                                                    ref STARTUPINFOEX lpStartupInfo,
                                                    ref PROCESS_INFORMATION lpProcessInformation);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool DeleteProcThreadAttributeList(IntPtr lpAttributeList);

        [Flags]
        public enum ProcessAccessFlags : uint
        {
            All = 0x001F0FFF,
            Terminate = 0x00000001,
            CreateThread = 0x00000002,
            VirtualMemoryOperation = 0x00000008,
            VirtualMemoryRead = 0x00000010,
            VirtualMemoryWrite = 0x00000020,
            DuplicateHandle = 0x00000040,
            CreateProcess = 0x000000080,
            SetQuota = 0x00000100,
            SetInformation = 0x00000200,
            QueryInformation = 0x00000400,
            QueryLimitedInformation = 0x00001000,
            Synchronize = 0x00100000
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr OpenProcess(
        ProcessAccessFlags processAccess,
        bool bInheritHandle,
        int processId
);


        const uint PROC_THREAD_ATTRIBUTE_PARENT_PROCESS = 0x00020000;
        const uint PROC_THREAD_ATTRIBUTE_HANDLE_LIST = 0x00020002;
        const uint EXTENDED_STARTUPINFO_PRESENT = 0x00080000;

        private static IntPtr MyInitializeProcThreadAttributeList(int attributeCount)
        {
            const int reserved = 0;
            var size = IntPtr.Zero;
            bool wasInitialized = InitializeProcThreadAttributeList(IntPtr.Zero, attributeCount, reserved, ref size);
            if (wasInitialized || size == IntPtr.Zero)
            {
                throw new Exception(string.Format("Couldn't get the size of the attribute list for {0} attributes", attributeCount));
            }

            IntPtr lpAttributeList = Marshal.AllocHGlobal(size);
            if (lpAttributeList == IntPtr.Zero)
            {
                throw new Exception("Couldn't reserve space for a new attribute list");
            }

            wasInitialized = InitializeProcThreadAttributeList(lpAttributeList, attributeCount, reserved, ref size);
            if (!wasInitialized)
            {
                throw new Exception("Couldn't create new attribute list");
            }

            return lpAttributeList;
        }

        public static void OpenProcessWithParent(IntPtr parentProcessHandle, string filepath, string commandline)
        {
            var handleArray = new IntPtr[] { parentProcessHandle };
            var pinnedHandleArray = GCHandle.Alloc(handleArray, GCHandleType.Pinned);
            IntPtr handles = pinnedHandleArray.AddrOfPinnedObject();

            var attributeList = MyInitializeProcThreadAttributeList(1);

            bool success = UpdateProcThreadAttribute(
                            attributeList,
                            0,
                            PROC_THREAD_ATTRIBUTE_PARENT_PROCESS,
                            handles,
                            (IntPtr)(IntPtr.Size),
                            IntPtr.Zero,
                            IntPtr.Zero);

            if (!success)
            {
                throw new Exception("Error adding inheritable handles to process launch");
            }

            STARTUPINFOEX info = new STARTUPINFOEX();
            info.StartupInfo.cb = Marshal.SizeOf(info);
            //隐藏被运行程序的窗口
            info.StartupInfo.dwFlags= 1;
            //隐藏被运行程序的窗口
            info.StartupInfo.wShowWindow = 0;
            info.lpAttributeList = attributeList;

            PROCESS_INFORMATION procInfo = new PROCESS_INFORMATION();
            success = CreateProcessAsUser(IntPtr.Zero, filepath, commandline, IntPtr.Zero, IntPtr.Zero, false, EXTENDED_STARTUPINFO_PRESENT, IntPtr.Zero, null, ref info, ref procInfo);
            if (!success)
            {
                throw new Exception("CreateProcessAsUser fail");
            }

            DeleteProcThreadAttributeList(attributeList);
        }
    }
}