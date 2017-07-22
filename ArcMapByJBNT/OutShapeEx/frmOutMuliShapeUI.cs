using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;


namespace ArcMapByJBNT
{
    /// <summary>
    /// 定义 输出多个shape文件窗体功能
    /// </summary>
    public partial class frmOutMuliShapeUI : Form
    {        
        private frmProgressBar1 pb = null;

        public frmOutMuliShapeUI()
        {
            InitializeComponent();
        }

        //关闭事件
        private void Btn_Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //Load 事件
        private void frmOutMuliShapeUI_Load(object sender, EventArgs e)
        {
            this.checkedListBox1.Items.Clear();
            IFeatureClass fc = null;
            ZhMapClass zhmap = new ZhMapClass(ArcMap.Document.FocusMap);
            IFeatureClass[] fcArray = zhmap.GetFeatureClassArray();
            for (int i = 0; i < fcArray.Length; i++)
            {
                fc = fcArray[i];
                if (fc != null)
                {
                    CommonComboBoxItem itm = new CommonComboBoxItem();
                    itm.Text = fc.AliasName;
                    itm.Tag = fc;
                    this.checkedListBox1.Items.Add(itm);
                }
            }
        }

        //全选/全不选事件
        private void CB_CheckALL_CheckedChanged(object sender, EventArgs e)
        {
            bool checkall = CB_CheckALL.Checked;
            for (int i = 0; i < this.checkedListBox1.Items.Count; i++)
            {
                checkedListBox1.SetItemChecked(i, checkall);
            }

        }

        //输出shape功能
        private void Btn_OutPutShape_Click(object sender, EventArgs e)
        {
            try
            {
                this.Btn_OutPutShape.Enabled = false;
                this.Cursor = Cursors.WaitCursor;

                //获取选中的要素类
                List<IFeatureClass> fcList = new List<IFeatureClass>();
                for (int i = 0; i < this.checkedListBox1.Items.Count; i++)
                {
                    if (this.checkedListBox1.GetItemChecked(i) == true)
                    {   //选中要素类
                        CommonComboBoxItem cbitem = this.checkedListBox1.Items[i] as CommonComboBoxItem;
                        fcList.Add(cbitem.Tag as IFeatureClass);
                    }
                }
                //获取要输出的目录:
                string outShapeFilePath = "";
                string outShapeDir = "";

                outShapeDir = this.textBox1.Text;

                if (fcList != null && fcList.Count > 0)
                {
                    pb = new frmProgressBar1();
                    pb.Text = "输出多shape文件功能";
                    pb.Caption1.Text = "输出shape进度...";
                    pb.progressBar1.Maximum = fcList.Count;
                    pb.progressBar1.Value = 0;
                    pb.Show(this);
                    Application.DoEvents();

                    string tmpLayerName = "";
                    IFeatureClass fc = null;
                    OutShapeClass shpOp = new OutShapeClass();
                    for (int i = 0; i < fcList.Count; i++)
                    {
                        fc = fcList[i];
                        if (fc is IDataset)
                        {   //英文名
                            tmpLayerName = (fc as IDataset).Name;
                        }
                        else
                        {   //中文名
                            tmpLayerName = fc.AliasName;
                        }
                        outShapeFilePath = outShapeDir + "\\" + tmpLayerName + ".shp";
                        pb.Caption1.Text = "正在输出要素类:" + tmpLayerName+"("+fc.AliasName+")";
                        if (pb.progressBar1.Value <= pb.progressBar1.Maximum)
                        {
                            pb.progressBar1.Value += 1;
                        }
                        Application.DoEvents();
                        //对整表要素
                        shpOp.OutPut(fc, outShapeFilePath);
                    }
                    MessageBox.Show("输出完成!", "提示");
                }
            }
            catch (Exception ee)
            {
                Log.WriteLine(ee);
                MessageBox.Show(ee.Message, "提示");
            }
            finally
            {
                this.Btn_OutPutShape.Enabled = true;
                this.Cursor = Cursors.Default;
                if (pb != null)
                {
                    pb.Close();
                    pb.Dispose();
                    pb = null;
                }
            }
        }

        //选择 另存到目录内为：
        private void Btn_SaveAsShape_Click(object sender, EventArgs e)
        {
            try
            {
                DirectorySelection dirSel = new DirectorySelection(Environment.SpecialFolder.Desktop, "", true);
                if (dirSel.IsDirectorySelected() == true)
                {
                    string dirPath = dirSel.GetSelectedPath;
                    this.textBox1.Text = dirPath;

                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "提示");
            }
        }

    }
}
