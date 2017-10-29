using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace ArcMapByCommon
{
    public partial class frmScanUnionOneJpgUI : Form
    {
        public frmScanUnionOneJpgUI()
        {
            InitializeComponent();
        }

        private void frmScanUnionOneJpgUI_Load(object sender, EventArgs e)
        {

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
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        //扫描识别重命名功能 功能
        private void btnScanJpg_Click(object sender, EventArgs e)
        {
            try
            {
                frmScan2DRenameJpgFileUI ui = new frmScan2DRenameJpgFileUI();
                ui.Show(this);
            }
            catch (Exception ee)
            {
                MessageBox.Show(this, ee.StackTrace, "提示");
            }
        }
        //
        //jpg图片 合并功能  OK
        private void UninJpg(string inBHKjpgPath, string inDKjpgPath, string outBHKjpgPath)
        {            
            string imgFilePath1 = inBHKjpgPath;
            string imgFilePath2 = inDKjpgPath;
            //
            System.Drawing.Image img1 = null;
            System.Drawing.Image img2 = null;
            //
            Bitmap newImg = null;
            Graphics g = null;
            try
            {
                img1 = System.Drawing.Image.FromFile(imgFilePath1);
                img2 = System.Drawing.Image.FromFile(imgFilePath2);
                //计算合并后图片宽度和高度
                //获取最小宽度
                int MaxWidth = img1.Width;
                int UWidth = img1.Width;
                if (img1.Width < img2.Width)
                {
                    UWidth = img1.Width;
                    MaxWidth = img2.Width;
                }
                else
                {
                    UWidth = img2.Width;
                    MaxWidth = img1.Width;
                }
                double blxs = UWidth * 1.0 / MaxWidth;
                int MaxHeight = img1.Height;
                if (img1.Height < img2.Height)
                {
                    MaxHeight = img2.Height;
                }
                int OneHeight = (int)Math.Round(MaxHeight * blxs);
                int UHeight = OneHeight * 2;  //img1.Height + img2.Height;
                //
                newImg = new Bitmap(UWidth, UHeight);
                g = Graphics.FromImage(newImg);
                g.Clear(Color.White);
                //
                int posX = 0;
                int posY = 0;
                //画保护卡图片
                posX = 0;
                posY = 0;
                g.DrawImage(img1, posX, posY, UWidth, OneHeight);
                //画位置示意图图片
                posX = 0;
                posY = OneHeight;
                g.DrawImage(img2, posX, posY, UWidth, OneHeight);
                //
                if (img1 != null)
                {
                    img1.Dispose();
                    img1 = null;
                }
                if (img2 != null)
                {
                    img2.Dispose();
                    img2 = null;
                }
                newImg.Save(outBHKjpgPath);
                if (g != null)
                {
                    g.Dispose();
                    g = null;
                }
                if (newImg != null)
                {
                    newImg.Dispose();
                    newImg = null;
                }
            }
            catch (Exception ee)
            {
                Log.WriteLine(ee.StackTrace);
            }
            finally
            {
                if (img1 != null)
                {
                    img1.Dispose();
                    img1 = null;
                }
                if (img2 != null)
                {
                    img2.Dispose();
                    img2 = null;
                }
                if (g != null)
                {
                    g.Dispose();
                    g = null;
                }
                if (newImg != null)
                {
                    newImg.Dispose();
                    newImg = null;
                }
            }
        }
        private void StartUnionJpgByDir(string IncunDir,string outCunDir,ProgressDialog onePb)
        {
            #region 合并ing
            string[] jpgPathArray = CommonClass.GetFiles(IncunDir, new string[] { "*.jpg" }, true);
            if (jpgPathArray != null && jpgPathArray.Length > 0)
            {
                List<string> dkJpgList = new List<string>();
                List<string> bhkJpgList = new List<string>();
                foreach (string jpgPath in jpgPathArray)
                {
                    FileInfo jpgFileInfo = new FileInfo(jpgPath);
                    if (jpgFileInfo.Name.ToLower().IndexOf("位置示意图.jpg") == -1)
                    {   //分离出保护卡.jpg
                        if (bhkJpgList.Contains(jpgFileInfo.Name) == false)
                        {
                            bhkJpgList.Add(jpgFileInfo.Name);
                        }
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
                onePb.Message = "正在合并...";
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
                    onePb.Message = "正在合并...(" + bhkJpgName + ") 第" + bhkUnionIndex + "个/共" + bhkJpgList.Count + "个";
                    Application.DoEvents();
                    //.....
                    string temp_dkjpg = bhkJpgName.Replace(".jpg", "位置示意图.jpg");
                    string temp_dkjpgPath = IncunDir + "\\" + temp_dkjpg;
                    if (File.Exists(temp_dkjpgPath) == true)
                    {
                        string outbhkjpgPath = outCunDir + "\\" + bhkJpgName;
                        //
                        string inbhk_jpgPath = IncunDir + "\\" + bhkJpgName;
                        string indk_jpgPath = IncunDir + "\\" + temp_dkjpg;
                        //
                        this.UninJpg(inbhk_jpgPath, indk_jpgPath, outbhkjpgPath);
                    }
                    else
                    {
                        Log.WriteLine("无位置示意图未合并操作：" + temp_dkjpgPath);
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
        //开始合并 功能
        private void btnImport_Click(object sender, EventArgs e)
        {
            ProgressDialog2 pd = null;
            ProgressDialog onePb = null;
            try
            {
                string dirPath = this.textBox1.Text;
                if (System.IO.Directory.Exists(dirPath) == false)
                {
                    MessageBox.Show("请先选择一个县级保护卡目录", "提示");
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
                pd.Text = "扫描合并保护卡进度...";
                pd.Minimum1 = 0;
                pd.Maximum1 = 10;
                pd.Value1 = 0;
                pd.Message1 = "正在扫描合并...";
                pd.Show(this);
                Application.DoEvents();
                //
                //获取目录下面的所有乡镇列表
                string[] XiangDirArray = System.IO.Directory.GetDirectories(dirPath);
                if (XiangDirArray != null && XiangDirArray.Length > 0)
                {
                    pd.Maximum1 = XiangDirArray.Length;
                    pd.Value1 = 0;
                    pd.Message1 = "正在扫描合并...";
                    Application.DoEvents();
                    int XiangIndex = 0;
                    foreach (string XiangDir in XiangDirArray)
                    {
                        DirectoryInfo XingDirInfo = new DirectoryInfo(XiangDir);
                        XiangIndex += 1;
                        pd.Value1 = XiangIndex;
                        pd.Message1 = "正在扫描合并...(" + XingDirInfo.Name + ") 第" + pd.Value1 + "个/共" + pd.Maximum1 + "个";
                        Application.DoEvents();  
                        //导出目录里创建对应的乡镇目录
                        string outXiangDir = OutDirPath +"\\"+ XingDirInfo.Name;
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
                            pd.Message2 = "正在扫描合并...";
                            Application.DoEvents();
                            int cunIndex = 0;
                            foreach (string cunDir in cunDirArray)
                            {
                                DirectoryInfo cunDirInfo = new DirectoryInfo(cunDir);
                                cunIndex += 1;
                                pd.Value2 = cunIndex;
                                pd.Message2 = "正在扫描合并...(" + cunDirInfo.Name + ") 第" + pd.Value1 + "个/共" + pd.Maximum1 + "个";
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
                                        //pd.Message2 = "正在扫描合并...(" + zhuDirInfo.Name + ") 第" + pd.Value1 + "个/共" + pd.Maximum1 + "个";
                                        Application.DoEvents();
                                        //导出目录里创建对应的村目录
                                        string outZhuDir = outCunDir + "\\" + zhuDirInfo.Name;
                                        if (System.IO.Directory.Exists(outZhuDir) == false)
                                        {
                                            Directory.CreateDirectory(outZhuDir);
                                        }
                                        //@@@@@@@@@@@@@@@@@@@@@@@@@组级内合并
                                        this.StartUnionJpgByDir(zhuDir, outZhuDir, onePb);
                                        //@@@@@@@@@@@@@@@@@@@@@@@@@
                                    }
                                }
                                else
                                {   //无组级文件夹(即为村级)
                                    //@@@@@@@@@@@@@@@@@@@@@@@@@村级内合并
                                    this.StartUnionJpgByDir(cunDir, outCunDir, onePb);
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
                    MessageBox.Show(this,"合并完毕！", "提示");
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

        

        
        //
    }
}
