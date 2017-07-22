using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;

using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;


namespace ArcMapByCommon
{
    /// <summary>
    /// 定义 椭球面积计算LayerList 窗体
    /// </summary>
    partial class frmEllipseAreaLayerListUI : Form
    {
        public frmEllipseAreaLayerListUI()
        {
            InitializeComponent();
            this.HelpRequested += new HelpEventHandler(frmEllipseAreaLayerListUI_HelpRequested);
        }

        void frmEllipseAreaLayerListUI_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            //GISApplication.MainFormEx.OnLineHelpLookSession.OpenHelpForm(this.ToString());
        }

        private void frmEllipseAreaLayerListUI_Load(object sender, EventArgs e)
        {
            //初始化图层
            this.comboBox1.Items.Clear();
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
                    this.comboBox1.Items.Add(itm);
                }
            }
        }

        //选择图层已改变 事件
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //初始化当前图层的列字段名称
            object obj = this.comboBox1.SelectedItem;
            if (obj != null && obj is CommonComboBoxItem)
            {
                CommonComboBoxItem item = obj as CommonComboBoxItem;
                IFeatureClass fc = item.Tag as IFeatureClass;
                ZhFeatureClass zhfeatclass = new ZhPolylineFeatureClass(fc);
                List<string> fieldNameArray = zhfeatclass.GetFieldNames();
                if (fieldNameArray != null && fieldNameArray.Count > 0)
                {
                    this.CB_Fields.Items.Clear();
                    foreach (string fd in fieldNameArray)
                    {
                        this.CB_Fields.Items.Add(fd);
                    }
                }
                this.RefreshSpatialReferenceText(fc);
            }
            //---
        }

        //
        private void RefreshSpatialReferenceText(IFeatureClass pFeatureClass)
        {
            this.textBox3.Text = "";
            IGeoDataset ds = pFeatureClass as IGeoDataset;
            if (ds != null)
            {
                ISpatialReference sr = ds.SpatialReference;
                if (sr != null)
                {
                    //获取空间参考描述信息

                    //原来是ProjectionConversion
                    ProjectionConversionShapeFileClass pc = new ProjectionConversionShapeFileClass();
                    string SRText = pc.getSpatialReferenceString(sr);
                    this.textBox3.Text = SRText;
                    pc = null;
                }
                sr = null;
            }
            ds = null;
        }

        //开始计算
        private void Btn_Compute_Click(object sender, EventArgs e)
        {
            string tpFiledName = this.CB_Fields.Text;

            object obj = this.comboBox1.SelectedItem;
            if (obj != null && obj is CommonComboBoxItem)
            {
                CommonComboBoxItem item = obj as CommonComboBoxItem;
                IFeatureClass pFeatureClass = item.Tag as IFeatureClass;
                decimal ellipseArea = 0;
                AnyTrapeziaEllipseAreaCompute_JF jf = new AnyTrapeziaEllipseAreaCompute_JF();
                //获取坐标系
                jf.Datum = EnumProjectionDatum.CGCS2000;
                if (this.rb2000.Checked == true)
                {
                    jf.Datum = EnumProjectionDatum.CGCS2000;
                }
                if (this.rb_xian80.Checked == true)
                {
                    jf.Datum = EnumProjectionDatum.Xian80;
                }
                if (this.rb_beijing54.Checked == true)
                {
                    jf.Datum = EnumProjectionDatum.Bejing54;
                }
                //获取大数设置
                jf.IsBigNumber = false;
                if (this.CB_IsBigNumber.Checked == true)
                {
                    jf.IsBigNumber = true;
                }
                //获取分度设置
                jf.Strip = EnumStrip.Strip6;
                if (CB_FD.Checked == true)
                {
                    jf.Strip = EnumStrip.Strip3;
                }
                //获取中央子午线
                jf.L0 = CommonClass.TDec(this.txt_DD.Text) * (int)jf.Strip;

                int fIndex = 0;
                int tmpFCount = pFeatureClass.FeatureCount(null);
                frmProgressBar1 pb = new frmProgressBar1();
                pb.Text = "正在进行椭球面积计算...";
                pb.KDCaption1 = "正在进行椭球面积计算...";
                pb.KDProgressBar1.Maximum = tmpFCount + 1;
                pb.KDProgressBar1.Value = 0;
                pb.Show();
                Application.DoEvents();

                IFeatureCursor fcur = pFeatureClass.Update(null, false);
                IFeature feat = fcur.NextFeature();
                while (feat != null)
                {
                    fIndex += 1;
                    if (fIndex % 500 == 0)
                    {
                        pb.KDCaption1 = "已计算完成要素个数:" + fIndex.ToString() + "/总" + tmpFCount.ToString();
                        pb.KDProgressBar1.Value = fIndex;
                        Application.DoEvents();
                        fcur.Flush();
                    }
                    ellipseArea = jf.Compute(feat.ShapeCopy);
                    //save ellipseArea...
                    ZhFeature zhfeat = new ZHFeaturePolygon(feat);
                    zhfeat.setFieldValue(tpFiledName, ellipseArea);                    

                    fcur.UpdateFeature(feat);                    

                    feat = fcur.NextFeature();
                }
                fcur.Flush();

                TokayWorkspace.ComRelease(fcur);
                fcur = null;


                pb.Close();
                pb.Dispose();
                pb = null;

                MessageBox.Show(this, "计算椭球面积完毕!", "提示");
            }
        }

        private void Btn_addEllipField_Click(object sender, EventArgs e)
        {
            object obj = this.comboBox1.SelectedItem;
            if (obj != null && obj is CommonComboBoxItem)
            {
                CommonComboBoxItem item = obj as CommonComboBoxItem;
                IFeatureClass fc = item.Tag as IFeatureClass;

                //
                if (fc.Fields.FindField("TQMJ") < 0)
                {
                    TokayWorkspace.CreateNumberField(fc as ITable, "TQMJ", "椭球面积");
                }
                if (fc.Fields.FindField("TQMJ2000") < 0)
                {
                    TokayWorkspace.CreateNumberField(fc as ITable, "TQMJ2000", "椭球面积2000");
                }
                if (fc.Fields.FindField("TXMJ") < 0)
                {
                    TokayWorkspace.CreateNumberField(fc as ITable, "TXMJ", "图形面积");
                }
            }
            this.comboBox1_SelectedIndexChanged(null, null);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ProgressDialog pd = null;
            try
            {
                CommonComboBoxItem item = this.comboBox1.SelectedItem as CommonComboBoxItem;
                if (item == null)
                {
                    MessageBox.Show("请先选择要计算的图层", "提示");
                    return;
                }
                string fd_item = this.CB_Fields.Text;
                if (fd_item=="")
                {
                    MessageBox.Show("请先选择要计算的字段", "提示");
                    return;
                }
                //ILayer layer = item.Tag as ILayer;
                //IFeatureLayer featLayer = layer as IFeatureLayer;
                IFeatureClass fc = item.Tag as IFeatureClass;
                //                
                string fdName = this.CB_Fields.Text;
                //
                int fCount = fc.FeatureCount(null);
                if (fCount > 0)
                {
                    IFeatureCursor pcursor = fc.Update(null, false);
                    IFeature pf = pcursor.NextFeature();
                    int index = fc.FindField(fdName);
                    pd = new ProgressDialog();
                    pd.Text = "进度";
                    pd.Message = "计算字段图形面积中......";
                    pd.Minimum = 0;
                    pd.Maximum = fCount;
                    pd.Show(this);
                    //
                    Application.DoEvents();
                    double t_area = 0.0;
                    int n = 0;
                    while (pf != null)
                    {
                        n = n + 1;
                        if (n % 200 == 0)
                        {
                            pd.Value = n;
                            pd.Message = "计算字段图形面积中......" + pd.Value.ToString() + "/" + pd.Maximum.ToString();
                            Application.DoEvents();
                            pcursor.Flush();
                        }
                        IArea area = (pf.Shape as IArea);
                        if (area != null)
                        {
                            t_area = area.Area;
                            pf.set_Value(index, t_area);
                            //
                            pcursor.UpdateFeature(pf);
                        }
                        //
                        pf = pcursor.NextFeature();
                    }
                    pcursor.Flush();
                    if (pcursor != null)
                    {
                        TokayWorkspace.ComRelease(pcursor);
                        pcursor = null;
                    }
                    if (pd != null)
                    {
                        pd.Dispose();
                        pd = null;
                    }
                    MessageBox.Show("计算完毕！", "提示");
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.StackTrace, "提示");
            }
            finally
            {
                if (pd != null)
                {
                    pd.Dispose();
                    pd = null;
                }
            }
        }
        //

    }
}
