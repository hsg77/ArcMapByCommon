/*
 * email:hsg77@163.com
 * vp:hsg
 * date:2007-07-11
 * 
 */
using System;
using System.Collections.Generic;
using System.Text;

using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.esriSystem;

using System.Windows.Forms;


namespace ArcMapByCommon
{
    //ShapeFile 投影转换 类                      //原来是ProjectionConversion
    public class ProjectionConversionShapeFileClass : ProjectionConversionClass, IDataTransform, IDataTransformShapeFile
    {
        public ProjectionConversionShapeFileClass()
        {
        }

        #region IDataTransformShapeFile 成员

        private string m_ImportShapeFile = "";
        public string ImportShapeFile
        {
            get
            {
                return m_ImportShapeFile;
            }
            set
            {
                m_ImportShapeFile = value;
            }
        }
        private string m_ExportShapeFile = "";
        public string ExportShapeFile
        {
            get
            {
                return m_ExportShapeFile;
            }
            set
            {
                m_ExportShapeFile = value;
            }
        }

        #endregion

        //ShapeFile 投影转换
        public override bool Transform()
        {
            //获取源文件IFeatureClass
            //首先获取文件名，文件路径及文件类型

            string filename = "";
            string url = "";
            string file_ext = "";
            this.SplitShapeFilePath(this.ImportShapeFile, ref filename, ref url, ref file_ext);

            //----
            ShapefileWorkspaceFactoryClass wsf = new ShapefileWorkspaceFactoryClass();
            IWorkspace ws = wsf.OpenFromFile(url, 0);

            IFeatureWorkspace pFeatureWorkspace;
            pFeatureWorkspace = (IFeatureWorkspace)ws;

            IFeatureClass pFeatureClass = null;
            pFeatureClass = pFeatureWorkspace.OpenFeatureClass(filename);

            //源文件投影空间参考

            ISpatialReference SourceSpatialReference = this.getISpatialReference(pFeatureClass);
            this.ImportSpatialReference = SourceSpatialReference;

            //设置范围值

            //this.SetSpatialReferenceDoMain(this.ImportSpatialReference, 0, 0, 999999999.99, 9999999.99);
            //this.SetSpatialReferenceDoMain(this.ExportSpatialReference, 0, 0, 999999999.99, 9999999.99);
                        
            //目标文件投影空间参考

            ISpatialReference ObjectSpatialReference = this.ExportSpatialReference;
            if (SourceSpatialReference != null && ObjectSpatialReference != null)
            {
                //转换
                //生成空的目标文件
                string objfilename = "";
                string objurl = "";
                string objfile_ext = "";
                this.SplitShapeFilePath(this.ExportShapeFile, ref objfilename, ref objurl, ref objfile_ext);

                //----
                ShapefileWorkspaceFactoryClass objwsf = new ShapefileWorkspaceFactoryClass();
                IWorkspace objws = objwsf.OpenFromFile(objurl, 0);

                IFeatureWorkspace objpFeatureWorkspace;
                objpFeatureWorkspace = (IFeatureWorkspace)objws;


                esriFeatureType featureType = esriFeatureType.esriFTSimple;
                esriGeometryType GeometryType = esriGeometryType.esriGeometryNull;
                featureType = pFeatureClass.FeatureType;
                GeometryType = pFeatureClass.ShapeType;

                //Create FeatureClass
                IFeatureClass objFeatureClass = this.CreateFeatureClass(objpFeatureWorkspace, objfilename, featureType, GeometryType);

                //copy 要素表字段结构

                IFields Fields = pFeatureClass.Fields;
                IField field = null;
                for (int i = 0; i < Fields.FieldCount; i++)
                {
                    field = Fields.get_Field(i);
                    if (objFeatureClass.Fields.FindField(field.Name) <= -1)
                    {
                        objFeatureClass.AddField(field);
                    }
                }
                //
                this.ShapeFileProjectionConvert(SourceSpatialReference, ObjectSpatialReference, pFeatureClass, objFeatureClass);

                objFeatureClass = null;

            }
            else
            {
                MessageBox.Show("源文件或目标文件的投影空间参考为空", "投影转换提示");
            }

            return true;
        }

