/*********************************************
 * 功能：定义输出shape文件功能
 * vp:hsg
 * create date:2009-06-29
 * 
 *********************************************/

using System;
using System.Collections.Generic;

using System.Text;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Carto;
using System.Windows.Forms;


namespace ArcMapByJBNT
{
    /// <summary>
    /// 定义输出shape文件功能
    /// </summary>
    public class OutShapeClass
    {
        public IWin32Window ParentForm = null;
        public string OIDFieldName = "FID";
        public string GeometryFieldName="SHAPE";
        //
        public bool IsObjGeo = false;
        //输出单个图层中所有要素集合到shp文件中功能 OK
        public bool OutPut(IFeatureClass featclass, string outShapeFilePath)
        {
            bool rbc = false;
            if (System.IO.File.Exists(outShapeFilePath) == true)
            {   //输出Shape文件
                LocalShapeFileOperator delshpOp = new LocalShapeFileOperator();
                delshpOp.LocalShapePathFileName = outShapeFilePath;
                delshpOp.DeleteShapeFile();
                delshpOp.Dispose();
            }

            esriGeometryType geoType = featclass.ShapeType;

            //创建新的Shape文件
            LocalShapeFileOperator shpOp = new LocalShapeFileOperator();
            shpOp.LocalShapePathFileName = outShapeFilePath;

            string dir = shpOp.getDir(outShapeFilePath);
            string name = shpOp.getFileName(outShapeFilePath);
            ShapefileWorkspaceFactoryClass objwsf = new ShapefileWorkspaceFactoryClass();
            IWorkspace objws = objwsf.OpenFromFile(dir, 0);
            IFeatureWorkspace feaureWS = objws as IFeatureWorkspace;
            //设置投影
            shpOp.ShapeSpatialReference = (featclass as IGeoDataset).SpatialReference;
            //设置shp文件的oid字段和几何字段
            if (IsObjGeo == false)
            {
                if (featclass.HasOID == true)
                {
                    this.OIDFieldName = featclass.OIDFieldName;
                }
                this.GeometryFieldName = featclass.ShapeFieldName;
            }
            shpOp.OIDFieldName = this.OIDFieldName;
            shpOp.GeometryFieldName = this.GeometryFieldName;
            //创建要素类
            IFeatureClass fc = shpOp.CreateFeatureClass(feaureWS, name, esriFeatureType.esriFTSimple, geoType);
            if (fc != null)
            {
                TokayWorkspace.ComRelease(fc);
                fc = null;
                rbc = true;
            }
            TokayWorkspace.ComRelease(objws);
            feaureWS = null;
            objws = null;
            objwsf = null;
            //拷贝字段结构
            IFeatureClass Dfeatclass=shpOp.getIFeatureClass();
            ZhFeatureClass Szhfeatclass = new ZhPointFeatureClass(featclass);            
            Szhfeatclass.CopyFieldsToObjectFeatureClass(Dfeatclass);

            //拷贝几何对象和属性数据
            ZhFeature Szhfeat = null;
            IFeatureBuffer DfeatBuffer=null;
            ZhFeature Dzhfeat=null;
            IFeatureCursor featCurInsert = Dfeatclass.Insert(true);

            //获取总要素个数
            int fIndex = 0;
            int tmpFCount = Szhfeatclass.FeatureClass.FeatureCount(null);
            frmProgressBar1 pb = new frmProgressBar1();
            pb.Text = "正在输出shp文件...";
            pb.Caption1.Text = "正在输出shp文件...";
            pb.progressBar1.Maximum = tmpFCount+1;
            pb.progressBar1.Value = 0;
            pb.Show(this.ParentForm);
            Application.DoEvents();
            //
            IGeometry geo = null;
            //
            IFeatureCursor featcur = Szhfeatclass.FeatureClass.Search(null, false);
            IFeature feat=featcur.NextFeature();
            while (feat != null)
            {
                fIndex += 1;
                if (fIndex % 500 == 0)
                {
                    pb.Caption1.Text = "已输出要素个数:"+fIndex.ToString()+"/总"+tmpFCount.ToString();
                    pb.progressBar1.Value = fIndex;
                    Application.DoEvents();
                    featCurInsert.Flush();
                }
                DfeatBuffer = Dfeatclass.CreateFeatureBuffer();
               
                //拷贝几何对象
                geo=feat.ShapeCopy;
                //去掉Z值
                IZAware pZaware = geo as IZAware;
                if (pZaware.ZAware == true)
                {
                    pZaware.DropZs();
                    pZaware.ZAware = false;
                }
                //
                DfeatBuffer.Shape =geo;
                //
                //拷贝属性数据
                Szhfeat = new ZHFeaturePoint(feat);
                Szhfeat.CopyField(ref DfeatBuffer);

                featCurInsert.InsertFeature(DfeatBuffer);

                feat = featcur.NextFeature();
            }
            featCurInsert.Flush();
            TokayWorkspace.ComRelease(featcur);
            featcur = null;
            TokayWorkspace.ComRelease(featCurInsert);
            featCurInsert = null;
            rbc = true;

            pb.Close();
            pb.Dispose();
            pb = null;

            return rbc;
        }
        //输出单个图层中已选中的要素集合到shp文件中功能 OK
        public bool OutPutSelectedFeature(IFeatureLayer featLayer, string outShapeFilePath)
        {
            IFeatureClass featclass = featLayer.FeatureClass;

            bool rbc = false;
            if (System.IO.File.Exists(outShapeFilePath) == true)
            {   //输出Shape文件
                LocalShapeFileOperator delshpOp = new LocalShapeFileOperator();
                delshpOp.LocalShapePathFileName = outShapeFilePath;
                delshpOp.DeleteShapeFile();
                delshpOp.Dispose();
            }

            esriGeometryType geoType = featclass.ShapeType;

            //创建新的Shape文件
            LocalShapeFileOperator shpOp = new LocalShapeFileOperator();
            shpOp.LocalShapePathFileName = outShapeFilePath;

            string dir = shpOp.getDir(outShapeFilePath);
            string name = shpOp.getFileName(outShapeFilePath);
            ShapefileWorkspaceFactoryClass objwsf = new ShapefileWorkspaceFactoryClass();
            IWorkspace objws = objwsf.OpenFromFile(dir, 0);
            IFeatureWorkspace feaureWS = objws as IFeatureWorkspace;
            //设置投影
            shpOp.ShapeSpatialReference = (featclass as IGeoDataset).SpatialReference;
            //设置shp文件的oid字段和几何字段
            if (IsObjGeo == false)
            {
                if (featclass.HasOID == true)
                {
                    this.OIDFieldName = featclass.OIDFieldName;
                }
                this.GeometryFieldName = featclass.ShapeFieldName;
            }
            shpOp.OIDFieldName = this.OIDFieldName;
            shpOp.GeometryFieldName = this.GeometryFieldName;
            //创建要素类
            IFeatureClass fc = shpOp.CreateFeatureClass(feaureWS, name, esriFeatureType.esriFTSimple, geoType);
            if (fc != null)
            {
                TokayWorkspace.ComRelease(fc);
                fc = null;
                rbc = true;
            }
            TokayWorkspace.ComRelease(objws);
            feaureWS = null;
            objws = null;
            objwsf = null;
            //拷贝字段结构
            IFeatureClass Dfeatclass = shpOp.getIFeatureClass();
            ZhFeatureClass Szhfeatclass = new ZhPointFeatureClass(featclass);
            Szhfeatclass.CopyFieldsToObjectFeatureClass(Dfeatclass);

            //拷贝几何对象和属性数据
            ZhFeature Szhfeat = null;
            IFeatureBuffer DfeatBuffer = null;
            ZhFeature Dzhfeat = null;
            IFeatureCursor featCurInsert = Dfeatclass.Insert(true);

            //获取已选择要的要素集合
            IFeatureSelection featSel = featLayer as IFeatureSelection;
            ICursor cur = null;
            featSel.SelectionSet.Search(null, false, out cur);
            if (cur != null)
            {

                IFeatureCursor featcur = cur as IFeatureCursor;// Szhfeatclass.FeatureClass.Search(null, false);
                if (featcur != null)
                {
                    IFeature feat = featcur.NextFeature();
                    while (feat != null)
                    {
                        DfeatBuffer = Dfeatclass.CreateFeatureBuffer();

                        //拷贝几何对象
                        DfeatBuffer.Shape = feat.ShapeCopy;

                        //拷贝属性数据
                        Szhfeat = new ZHFeaturePoint(feat);
                        Szhfeat.CopyField(ref DfeatBuffer);

                        featCurInsert.InsertFeature(DfeatBuffer);

                        feat = featcur.NextFeature();
                    }
                }
                TokayWorkspace.ComRelease(featcur);
                featcur = null;
            }
            featCurInsert.Flush();
            
            TokayWorkspace.ComRelease(featCurInsert);
            featCurInsert = null;
            rbc = true;

            return rbc;
        }
        //输出通过一个图层已选中的要素去选择输出其他图层shp文件功能 OK
        public bool OutPutSelectedFeatureByOverlap(IFeatureLayer SelectedFeatLayer, IFeatureClass outFeatClass, string outShapeFilePath)
        {
            bool rbc = false;
            if (System.IO.File.Exists(outShapeFilePath) == true)
            {   //输出Shape文件
                LocalShapeFileOperator delshpOp = new LocalShapeFileOperator();
                delshpOp.LocalShapePathFileName = outShapeFilePath;
                delshpOp.DeleteShapeFile();
                delshpOp.Dispose();
            }

            esriGeometryType geoType = outFeatClass.ShapeType;

            //创建新的Shape文件
            LocalShapeFileOperator shpOp = new LocalShapeFileOperator();
            shpOp.LocalShapePathFileName = outShapeFilePath;
            shpOp.GeometryType = geoType;
            //
            string dir = shpOp.getDir(outShapeFilePath);
            string name = shpOp.getFileName(outShapeFilePath);
            ShapefileWorkspaceFactoryClass objwsf = new ShapefileWorkspaceFactoryClass();
            IWorkspace objws = objwsf.OpenFromFile(dir, 0);
            IFeatureWorkspace feaureWS = objws as IFeatureWorkspace;
            //设置投影
            shpOp.ShapeSpatialReference = (outFeatClass as IGeoDataset).SpatialReference;
            //设置shp文件的oid字段和几何字段
            if (IsObjGeo == false)
            {
                if (outFeatClass.HasOID == true)
                {
                    this.OIDFieldName = outFeatClass.OIDFieldName;
                }
                this.GeometryFieldName = outFeatClass.ShapeFieldName;
            }
            shpOp.OIDFieldName = this.OIDFieldName;
            shpOp.GeometryFieldName = this.GeometryFieldName;
            //创建要素类
            IFeatureClass fc = shpOp.CreateFeatureClass(feaureWS, name, esriFeatureType.esriFTSimple, geoType);
            if (fc != null)
            {
                TokayWorkspace.ComRelease(fc);
                fc = null;
                rbc = true;
            }
            TokayWorkspace.ComRelease(objws);
            feaureWS = null;
            objws = null;
            objwsf = null;
            //拷贝字段结构
            IFeatureClass Dfeatclass = shpOp.getIFeatureClass();
            ZhFeatureClass Szhfeatclass = new ZhPointFeatureClass(outFeatClass);
            Szhfeatclass.CopyFieldsToObjectFeatureClass(Dfeatclass);

            //拷贝几何对象和属性数据
            ZhFeature Szhfeat = null;
            IFeatureBuffer DfeatBuffer = null;
            ZhFeature Dzhfeat = null;
            IFeatureCursor featCurInsert = Dfeatclass.Insert(true);

            //获取总要素个数
            int fIndex = 0;
            int tmpFCount = Szhfeatclass.FeatureClass.FeatureCount(null);
            frmProgressBar1 pb = new frmProgressBar1();
            pb.Text = "正在输出shp文件...";
            pb.Caption1.Text = "正在输出shp文件...";
            pb.progressBar1.Maximum = tmpFCount + 1;
            pb.progressBar1.Value = 0;
            pb.Show(this.ParentForm);
            Application.DoEvents();

            //获取已选择要的要素集合
            IFeatureSelection featSel = SelectedFeatLayer as IFeatureSelection;
            ICursor SelectedCur = null;
            featSel.SelectionSet.Search(null, false, out SelectedCur);
            if (SelectedCur != null)
            {
                IFeatureCursor Selectedfeatcur = SelectedCur as IFeatureCursor;// Szhfeatclass.FeatureClass.Search(null, false);
                if (Selectedfeatcur != null)
                {
                    IFeature Selectedfeat = Selectedfeatcur.NextFeature();
                    while (Selectedfeat != null)
                    {
                        ISpatialFilter sFilter = new SpatialFilterClass();
                        sFilter.Geometry = Selectedfeat.ShapeCopy;
                        sFilter.GeometryField = Szhfeatclass.FeatureClass.ShapeFieldName;
                        sFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;  //相交               
                        //总个数
                        pb.progressBar1.Maximum = Szhfeatclass.FeatureClass.FeatureCount(sFilter);
                        pb.progressBar1.Value = 0;
                        Application.DoEvents();
                        fIndex = 0;
                        //
                        IFeatureCursor featcur = Szhfeatclass.FeatureClass.Search(sFilter, false);
                        IFeature feat = featcur.NextFeature();
                        while (feat != null)
                        {
                            fIndex += 1;
                            //
                            pb.Caption1.Text = "已输出要素个数... 第" + fIndex.ToString() + "个/总" + pb.progressBar1.Maximum.ToString()+"个";
                            pb.progressBar1.Value = fIndex;
                            Application.DoEvents();
                            if (fIndex % 500 == 0)
                            {
                                featCurInsert.Flush();
                            }
                            //
                            DfeatBuffer = Dfeatclass.CreateFeatureBuffer();
                            //拷贝几何对象
                            DfeatBuffer.Shape = feat.ShapeCopy;
                            //拷贝属性数据
                            Szhfeat = new ZHFeaturePoint(feat);
                            Szhfeat.CopyField(ref DfeatBuffer);
                            //
                            featCurInsert.InsertFeature(DfeatBuffer);
                            //下一个要素
                            feat = featcur.NextFeature();
                        }
                        featCurInsert.Flush();
                        if (featcur != null)
                        {
                            TokayWorkspace.ComRelease(featcur);
                            featcur = null;
                        }
                        //下一个选中的要素
                        Selectedfeat = Selectedfeatcur.NextFeature();
                    }
                    if (Selectedfeatcur != null)
                    {
                        TokayWorkspace.ComRelease(Selectedfeatcur);
                        Selectedfeatcur = null;
                    }
                }
                if (featCurInsert != null)
                {
                    TokayWorkspace.ComRelease(featCurInsert);
                    featCurInsert = null;
                }
                rbc = true;
            }
            pb.Close();
            pb.Dispose();
            pb = null;
            //
            return rbc;
        }

