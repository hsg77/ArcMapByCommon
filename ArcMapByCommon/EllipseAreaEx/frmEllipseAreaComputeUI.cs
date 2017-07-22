using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.Geometry;

namespace ArcMapByCommon
{
    /// <summary>
    /// 定义计算椭球面积工具窗体
    /// </summary>
    public partial class frmEllipseAreaComputeUI : Form
    {
        public frmEllipseAreaComputeUI()
        {
            InitializeComponent();
        }

        ////选择源投影文件
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                //select Grid File
                OpenFileDialog of = new OpenFileDialog();
                of.Title = "选择Shape文件";
                of.Filter = "Shape文件(*.shp)|*.shp";
                of.ShowDialog();
                string tpFileName = of.FileName;
                if (tpFileName != null && tpFileName != "")
                {
                    this.textBox1.Text = tpFileName;
                    this.RefreshSpatialReferenceText(tpFileName);
                }
                of = null;
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "提示");
            }
        }

        //----
        private void RefreshSpatialReferenceText(string file)
        {
            this.textBox3.Text = "";
            try
            {
                //首先获取文件名，文件路径及文件类型

                string[] sArray = file.Split('\\');
                string filename = sArray[sArray.Length - 1];
                string url = "";
                for (int i = 0; i < sArray.Length - 1; i++)
                {
                    url = url + sArray[i] + "\\";
                }
                string[] sFile = filename.Split('.');
                string file_ext = "";
                if (sFile.Length > 0)
                    file_ext = sFile[sFile.Length - 1];
                file_ext = file_ext.ToLower();
                //----
                ShapefileWorkspaceFactoryClass wsf = new ShapefileWorkspaceFactoryClass();
                IWorkspace ws = wsf.OpenFromFile(url, 0);

                IFeatureWorkspace pFeatureWorkspace;
                pFeatureWorkspace = (IFeatureWorkspace)ws;

                IFeatureClass pFeatureClass = null;
                pFeatureClass = pFeatureWorkspace.OpenFeatureClass(filename);

                this.RefreshSpatialReferenceText(pFeatureClass);

                ZhFeatureClass zhfeaclass = new ZhPolygonFeatureClass(pFeatureClass);
                List<string> FieldNames = zhfeaclass.GetFieldNames();
                this.CB_Fields.Items.Clear();
                foreach (string fn in FieldNames)
                {
                    this.CB_Fields.Items.Add(fn);
                }

                TokayWorkspace.ComRelease(pFeatureClass);
                pFeatureClass = null;

                TokayWorkspace.ComRelease(ws);
                ws = null;
                wsf = null;
            }
            catch (Exception ee)
            {
                this.textBox3.Text = "";
                MessageBox.Show(ee.Message, "选择文件提示");
            }
        }

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
                    ProjectionConversionClass pc = new ProjectionConversionShapeFileClass();
                    string SRText = pc.getSpatialReferenceString(sr);
                    this.textBox3.Text = SRText;
                    pc = null;
                }
                sr = null;
            }
            ds = null;
        }

        //关闭
        private void Btn_Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        //开始计算
        private void Btn_Compute_Click(object sender, EventArgs e)
        {
            try
            {
                this.Btn_Compute.Enabled = false;
                this.Cursor = Cursors.WaitCursor;

                string tpFileName = this.textBox1.Text;
                if (tpFileName == "" || System.IO.File.Exists(tpFileName) == false)
                {
                    MessageBox.Show("需先选择一个shp文件", "提示");
                    return;
                }

                string tpFiledName = this.CB_Fields.Text;

                string[] sArray = tpFileName.Split('\\');
                string filename = sArray[sArray.Length - 1];
                string url = "";
                for (int i = 0; i < sArray.Length - 1; i++)
                {
                    url = url + sArray[i] + "\\";
                }
                string[] sFile = filename.Split('.');
                string file_ext = "";
                if (sFile.Length > 0)
                    file_ext = sFile[sFile.Length - 1];
                file_ext = file_ext.ToLower();
                //----
                ShapefileWorkspaceFactoryClass wsf = new ShapefileWorkspaceFactoryClass();
                IWorkspace ws = wsf.OpenFromFile(url, 0);

                IFeatureWorkspace pFeatureWorkspace;
                pFeatureWorkspace = (IFeatureWorkspace)ws;

                IFeatureClass pFeatureClass = null;
                pFeatureClass = pFeatureWorkspace.OpenFeatureClass(filename);

                decimal ellipseArea = 0;
                AnyTrapeziaEllipseAreaCompute_JF jf = new AnyTrapeziaEllipseAreaCompute_JF();
                jf.Datum = EnumProjectionDatum.Bejing54;
                if (CB_Datum.Checked == true)
                {
                    jf.Datum = EnumProjectionDatum.Xian80;
                }
                jf.IsBigNumber = false;
                if (this.CB_IsBigNumber.Checked == true)
                {
                    jf.IsBigNumber = true;
                }
                jf.Strip = EnumStrip.Strip6;
                if (CB_FD.Checked == true)
                {
                    jf.Strip = EnumStrip.Strip3;
                }

                jf.L0 = CommonClass.TDec(this.txt_DD.Text) * (int)jf.Strip;

                int fIndex = 0;
                int tmpFCount = pFeatureClass.FeatureCount(null);
                frmProgressBar1 pb = new frmProgressBar1();
                pb.Text = "正在进行椭球面积计算...";
                pb.Caption1.Text = "正在进行椭球面积计算...";
                pb.progressBar1.Maximum = tmpFCount + 1;
                pb.progressBar1.Value = 0;
                pb.Show();
                Application.DoEvents();

                IFeatureCursor fcur = pFeatureClass.Update(null, false);
                IFeature feat = fcur.NextFeature();
                while (feat != null)
                {
                    fIndex += 1;
                    if (fIndex % 500 == 0)
                    {
                        pb.Caption1.Text = "已计算完成要素个数:" + fIndex.ToString() + "/总" + tmpFCount.ToString();
                        pb.progressBar1.Value = fIndex;
                        Application.DoEvents();
                        fcur.Flush();
                    }
                    ellipseArea = jf.Compute(feat.ShapeCopy);
                    //save ellipseArea...
                    ZhFeature zhfeat = new ZHFeaturePolygon(feat);
                    zhfeat.setFieldValue(tpFiledName, ellipseArea);
                    zhfeat.setFieldValue("TXMJ", zhfeat.GeometryArea);

                    fcur.UpdateFeature(feat);
                    //zhfeat.SaveFeature();

                    feat = fcur.NextFeature();
                }
                fcur.Flush();

                TokayWorkspace.ComRelease(fcur);
                fcur = null;

                TokayWorkspace.ComRelease(ws);
                ws = null;

                pb.Close();
                pb.Dispose();
                pb = null;

                MessageBox.Show(this, "计算椭球面积完毕!", "提示");
            }
            catch (Exception ee)
            {
                Log.WriteLine(ee);
                MessageBox.Show(this, ee.Message, "提示");
            }
            finally
            {
                this.Btn_Compute.Enabled = true;
                this.Cursor = Cursors.Default;
            }

        }

        //添加椭球面积字段
        private void Btn_addEllipField_Click(object sender, EventArgs e)
        {
            try
            {
                string tpFileName = this.textBox1.Text;
                LocalShapeFileOperator shpOp = new LocalShapeFileOperator();
                shpOp.LocalShapePathFileName = tpFileName;
                IFeatureClass fc = shpOp.getIFeatureClass();
                shpOp.AddCustomAreaField(fc, "TQMJ", "椭球面积");
                shpOp.AddCustomAreaField(fc, "TXMJ", "图形面积");
                shpOp.Dispose();
                TokayWorkspace.ComRelease(fc);
                fc = null;

                MessageBox.Show("添加椭球面积(TQMJ)字段成功!", "提示");
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "提示");
            }
        }

        //开始经纬度计算椭球面积
        private void Btn_JWDComputeElliArea_Click(object sender, EventArgs e)
        {
            try
            {
                this.Btn_JWDComputeElliArea.Enabled = false;
                this.Cursor = Cursors.WaitCursor;

                string tpFileName = this.textBox1.Text;
                if (tpFileName == "" || System.IO.File.Exists(tpFileName) == false)
                {
                    MessageBox.Show("需先选择一个shp文件", "提示");
                    return;
                }

                string tpFiledName = this.CB_Fields.Text;

                string[] sArray = tpFileName.Split('\\');
                string filename = sArray[sArray.Length - 1];
                string url = "";
                for (int i = 0; i < sArray.Length - 1; i++)
                {
                    url = url + sArray[i] + "\\";
                }
                string[] sFile = filename.Split('.');
                string file_ext = "";
                if (sFile.Length > 0)
                    file_ext = sFile[sFile.Length - 1];
                file_ext = file_ext.ToLower();
                //----
                ShapefileWorkspaceFactoryClass wsf = new ShapefileWorkspaceFactoryClass();
                IWorkspace ws = wsf.OpenFromFile(url, 0);

                IFeatureWorkspace pFeatureWorkspace;
                pFeatureWorkspace = (IFeatureWorkspace)ws;

                IFeatureClass pFeatureClass = null;
                pFeatureClass = pFeatureWorkspace.OpenFeatureClass(filename);

                decimal ellipseArea = 0;
                AnyTrapeziaEllipseAreaCompute_JF_WGS1984 jf = new AnyTrapeziaEllipseAreaCompute_JF_WGS1984();

                IFeatureCursor fcur = pFeatureClass.Search(null, false);
                IFeature feat = fcur.NextFeature();
                while (feat != null)
                {
                    ellipseArea = jf.Compute(feat.ShapeCopy);
                    //save ellipseArea...
                    ZhFeature zhfeat = new ZHFeaturePolygon(feat);
                    zhfeat.setFieldValue(tpFiledName, ellipseArea);
                    zhfeat.setFieldValue("TXMJ", zhfeat.GeometryArea);
                    zhfeat.SaveFeature();

                    feat = fcur.NextFeature();
                }
                TokayWorkspace.ComRelease(fcur);
                fcur = null;

                TokayWorkspace.ComRelease(ws);
                ws = null;

                MessageBox.Show(this, "计算椭球面积完毕!", "提示");
            }
            catch (Exception ee)
            {
                Log.WriteLine(ee);
                MessageBox.Show(this, ee.Message, "提示");
            }
            finally
            {
                this.Btn_JWDComputeElliArea.Enabled = true;
                this.Cursor = Cursors.Default;
            }
        }


    }
}