        private IFeatureClass CreateFeatureClass(IFeatureWorkspace FeatureWorkspace, string LayerName, esriFeatureType featureType, esriGeometryType GeometryType)
        {
            IFields fields = new FieldsClass();
            IFieldsEdit edit = fields as IFieldsEdit;

            //创建OBJECTID字段
            IField field3 = new FieldClass();
            IFieldEdit edit2 = field3 as IFieldEdit;
            edit2.Name_2 = "OBJECTID";
            edit2.AliasName_2 = "OBJECTID";
            edit2.Type_2 = esriFieldType.esriFieldTypeOID;
            edit.AddField(field3);

            //创建Shape字段
            IGeometryDef def = new GeometryDefClass();
            IGeometryDefEdit edit4 = def as IGeometryDefEdit;
            edit4.GeometryType_2 = GeometryType;
            edit4.GridCount_2 = 1;
            edit4.set_GridSize(0, 1000);
            edit4.AvgNumPoints_2 = 2;
            edit4.HasM_2 = false;
            edit4.HasZ_2 = false;
            edit4.SpatialReference_2 = this.ExportSpatialReference;

            IField field4 = new FieldClass();
            IFieldEdit edit3 = field4 as IFieldEdit;
            edit3.Name_2 = "SHAPE";
            edit3.AliasName_2 = "SHAPE";
            edit3.Type_2 = esriFieldType.esriFieldTypeGeometry;
            edit3.GeometryDef_2 = def;
            edit.AddField(field4);

            string ShapeFiledName = field4.Name;

            UID uid = null;
            UID uid2 = null;

            switch (featureType)
            {
                case esriFeatureType.esriFTSimple:   //FeatureClass
                    IObjectClassDescription description4 = new FeatureClassDescriptionClass();
                    uid = description4.InstanceCLSID;
                    uid2 = description4.ClassExtensionCLSID;
                    break;
                case esriFeatureType.esriFTAnnotation: //AnnotationFeatureClass
                    IObjectClassDescription description = new AnnotationFeatureClassDescriptionClass();
                    uid = description.InstanceCLSID;
                    uid2 = description.ClassExtensionCLSID;
                    GeometryType = esriGeometryType.esriGeometryPolygon;
                    break;
            }
            //创建要素对象
            IFeatureClass fc = FeatureWorkspace.CreateFeatureClass(LayerName, fields, uid, uid2, featureType, ShapeFiledName, null);

            return fc;
        }

