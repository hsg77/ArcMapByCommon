using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace ArcMapByCommon.ADF.API
{
    /// <summary>
    /// 封装关闭window操作系统的API类
    /// vp:hsg
    /// create date:2014-05
    /// </summary>
    public class ExitWindowClass
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct TokPriv1Luid
        {
            public int Count;
            public long Luid;
            public int Attr;
        }

        [DllImport("kernel32.dll", ExactSpelling = true)]
        public static extern IntPtr GetCurrentProcess();

        [DllImport("advapi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern bool OpenProcessToken(IntPtr h, int acc, ref   IntPtr phtok);

        [DllImport("advapi32.dll", SetLastError = true)]
        public static extern bool LookupPrivilegeValue(string host, string name, ref   long pluid);

        [DllImport("advapi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern bool AdjustTokenPrivileges(IntPtr htok, bool disall,
                                        ref   TokPriv1Luid newst, int len, IntPtr prev, IntPtr relen);

        //[DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
        //public static extern bool ExitWindowsEx(int flg, int rea);

        public const int SE_PRIVILEGE_ENABLED = 0x00000002;
        public const int TOKEN_QUERY = 0x00000008;
        public const int TOKEN_ADJUST_PRIVILEGES = 0x00000020;
        public const string SE_SHUTDOWN_NAME = "SeShutdownPrivilege";
        public const int EWX_LOGOFF = 0x00000000;
        public const int EWX_SHUTDOWN = 0x00000001; //关机
        public const int EWX_REBOOT = 0x00000002;  //重启
        public const int EWX_FORCE = 0x00000004;
        public const int EWX_POWEROFF = 0x00000008;  //关电源
        public const int EWX_FORCEIFHUNG = 0x00000010;

        public static void DoExitWin(int flg)
        {
            bool ok;
            TokPriv1Luid tp;
            IntPtr hproc = GetCurrentProcess();
            IntPtr htok = IntPtr.Zero;
            ok = OpenProcessToken(hproc, TOKEN_ADJUST_PRIVILEGES | TOKEN_QUERY, ref htok);
            tp.Count = 1;
            tp.Luid = 0;
            tp.Attr = SE_PRIVILEGE_ENABLED;
            ok = LookupPrivilegeValue(null, SE_SHUTDOWN_NAME, ref tp.Luid);
            ok = AdjustTokenPrivileges(htok, false, ref tp, 0, IntPtr.Zero, IntPtr.Zero);
            ExitWindowsEx(flg, 0);
        }
        public static void DoExitWinShutDown()
        {
            DoExitWin(EWX_SHUTDOWN);
        }
        public static void DoExitWinPowerOff()
        {
            DoExitWin(EWX_POWEROFF);
        }


        [DllImport("user32.dll", EntryPoint = "ExitWindowsEx", CharSet = CharSet.Ansi)]
        private static extern int ExitWindowsEx(int uFlags, int dwReserved);
        //注销计算机
        public static void logout()
        {
            ExitWindowsEx(0, 0);
        }
        //关闭计算机
        public static void closepc()
        {
            //创建访问控制本地系统进程的对象实例
            System.Diagnostics.Process myprocess = new System.Diagnostics.Process();
            myprocess.StartInfo.FileName = "cmd.exe";
            myprocess.StartInfo.UseShellExecute = false;
            myprocess.StartInfo.RedirectStandardInput = true;
            myprocess.StartInfo.RedirectStandardOutput = true;
            myprocess.StartInfo.RedirectStandardError = true;
            myprocess.StartInfo.CreateNoWindow = true;
            myprocess.Start();
            myprocess.StandardInput.WriteLine("shutdown -s -t 0");
        }
        //重新启动计算机
        public static void afreshstartpc()
        {
            //创建访问控制本地系统进程的对象实例
            System.Diagnostics.Process myprocess = new System.Diagnostics.Process();
            myprocess.StartInfo.FileName = "cmd.exe";
            myprocess.StartInfo.UseShellExecute = false;
            myprocess.StartInfo.RedirectStandardInput = true;
            myprocess.StartInfo.RedirectStandardOutput = true;
            myprocess.StartInfo.RedirectStandardError = true;
            myprocess.StartInfo.CreateNoWindow = true;
            myprocess.Start();
            myprocess.StandardInput.WriteLine("shutdown -r -t 0");
        }
    }
}
