using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;

using System.IO;
using ESRI.ArcGIS.Geometry;
using GIS.ArcGIS;

namespace ArcMapByCommon
{
    /// <summary>
    /// 功能：空间叠加传值功能
    /// vp:hsg
    /// create date:2012
    /// modify date:2012-08-27
    /// </summary>
    public partial class frmUpdateFeatureByGeoSpatialOverlapUI : Form
    {
        private string tmpMdbFilePath = "";
        private string tmpLayerName = "";
                
        public frmUpdateFeatureByGeoSpatialOverlapUI()
        {
            InitializeComponent();
        }

        //load 事件
        private void frmUpdateFeatureByGeoSpatialOverlapUI_Load(object sender, EventArgs e)
        {
            //初始化图层
            this.CB_LayerList1.Items.Clear();
            this.CB_LayerList2.Items.Clear();
            IFeatureClass fc = null;
            //
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
                    this.CB_LayerList1.Items.Add(itm);
                    this.CB_LayerList2.Items.Add(itm);
                }
            }
        }

        private void GetFields(IFeatureClass fc)
        {
            ZhFeatureClass zhfeatclass = new ZhPolylineFeatureClass(fc);
            
            //获取分组字段集合
            List<string> fieldNameArray = zhfeatclass.GetFieldNames();
            if (fieldNameArray != null && fieldNameArray.Count > 0)
            {                
                this.CB_Fields2.Items.Clear();                
                foreach (string fd in fieldNameArray)
                {
                    this.CB_Fields2.Items.Add(fd);
                }
                this.CB_Fields2.Sorted = true;
            }
        }

        private void Btn_PreHandle_Click(object sender, EventArgs e)
        {
            //完成导出为mdb空间库中
            try
            {
                //this.Btn_PreHandle.Enabled = false;
                this.Cursor = Cursors.WaitCursor;
                
                    #region //两个图层作叠加相交运算输出到一个mdb空间数据库中
                    string fcName1 = "";
                    string fcName2 = "";
                    IFeatureClass fc1 = null;
                    IFeatureClass fc2 = null;
                    object obj = this.CB_LayerList1.SelectedItem;
                    if (obj != null && obj is CommonComboBoxItem)
                    {
                        CommonComboBoxItem item = obj as CommonComboBoxItem;
                        fc1 = item.Tag as IFeatureClass;
                        fcName1 = (fc1 as IDataset).Name;
                    }
                    obj = this.CB_LayerList2.SelectedItem;
                    if (obj != null && obj is CommonComboBoxItem)
                    {
                        CommonComboBoxItem item = obj as CommonComboBoxItem;
                        fc2 = item.Tag as IFeatureClass;
                        fcName2 = (fc2 as IDataset).Name;
                    }
                    tmpLayerName = fcName1 + "_" + fcName2;

                    double TopoTolerance = 0.0001;
                    IntersectCalculateMdbEx mdbIsCal = new IntersectCalculateMdbEx();
                    if (mdbIsCal.Execute(fc1, fc2, this.tmpMdbFilePath + "\\" + tmpLayerName, "INPUT", TopoTolerance) == true)
                    {
                        gdbUtil sic = new gdbUtil();
                        sic.enumSDEServer = EnumSDEServer.Access;
                        sic.SDE_DataBase = this.tmpMdbFilePath;
                        sic.OpenSDEConnection();
                        
                        IFeatureClass mdbFc = sic.getFeatureClass(tmpLayerName);
                        if (mdbFc != null)
                        {
                            this.GetFields(mdbFc);
                        }
                        TokayWorkspace.ComRelease(mdbFc);
                        mdbFc = null;
                        sic.Dispose();
                        sic = null;
                    }


                    #endregion
               
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "提示");
            }
            finally
            {
                //this.Btn_PreHandle.Enabled = true;
                this.Cursor = Cursors.Default;
            }
        }
                

        //选择目标图层已改变 事件
        private void CB_LayerList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //初始化当前图层的列字段名称
            object obj = this.CB_LayerList1.SelectedItem;
            if (obj != null && obj is CommonComboBoxItem)
            {
                CommonComboBoxItem item = obj as CommonComboBoxItem;
                IFeatureClass fc = item.Tag as IFeatureClass;
                ZhFeatureClass zhfeatclass = new ZhPolylineFeatureClass(fc);
                //获取字段集合
                List<string> fieldNameArray = zhfeatclass.GetFieldNames();
                if (fieldNameArray != null && fieldNameArray.Count > 0)
                {
                    this.CB_Fields1.Items.Clear();                    
                    foreach (string fd in fieldNameArray)
                    {
                        this.CB_Fields1.Items.Add(fd);                        
                    }
                }
            }
        }
        //选择源图层已改变 事件
        private void CB_LayerList2_SelectedIndexChanged(object sender, EventArgs e)
        {
            //初始化当前图层的列字段名称
            object obj = this.CB_LayerList2.SelectedItem;
            if (obj != null && obj is CommonComboBoxItem)
            {
                CommonComboBoxItem item = obj as CommonComboBoxItem;
                IFeatureClass fc = item.Tag as IFeatureClass;
                ZhFeatureClass zhfeatclass = new ZhPolylineFeatureClass(fc);
                //获取字段集合
                List<string> fieldNameArray = zhfeatclass.GetFieldNames();
                if (fieldNameArray != null && fieldNameArray.Count > 0)
                {
                    this.CB_Fields2.Items.Clear();
                    foreach (string fd in fieldNameArray)
                    {
                        this.CB_Fields2.Items.Add(fd);
                    }
                }
            }
        }

        //快速叠加传值功能
        private void QuickOverlapTranValue()
        {
            IFeatureCursor fcur = null;
            mdbAccessLayerClass mdbOp = null;
            frmProgressBar1 pb = null;
            try
            {
                this.button1.Enabled = false;
                this.Cursor = Cursors.WaitCursor;

                //获取参数
                string updateField = this.CB_Fields1.Text.Trim();
                string valFromField = this.CB_Fields2.Text.Trim();

                pb = new frmProgressBar1();
                pb.Text = "空间叠加传值进度...";
                pb.Caption1.Text = "预处理中...";
                pb.progressBar1.Maximum = 10;
                pb.progressBar1.Value = 0;
                pb.Show(this);
                Application.DoEvents();

                //1 获取和生成临时空间数据库
                string tmpdir = Application.StartupPath + "\\TmpDir";
                if (System.IO.Directory.Exists(tmpdir) == false)
                {
                    System.IO.Directory.CreateDirectory(tmpdir);
                }
                this.tmpMdbFilePath = tmpdir + "\\GeoTransValue.mdb";
                if (System.IO.File.Exists(this.tmpMdbFilePath) == true)
                {
                    CommonClass.DeleteFile(this.tmpMdbFilePath);
                }
                //--
                TokayWorkspace.CreateGeodatabase(this.tmpMdbFilePath);
                //2 开始空间叠加操作
                pb.Caption1.Text = "空间叠加中...";
                pb.progressBar1.Value += 1;
                Application.DoEvents();
                //两个图层作叠加相交运算输出到一个mdb空间数据库中
                string FID_fcName1 = "";
                string fcName1 = "";
                string fcName2 = "";
                string fd_Shape_Area = "shape_area";
                IFeatureClass fc1 = null;
                IFeatureClass fc2 = null;
                //目标图层
                object obj = this.CB_LayerList1.SelectedItem;
                if (obj != null && obj is CommonComboBoxItem)
                {
                    CommonComboBoxItem item = obj as CommonComboBoxItem;
                    fc1 = item.Tag as IFeatureClass;
                    fcName1 = (fc1 as IDataset).Name;
                }
                //源图层
                obj = this.CB_LayerList2.SelectedItem;
                if (obj != null && obj is CommonComboBoxItem)
                {
                    CommonComboBoxItem item = obj as CommonComboBoxItem;
                    fc2 = item.Tag as IFeatureClass;
                    fcName2 = (fc2 as IDataset).Name;
                    fd_Shape_Area = fc2.ShapeFieldName + "_area";
                }
                //生成的相交图层名称
                tmpLayerName = fcName1 + "_" + fcName2;
                FID_fcName1 = "FID_" + fcName1;

                double TopoTolerance = 0.0001;
                IntersectCalculateMdbEx mdbIsCal = new IntersectCalculateMdbEx();
                if (mdbIsCal.Execute(fc1, fc2, this.tmpMdbFilePath + "\\" + tmpLayerName, "INPUT", TopoTolerance) == true)
                {
                    #region 空间传值中
                    pb.Caption1.Text = "空间传值中...";
                    pb.progressBar1.Value += 1;
                    Application.DoEvents();
                    //
                    ZhFeature zhfeat = null;
                    int index_fc1 = fc1.Fields.FindField(valFromField);
                    if (index_fc1 >= 0)
                    {
                        valFromField = valFromField + "_1";
                    }

                    mdbOp = new mdbAccessLayerClass(this.tmpMdbFilePath);
                    string x = "select " + FID_fcName1 + "," + valFromField + ",sum(" + fd_Shape_Area + ") as geo_area ";
                    x += " from " + tmpLayerName + " ";
                    x += " group by " + FID_fcName1 + "," + valFromField + "";
                    DataTable dt = mdbOp.GetMdbDB.ExecuteDataTable(x);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        #region 传值中
                        string oid = "";
                        int fc1_count = 0;
                        int index = 0;
                        string objval = "";
                        double geo_area = 0.0;
                        double geo_area_max = 0.0;
                        //更新值
                        fc1_count = fc1.FeatureCount(null);
                        pb.progressBar1.Value = 0;
                        pb.progressBar1.Maximum = fc1_count + 1;
                        Application.DoEvents();
                        //
                        fcur = fc1.Update(null, false);
                        IFeature feat = fcur.NextFeature();
                        while (feat != null)
                        {
                            index += 1;
                            if (index % 50 == 0)
                            {
                                pb.Caption1.Text = "空间叠加传值中...第[" + index + "]个/共[" + fc1_count + "]个";
                                pb.progressBar1.Value = index;
                                Application.DoEvents();
                                fcur.Flush();
                            }
                            zhfeat = new ZHFeaturePolygon(feat);
                            oid = zhfeat.getObjectID();
                            //
                            DataRow[] drArray = dt.Select(FID_fcName1 + "=" + oid);
                            if (drArray != null && drArray.Length >= 1)
                            {
                                objval = "";
                                geo_area = 0.0;
                                geo_area_max = 0.0;
                                //获取面积最大的面积值
                                foreach (DataRow dr in drArray)
                                {
                                    geo_area = CommonClass.TNum(dr["geo_area"].ToString());
                                    if (geo_area >= geo_area_max)
                                    {
                                        objval = dr[valFromField].ToString();
                                        geo_area_max = geo_area;
                                    }
                                }
                                //设置值
                                zhfeat.setFieldValue(updateField, objval);
                                fcur.UpdateFeature(feat);
                            }
                            feat = fcur.NextFeature();
                        }
                        fcur.Flush();
                        if (fcur != null)
                        {
                            TokayWorkspace.ComRelease(fcur);
                            fcur = null;
                        }
                        #endregion
                    }
                    if (mdbOp != null)
                    {
                        mdbOp.GetMdbDB.Dispose();
                        mdbOp = null;
                    }
                    #endregion
                    if (pb != null)
                    {
                        pb.Close();
                        pb.Dispose();
                        pb = null;
                    }
                    MessageBox.Show("传值完毕!", "提示");
                }
                else
                {
                    MessageBox.Show("空间叠加失败!" + mdbIsCal.Message, "提示");
                }
            }
            catch (Exception ee)
            {
                Log.WriteLine(ee);
                MessageBox.Show(ee.Message, "提示");
            }
            finally
            {
                this.button1.Enabled = true;
                this.Cursor = Cursors.Default;

                if (fcur != null)
                {
                    TokayWorkspace.ComRelease(fcur);
                    fcur = null;
                }
                if (mdbOp != null)
                {
                    mdbOp.GetMdbDB.Dispose();
                    mdbOp = null;
                }
                if (pb != null)
                {
                    pb.Close();
                    pb.Dispose();
                    pb = null;
                }
            }
        }
        //开始更新处理 事件
        private void button1_Click(object sender, EventArgs e)
        {
            if (this.fd_IsQuick.Checked == true)
            {   //快速叠加传值
                this.QuickOverlapTranValue();
            }
            else
            {   //自定义叠加传值
                this.CustomOverlapTranValue();
            }            
        }
        //自定义叠加传值
        private void CustomOverlapTranValue()
        {            
            frmProgressBar1 pb = null;
            //
            IFeatureCursor fCur = null;
            IFeatureCursor fCur_fc2 = null;
            //
            try
            {
                this.button1.Enabled = false;
                this.Cursor = Cursors.WaitCursor;

                //获取参数
                string updateField = this.CB_Fields1.Text.Trim();
                string valFromField = this.CB_Fields2.Text.Trim();

                pb = new frmProgressBar1();
                pb.Text = "空间叠加传值进度";
                pb.Caption1.Text = "预处理中......";
                pb.progressBar1.Maximum = 10;
                pb.progressBar1.Value = 0;
                pb.Show(this);
                Application.DoEvents();
                                
                //2 开始空间叠加操作
                pb.Caption1.Text = "空间叠加中...";
                pb.progressBar1.Value += 1;
                Application.DoEvents();
                //                
                IFeatureClass fc1 = null;
                IFeatureClass fc2 = null;
                //
                object objval = "";
                double geo_area = 0.0;
                double geo_area_max = 0.0;
                //目标图层
                object obj = this.CB_LayerList1.SelectedItem;
                if (obj != null && obj is CommonComboBoxItem)
                {
                    CommonComboBoxItem item = obj as CommonComboBoxItem;
                    fc1 = item.Tag as IFeatureClass;                    
                }
                //源图层
                obj = this.CB_LayerList2.SelectedItem;
                if (obj != null && obj is CommonComboBoxItem)
                {
                    CommonComboBoxItem item = obj as CommonComboBoxItem;
                    fc2 = item.Tag as IFeatureClass;                    
                }
                //
                if (fc1 != null && fc2 != null)
                {
                    pb.progressBar1.Maximum= fc1.FeatureCount(null);
                    Application.DoEvents();
                    //目标图层
                    fCur = fc1.Update(null, false);
                    if (fCur != null)
                    {
                        int recIndex = 0;
                        IFeature feat_fc1 = fCur.NextFeature();
                        while (feat_fc1 != null)
                        {
                            ZhFeature zhfeat_fc1 = new ZHFeaturePolygon(feat_fc1);
                            objval = "";
                            geo_area = 0.0;
                            geo_area_max = 0.0;
                            //获取面积最大的面积值
                            #region //与fc2相交运算功能
                            ISpatialFilter sFilter = new SpatialFilterClass();
                            sFilter.Geometry = feat_fc1.ShapeCopy;
                            sFilter.GeometryField = fc2.ShapeFieldName;
                            sFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                            fCur_fc2 = fc2.Search(sFilter, false);
                            IFeature feat_fc2 = fCur_fc2.NextFeature();
                            while (feat_fc2 != null)
                            {
                                ZhFeature zhfeat_fc2 = new ZHFeaturePolygon(feat_fc2);                                
                                IGeometry t_geo = null;
                                #region //获取相交对象
                                IGeometry topGeo = feat_fc1.ShapeCopy;
                                IGeometry topGeo2 = feat_fc2.ShapeCopy;
                                ITopologicalOperator Topoperator = (ITopologicalOperator)topGeo;
                                Topoperator.Simplify();
                                ITopologicalOperator TopGeometry = (ITopologicalOperator)topGeo2;
                                TopGeometry.Simplify();
                                try
                                {
                                    t_geo = Topoperator.Intersect(topGeo2, topGeo.Dimension);                                           
                                }
                                catch (Exception ee)
                                {
                                    t_geo = null;
                                }
                                #endregion
                                if (t_geo is IArea)
                                {   //面积
                                    geo_area = (t_geo as IArea).Area;
                                }
                                else if(t_geo is ICurve)
                                {   //长度
                                    geo_area = (t_geo as ICurve).Length;
                                }
                                if (geo_area >= geo_area_max)
                                {
                                    objval = zhfeat_fc2.getFieldValue(valFromField);
                                    geo_area_max = geo_area;
                                }
                                //下一个要素
                                feat_fc2 = fCur_fc2.NextFeature();
                            }
                            if (fCur_fc2 != null)
                            {
                                TokayWorkspace.ComRelease(fCur_fc2);
                                fCur_fc2 = null;
                            }
                            #endregion
                            //设置值
                            zhfeat_fc1.setFieldValue(updateField, objval);                            
                            fCur.UpdateFeature(feat_fc1);
                            //
                            recIndex += 1;
                            pb.progressBar1.Value = recIndex;
                            pb.Caption1.Text = "正在叠加传值......... 第" + pb.progressBar1.Value + "个/共" + pb.progressBar1.Maximum + "个";
                            Application.DoEvents();
                            //
                            //下一个要素
                            feat_fc1 = fCur.NextFeature();
                        }
                    }
                    if (fCur != null)
                    {
                        fCur.Flush();
                    }
                    if (fCur != null)
                    {
                        TokayWorkspace.ComRelease(fCur);
                        fCur = null;
                    }                    
                    if (pb != null)
                    {
                        pb.Close();
                        pb.Dispose();
                        pb = null;
                    }
                    MessageBox.Show("传值完毕!", "提示");
                }
            }
            catch (Exception ee)
            {
                Log.WriteLine(ee);
                MessageBox.Show(ee.Message, "提示");
            }
            finally
            {
                this.button1.Enabled = true;
                this.Cursor = Cursors.Default;

                if (fCur != null)
                {
                    TokayWorkspace.ComRelease(fCur);
                    fCur = null;
                }
                if (fCur_fc2 != null)
                {
                    TokayWorkspace.ComRelease(fCur_fc2);
                    fCur_fc2 = null;
                } 
                if (pb != null)
                {
                    pb.Close();
                    pb.Dispose();
                    pb = null;
                }
            }
        }

        private void btnRepairGeometry_Click(object sender, EventArgs e)
        {
            try
            {
                //Lib.GisArcGIS.DataManagerEx.RepairGeometryOpenedLayerListCmd cmd = new Lib.GisArcGIS.DataManagerEx.RepairGeometryOpenedLayerListCmd();
                //cmd.OnCreate(BFBuildApplication.BBInstance.Workbench.GMapViewControl.Object);
                //cmd.OnClick();
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "提示");
            }
        }
        
        //
    }
}
