using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;
using System.Diagnostics;
using System.Windows.Forms;
using System.IO;
using System.Collections;
using System.Data;
using System.Net.Mail;
using System.Security.Permissions;
using System.Net;
using ArcMapByJBNT.ADF.API;

namespace ArcMapByJBNT
{
    /// <summary>
    /// 公共用函数库1  静态类
    /// </summary>
    public static class CommonClass
    {
        //机器注册表的开机自启动
        /// <summary>
        /// 程序是否是自动运行
        /// </summary>
        /// <param name="name">键名</param>
        /// <param name="path">启动路径（包括文件名）</param>
        public static bool IsAutoRun(string name, string path)
        {
            RegistryKey HKLM = Registry.LocalMachine;
            string str = "";

            try
            {
                RegistryKey Run = HKLM.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run");
                str = Run.GetValue(name, "").ToString();
                Run.Close();
            }
            catch (Exception ex)
            {
                Log.WriteLine(ex);
            }
            finally
            {
                HKLM.Close();
            }

            if (str != "")
                return true;
            else
                return false;
        }

        /// <summary>
        /// 设置或取消程序自动运行
        /// </summary>
        /// <param name="enable">指示程序是设置为自动运行还是取消自动运行：true-设置;false-取消</param>
        /// <param name="name">键名</param>
        /// <param name="path">启动路径（包括文件名）</param>
        public static void SetAutoRun(bool enable, string name, string path)
        {
            try
            {
                RegistryKey HKLM = Registry.LocalMachine;
                RegistryKey Run = HKLM.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run");

                if (enable)
                {
                    Run.SetValue(name, path);
                    HKLM.Close();
                }
                else
                {
                    Run.DeleteValue(name);
                    HKLM.Close();
                }
            }
            catch (Exception ex)
            {
                Log.WriteLine(ex);                
            }
        }

        //用户注册表的开机自启动
        public static bool CurrentUser_IsAutoRun(string name, string path)
        {
            RegistryKey HKLM = Registry.CurrentUser;
            string str = "";
            try
            {
                RegistryKey Run = HKLM.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run");
                str = Run.GetValue(name, "").ToString();
                Run.Close();
            }
            catch (Exception ex)
            {
                Log.WriteLine(ex);
            }
            finally
            {
                HKLM.Close();
            }

            if (str != "")
                return true;
            else
                return false;
        }

        public static void CurrentUser_SetAutoRun(bool enable, string name, string path)
        {
            try
            {
                RegistryKey HKLM = Registry.CurrentUser;
                RegistryKey Run = HKLM.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run");

                if (enable)
                {
                    Run.SetValue(name, path);
                    HKLM.Close();
                }
                else
                {
                    Run.DeleteValue(name);
                    HKLM.Close();
                }
            }
            catch (Exception ex)
            {
                Log.WriteLine(ex);                
            }
        }
        //
        public static void SetStringToClipboard(string str)
        {
            Clipboard.SetDataObject(str);
        }

        public static string GetStringFromClipboard()
        {
            IDataObject iData = Clipboard.GetDataObject();

            if (iData.GetDataPresent(DataFormats.Text))
            {
                return (String)iData.GetData(DataFormats.Text);
            }
            else return "";
        }
        /// <summary>
        /// 返回当前计算机系统时间是星期几的方法
        /// </summary>
        /// <returns>星期一~星期六~星期日</returns>
        public static string getCurrentDayOfWeek()
        {
            string week = "星期";
            int dw = (int)System.DateTime.Now.DayOfWeek;
            string whatVal = "";
            switch (dw.ToString())
            {
                case "0":
                    whatVal = "日";
                    break;
                case "1":
                    whatVal = "一";
                    break;
                case "2":
                    whatVal = "二";
                    break;
                case "3":
                    whatVal = "三";
                    break;
                case "4":
                    whatVal = "四";
                    break;
                case "5":
                    whatVal = "五";
                    break;
                case "6":
                    whatVal = "六";
                    break;
            }
            week += whatVal;
            return week;

        }

        /// <summary>
        /// 执行关闭Window操作系统的方法
        /// </summary>
        public static void DoExitWindowsShutDown()
        {
            ExitWindowClass.DoExitWinShutDown();
        }
        /// <summary>
        /// 设置本机系统时间的方法
        /// </summary>
        /// <param name="setDateTime"></param>
        public static void SetLocalSystemDateTime(DateTime setDateTime)
        {
            //根据得到的时间日期，来定义时间、日期
            SystemTime st = new SystemTime();
            st.wYear = (short)setDateTime.Year;
            st.wDay = (short)setDateTime.Day;
            st.wMonth = (short)setDateTime.Month;
            st.wHour = (short)setDateTime.Hour;
            st.wMinute = (short)setDateTime.Minute;
            st.wSecond = (short)setDateTime.Second;
            //修改本地端的时间和日期
            Kernel32.SetLocalTime(ref st);
        }

        /// <summary>
        /// 将列表1与列表2合并
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public static List<T> UnionList<T>(List<T> source, List<T> list)
        {
            foreach (T item in list)
                source.Add(item);

            return source;
        }
        /// <summary>
        /// 将数组source与数组list进行合并并返回
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public static T[] UnionList<T>(T[] source, T[] list)
        {
            List<T> temp_list = new List<T>();

            foreach (T item in list)
                temp_list.Add(item);

            foreach (T item in source)
                temp_list.Add(item);

            return temp_list.ToArray();
        }