        //输出内环面 shape
        public bool OutPutPolygonInnerRing(IFeatureClass featclass, string outShapeFilePath)
        {
            bool rbc = false;
            if (System.IO.File.Exists(outShapeFilePath) == true)
            {   //输出Shape文件
                LocalShapeFileOperator delshpOp = new LocalShapeFileOperator();
                delshpOp.LocalShapePathFileName = outShapeFilePath;
                delshpOp.DeleteShapeFile();
                delshpOp.Dispose();
            }

            esriGeometryType geoType = featclass.ShapeType;

            //创建新的Shape文件
            LocalShapeFileOperator shpOp = new LocalShapeFileOperator();
            shpOp.LocalShapePathFileName = outShapeFilePath;

            string dir = shpOp.getDir(outShapeFilePath);
            string name = shpOp.getFileName(outShapeFilePath);
            ShapefileWorkspaceFactoryClass objwsf = new ShapefileWorkspaceFactoryClass();
            IWorkspace objws = objwsf.OpenFromFile(dir, 0);
            IFeatureWorkspace feaureWS = objws as IFeatureWorkspace;
            //设置投影
            ISpatialReference sr=(featclass as IGeoDataset).SpatialReference;
            shpOp.ShapeSpatialReference = sr;
            //设置shp文件的oid字段和几何字段
            if (IsObjGeo == false)
            {
                if (featclass.HasOID == true)
                {
                    this.OIDFieldName = featclass.OIDFieldName;
                }
                this.GeometryFieldName = featclass.ShapeFieldName;
            }
            shpOp.OIDFieldName = this.OIDFieldName;
            shpOp.GeometryFieldName = this.GeometryFieldName;
            //创建要素类
            IFeatureClass fc = shpOp.CreateFeatureClass(feaureWS, name, esriFeatureType.esriFTSimple, geoType);
            if (fc != null)
            {
                TokayWorkspace.ComRelease(fc);
                fc = null;
                rbc = true;
            }
            TokayWorkspace.ComRelease(objws);
            feaureWS = null;
            objws = null;
            objwsf = null;
            //拷贝字段结构
            IFeatureClass Dfeatclass = shpOp.getIFeatureClass();
            ZhFeatureClass Szhfeatclass = new ZhPointFeatureClass(featclass);
            Szhfeatclass.CopyFieldsToObjectFeatureClass(Dfeatclass);

            //拷贝几何对象和属性数据
            ZhFeature Szhfeat = null;
            IFeatureBuffer DfeatBuffer = null;
            ZhFeature Dzhfeat = null;
            IFeatureCursor featCurInsert = Dfeatclass.Insert(true);

            //获取总要素个数
            int fIndex = 0;
            int tmpFCount = Szhfeatclass.FeatureClass.FeatureCount(null);
            frmProgressBar1 pb = new frmProgressBar1();
            pb.Text = "正在输出shp文件...";
            pb.Caption1.Text = "正在输出shp文件...";
            pb.progressBar1.Maximum = tmpFCount + 1;
            pb.progressBar1.Value = 0;
            pb.Show(this.ParentForm);
            Application.DoEvents();

            Dictionary<string, IRing> RingDic =null; 
            object refobj=Type.Missing;

            IFeatureCursor featcur = Szhfeatclass.FeatureClass.Search(null, false);
            IFeature feat = featcur.NextFeature();
            while (feat != null)
            {
                fIndex += 1;
                if (fIndex % 200 == 0)
                {
                    pb.Caption1.Text = "已输出要素个数:" + fIndex.ToString() + "/总" + tmpFCount.ToString();
                    pb.progressBar1.Value = fIndex;
                    Application.DoEvents();
                    featCurInsert.Flush();
                }
                //获取内环面几何对象
                RingDic = PolygonHelper.GetAllOnlyInteriorRings(feat.ShapeCopy as IPolygon);
                if (RingDic != null && RingDic.Values.Count > 0)
                {
                    foreach (IRing r in RingDic.Values)
                    {
                        //内环构面并输出保存
                        DfeatBuffer = Dfeatclass.CreateFeatureBuffer();

                        PolygonClass pclass = new PolygonClass();
                        pclass.AddGeometry(r, ref refobj, ref refobj);
                        pclass.SimplifyPreserveFromTo();
                        pclass.SpatialReference = sr;
                                                
                        DfeatBuffer.Shape = pclass;

                        //拷贝属性数据
                        Szhfeat = new ZHFeaturePoint(feat);
                        Szhfeat.CopyField(ref DfeatBuffer);

                        featCurInsert.InsertFeature(DfeatBuffer);
                        //
                    }
                }
                feat = featcur.NextFeature();
            }
            featCurInsert.Flush();
            TokayWorkspace.ComRelease(featcur);
            featcur = null;
            TokayWorkspace.ComRelease(featCurInsert);
            featCurInsert = null;
            rbc = true;

            pb.Close();
            pb.Dispose();
            pb = null;

            return rbc;
        }
        //
        //输出外环面 shape
        public bool OutPutPolygonExteriorRing(IFeatureClass featclass, string outShapeFilePath,bool IsGetExtRingOfHaveInnerRing)
        {
            bool rbc = false;
            if (System.IO.File.Exists(outShapeFilePath) == true)
            {   //输出Shape文件
                LocalShapeFileOperator delshpOp = new LocalShapeFileOperator();
                delshpOp.LocalShapePathFileName = outShapeFilePath;
                delshpOp.DeleteShapeFile();
                delshpOp.Dispose();
            }

            esriGeometryType geoType = featclass.ShapeType;

            //创建新的Shape文件
            LocalShapeFileOperator shpOp = new LocalShapeFileOperator();
            shpOp.LocalShapePathFileName = outShapeFilePath;

            string dir = shpOp.getDir(outShapeFilePath);
            string name = shpOp.getFileName(outShapeFilePath);
            ShapefileWorkspaceFactoryClass objwsf = new ShapefileWorkspaceFactoryClass();
            IWorkspace objws = objwsf.OpenFromFile(dir, 0);
            IFeatureWorkspace feaureWS = objws as IFeatureWorkspace;
            //设置投影
            ISpatialReference sr = (featclass as IGeoDataset).SpatialReference;
            shpOp.ShapeSpatialReference = sr;
            //设置shp文件的oid字段和几何字段
            if (IsObjGeo == false)
            {
                if (featclass.HasOID == true)
                {
                    this.OIDFieldName = featclass.OIDFieldName;
                }
                this.GeometryFieldName = featclass.ShapeFieldName;
            }
            shpOp.OIDFieldName = this.OIDFieldName;
            shpOp.GeometryFieldName = this.GeometryFieldName;
            //创建要素类
            IFeatureClass fc = shpOp.CreateFeatureClass(feaureWS, name, esriFeatureType.esriFTSimple, geoType);
            if (fc != null)
            {
                TokayWorkspace.ComRelease(fc);
                fc = null;
                rbc = true;
            }
            TokayWorkspace.ComRelease(objws);
            feaureWS = null;
            objws = null;
            objwsf = null;
            //拷贝字段结构
            IFeatureClass Dfeatclass = shpOp.getIFeatureClass();
            ZhFeatureClass Szhfeatclass = new ZhPointFeatureClass(featclass);
            Szhfeatclass.CopyFieldsToObjectFeatureClass(Dfeatclass);

            //拷贝几何对象和属性数据
            ZhFeature Szhfeat = null;
            IFeatureBuffer DfeatBuffer = null;            
            IFeatureCursor featCurInsert = Dfeatclass.Insert(true);

            //获取总要素个数
            int fIndex = 0;
            int tmpFCount = Szhfeatclass.FeatureClass.FeatureCount(null);
            frmProgressBar1 pb = new frmProgressBar1();
            pb.Text = "正在输出shp文件...";
            pb.Caption1.Text = "正在输出shp文件...";
            pb.progressBar1.Maximum = tmpFCount + 1;
            pb.progressBar1.Value = 0;
            pb.Show(this.ParentForm);
            Application.DoEvents();
                        
            object refobj = Type.Missing;
            IPolygon s_p=null;
            IRing[] ExtRingArray = null;
            IRing[] InnerRingArray = null;
                        

            IFeatureCursor featcur = Szhfeatclass.FeatureClass.Search(null, false);
            IFeature feat = featcur.NextFeature();
            while (feat != null)
            {
                fIndex += 1;
                if (fIndex % 200 == 0)
                {
                    pb.Caption1.Text = "已输出要素个数:" + fIndex.ToString() + "/总" + tmpFCount.ToString();
                    pb.progressBar1.Value = fIndex;
                    Application.DoEvents();
                    featCurInsert.Flush();
                }
                s_p=feat.ShapeCopy as IPolygon;
                //获取外环面几何对象
                ExtRingArray = PolygonHelper.GetExteriorRings(s_p);
                if (ExtRingArray != null && ExtRingArray.Length > 0)
                {
                    foreach (IRing r in ExtRingArray)
                    {
                        if (IsGetExtRingOfHaveInnerRing == true)
                        {
                            InnerRingArray = PolygonHelper.GetInteriorRingsByExterior(r, s_p);
                            if (InnerRingArray != null && InnerRingArray.Length > 0)
                            {   //有内环
                                //外环构面并输出保存
                                DfeatBuffer = Dfeatclass.CreateFeatureBuffer();

                                PolygonClass pclass = new PolygonClass();
                                pclass.AddGeometry(r, ref refobj, ref refobj);
                                pclass.SimplifyPreserveFromTo();
                                pclass.SpatialReference = sr;

                                DfeatBuffer.Shape = pclass;

                                //拷贝属性数据
                                Szhfeat = new ZHFeaturePoint(feat);
                                Szhfeat.CopyField(ref DfeatBuffer);

                                featCurInsert.InsertFeature(DfeatBuffer);
                                //                        
                            }
                        }
                        else
                        {
                            //外环构面并输出保存
                            DfeatBuffer = Dfeatclass.CreateFeatureBuffer();

                            PolygonClass pclass = new PolygonClass();
                            pclass.AddGeometry(r, ref refobj, ref refobj);
                            pclass.SimplifyPreserveFromTo();
                            pclass.SpatialReference = sr;

                            DfeatBuffer.Shape = pclass;

                            //拷贝属性数据
                            Szhfeat = new ZHFeaturePoint(feat);
                            Szhfeat.CopyField(ref DfeatBuffer);

                            featCurInsert.InsertFeature(DfeatBuffer);
                            //
                        }
                    }
                }
                feat = featcur.NextFeature();
            }
            featCurInsert.Flush();
            TokayWorkspace.ComRelease(featcur);
            featcur = null;
            TokayWorkspace.ComRelease(featCurInsert);
            featCurInsert = null;
            rbc = true;

            pb.Close();
            pb.Dispose();
            pb = null;

            return rbc;
        }
        //
    }

   
}
