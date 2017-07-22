using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace ArcMapByCommon.ADF.API
{
    /// <summary>
    /// Window API Kernel32.dll 封装
    /// </summary>
    public static class Kernel32
    {
        /// <summary>
        /// const MAX_PATH = 260
        /// </summary>
        public const int MAX_PATH = 260;
        public const long INVALID_HANDLE_VALUE = -1;
        public const long FILE_ATTRIBUTE_ARCHIVE = 0x20;// &H20;
        public const long FILE_ATTRIBUTE_COMPRESSED = 0x800;// &H800;
        public const long FILE_ATTRIBUTE_DIRECTORY = 0x10;// &H10;
        public const long FILE_ATTRIBUTE_HIDDEN = 0x2;//&H2;
        public const long FILE_ATTRIBUTE_NORMAL = 0x80;//&H80;
        public const long FILE_ATTRIBUTE_READONLY = 0x1;// &H1;
        public const long FILE_ATTRIBUTE_TEMPORARY = 0x100;// &H100;
        public const long FILE_ATTRIBUTE_FLAGS = FILE_ATTRIBUTE_ARCHIVE | FILE_ATTRIBUTE_HIDDEN | FILE_ATTRIBUTE_NORMAL | FILE_ATTRIBUTE_READONLY;

        [DllImport("Kernel32.dll", CharSet = CharSet.Auto)]
        public static extern int GetCurrentThreadId();
        [DllImport("Kernel32.dll", CharSet = CharSet.Auto)]
        public static extern long FindFirstFile(string lpFileName, WIN32_FIND_DATA lpFindFileData);
        [DllImport("Kernel32.dll", CharSet = CharSet.Auto)]
        public static extern long FindClose(long hFindFile);
        [DllImport("Kernel32.dll", CharSet = CharSet.Auto)]
        public static extern long FindNextFile(long hFindFile, WIN32_FIND_DATA lpFindFileData);
        [DllImport("Kernel32.dll", CharSet = CharSet.Auto)]
        public static extern long DeleteFile(string lpFileName);
        [DllImport("Kernel32.dll", CharSet = CharSet.Auto)]
        public static extern long RemoveDirectory(string lpPathName);

        [DllImport("kernel32")]//返回0表示失败，非0为成功
        public static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]//返回取得字符串缓冲区的长度
        public static extern long GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
        //设置系统时间API
        [DllImport("kernel32.dll")]
        public static extern int SetLocalTime(ref SystemTime lpSystemTime);
    }
    public struct SystemTime
    {
        public short wYear;
        public short wMonth;
        public short wDayOfWeek;
        public short wDay;
        public short wHour;
        public short wMinute;
        public short wSecond;
        public short wMilliseconds;
    }
    //
    public struct FILETIME
    {
        public long dwLowDateTime;
        public long dwHighDateTime;
    }

    public struct WIN32_FIND_DATA
    {
        public long dwFileAttributes;   //'--48,16
        public FILETIME ftCreationTime;
        public FILETIME ftLastAccessTime; // As FILETIME
        public FILETIME ftLastWriteTime; // As FILETIME
        public long nFileSizeHigh;// As Long
        public long nFileSizeLow;// As Long
        public long dwReserved0;// As Long
        public long dwReserved1;// As Long
        public string cFileName;// As String * MAX_PATH
        public string cAlternate;// As String * 14
    }
}