        public static List<T> ArrayToList<T>(T[] array)
        {
            List<T> tList = new List<T>();

            foreach (T t in array)
                tList.Add(t);

            return tList;
        }
        public static bool Contains<T>(T[] array, T val)
        {
            bool rbc = false;
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i].Equals(val))
                {
                    rbc = true;
                    break;
                }
            }
            return rbc;
        }   

        #region 静态函数
        /// <summary>
        /// 向注册表中写入一个值
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public static void SetRegistryValue(string name, object value, RegistryValueKind valueKind)
        {
            RegistryKey reg = Application.UserAppDataRegistry;
            reg.SetValue(name, value, valueKind);
            reg.Close();
        }

        public static void SetRegistryValue(string name, object value)
        {
            SetRegistryValue(name, value, RegistryValueKind.String);
        }
        /// <summary>
        /// 从注册表中获取一个值
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static object GetRegistryValue(string name)
        {
            object value = null;

            RegistryKey reg = Application.UserAppDataRegistry;
            if (reg != null)
            {
                try
                {
                    value = reg.GetValue(name);
                }
                finally
                {
                    reg.Close();
                }
            }

            return value;
        }
        #endregion

        #region 临时数据缓存管理

        /// <summary>
        /// 获取Buffer字符串数组
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>字符串数组</returns>
        public static string[] GetBuffer(string key)
        {
            object o = GetRegistryValue(key);
            if (o != null && o is string[])
            {
                return o as string[];
            }
            else
            {
                return new string[] { };
            }
        }

        /// <summary>
        /// 设置Buffer值
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="value">Buffer值</param>
        /// <param name="count">Buffer长度</param>
        public static void SetBuffer(string key, string[] value, uint count)
        {
            if (count == 0 || value.Length <= count)
            {
                SetRegistryValue(key, value, RegistryValueKind.MultiString);
            }
            else
            {
                List<string> list = new List<string>();
                for (int i = 0; i < count; i++)
                {
                    list.Add(value[i]);
                }

                SetRegistryValue(key, list.ToArray(), RegistryValueKind.MultiString);
            }
        }

        /// <summary>
        /// 设置Buffer值，默认长度为20
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="value">Buffer值</param>
        public static void SetBuffer(string key, string[] value)
        {
            SetBuffer(key, value, 20);
        }

        /// <summary>
        /// 添加Buffer值，默认长度为20
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="value">字符串</param>
        public static void AppendBuffer(string key, string value)
        {
            AppendBuffer(key, value, true);
        }

        /// <summary>
        /// 添加Buffer值
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="value">Buffer值</param>
        /// <param name="lock_length">是否锁定Buffer长度，true为20，false为不限长度</param>
        public static void AppendBuffer(string key, string value, bool lock_length)
        {
            string[] array = GetBuffer(key);
            List<string> list = ArrayToList(array);
            list.Insert(0, value);

            if (lock_length)
            {
                SetBuffer(key, list.ToArray());
            }
            else
            {
                SetBuffer(key, list.ToArray(), (uint)list.Count);
            }
        }

        #endregion

        #region 临时目录与临时文件管理
        /// <summary>
        /// 临时文件目录存放注册表的键名
        /// </summary>
        public const string C_TEMP_DIRECTORY = "TempDirectory";
        /// <summary>
        /// 获取临时目录
        /// </summary>
        /// <returns></returns>
        public static string GetTempPath()
        {
            string tmpPath = string.Empty;
            object o = CommonClass.GetRegistryValue(C_TEMP_DIRECTORY);
            if (o != null)
            {
                tmpPath = o.ToString();
                if (tmpPath.Substring(tmpPath.Length - 1, 1) != "\\")
                    tmpPath = tmpPath + "\\";
            }
            if (Directory.Exists(tmpPath) == false)
                tmpPath = Path.GetTempPath();
            return tmpPath;
        }
        /// <summary>
        /// 生成一个临时文件名称（不生成临时文件）
        /// </summary>
        /// <param name="extension">扩展名：如shp或mdb</param>
        /// <returns></returns>
        public static string GetTempFile(string extension)
        {
            string tmpFileName = GetTempPath() + Guid.NewGuid().ToString().Replace("-", "")
                + "." + extension;
            object obj = CommonClass.GetRegistryValue("TEMPFILELIST");
            List<string> con_list = new List<string>();
            if (obj is string[])
            {
                con_list = CommonClass.ArrayToList(obj as string[]);
            }
            con_list.Add(tmpFileName);
            CommonClass.SetRegistryValue("TEMPFILELIST", con_list.ToArray(), RegistryValueKind.MultiString);
            return tmpFileName;
        }
        /// <summary>
        /// 清空临时文件
        /// </summary>
        public static void DeleteAllTempFile()
        {
            object obj = CommonClass.GetRegistryValue("TEMPFILELIST");
            if (obj is string[])
            {
                foreach (string s in (obj as string[]))
                    DeleteFile(s);
            }
            CommonClass.SetRegistryValue("TEMPFILELIST", new string[] { }, RegistryValueKind.MultiString);
        }
        /// <summary>
        /// 删除相同名称的文件：如aa.shp和aa.txt
        /// </summary>
        /// <param name="fileName"></param>
        public static void DeleteFile_NameSame(string fileName)
        {
            string name = Path.GetFileNameWithoutExtension(fileName);
            string ext = Path.GetExtension(fileName);
            string path = fileName.Substring(0, fileName.Length - name.Length - ext.Length);

            if (Directory.Exists(path))
            {
                foreach (string f in Directory.GetFiles(path))
                {
                    try
                    {
                        if (f.Contains(name))
                        {
                            FileInfo fi = new FileInfo(f);
                            fi.Attributes = FileAttributes.Normal;

                            File.Delete(f);
                        }
                    }
                    catch(Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine(ex.ToString());
                    }
                }
            }
        }

        public static void SetNormalFileAttriblue(string filePath)
        {
            FileInfo fi = new FileInfo(filePath);
            fi.Attributes = FileAttributes.Normal;
            fi.IsReadOnly = false;
            fi = null;
        }
        #endregion

        /// <summary>
        /// 分割字符串取其中的某项值
        /// </summary>
        /// <param name="ArrStr">复杂字符串</param>
        /// <param name="Spliter">分隔符</param>
        /// <param name="Pos">所取位子的字符串</param>
        /// <returns></returns>
        public static string SplitStr(string ArrStr, char Spliter, int Pos)
        {
            #region 实现细节
            //Response.Write(KdGis.Common.SplitStr("abc.efg.hy.mcb",'.',4));
            string Rbc = "";
            string[] ArrayStr = ArrStr.Split(new char[] { Spliter });
            Pos -= 1;
            if (Pos >= 0 && Pos < ArrayStr.Length)
            {
                try
                {
                    Rbc = ArrayStr[Pos].ToString();
                }
                catch(Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.ToString());
                    Rbc = "";
                }
            }
            else
            {
                Rbc = "";
            }

            return Rbc;
            #endregion
        }

        public static string SplitStr(string ArrStr, string Spliter, int Pos)
        {
            #region 实现细节
            //Response.Write(KdGis.Common.SplitStr("abc.efg.hy.mcb",'.',4));
            int i = 0, P1 = 0;
            string Str = "", str3 = "";
            int TotalRecNum = 0;
            try
            {

                Str = ArrStr;
                TotalRecNum = 0;
                for (i = 1; i <= Pos; i++)
                {
                    //P1 = InStr(1, Str, delimiter);
                    P1 = Str.IndexOf(Spliter, 0);
                    if (P1 == -1)
                    {
                        if ((TotalRecNum == Pos) || (TotalRecNum + 1 == Pos))
                            str3 = Str;
                        else
                            str3 = "";
                        break;
                    }
                    //str3 = Mid(Str, 1, P1 - 1);
                    //Str = Mid(Str, P1 + 1);
                    str3 = Str.Substring(0, P1);
                    Str = Str.Substring(P1 + Spliter.Length);
                    TotalRecNum = TotalRecNum + 1;
                }
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                str3 = "";
            }
            return str3;
            #endregion
        }

        /// <summary>
        /// 返回分隔字符的个数
        /// </summary>
        /// <param name="ArrStr"></param>
        /// <param name="Spliter"></param>
        /// <returns></returns>
        public static int SplitStrCount(string ArrStr, char Spliter)
        {
            #region 实现细节
            int Rbc = 0;
            string[] ArrayStr = ArrStr.Split(new char[] { Spliter });
            Rbc = ArrayStr.Length;

            return Rbc;
            #endregion
        }

        public static string getStringByArray(object[] stringArray, char Spliter)
        {
            string tpString = "";
            for (int i = 0; i < stringArray.Length; i++)
            {
                if (tpString.Length <= 0)
                {
                    tpString = stringArray[i].ToString();
                }
                else
                {
                    tpString = tpString + Spliter.ToString() + stringArray[i].ToString();
                }
            }
            return tpString;
        }

        public static string getStringByArray(object[] stringArray, string Spliter)
        {
            string tpString = "";
            for (int i = 0; i < stringArray.Length; i++)
            {
                if (tpString.Length <= 0)
                {
                    tpString = stringArray[i].ToString();
                }
                else
                {
                    tpString = tpString + Spliter.ToString() + stringArray[i].ToString();
                }
            }
            return tpString;
        }

        public static SortedList SortArrayReturnSortedList(object[] pArray)
        {
            SortedList tpSortedList = new SortedList();
            tpSortedList.Clear();
            for (int i = 0; i < pArray.Length; i++)
            {
                tpSortedList.Add(pArray[i].ToString(), pArray[i]);
            }
            return tpSortedList;
        }

        public static IList<string> SortArrayReturnList(object[] pArray)
        {
            SortedList<string, object> tpSortedList = new SortedList<string, object>();
            tpSortedList.Clear();
            for (int i = 0; i < pArray.Length; i++)
            {
                tpSortedList.Add(pArray[i].ToString(), pArray[i]);
            }
            return tpSortedList.Keys;
        }

        public static decimal TDec(object WantToDoubleNumber)
        {
            #region 实现细节
            decimal Rbc = 0;
            try
            {
                Rbc = System.Decimal.Parse(WantToDoubleNumber.ToString());
            }
            catch
            {
                Rbc = 0;
            }
            return Rbc;
            #endregion
        }

        public static double TNum(object WantToDoubleNumber)
        {
            #region 实现细节
            double Rbc = 0;
            try
            {
                if (double.TryParse(WantToDoubleNumber.ToString(), out Rbc) == false)
                {
                    Rbc = 0;
                }
            }
            catch
            {
                Rbc = 0;
            }
            return Rbc;
            #endregion
        }

        public static long TLng(object WantToDoubleNumber)
        {
            #region 实现细节
            long Rbc = 0;
            try
            {
                Rbc = long.Parse(WantToDoubleNumber.ToString());
            }
            catch
            {
                Rbc = 0;
            }
            return Rbc;
            #endregion
        }

        public static int TInt(object WantToDoubleNumber)
        {
            #region 实现细节
            int Rbc = 0;
            try
            {
                if (int.TryParse(WantToDoubleNumber.ToString(), out Rbc) == false)
                {
                    Rbc = 0;
                }
            }
            catch
            {
                Rbc = 0;
            }
            return Rbc;
            #endregion
        }

        public static string TNV(object WantToDoubleNumber)
        {
            #region 实现细节
            string Rbc = "";
            try
            {
                Rbc = WantToDoubleNumber.ToString();
            }
            catch
            {
                Rbc = "";
            }
            return Rbc;
            #endregion
        }

        public static string ToDateStr(string srcStr)
        {   //20120712->2012-07-12
            #region 实现细节
            if (srcStr == "")
            {
                return "";
            }
            try
            {
                DateTime dt = Convert.ToDateTime(srcStr);
                string tmpStr;
                tmpStr = FormatLen(dt.Year.ToString(), "0", 4) + "-" + FormatLen(dt.Month.ToString(), "0", 2) + "-" + FormatLen(dt.Day.ToString(), "0", 2);
                return tmpStr;
            }
            catch
            {
                return "";
            }
            #endregion
        }

        public static DateTime ToDateTime(string srcStr)
        {
            srcStr = srcStr.Replace("/", "-");
            srcStr = srcStr.Replace("\\", "-");
            #region 实现细节
            DateTime dt = System.DateTime.Now;
            if (srcStr == "")
            {
                return dt;
            }
            try
            {
                bool rbc= DateTime.TryParse(srcStr,out dt);
                if (rbc == false)
                {
                    dt = System.DateTime.Now;
                }
            }
            catch(Exception ee)
            {
                System.Diagnostics.Debug.WriteLine(ee.ToString());
            }
            return dt;
            #endregion
        }
        public static string GetDateStr(DateTime dt)
        {
            return dt.ToString("yyyy-MM-dd");
        }
        public static string GetDateTimeStr(DateTime dt)
        {
            return dt.ToString("yyyy-MM-dd HH:mm:ss");
        }


        public static string FormatLen(string srcStr, string prefix, int len)
        {
            #region 实现细节
            int strLen = srcStr.Length;
            if (strLen >= len)
                return srcStr;

            string tmpStr = srcStr;
            for (int i = 0; i < len - strLen; i++)
            {
                tmpStr = prefix + tmpStr;
            }
            return tmpStr;
            #endregion
        }
        public static string GetFormatStr_0(int index, int len)
        {
            string rbc = index.ToString();
            int sj_len = index.ToString().Length;
            if (sj_len < len)
            {
                rbc = "";
                for (int i = 0; i < len - sj_len; i++)
                {
                    rbc += "0";
                }
                rbc += index.ToString();
            }
            return rbc;
        }
        //格式化字符串长度函数
        public static string To_Str(string pstr, int strlen)
        {
            string old_str = pstr;
            old_str = old_str.Trim();
            while (old_str.Length < strlen)
            {
                old_str = "0" + old_str;
            }
            return old_str;
        }

        /// <summary>
        /// 平方米转换为亩
        /// </summary>
        /// <param name="M2"></param>
        /// <returns></returns>
        public static double M2ToMeng(double M2)
        {
            double Meng = 0;
            try
            {
                Meng = M2 * 0.0015;
            }
            catch
            {
                Meng = 0;
            }
            return Meng;
        }
        public static double M2ToHectare(double M2)
        {
            double Hectare = 0;
            try
            {
                Hectare = M2 * 0.0001;
            }
            catch
            {
                Hectare = 0;
            }
            return Hectare;
        }

        /// <summary>
        /// //面积单位转换 
        /// </summary>
        /// <param name="oldName">原面积单位</param>
        /// <param name="newName">新面积单位</param>
        /// <param name="oldValue">原值</param>
        /// <returns>新值</returns>
        public static double AreaUnitConvert(string oldName, string newName, double oldValue)
        {
            double valueMeter = 0;
            double newValue = 0;
            switch (oldName)
            {
                case "公顷":
                    valueMeter = oldValue * 10000;
                    break;
                case "亩":
                    valueMeter = oldValue / 15 * 10000;
                    break;
                case "平方米":
                    valueMeter = oldValue;
                    break;
            }
            switch (newName)
            {
                case "公顷":
                    newValue = valueMeter / 10000;
                    break;
                case "亩":
                    newValue = valueMeter / 10000 * 15;
                    break;
                case "平方米":
                    newValue = valueMeter;
                    break;
            }
            return newValue;
        }

        public static void ReadDirectoryAll(TreeView tempTV, string RootDirPath, string TextPath, string NodeKey, WIN32_FIND_DATA lpFindFileData)
        {
            long hFindFile = 0;
            long hSucced = 0;
            string DirNames = "";
            string tempDirName = "";
            string tempDirFilePath = "";
            hFindFile = Kernel32.FindFirstFile(RootDirPath, lpFindFileData);
            try
            {
                if (hFindFile != -1)
                {
                    // '--获取到.根目录
                    do
                    {
                        tempDirName = "";
                        //tempDirName = left$(lpFindFileData.cFileName, InStr(1, lpFindFileData.cFileName, vbNullChar) - 1)
                        tempDirName = lpFindFileData.cFileName;// InStr(1, lpFindFileData.cFileName, vbNullChar) - 1)

                        if (tempDirName != "." && tempDirName != "..")
                        {
                            if (lpFindFileData.dwFileAttributes == Kernel32.FILE_ATTRIBUTE_DIRECTORY)
                            {//
                                //'--添加目录
                                DirNames = tempDirName;
                                //'--tv.Nodes.Add(Relative,Relationship,Key,Text,Image,SelectedImage)
                                //'--KEY="TYPE◆filepath◆Name"
                                tempDirFilePath = TextPath + tempDirName + "\\";
                                TreeNode Temp_Node = new TreeNode();
                                Temp_Node.Tag = "DIR◆" + tempDirFilePath + "◆" + tempDirName;
                                Temp_Node.Text = tempDirName;
                                //Temp_Node.ImageIndex = 0;
                                tempTV.Nodes.Add(Temp_Node);
                                //Set Temp_Node = tempTV.Nodes.Add(NodeKey, tvwChild, "DIR" & "◆" & tempDirFilePath & "◆" & tempDirName, tempDirName, 1, 2)

                                //'--调用递归算法
                                //ReadDirectoryAll(tempTV, tempDirFilePath + "*.*", tempDirFilePath, Temp_Node.Key, lpFindFileData);
                            }
                            else
                            {
                                //'--添加文件
                                DirNames = tempDirName;
                                tempDirFilePath = TextPath + tempDirName;
                                //Dim Temp_Node1 As Node
                                //Set Temp_Node1 = tempTV.Nodes.Add(NodeKey, tvwChild, "FILE" & "◆" & tempDirFilePath & "◆" & tempDirName, tempDirName, 3, 3)

                            }
                        }
                        lpFindFileData.cFileName = "";
                        hSucced = Kernel32.FindNextFile(hFindFile, lpFindFileData);
                    }
                    while (hSucced == 0);  //'--非零成功
                    //Loop Until (hSucced = 0)  
                }
                Kernel32.FindClose(hFindFile);

            }
            catch
            {
                Kernel32.FindClose(hFindFile);
            }

        }

        /// <summary>
        /// 提示信息对话框
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static DialogResult HintMsgBox(string msg)
        {
            return HintMsgBox(null, msg);
        }
        public static DialogResult HintMsgBox(IWin32Window owner, string msg)
        {
            return MessageBox.Show(owner, msg, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// 终止对话框
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static DialogResult StopMsgBox(string msg)
        {
            return StopMsgBox(null, msg);
        }
        public static DialogResult StopMsgBox(IWin32Window owner, string msg)
        {
            return MessageBox.Show(owner, msg, "终止", MessageBoxButtons.OK, MessageBoxIcon.Stop);
        }

        /// <summary>
        /// 错误对话框
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static DialogResult ErrorMsgBox(string msg)
        {
            return ErrorMsgBox(null, msg);
        }
        public static DialogResult ErrorMsgBox(IWin32Window owner, string msg)
        {
            return MessageBox.Show(owner, msg, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// 警告对话框
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static DialogResult WarningMsgBox(string msg)
        {
            return WarningMsgBox(null, msg);
        }
        public static DialogResult WarningMsgBox(IWin32Window owner, string msg)
        {
            return MessageBox.Show(owner, msg, "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        /// <summary>
        /// 询问对话框
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static DialogResult QuestionMsgBox(string msg)
        {
            return QuestionMsgBox(null, msg);
        }
        public static DialogResult QuestionMsgBox(string msg, MessageBoxDefaultButton button)
        {
            return QuestionMsgBox(null, msg, button);
        }
        public static DialogResult QuestionMsgBox(IWin32Window owner, string msg)
        {
            return QuestionMsgBox(owner, msg, MessageBoxDefaultButton.Button2);
        }
        public static DialogResult QuestionMsgBox(IWin32Window owner, string msg,
            MessageBoxDefaultButton button)
        {
            return MessageBox.Show(owner, msg, "提示", MessageBoxButtons.YesNo,
                MessageBoxIcon.Question, button);
        }

        /// <summary>
        /// 根据表名和DataSet获取表
        /// </summary>
        /// <param name="table_name"></param>
        /// <param name="ds"></param>
        /// <returns></returns>
        public static DataTable GetDataTableByName(string table_name, DataSet ds)
        {
            if (ds != null)
            {
                foreach (DataTable dt in ds.Tables)
                {
                    if (dt.TableName.ToUpper() == table_name.ToUpper())
                        return dt;
                }
            }

            return null;
        }
        /// <summary>
        /// 查询文件
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="filter"></param>
        /// <param name="fullPath"></param>
        /// <returns></returns>
        public static string[] GetFiles(string dir, string[] filter, bool fullPath)
        {
            List<string> files = new List<string>();
            if ((System.IO.Directory.Exists(dir)) && (filter != null))
            {
                foreach (string f in filter)
                {
                    try
                    {
                        string[] strs = System.IO.Directory.GetFiles(dir, f, System.IO.SearchOption.AllDirectories);
                        if (strs != null)
                        {
                            for (int i = 0; i < strs.Length; i++)
                            {
                                if (fullPath)
                                {
                                    files.Add(strs[i]);
                                }
                                else
                                {
                                    int split = strs[i].LastIndexOf('\\');
                                    files.Add(strs[i].Substring(dir.Length, strs[i].Length - dir.Length));
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine(ex.ToString());
                    }
                }
            }
            return files.ToArray();
        }
        public static string[] GetFiles(string dir,bool fullPath)
        {
            List<string> files = new List<string>();
            if ((System.IO.Directory.Exists(dir)))
            {
                try
                {
                    string[] strs = System.IO.Directory.GetFiles(dir);
                    if (strs != null)
                    {
                        for (int i = 0; i < strs.Length; i++)
                        {
                            if (fullPath)
                            {
                                files.Add(strs[i]);
                            }
                            else
                            {
                                int split = strs[i].LastIndexOf('\\');
                                files.Add(strs[i].Substring(dir.Length, strs[i].Length - dir.Length));
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.ToString());
                }
            }
            return files.ToArray();
        }
        public static string[] GetFilesLoopSubDir(string dir, bool fullPath)
        {
            List<string> files = new List<string>();
            if ((System.IO.Directory.Exists(dir)))
            {
                try
                {
                    //获取当前目录下所有文件列表
                    string[] strs = System.IO.Directory.GetFiles(dir);
                    if (strs != null)
                    {
                        for (int i = 0; i < strs.Length; i++)
                        {                            
                            if (fullPath)
                            {
                                files.Add(strs[i]);
                            }
                            else
                            {
                                int split = strs[i].LastIndexOf('\\');
                                files.Add(strs[i].Substring(dir.Length, strs[i].Length - dir.Length));
                            }
                        }
                    }
                    //获取当前目录下所有子目录
                    string[] subdirs = System.IO.Directory.GetDirectories(dir);
                    if (subdirs != null)
                    {
                        foreach (string subdir in subdirs)
                        {
                            string pdirName = subdir;
                            pdirName=pdirName.Replace(dir, "");
                            //开始递归
                            string[] subdir_strs = GetFilesLoopSubDir(subdir, fullPath);
                            if (subdir_strs != null)
                            {
                                foreach (string subf in subdir_strs)
                                {
                                    files.Add(pdirName+""+subf);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.ToString());
                }
            }
            return files.ToArray();
        }
        /// <summary>
        /// 复制整个文件夹和文件夹下的子文件夹和子文件
        /// </summary>
        /// <param name="sourceFolder"></param>
        /// <param name="destFolder"></param>
        /// <param name="overwrite"></param>
        /// <param name="fileAttribute"></param>
        public static void CopyFolder(string sourceFolder, string destFolder, bool overwrite, FileAttributes fileAttribute)
        {
            if (!Directory.Exists(destFolder)) Directory.CreateDirectory(destFolder);

            string[] files = Directory.GetFiles(sourceFolder);
            foreach (string sourceFile in files)
            {
                string destFile = destFolder + "\\" + Path.GetFileName(sourceFile);
                File.Copy(sourceFile, destFile, overwrite);
                File.SetAttributes(destFile, fileAttribute);
            }

            string[] folders = Directory.GetDirectories(sourceFolder);
            foreach (string sourceDir in folders)
            {
                string[] strArray = sourceDir.Split('\\');
                if (strArray.Length > 1)
                {
                    CopyFolder(sourceDir, destFolder + "\\" + strArray[strArray.Length - 1], overwrite);
                }
            }
        }
        /// <summary>
        /// 拷贝文件夹 功能
        /// </summary>
        /// <param name="sourceFolder"></param>
        /// <param name="destFolder"></param>
        /// <param name="overwrite"></param>
        public static void CopyFolder(string sourceFolder, string destFolder, bool overwrite)
        {
            CopyFolder(sourceFolder, destFolder, overwrite, FileAttributes.Normal);
        }

        /// <summary>
        /// //--主要用于Excel文件的导出之用
        /// //--vp:hsg
        /// //--date:2005-07-06
        /// //--根据ASC码转换为英文字母的通用算法
        /// //--从A-Z ->AA-AZ ->ZA-ZZ
        /// </summary>
        /// <param name="ascAZ"></param>
        /// <returns></returns>
        public static string GetAZ(int ascAZ)
        {
            string rbc = "";
            //--把ASC码值转化为字母 如：65->A; 90->Z; 91->AA; 92->AB
            //ascAZ = 65 + mshgrid.Cols - 2
            int blss = 0;
            int ys = 0;
            blss = (int)((ascAZ - 65) / 26);
            ys = (ascAZ - 65) % 26;      //余数=(ascAZ - 65) Mod 26
            if (blss == 0)
            {
                //rbc=Chr(ascAZ)
                char ch = (char)ascAZ;
                rbc = ch.ToString();
            }
            else
            {
                //rbc = Chr(blss + 64) & Chr(ascAZ - 26 * blss);
                char b_char = (char)(blss + 64);            //双位前
                char a_char = (char)(ascAZ - 26 * blss);    //双位后
                rbc = b_char.ToString() + a_char.ToString();
            }
            return rbc;
        }

        /// <summary>
        /// 分解字符串为一个数组(以/为分隔符)
        /// </summary>
        /// <param name="bz">被分解字符串</param>
        /// <returns></returns>
        public static List<string> GetListBzByFj(string bz)
        {
            List<string> tmpArray = new List<string>();

            string tmpbz = bz;
            tmpbz = tmpbz.Replace("(", "");
            tmpbz = tmpbz.Replace(")", "");

            string[] tmpbzArray = tmpbz.Split(new char[] { '/' });
            if (tmpbzArray.Length > 0)
            {
                foreach (string tmp in tmpbzArray)
                {
                    tmpArray.Add(tmp);
                }
            }
            return tmpArray;
        }

        /// <summary>
        /// 获取一个新的guid编号
        /// </summary>
        /// <param name="IsAbcHeader">是字母开头的guid编号</param>
        /// <param name="IsUpper">是大写的guid编号</param>
        /// <param name="IsLess">是无短线-的guid编号</param>
        /// <returns></returns>
        public static string GetNewGuid(bool IsAbcHeader, bool IsUpper, bool IsLess)
        {
            string guid = Guid.NewGuid().ToString().ToLower();

            if (IsAbcHeader)
            {
                while (char.IsNumber(guid, 0))
                {
                    guid = Guid.NewGuid().ToString().ToLower();
                }
            }

            if (IsUpper)
            {
                guid = guid.ToUpper();
            }

            if (IsLess)
            {
                guid = guid.Replace("-", "");
            }

            return guid;
        }

        /// <summary>
        /// 打开文件 (启用一个新进程打开一个文件)
        /// </summary>
        public static bool OpenFile(string tmpFilePath)
        {
            bool rbc = false;
            try
            {
                Process p = new Process();
                p.StartInfo.FileName = tmpFilePath;
                p.StartInfo.UseShellExecute = true;
                p.Start();
                rbc = true;
            }
            catch (Exception ee)
            {
                rbc = false;                
            }
            return rbc;
        }
        public static bool DeleteFile(string FilePath)
        {
            bool rbc = false;
            try
            {
                if (System.IO.File.Exists(FilePath) == true)
                {
                    File.Delete(FilePath);
                }
                rbc = true;
            }
            catch (Exception ee)
            {
                rbc = false;
            }
            return rbc;
        }
        //生成随机数函数
        public static string GetRandomInt_StrLen3()
        {
            //产生随机数3位
            int sTmp = GetRandomInt(3);
            string index = GetNormalStrLenN(sTmp, 3);
            return index;
        }
        public static string GetRandomInt_StrLen5()
        {
            //产生随机数5位
            int sTmp = GetRandomInt(5);
            string index = GetNormalStrLenN(sTmp, 5);
            return index;            
        }
        public static string GetRandomInt_StrLenN(int N)
        {
            //产生随机数N位
            int sTmp = GetRandomInt(N);
            string index = GetNormalStrLenN(sTmp, N);            
            return index;
        }
        public static int GetRandomInt(int intLength)
        {
            //产生随机数N位
            int MaxVal = 9;            
            if (intLength <= 0) intLength = 1;
            string mv = "";
            for (int i = 0; i < intLength; i++)
            {
                mv += "9";
            }
            MaxVal = TInt(mv);
            int sTmp = GetRandomInt(0, MaxVal);
            return sTmp;
        }
        public static int GetRandomInt(int MinValue,int MaxValue)
        {
            Random rdm1 = new Random(unchecked((int)DateTime.Now.Ticks));
            int sTmp = rdm1.Next(MinValue, MaxValue);
            return sTmp;
        }
        public static string GetNormalStrLenN(int RandomInt,int N)
        {
            string sTmp = RandomInt.ToString();
            string temp = "";
            if (sTmp.Length < N)
            {
                //小于N位时前面补0的个数 如12->00012
                for (int i = 0; i < N - sTmp.Length; i++)
                {
                    temp = temp + "0";
                }
            }
            string index = temp + sTmp;
            return index;
        }
        //释放DataTable内存对象
        public static void Release_dt(DataTable rel_dt)
        {
            if (rel_dt != null)
            {
                rel_dt.Clear();
                rel_dt.Dispose();
                rel_dt = null;
            }
        }
        //钱钱数转中文大写
        public static string MoneyNumberCn(double ANumber)
        {
            const string cPointCn = "点拾佰仟万拾佰仟亿拾佰仟";
            const string cNumberCn = "零壹贰叁肆伍陆柒捌玖";
            string S = ANumber.ToString();
            if (S == "0") return "" + cPointCn[0];
            if (!S.Contains(".")) S += ".";
            int P = S.IndexOf(".");
            string Result = "";
            for (int i = 0; i < S.Length; i++)
            {
                if (P == i)
                {
                    Result = Result.Replace("零拾零", "零");
                    Result = Result.Replace("零百零", "零");
                    Result = Result.Replace("零仟零", "零");
                    Result = Result.Replace("零拾", "零");
                    Result = Result.Replace("零佰", "零");
                    Result = Result.Replace("零仟", "零");
                    Result = Result.Replace("零万", "万");
                    Result = Result.Replace("零亿", "亿");
                    Result = Result.Replace("亿万", "亿");
                    Result = Result.Replace("零点", "点");
                }
                else
                {
                    if (P > i)
                    {
                        Result += "" + cNumberCn[S[i] - '0'] + cPointCn[P - i - 1];
                    }
                    else
                    {
                        Result += "" + cNumberCn[S[i] - '0'];
                    }
                }
            }
            if (Result.Substring(Result.Length - 1, 1) == "" + cPointCn[0])
            {
                Result = Result.Remove(Result.Length - 1); // 一点-> 一
            }
            if (Result[0] == cPointCn[0])
            {
                Result = cNumberCn[0] + Result; // 点三-> 零点三
            }
            if ((Result.Length > 1) && (Result[1] == cPointCn[1]) &&
                   (Result[0] == cNumberCn[1]))
            {
                Result = Result.Remove(0, 1); // 一十三-> 十三
            }
            Result = Result.Replace("零零", "零");
            return Result;
        }

        //判断身份证号对应人是男的函数
        public static bool IsBoySex(string idCard)
        {
            string sexChar = "1";
            if (idCard == null) idCard = string.Empty;
            if (idCard.Length == 18)
                sexChar = idCard.Substring(16, 1);
            else if (idCard.Length == 15)
                sexChar = idCard.Substring(14, 1);

            switch (sexChar)
            {
                case "1":
                case "3":
                case "5":
                case "7":
                case "9":
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// 根据身份证号获取出生日期
        /// </summary>
        /// <param name="idcard"></param>
        /// <returns></returns>
        public static DateTime GetBirthday(string idcard)
        {
            if (idcard == null) idcard = string.Empty;
            string year = "1900";
            string month = "01";
            string day = "01";
            if (idcard.Length == 18)
            {
                year = idcard.Substring(6, 4);
                month = idcard.Substring(10, 2);
                day = idcard.Substring(12, 2);
            }
            else if (idcard.Length == 15)
            {
                year = "19" + idcard.Substring(6, 2);
                month = idcard.Substring(8, 2);
                day = idcard.Substring(10, 2);
            }

            DateTime birthday = DateTime.MinValue;
            DateTime.TryParse(string.Format("{0}-{1}-{2}", year, month, day), out birthday);
            return birthday;
        }

        //发送邮件函数        
        [SmtpPermission(SecurityAction.Assert)]
        [SecurityPermission(SecurityAction.Assert)]
        public static void SendMail(string to, string from,
            string subject, string body,
            string userName, string password,
            string smtpHost)
        {
            MailAddress addressFrom = new MailAddress(from);
            MailAddress addressTo = new MailAddress(to);
            MailMessage message = new MailMessage(addressFrom, addressTo);
            message.Subject = subject;//设置邮件言主题
            message.IsBodyHtml = true;//设置邮件正文为html格式
            message.Body = body; //设置邮件内容
            SmtpClient client = new SmtpClient(smtpHost);
            //设置发送邮件身份验证方式
            //注意如果发件人地址是abc@def.com,则用户名是abc,而不是abc@def.com
            client.Credentials = new NetworkCredential(userName, password);
            //
            client.Send(message);
        }
    }

    public struct FGuid : IFormattable, IComparable, IComparable<Guid>, IEquatable<Guid>
    {
        private Guid m_Guid;

        public static readonly Guid Empty = Guid.Empty;

        public FGuid(byte[] b)
        {
            m_Guid = new Guid(b);
        }
        public FGuid(string g)
        {
            m_Guid = new Guid(g);
        }
        public FGuid(int a, short b, short c, byte[] d)
        {
            m_Guid = new Guid(a, b, c, d);
        }
        public FGuid(int a, short b, short c, byte d, byte e, byte f, byte g, byte h, byte i, byte j, byte k)
        {
            m_Guid = new Guid(a, b, c, d, e, f, g, h, i, j, k);
        }
        public FGuid(uint a, ushort b, ushort c, byte d, byte e, byte f, byte g, byte h, byte i, byte j, byte k)
        {
            m_Guid = new Guid(a, b, c, d, e, f, g, h, i, j, k);
        }

        #region IFormattable 成员

        public string ToString(string format, IFormatProvider formatProvider)
        {
            return m_Guid.ToString(format, formatProvider);
        }

        #endregion

        #region IComparable 成员

        public int CompareTo(object obj)
        {
            return m_Guid.CompareTo(obj);
        }

        #endregion

        #region IComparable<Guid> 成员

        public int CompareTo(Guid other)
        {
            return m_Guid.CompareTo(other);
        }

        #endregion

        #region IEquatable<Guid> 成员

        public bool Equals(Guid other)
        {
            return m_Guid.Equals(other);
        }

        #endregion

        public override bool Equals(object o)
        {
            return m_Guid.Equals(o);
        }

        public override int GetHashCode()
        {
            return m_Guid.GetHashCode();
        }

        public static Guid NewGuid()
        {
            Guid g = Guid.NewGuid();
            while (char.IsNumber(g.ToString(), 0))
            {
                g = Guid.NewGuid();
            }

            return g;
        }

        public static string NewGuidString(int length)
        {
            string v = NewGuid().ToString().Replace("-", "");
            if (length <= 0)
                return v;
            else if (length > v.Length)
                throw new Exception("参数" + length.ToString() + "超出索引范围。");
            else
                return v.Substring(0, length);
        }

        public byte[] ToByteArray()
        {
            return m_Guid.ToByteArray();
        }

        public override string ToString()
        {
            return m_Guid.ToString();
        }

        public string ToString(string format)
        {
            return m_Guid.ToString(format);
        }
    }

    /// <summary>
    /// 计算时间类 静态类
    /// </summary>
    public static class ComputeTimeClass
    {
        private static DateTime m_Start;

        public static string getHourMinSecondString(int seconds)
        {
            int m_Second = seconds;
            if (m_Second < 0) m_Second = 0;

            //计算时间是否超过60秒或60分
            int m = (int)(m_Second / 60);
            int h = (int)(m / 60);
            //格式化显示字符串
            string s = "";
            if (h > 0)
            {
                s = string.Format("{0}小时{1}分{2}秒", h, m % 60, m_Second % 60);
            }
            else if (m > 0)
            {
                s = string.Format("{0}分{1}秒", m, m_Second % 60);
            }
            else
            {
                s = string.Format("{0}秒", m_Second);
            }
            return s;
        }

        public static void StartRecordTime()
        {
            m_Start = DateTime.Now;
        }
        public static string StopRecordTime()
        {
            DateTime m_Start2 = DateTime.Now;
            //已经用过的时间
            double millSecond = (m_Start2 - m_Start).TotalMilliseconds;
            //if (millSecond < 0)
            int overSecond = (int)(millSecond / 1000);
            return getHourMinSecondString(overSecond);
        }
    }

}
