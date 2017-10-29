using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using ZXing;
using System.Drawing.Drawing2D;

namespace ArcMapByCommon
{
    public partial class frmScan2DRenameJpgFileUI : Form
    {
        public frmScan2DRenameJpgFileUI()
        {
            InitializeComponent();
        }

        private void frmScan2DRenameJpgFileUI_Load(object sender, EventArgs e)
        {

        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void btnDirSelect_Click(object sender, EventArgs e)
        {
            try
            {
                DirectorySelection dirsel = new DirectorySelection(Environment.SpecialFolder.MyComputer, "", false);//("承包地块图层*.shp|*.shp", false);
                if (dirsel.IsDirectorySelected() == true)
                {
                    this.textBox1.Text = dirsel.GetSelectedPath;
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "提示");
            }
        }
        private void buttonEdit1_Click(object sender, EventArgs e)
        {
            try
            {
                DirectorySelection dirsel = new DirectorySelection(Environment.SpecialFolder.MyComputer, "", false);//("承包地块图层*.shp|*.shp", false);
                if (dirsel.IsDirectorySelected() == true)
                {
                    this.textBox2.Text = dirsel.GetSelectedPath;
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "提示");
            }
        }        
        private int ScanErrorCount = 0;
        //开始扫描识别  功能
        private void btnImport_Click(object sender, EventArgs e)
        {
            ProgressDialog2 pd = null;
            ProgressDialog onePb = null;
            try
            {
                ScanErrorCount = 0;
                //
                string logPath = Application.StartupPath + "\\AppLogError.log";
                if (File.Exists(logPath) == true)
                {
                    CommonClass.DeleteFile(logPath);
                }
                //
                string dirPath = this.textBox1.Text;
                if (System.IO.Directory.Exists(dirPath) == false)
                {
                    MessageBox.Show("请先选择一个县级扫描后保护卡目录", "提示");
                    return;
                }
                //导出目录
                string OutDirPath = this.textBox2.Text;
                if (System.IO.Directory.Exists(dirPath) == false)
                {
                    MessageBox.Show("导出目录不存在，需要先创建！", "提示");
                    return;
                }
                pd = new ProgressDialog2();
                pd.Text = "扫描识别保护卡进度...";
                pd.Minimum1 = 0;
                pd.Maximum1 = 10;
                pd.Value1 = 0;
                pd.Message1 = "正在扫描识别...";
                pd.Show(this);
                Application.DoEvents();
                //
                //获取目录下面的所有乡镇列表
                string[] XiangDirArray = System.IO.Directory.GetDirectories(dirPath);
                if (XiangDirArray != null && XiangDirArray.Length > 0)
                {
                    pd.Maximum1 = XiangDirArray.Length;
                    pd.Value1 = 0;
                    pd.Message1 = "正在扫描识别...";
                    Application.DoEvents();
                    int XiangIndex = 0;
                    foreach (string XiangDir in XiangDirArray)
                    {
                        DirectoryInfo XingDirInfo = new DirectoryInfo(XiangDir);
                        XiangIndex += 1;
                        pd.Value1 = XiangIndex;
                        pd.Message1 = "正在扫描识别...(" + XingDirInfo.Name + ") 第" + pd.Value1 + "个/共" + pd.Maximum1 + "个";
                        Application.DoEvents();
                        //导出目录里创建对应的乡镇目录
                        string outXiangDir = OutDirPath + "\\" + XingDirInfo.Name;
                        if (System.IO.Directory.Exists(outXiangDir) == false)
                        {
                            Directory.CreateDirectory(outXiangDir);
                        }
                        //========================================
                        string[] cunDirArray = System.IO.Directory.GetDirectories(XiangDir);
                        if (cunDirArray != null && cunDirArray.Length > 0)
                        {
                            pd.Maximum2 = cunDirArray.Length;
                            pd.Value2 = 0;
                            pd.Message2 = "正在扫描识别...";
                            Application.DoEvents();
                            int cunIndex = 0;
                            foreach (string cunDir in cunDirArray)
                            {
                                DirectoryInfo cunDirInfo = new DirectoryInfo(cunDir);
                                cunIndex += 1;
                                pd.Value2 = cunIndex;
                                pd.Message2 = "正在扫描识别...(" + cunDirInfo.Name + ") 第" + pd.Value1 + "个/共" + pd.Maximum1 + "个";
                                Application.DoEvents();
                                //导出目录里创建对应的村目录
                                string outCunDir = outXiangDir + "\\" + cunDirInfo.Name;
                                if (System.IO.Directory.Exists(outCunDir) == false)
                                {
                                    Directory.CreateDirectory(outCunDir);
                                }
                                //--------------------------
                                string[] zhuDirArray = System.IO.Directory.GetDirectories(cunDir);
                                if (zhuDirArray != null && zhuDirArray.Length > 0)
                                {   //有组级文件夹
                                    Application.DoEvents();
                                    int zhuIndex = 0;
                                    foreach (string zhuDir in zhuDirArray)
                                    {
                                        DirectoryInfo zhuDirInfo = new DirectoryInfo(zhuDir);
                                        zhuIndex += 1;
                                        //pd.Value2 = zhuIndex;
                                        //pd.Message2 = "正在扫描识别...(" + zhuDirInfo.Name + ") 第" + pd.Value1 + "个/共" + pd.Maximum1 + "个";
                                        Application.DoEvents();
                                        //导出目录里创建对应的村目录
                                        string outZhuDir = outCunDir + "\\" + zhuDirInfo.Name;
                                        if (System.IO.Directory.Exists(outZhuDir) == false)
                                        {
                                            Directory.CreateDirectory(outZhuDir);
                                        }
                                        //@@@@@@@@@@@@@@@@@@@@@@@@@组级内扫描识别
                                        this.StartScanJpgByDir(zhuDir, outZhuDir, onePb);
                                        //@@@@@@@@@@@@@@@@@@@@@@@@@
                                    }
                                }
                                else
                                {   //无组级文件夹(即为村级)
                                    //@@@@@@@@@@@@@@@@@@@@@@@@@村级内扫描识别
                                    this.StartScanJpgByDir(cunDir, outCunDir, onePb);
                                    //@@@@@@@@@@@@@@@@@@@@@@@@@
                                }
                                //--------------------------
                            }
                        }
                        //========================================
                    }
                    if (pd != null)
                    {
                        pd.Close();
                        pd = null;
                    }
                    MessageBox.Show(this, "扫描识别完毕！未识别图片个数：" + ScanErrorCount, "提示");
                }
            }
            catch (Exception ee)
            {
                Log.WriteLine(ee.StackTrace);
                MessageBox.Show(ee.StackTrace, "提示");
            }
            finally
            {
                if (onePb != null)
                {
                    onePb.Close();
                    onePb = null;
                }
                if (pd != null)
                {
                    pd.Close();
                    pd = null;
                }
            }
        }
        private void StartScanJpgByDir(string IncunDir, string outCunDir, ProgressDialog onePb)
        {
            #region 扫描ing
            string[] jpgPathArray = CommonClass.GetFiles(IncunDir, new string[] { "*.jpg" }, true);
            if (jpgPathArray != null && jpgPathArray.Length > 0)
            {
                List<string> bhkJpgList = new List<string>();
                foreach (string jpgPath in jpgPathArray)
                {
                    FileInfo jpgFileInfo = new FileInfo(jpgPath);
                    if (bhkJpgList.Contains(jpgFileInfo.Name) == false)
                    {
                        bhkJpgList.Add(jpgFileInfo.Name);
                    }
                }
                //-------------------
                if (onePb != null)
                {
                    onePb.Close();
                    onePb = null;
                }
                onePb = new ProgressDialog();
                onePb.Maximum = bhkJpgList.Count;
                onePb.Value = 0;
                onePb.Message = "正在扫描识别...";
                onePb.Show(this);
                Application.DoEvents();
                //
                int bhkUnionIndex = 0;
                //开始合并                                        
                foreach (string bhkJpgName in bhkJpgList)
                {
                    //.....
                    bhkUnionIndex += 1;
                    onePb.Value = bhkUnionIndex;
                    onePb.Message = "正在扫描识别...(" + bhkJpgName + ") 第" + bhkUnionIndex + "个/共" + bhkJpgList.Count + "个";
                    Application.DoEvents();
                    //
                    string inbhk_jpgPath = IncunDir + "\\" + bhkJpgName;
                    if (File.Exists(inbhk_jpgPath) == true)
                    {                        
                        //
                        this.ScaningJpg(inbhk_jpgPath, outCunDir);
                    }
                }
                if (onePb != null)
                {
                    onePb.Close();
                    onePb = null;
                }
                //--------------------
            }
            #endregion
        }
        private void ScaningJpg(string inbhk_jpgPath, string outCunDir)
        {
            Bitmap map = null;
            try
            {
                QrFile qfile = new QrFile(inbhk_jpgPath, "");
                map = new Bitmap(inbhk_jpgPath);
                qfile.QrCode = this.ReadQR(map);   //读取二维码值
                map.Dispose();
                map = null;
                if (!string.IsNullOrEmpty(qfile.QrCode))
                {   //已扫描识别
                    string codetype = qfile.QrCode.Length > 3 ? qfile.QrCode.Substring(0, 3).ToLower() : qfile.QrCode.ToLower();
                    string newname = string.Empty;
                    string[] qrarray = qfile.QrCode.Split(',');
                    switch (codetype)
                    {
                        case "bhk"://保护卡
                            {
                                if (qrarray.Length >= 3)
                                {
                                    string xm = qrarray.Length > 1 ? qrarray[1] : "";
                                    string sfzh = qrarray.Length > 2 ? qrarray[2] : "";
                                    sfzh = !string.IsNullOrEmpty(sfzh) ? CryptoUtil.DecryptString(sfzh, "") : "";
                                    newname = string.Format("{0}({1})", xm, sfzh);
                                }
                                else
                                {   //扫描不正确的数据
                                    newname = "";
                                }
                            }
                            break;
                        case "zrs"://责任书
                            newname = qrarray.Length > 1 ? qrarray[1] : "";
                            break;
                    }
                    if (newname.Trim() != "")
                    {
                        string outFileName = Path.Combine(outCunDir, newname + Path.GetExtension(inbhk_jpgPath));
                        if (File.Exists(outFileName) == false)
                        {
                            File.Copy(inbhk_jpgPath, outFileName);
                        }
                    }
                    else
                    {
                        ScanErrorCount += 1;
                        Log.WriteLine(string.Format("文件{0}扫描识别失败", qfile.Path));
                    }
                    //
                }
                else
                {
                    ScanErrorCount += 1;
                    Log.WriteLine(string.Format("文件{0}扫描识别失败", qfile.Path));
                }
            }
            catch (Exception ex)
            {
                Log.WriteLine(ex.StackTrace);
            }
            finally
            {
                if (map != null)
                {
                    map.Dispose();
                    map = null;
                }
            }
        }
        public  string ReadQR(Bitmap bmp)
        {   //标准二维码内容：
            //yzh:01bhk511025001005,习远平,8F636682FCF34......AC3D24C
            string qrcode = "";
            Bitmap nbmp = null;
            Bitmap HalfImg = null;
            try
            {
                BarcodeReader reader = new BarcodeReader();
                reader.Options.CharacterSet = "UTF-8";
                Result result = null;
                result = reader.Decode(bmp); 
                //--------------------
                //取右半边图片
                if (result == null)
                {
                    HalfImg = this.GetRightHalfImage(bmp); //this.GetRightHalfImage(bmp);
                    result = reader.Decode(HalfImg);                    
                }
                //--------------------
                decimal bs = 1;
                while (result == null && bs >= 0.2M)
                {
                    bs -= 0.25M;
                    if (bs <= 0)
                        break;
                    nbmp = ImageZoom(HalfImg, bs);
                    if (nbmp != null)
                    {
                        result = reader.Decode(nbmp);
                        nbmp.Dispose();
                        nbmp = null;
                    }
                }
                if (result == null)
                {
                    bs = 1;
                }
                while (result == null)
                {
                    if (bs < 5)
                    {
                        bs += 0.25M;
                        nbmp = ImageZoom(HalfImg, bs);
                        if (nbmp != null)
                        {
                            result = reader.Decode(nbmp);
                            nbmp.Dispose();
                            nbmp = null;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                //--------------------
                if (result == null)
                {
                    qrcode = "";
                }
                else
                {
                    qrcode = result.Text;
                }
                if (qrcode.Length > 6 && qrcode.IndexOf("yzh:") == 0)
                {
                    qrcode = qrcode.Substring(6, qrcode.Length - 6);
                }
                else
                {   //无前缀的二维码 yzh:01    补充一个字符串"bhk"
                    qrcode = "bhk" + qrcode;                               
                    //Net.Log.Log.WriteLine("二维码值格式有问题：" + qrcode);
                }
                //
            }
            catch (Exception ee)
            {
                qrcode = "bhk";
            }
            finally
            {
                if (nbmp != null)
                {
                    nbmp.Dispose();
                    nbmp = null;
                }                
            }
            return qrcode;
        }
        private Bitmap GetRightHalfImage(Bitmap fullbmp)
        {
            Bitmap newbmp = null;
            Graphics g = null;
            try
            {
                if (fullbmp != null)
                {
                    int hWidth = fullbmp.Width / 2;
                    int hHeight = fullbmp.Height/3;
                    int newHeight = fullbmp.Height - hHeight;
                    newbmp = new Bitmap(hWidth, newHeight);
                    g = Graphics.FromImage(newbmp);
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    g.SmoothingMode = SmoothingMode.HighQuality;
                    g.CompositingQuality = CompositingQuality.HighQuality;
                    //取右半边图片
                    g.DrawImage(fullbmp, new Rectangle(0, 0, newbmp.Width, newHeight), new Rectangle(hWidth, hHeight, hWidth, newHeight), GraphicsUnit.Pixel);
                    //
                    if (g != null)
                    {
                        g.Dispose();
                        g = null;
                    }
                }
            }
            catch (Exception ee)
            {
            }
            finally
            {
                if (g != null)
                {
                    g.Dispose();
                    g = null;
                }
            }
            return newbmp;
        }
        private Bitmap GetRightCenterMiniRectImage(Bitmap fullbmp)
        {
            Bitmap newbmp = null;
            Graphics g = null;
            try
            {
                if (fullbmp != null)
                {
                    int hWidth = fullbmp.Width /2;
                    int hHeight = (int)(fullbmp.Height*0.28);       // /3;
                    int posWidth = fullbmp.Width /2;
                    int posHeight = (int)(fullbmp.Height * 0.44);   // 2 / 5;
                    newbmp = new Bitmap(hWidth, hHeight);
                    g = Graphics.FromImage(newbmp);
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    g.SmoothingMode = SmoothingMode.HighQuality;
                    g.CompositingQuality = CompositingQuality.HighQuality;
                    //取右边图片大约中心小矩形框图片
                    g.DrawImage(fullbmp, new Rectangle(0, 0, hWidth, hHeight), new Rectangle(posWidth, posHeight, hWidth, hHeight), GraphicsUnit.Pixel);
                    //newbmp.Save("d:\\1.jpg");
                    //
                    if (g != null)
                    {
                        g.Dispose();
                        g = null;
                    }
                }
            }
            catch (Exception ee)
            {
            }
            finally
            {
                if (g != null)
                {
                    g.Dispose();
                    g = null;
                }
            }
            return newbmp;
        }
        public  Bitmap ImageZoom(Bitmap bmp, decimal bs)
        {
            Bitmap newbmp = null;
            Graphics g = null;
            try
            {
                newbmp = new Bitmap((int)(bmp.Width * bs), (int)(bmp.Height * bs));
                g = Graphics.FromImage(newbmp);
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.DrawImage(bmp, new Rectangle(0, 0, newbmp.Width, newbmp.Height), new Rectangle(0, 0, bmp.Width, bmp.Height), GraphicsUnit.Pixel);

                if (g != null)
                {
                    g.Dispose();
                    g = null;
                }                
            }
            catch (Exception ee)
            {
            }
            finally
            {
                if (g != null)
                {
                    g.Dispose();
                    g = null;
                }
            }
            return newbmp;
        }
        //打开日志文件 功能
        private void btnOpenErrorLogFile_Click(object sender, EventArgs e)
        {
            try
            {
                string logPath = Application.StartupPath + "\\AppLogError.log";
                if (File.Exists(logPath) == true)
                {
                    CommonClass.OpenFile(logPath);
                }
                else
                {
                    MessageBox.Show("没有错误日志内容，不打开日志文件", "提示");
                }
            }
            catch(Exception ee)
            {
                MessageBox.Show(ee.StackTrace,"提示");
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {
                frmScanPhone2DValue ui = new frmScanPhone2DValue();
                ui.Show(this);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "提示");
            }
        }

        

        
        //
    }
    //
    public class QrFile
    {
        public QrFile(string path, string qrcode)
        {
            Path = path;
            QrCode = qrcode;
        }

        public string Path { get; set; }
        public string QrCode { get; set; }
    }
    //
}