        private void ShapeFileProjectionConvert(ISpatialReference SourceSpatialReference, ISpatialReference ObjectSpatialReference, IFeatureClass SourceFeatureClass, IFeatureClass ObjectFeatureClass)
        {
            int SourceFactoryCode = SourceSpatialReference.FactoryCode;
            int ObjectFactoryCode = ObjectSpatialReference.FactoryCode;
            //if (SourceFactoryCode == 0 || ObjectFactoryCode == 0)
            //{
            //    MessageBox.Show("源文件是自定义的投影坐标系统,这里暂不进行转换", "投影转换提示");
            //    return;
            //}
            EnumProjectionDatum SourceProjection = EnumProjectionDatum.Bejing54;  //默认
            EnumStrip SourceStrip = EnumStrip.Strip3; //默认

            EnumProjectionDatum ObjectProjection = EnumProjectionDatum.Xian80;  //默认
            EnumStrip ObjectStrip = EnumStrip.Strip3; //默认

            //源中央子午线
            double L0 = (SourceSpatialReference as IProjectedCoordinateSystem4GEN).GetCentralLongitude();

            bool IsBigNumber_Source = true;  //源文件默认是大数
            bool IsBigNumber_Object = true;  //目标默认是大数


            //------------------------------------------------------------------------------------------
            //高斯投影大数情况分析
            int BJ54D3_25srid = (int)esriSRProjCS4Type.esriSRProjCS_Beijing1954_3_Degree_GK_Zone_25;
            int BJ54D3_45srid = (int)esriSRProjCS4Type.esriSRProjCS_Beijing1954_3_Degree_GK_Zone_45;

            int BJ54D6_13srid = (int)esriSRProjCSType.esriSRProjCS_Beijing1954GK_13;
            int BJ54D6_23srid = (int)esriSRProjCSType.esriSRProjCS_Beijing1954GK_23;

            int Xian80D3_25srid = (int)esriSRProjCS4Type.esriSRProjCS_Xian1980_3_Degree_GK_Zone_25;
            int Xian80D3_45srid = (int)esriSRProjCS4Type.esriSRProjCS_Xian1980_3_Degree_GK_Zone_45;

            int Xian80D6_13srid = (int)esriSRProjCS4Type.esriSRProjCS_Xian1980_GK_Zone_13;
            int Xian80D6_23srid = (int)esriSRProjCS4Type.esriSRProjCS_Xian1980_GK_Zone_23;

            //源文件分析

            //BeiJing1954 3 degree
            if (SourceFactoryCode >= BJ54D3_25srid && SourceFactoryCode <= BJ54D3_45srid)
            {
                SourceProjection = EnumProjectionDatum.Bejing54;
                SourceStrip = EnumStrip.Strip3;
                IsBigNumber_Source = true;
            }

            //BeiJing1954 6 degree
            if (SourceFactoryCode >= BJ54D6_13srid && SourceFactoryCode <= BJ54D6_23srid)
            {
                SourceProjection = EnumProjectionDatum.Bejing54;
                SourceStrip = EnumStrip.Strip3;
                IsBigNumber_Source = true;
            }

            //Xian1980 3 degree
            if (SourceFactoryCode >= Xian80D3_25srid && SourceFactoryCode <= Xian80D3_45srid)
            {
                SourceProjection = EnumProjectionDatum.Xian80;
                SourceStrip = EnumStrip.Strip3;
                IsBigNumber_Source = true;
            }

            //Xian1980 6 degree
            if (SourceFactoryCode >= Xian80D6_13srid && SourceFactoryCode <= Xian80D6_23srid)
            {
                SourceProjection = EnumProjectionDatum.Xian80;
                SourceStrip = EnumStrip.Strip3;
                IsBigNumber_Source = true;
            }
            //目标文件分析
            //BeiJing1954 3 degree
            if (ObjectFactoryCode >= BJ54D3_25srid && ObjectFactoryCode <= BJ54D3_45srid)
            {
                ObjectProjection = EnumProjectionDatum.Bejing54;
                ObjectStrip = EnumStrip.Strip3;
                IsBigNumber_Object = true;
            }

            //BeiJing1954 6 degree
            if (ObjectFactoryCode >= BJ54D6_13srid && ObjectFactoryCode <= BJ54D6_23srid)
            {
                ObjectProjection = EnumProjectionDatum.Bejing54;
                ObjectStrip = EnumStrip.Strip3;
                IsBigNumber_Object = true;
            }

            //Xian1980 3 degree
            if (ObjectFactoryCode >= Xian80D3_25srid && ObjectFactoryCode <= Xian80D3_45srid)
            {
                ObjectProjection = EnumProjectionDatum.Xian80;
                ObjectStrip = EnumStrip.Strip3;
                IsBigNumber_Object = true;
            }

            //Xian1980 6 degree
            if (ObjectFactoryCode >= Xian80D6_13srid && ObjectFactoryCode <= Xian80D6_23srid)
            {
                ObjectProjection = EnumProjectionDatum.Xian80;
                ObjectStrip = EnumStrip.Strip3;
                IsBigNumber_Object = true;
            }
            //-------------------------------------------------------------------------------------
            ////高斯投影小数情况分析
            int BJ54D3_25srid_CM = (int)esriSRProjCS4Type.esriSRProjCS_Beijing1954_3_Degree_GK_CM_75E;
            int BJ54D3_45srid_CM = (int)esriSRProjCS4Type.esriSRProjCS_Beijing1954_3_Degree_GK_CM_135E;

            int BJ54D6_13srid_CM = (int)esriSRProjCSType.esriSRProjCS_Beijing1954GK_13N;
            int BJ54D6_23srid_CM = (int)esriSRProjCSType.esriSRProjCS_Beijing1954GK_23N;

            int Xian80D3_25srid_CM = (int)esriSRProjCS4Type.esriSRProjCS_Xian1980_3_Degree_GK_CM_75E;
            int Xian80D3_45srid_CM = (int)esriSRProjCS4Type.esriSRProjCS_Xian1980_3_Degree_GK_CM_135E;

            int Xian80D6_13srid_CM = (int)esriSRProjCS4Type.esriSRProjCS_Xian1980_GK_CM_75E;
            int Xian80D6_23srid_CM = (int)esriSRProjCS4Type.esriSRProjCS_Xian1980_GK_CM_135E;

            //源文件分析

            //BeiJing1954 3 degree
            if (SourceFactoryCode >= BJ54D3_25srid_CM && SourceFactoryCode <= BJ54D3_45srid_CM)
            {
                SourceProjection = EnumProjectionDatum.Bejing54;
                SourceStrip = EnumStrip.Strip3;
                IsBigNumber_Source = false;
            }

            //BeiJing1954 6 degree
            if (SourceFactoryCode >= BJ54D6_13srid_CM && SourceFactoryCode <= BJ54D6_23srid_CM)
            {
                SourceProjection = EnumProjectionDatum.Bejing54;
                SourceStrip = EnumStrip.Strip3;
                IsBigNumber_Source = false;
            }

            //Xian1980 3 degree
            if (SourceFactoryCode >= Xian80D3_25srid_CM && SourceFactoryCode <= Xian80D3_45srid_CM)
            {
                SourceProjection = EnumProjectionDatum.Xian80;
                SourceStrip = EnumStrip.Strip3;
                IsBigNumber_Source = false;
            }

            //Xian1980 6 degree
            if (SourceFactoryCode >= Xian80D6_13srid_CM && SourceFactoryCode <= Xian80D6_23srid_CM)
            {
                SourceProjection = EnumProjectionDatum.Xian80;
                SourceStrip = EnumStrip.Strip3;
                IsBigNumber_Source = false;
            }
            //目标文件分析
            //BeiJing1954 3 degree
            if (ObjectFactoryCode >= BJ54D3_25srid_CM && ObjectFactoryCode <= BJ54D3_45srid_CM)
            {
                ObjectProjection = EnumProjectionDatum.Bejing54;
                ObjectStrip = EnumStrip.Strip3;
                IsBigNumber_Object = false;
            }

            //BeiJing1954 6 degree
            if (ObjectFactoryCode >= BJ54D6_13srid_CM && ObjectFactoryCode <= BJ54D6_23srid_CM)
            {
                ObjectProjection = EnumProjectionDatum.Bejing54;
                ObjectStrip = EnumStrip.Strip3;
                IsBigNumber_Object = false;
            }

            //Xian1980 3 degree
            if (ObjectFactoryCode >= Xian80D3_25srid_CM && ObjectFactoryCode <= Xian80D3_45srid_CM)
            {
                ObjectProjection = EnumProjectionDatum.Xian80;
                ObjectStrip = EnumStrip.Strip3;
                IsBigNumber_Object = false;
            }

            //Xian1980 6 degree
            if (ObjectFactoryCode >= Xian80D6_13srid_CM && ObjectFactoryCode <= Xian80D6_23srid_CM)
            {
                ObjectProjection = EnumProjectionDatum.Xian80;
                ObjectStrip = EnumStrip.Strip3;
                IsBigNumber_Object = false;
            }

            //------------------------------------------------------------------------------

            //KDFeatureClass sfc = new KDFeatureClass_TYPoint(SourceFeatureClass);
            //KDFeatureClass ofc = new KDFeatureClass_TYPoint(ObjectFeatureClass);
            ZhFeatureClass sfc = new ZhPointFeatureClass(SourceFeatureClass);
            ZhFeatureClass ofc = new ZhPointFeatureClass(ObjectFeatureClass);

            //KDFeature[] FeatArray = sfc.getSelectedFeaturesByQueryFilter(null);
            //KDFeature sfeat = null;
            //KDFeature ofeat = null;

            ZhFeature[] FeatArray = sfc.getSelectedFeaturesByQueryFilter(null);
            ZhFeature sfeat = null;
            ZhFeature ofeat = null;

            IGeometry geo = null;
            IPoint p = null;

            /*frmProgressBar1 pb = new frmProgressBar1();
            pb.Text = "坐标投影转换";
            pb.label1.Text = "总数：" + FeatArray.Length.ToString();
            pb.progressBar1.Maximum = FeatArray.Length + 1;
            pb.progressBar1.Value = 0;
            pb.Show();*/

            for (int i = 0; i < FeatArray.Length; i++)
            {
                /*if (pb.progressBar1.Value < pb.progressBar1.Maximum)
                {
                    pb.progressBar1.Value += 1;
                    pb.label1.Text = "第[" + pb.progressBar1.Value.ToString() + "]个/总数[" + FeatArray.Length.ToString() + "]";
                    pb.Refresh();
                }*/

                sfeat = FeatArray[i];
                
                //ofeat = ofc.CreateKDFeature();
                ofeat = ofc.CreateFeature();


                //copy 源字段的值到目标字段中

                sfeat.CopyField(ref ofeat);

                geo = sfeat.pFeature.ShapeCopy;

                //geo.Project(ObjectSpatialReference);
                ////坐标投影转换
                IGeometry ObjGeometry = null;
                if (geo is IPoint)
                {   //点

                    double x = 0;
                    double y = 0;
                    double B = 0;
                    double L = 0;
                    x = (geo as IPoint).X;
                    y = (geo as IPoint).Y;
                    this.GetBLFromXY(y, x, ref B, ref L, 0, SourceProjection, SourceStrip, L0, IsBigNumber_Source);

                    this.GetXYFromBL(B, L, ref x, ref y, 0, ObjectProjection, ObjectStrip, IsBigNumber_Object);
                    IPoint objP = new PointClass();
                    objP.X = y;
                    objP.Y = x;
                    objP.SpatialReference = ObjectSpatialReference;
                    //投影到目标点对象
                    ObjGeometry = objP as IGeometry;
                }
                else if (geo is IPolyline)
                {   //线

                    IPolyline objPolyline = new PolylineClass();
                    IPointCollection objPc = objPolyline as IPointCollection;
                    double x = 0;
                    double y = 0;
                    double B = 0;
                    double L = 0;
                    IPointCollection pcol = geo as IPointCollection;
                    object miss = Type.Missing;
                    for (int j = 0; j < pcol.PointCount; j++)
                    {
                        p = pcol.get_Point(j);
                        x = p.X;
                        y = p.Y;
                        this.GetBLFromXY(y, x, ref B, ref L, 0, SourceProjection, SourceStrip, L0, IsBigNumber_Source);

                        this.GetXYFromBL(B, L, ref x, ref y, 0, ObjectProjection, ObjectStrip, IsBigNumber_Object);
                        IPoint objPoint = new PointClass();
                        objPoint.X = y;
                        objPoint.Y = x;
                        objPoint.SpatialReference = ObjectSpatialReference;
                        objPc.AddPoint(objPoint, ref miss, ref miss);
                    }
                    (objPc as ITopologicalOperator).Simplify();
                    ObjGeometry = objPc as IGeometry;
                }
                else if (geo is IPolygon)
                {   //面

                    IPolygon objPolygon = new PolygonClass();
                    IGeometryCollection objPc = objPolygon as IGeometryCollection;
                    double x = 0;
                    double y = 0;
                    double B = 0;
                    double L = 0;
                    IGeometryCollection GeoCol = geo as IGeometryCollection;
                    object miss = Type.Missing;
                    IGeometry tpGeo = null;
                    IPointCollection pcol = null;
                    IRing newRing = null;
                    IPointCollection newRingPointColl = null;
                    for (int j = 0; j < GeoCol.GeometryCount; j++)
                    {
                        tpGeo = GeoCol.get_Geometry(j);  //面内环ring(内/外环)

                        pcol = tpGeo as IPointCollection;
                        newRing = new RingClass();
                        newRingPointColl = newRing as IPointCollection;
                        for (int k = 0; k < pcol.PointCount; k++)
                        {
                            p = pcol.get_Point(k);
                            x = p.X;
                            y = p.Y;
                            this.GetBLFromXY(y, x, ref B, ref L, 0, SourceProjection, SourceStrip, L0, IsBigNumber_Source);

                            this.GetXYFromBL(B, L, ref x, ref y, 0, ObjectProjection, ObjectStrip, IsBigNumber_Object);
                            IPoint objPoint = new PointClass();
                            objPoint.X = y;
                            objPoint.Y = x;
                            newRingPointColl.AddPoint(objPoint, ref miss, ref miss);
                        }
                        newRing.SpatialReference = ObjectSpatialReference;
                        objPc.AddGeometry(newRing as IGeometry, ref miss, ref miss);
                    }
                    (objPc as IGeometry).SpatialReference = ObjectSpatialReference;
                    (objPc as ITopologicalOperator).Simplify();
                    ObjGeometry = objPc as IGeometry;
                }
                else
                {
                    //注记  暂未写


                }
                ofeat.pFeature.Shape = ObjGeometry;
                ofeat.SaveFeature();

            }

            /*pb.Close();
            pb.Dispose();
            pb = null;zk*/

        }

        private void SplitShapeFilePath(string ShapeFilePath, ref string FileName, ref string url, ref string file_ext)
        {
            string[] objsArray = ShapeFilePath.Split('\\');
            FileName = objsArray[objsArray.Length - 1];
            url = "";
            for (int i = 0; i < objsArray.Length - 1; i++)
            {
                url = url + objsArray[i] + "\\";
            }
            string[] sFile = FileName.Split('.');
            file_ext = "";
            if (sFile.Length > 0)
            {
                file_ext = sFile[sFile.Length - 1];
            }
            file_ext = file_ext.ToLower();
        }

        
    }


    //ShapeFile 投影转换 接口
    public interface IDataTransformShapeFile : IDataTransform
    {
        /// <summary>
        /// 输入 源数据文件

        /// </summary>
        string ImportShapeFile { get;set;}
        /// <summary>
        /// 输出 目数据文件

        /// </summary>
        string ExportShapeFile { get;set;}

    }

}
