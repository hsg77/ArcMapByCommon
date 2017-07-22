using System;
using System.Collections.Generic;
using System.Text;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.esriSystem;
using System.IO;
using ESRI.ArcGIS.DataSourcesRaster;
using ESRI.ArcGIS.Display;
using System.Collections;

namespace ArcMapByCommon
{
    /// <summary>
    /// 管理Shape文件的相关操作的类功能
    /// </summary>
    public class ShapeInfoConfig : LocalShapeFileOperator, IShapeInfoConfig
    {
    }

    /// <summary>
    /// 
    /// </summary>
    public interface IShapeInfoConfig : ILocalShapeFileOperator
    {
    }

    /// <summary>
    ///   LocalShapeFileOperator  define class
    /// </summary>
    public class LocalShapeFileOperator : ILocalShapeFileOperator
    {
        public string OIDFieldName = "FID";
        public string GeometryFieldName = "SHAPE";
        public esriGeometryType GeometryType = esriGeometryType.esriGeometryPolygon;
        //
        #region ILocalShapeFileOperator 成员

        private string m_LocalShapePathFileName = "";
        /// <summary>
        /// LocalShapePathFileName get;set;
        /// </summary>
        public string LocalShapePathFileName
        {
            get
            {
                return m_LocalShapePathFileName;
            }
            set
            {
                m_LocalShapePathFileName = value;
            }
        }

        private ISpatialReference m_ShapeSpatialReference = null;
        /// <summary>
        ///  ShapeSpatialReference get;set;
        /// </summary>
        public ISpatialReference ShapeSpatialReference
        {
            get
            {
                return m_ShapeSpatialReference;
            }
            set
            {
                m_ShapeSpatialReference = value;
            }

        }

        /// <summary>
        ///  IsExists
        /// </summary>
        /// <returns></returns>
        public bool IsExists()
        {
            if (this.LocalShapePathFileName != "")
            {
                return System.IO.File.Exists(this.LocalShapePathFileName);
            }
            return false;
        }

        /// <summary>
        /// 虚态方法没有实现
        /// </summary>
        /// <returns></returns>
        public virtual bool CreateShapeFile()
        {
            bool rbc = false;
            try
            {
                string dir = this.getDir(this.LocalShapePathFileName);
                string name = this.getFileName(this.LocalShapePathFileName);

                ShapefileWorkspaceFactoryClass objwsf = new ShapefileWorkspaceFactoryClass();
                IWorkspace objws = objwsf.OpenFromFile(dir, 0);

                IFeatureWorkspace feaureWS = objws as IFeatureWorkspace;

                IFeatureClass fc = this.CreateFeatureClass(feaureWS, name, esriFeatureType.esriFTSimple, this.GeometryType);
                if (fc != null)
                {
                    ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(fc);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(fc);
                    fc = null;
                    rbc = true;
                }
                ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(objws);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(objws);
                feaureWS = null;
                objws = null;
                objwsf = null;

            }
            catch (Exception ee)
            {
                Log.WriteLine(ee);
                rbc = false;
            }
            return rbc;
        }

        /// <summary>
        ///  DeleteShapeFile
        /// </summary>
        /// <returns></returns>
        public bool DeleteShapeFile()
        {
            bool rbc = false;
            IFeatureClass fc = null;
            try
            {
                fc = this.getIFeatureClass();
                if (fc != null)
                {
                    IDataset fds = fc as IDataset;
                    fds.Delete();
                    fds = null;
                    TokayWorkspace.ComRelease(fc);
                    fc = null;
                }
                this.DeleteTempFile(this.LocalShapePathFileName);

                rbc = true;
            }
            catch (Exception ee)
            {
                System.Diagnostics.Debug.WriteLine(ee.ToString());
                rbc = false;
            }
            finally
            {
                if (fc != null)
                {
                    TokayWorkspace.ComRelease(fc);
                    fc = null;
                }
            }
            return rbc;
        }

        /// <summary>
        ///  getIFeatureClass
        /// </summary>
        /// <returns></returns>
        public virtual IFeatureClass getIFeatureClass()
        {
            if (System.IO.File.Exists(LocalShapePathFileName))
            {
                string dir = this.getDir(this.LocalShapePathFileName);
                string name = this.getFileNameEx(this.LocalShapePathFileName);

                ShapefileWorkspaceFactoryClass objwsf = new ShapefileWorkspaceFactoryClass();
                IWorkspace objws = objwsf.OpenFromFile(dir, 0);

                IFeatureWorkspace feaureWS = objws as IFeatureWorkspace;

                if (feaureWS != null)
                {
                    return feaureWS.OpenFeatureClass(name);
                }
            }

            return null;
        }

        /// <summary>
        ///  getZHFeatureClass
        /// </summary>
        /// <returns></returns>
        public ZhFeatureClass getZHFeatureClass()
        {
            IFeatureClass fc = this.getIFeatureClass();
            ZhFeatureClass zhfc = new ZhPointFeatureClass(fc);

            return zhfc;
        }

        /// <summary>
        /// deleteAllFeatures
        /// </summary>
        /// <returns></returns>
        public bool deleteAllFeatures()
        {
            bool rbc = false;
            try
            {
                IFeatureClass fc = this.getIFeatureClass();
                if (fc != null)
                {
                    ZhPointFeatureClass zhfc = new ZhPointFeatureClass(fc);
                    zhfc.DeleteAllFeatures();
                    TokayWorkspace.ComRelease(fc);
                    fc = null;
                    zhfc = null;

                    rbc = true;
                }
            }
            catch (Exception ee)
            {
                System.Diagnostics.Debug.WriteLine(ee.ToString());
                rbc = false;
            }
            return rbc;

        }

        #endregion

        /// <summary>
        /// getFileName
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        public string getFileName(string filepath)
        {
            string rbc = "";
            string[] splitArray = filepath.Split(new char[] { '\\' });
            if (splitArray.Length > 0)
            {
                string filenameEx = splitArray[splitArray.Length - 1];
                if (filenameEx != null && filenameEx != "")
                {
                    string[] filenameExArray = filenameEx.Split(new char[] { '.' });
                    if (filenameExArray.Length > 0)
                    {
                        rbc = filenameExArray[0];
                    }
                }
            }
            return rbc;
        }

        /// <summary>
        /// getFileNameEx
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        public string getFileNameEx(string filepath)
        {
            string rbc = "";
            string[] splitArray = filepath.Split(new char[] { '\\' });
            if (splitArray.Length > 0)
            {
                string filenameEx = splitArray[splitArray.Length - 1];
                if (filenameEx != null && filenameEx != "")
                {
                    rbc = filenameEx;
                }
            }
            return rbc;
        }

        /// <summary>
        /// getDir
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public string getDir(string filePath)
        {
            string rbc = "";
            int pos;
            if (filePath.IndexOf('\\') == 0)
            {
                rbc = "";
            }
            pos = filePath.LastIndexOf('\\');
            rbc = filePath.Substring(0, pos);
            return rbc;
        }

        /// <summary>
        /// CreateFeatureClass
        /// </summary>
        /// <param name="FeatureWorkspace"></param>
        /// <param name="LayerName"></param>
        /// <param name="featureType"></param>
        /// <param name="GeometryType"></param>
        /// <returns></returns>
        public IFeatureClass CreateFeatureClass(IFeatureWorkspace FeatureWorkspace, string LayerName, esriFeatureType featureType, esriGeometryType GeometryType)
        {
            //----------------
            ESRI.ArcGIS.Geodatabase.FeatureClassDescriptionClass objectClassDescription = new ESRI.ArcGIS.Geodatabase.FeatureClassDescriptionClass();

            // create the fields using the required fields method
            ESRI.ArcGIS.Geodatabase.IFields fields = objectClassDescription.RequiredFields;
            ESRI.ArcGIS.Geodatabase.IFieldsEdit fieldsEdit = (ESRI.ArcGIS.Geodatabase.IFieldsEdit)fields; // Explicit Cast
            IField fd = null;
            bool IsOIDField = false;
            bool IsGeometryField = false;
            for (int i = 0; i < fields.FieldCount; i++)
            {
                fd = fields.get_Field(i);
                if (fd.Type == esriFieldType.esriFieldTypeOID)
                {
                    (fd as IFieldEdit).Name_2 = this.OIDFieldName;
                    IsOIDField = true;
                }
                if (fd.Type == esriFieldType.esriFieldTypeGeometry)
                {
                    (fd as IFieldEdit).Name_2 = this.GeometryFieldName;
                    (fd.GeometryDef as IGeometryDefEdit).GeometryType_2 = GeometryType;
                    IsGeometryField = true;
                }
            }
            //----------------            

            if (IsOIDField == false)
            {
                //创建OBJECTID字段
                IField field3 = new FieldClass();
                IFieldEdit edit2 = field3 as IFieldEdit;
                edit2.Name_2 = this.OIDFieldName;
                edit2.AliasName_2 = this.OIDFieldName;
                edit2.Type_2 = esriFieldType.esriFieldTypeOID;
                fieldsEdit.AddField(field3);
            }
            string ShapeFiledName = this.GeometryFieldName;
            if (IsGeometryField == false)
            {
                //创建几何字段
                IGeometryDef def = new GeometryDefClass();
                IGeometryDefEdit edit4 = def as IGeometryDefEdit;
                edit4.GeometryType_2 = this.GeometryType;
                edit4.GridCount_2 = 1;
                edit4.set_GridSize(0, 1000);
                edit4.AvgNumPoints_2 = 2;
                edit4.HasM_2 = false;
                edit4.HasZ_2 = false;
                edit4.SpatialReference_2 = this.ShapeSpatialReference;
                //
                IField field4 = new FieldClass();
                IFieldEdit edit3 = field4 as IFieldEdit;
                edit3.Name_2 = this.GeometryFieldName;
                edit3.AliasName_2 = this.GeometryFieldName;
                edit3.Type_2 = esriFieldType.esriFieldTypeGeometry;
                edit3.GeometryDef_2 = def;
                fieldsEdit.AddField(field4);
                //
                ShapeFiledName = field4.Name;
            }

            UID uid = null;
            UID uid2 = null;
            switch (featureType)
            {
                case esriFeatureType.esriFTSimple:   //FeatureClass                    
                    uid = objectClassDescription.InstanceCLSID;
                    uid2 = objectClassDescription.ClassExtensionCLSID;
                    break;
                default:
                    break;
            }
            //创建要素对象
            IFeatureClass fc = FeatureWorkspace.CreateFeatureClass(LayerName, fields, uid, uid2, featureType, ShapeFiledName, null);
            //
            return fc;
        }

        /// <summary>
        /// 更新几何对象面积到一个自定义的字段中
        /// </summary>
        /// <param name="CustomAreaField">自定义的字段</param>
        /// <returns></returns>
        public bool UpdateShapeAreamToCustomAreaField(string CustomAreaField)
        {
            ZhFeature zhfeat = null;
            double interArea = 0;
            IGeometry geo = null;

            IFeatureClass fc = this.getIFeatureClass();
            if (fc != null)
            {
                IFeatureCursor featCur = fc.Update(null, false);
                IFeature feat = featCur.NextFeature();
                while (feat != null)
                {
                    interArea = 0;
                    zhfeat = new ZHFeaturePolygon(feat);
                    geo = zhfeat.pFeature.ShapeCopy;
                    if (geo is IArea)
                    {
                        interArea = (geo as IArea).Area;
                    }
                    zhfeat.setFieldValue(CustomAreaField, interArea);
                    featCur.UpdateFeature(feat);
                    feat = featCur.NextFeature();
                }
                featCur.Flush();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(featCur);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(fc);
            }

            return true;
        }

        public bool UpdateShapeAreamToCustomAreaField(IFeatureClass fc, string CustomAreaField)
        {
            ZhFeature zhfeat = null;
            double interArea = 0;
            IGeometry geo = null;

            if (fc != null)
            {
                IFeatureCursor featCur = fc.Update(null, false);
                IFeature feat = featCur.NextFeature();
                while (feat != null)
                {
                    interArea = 0;
                    zhfeat = new ZHFeaturePolygon(feat);
                    geo = zhfeat.pFeature.ShapeCopy;
                    if (geo is IArea)
                    {
                        interArea = (geo as IArea).Area;
                    }
                    zhfeat.setFieldValue(CustomAreaField, interArea);
                    featCur.UpdateFeature(feat);
                    feat = featCur.NextFeature();
                }
                featCur.Flush();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(featCur);
            }

            return true;
        }
        /// <summary>
        /// 更新几何对象长度到一个自定义的字段中
        /// </summary>
        /// <param name="CustomLengthField">自定义的字段</param>
        /// <returns></returns>
        public bool UpdateShapeLengthToCustomAreaField(string CustomLengthField)
        {
            ZhFeature zhfeat = null;
            double interLength = 0;
            IGeometry geo = null;

            IFeatureClass fc = this.getIFeatureClass();
            if (fc != null)
            {
                IFeatureCursor featCur = fc.Update(null, false);
                IFeature feat = featCur.NextFeature();
                while (feat != null)
                {
                    interLength = 0;
                    zhfeat = new ZHFeaturePolygon(feat);
                    geo = zhfeat.pFeature.ShapeCopy;
                    if (geo is ICurve)
                    {
                        interLength = (geo as ICurve).Length;
                    }
                    zhfeat.setFieldValue(CustomLengthField, interLength);
                    featCur.UpdateFeature(feat);
                    feat = featCur.NextFeature();
                }
                featCur.Flush();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(featCur);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(fc);
            }

            return true;
        }

        /// <summary>
        /// 更新几何对象长度到一个自定义的字段中
        /// </summary>
        /// <param name="CustomLengthField">自定义的字段</param>
        /// <returns></returns>
        public bool UpdateShapeLengthToCustomAreaField(IFeatureClass fc, string CustomLengthField)
        {
            ZhFeature zhfeat = null;
            double interLength = 0;
            IGeometry geo = null;

            if (fc != null)
            {
                IFeatureCursor featCur = fc.Update(null, false);
                IFeature feat = featCur.NextFeature();
                while (feat != null)
                {
                    interLength = 0;
                    zhfeat = new ZHFeaturePolygon(feat);
                    geo = zhfeat.pFeature.ShapeCopy;
                    if (geo is ICurve)
                    {
                        interLength = (geo as ICurve).Length;
                    }
                    zhfeat.setFieldValue(CustomLengthField, interLength);
                    featCur.UpdateFeature(feat);
                    feat = featCur.NextFeature();
                }
                featCur.Flush();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(featCur);

            }

            return true;
        }

        /// <summary>
        /// 添加  AddCustomAreaField  固定字段
        ///   InterArea=相交面积  (28,6)
        ///   InterLength=相交长度  (28,6)
        /// </summary>
        /// <param name="CustomAreaField"></param>
        /// <returns></returns>
        public bool AddDefinedCustomAreaField()
        {

            IFeatureClass fc = this.getIFeatureClass();
            if (fc != null)
            {
                //--InterArea
                this.AddCustomAreaField(fc, "InterArea", "相交面积");
                //--

                //--InterLength
                this.AddCustomAreaField(fc, "InterLengt", "相交长度");
                //--
                System.Runtime.InteropServices.Marshal.ReleaseComObject(fc);
            }

            return true;

        }
        public bool AddDefinedCustomAreaField(IFeatureClass fc)
        {
            if (fc != null)
            {
                //--InterArea
                this.AddCustomAreaField(fc, "InterArea", "相交面积");
                //--

                //--InterLength
                this.AddCustomAreaField(fc, "InterLengt", "相交长度");
                //--
            }

            return true;

        }

        /// <summary>
        ///  添加 自定义 数字型字段 AddCustomAreaField
        /// </summary>
        /// <param name="CustomAreaFieldName"></param>
        /// <param name="CustomAreaFieldAliasName"></param>
        /// <returns></returns>
        public bool AddCustomAreaField(IFeatureClass fc, string CustomAreaFieldName, string CustomAreaFieldAliasName)
        {
            if (fc != null)
            {
                //--Custom Field
                if (fc.Fields.FindField(CustomAreaFieldName) < 0)
                {
                    IField fd_Intermj = new FieldClass();
                    IFieldEdit mfd_Intermj = fd_Intermj as IFieldEdit;
                    mfd_Intermj.Name_2 = CustomAreaFieldName;
                    mfd_Intermj.AliasName_2 = CustomAreaFieldAliasName;

                    mfd_Intermj.Type_2 = esriFieldType.esriFieldTypeDouble;
                    mfd_Intermj.Length_2 = 28;
                    mfd_Intermj.Precision_2 = 28;   //总长度
                    mfd_Intermj.Scale_2 = 6;        //小数位数

                    fc.AddField(fd_Intermj);
                }
                return true;
            }
            return false;

        }
        /// <summary>
        /// 添加 自定义文本字段
        /// </summary>
        /// <param name="fc"></param>
        /// <param name="CustomFieldName"></param>
        /// <param name="CustomFieldAliasName"></param>
        /// <param name="Length"></param>
        /// <returns></returns>
        public bool AddCustomTextField(IFeatureClass fc, string CustomFieldName, string CustomFieldAliasName, int Length)
        {
            if (fc != null)
            {
                //--Custom Field
                if (fc.Fields.FindField(CustomFieldName) < 0)
                {
                    //--Custom Field
                    IField fd_Intermj = new FieldClass();
                    IFieldEdit mfd_Intermj = fd_Intermj as IFieldEdit;
                    mfd_Intermj.Name_2 = CustomFieldName;
                    mfd_Intermj.AliasName_2 = CustomFieldAliasName;

                    mfd_Intermj.Type_2 = esriFieldType.esriFieldTypeString;
                    mfd_Intermj.Length_2 = Length;  //总长度

                    fc.AddField(fd_Intermj);
                    //--
                }
                return true;
            }
            return false;
        }

        /// <summary>
        ///  DeleteTempFile
        /// </summary>
        /// <param name="tempShpFile"></param>
        protected void DeleteTempFile(string tempShpFile)
        {
            if (tempShpFile.Length > 4)
            {
                string path = tempShpFile.Substring(0, tempShpFile.Length - 4);

                if (File.Exists(path + ".shp")) File.Delete(path + ".shp");
                if (File.Exists(path + ".shp.xml")) File.Delete(path + ".shp.xml");
                if (File.Exists(path + ".prj")) File.Delete(path + ".prj");
                if (File.Exists(path + ".shx")) File.Delete(path + ".shx");
                if (File.Exists(path + ".sbx")) File.Delete(path + ".sbx");
                if (File.Exists(path + ".sbn")) File.Delete(path + ".sbn");
                if (File.Exists(path + ".dbf")) File.Delete(path + ".dbf");
            }
        }

        #region IDisposable 成员
        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
        }

        #endregion
    }



    public interface ILocalShapeFileOperator : IDisposable
    {
        string LocalShapePathFileName { get; set; }
        ISpatialReference ShapeSpatialReference { get; set; }

        bool IsExists();
        bool CreateShapeFile();
        bool DeleteShapeFile();
        IFeatureClass getIFeatureClass();
        bool deleteAllFeatures();

        ZhFeatureClass getZHFeatureClass();

        bool UpdateShapeAreamToCustomAreaField(string CustomAreaField);
        bool UpdateShapeLengthToCustomAreaField(string CustomLengthField);
        bool AddCustomAreaField(IFeatureClass fc, string CustomAreaFieldName, string CustomAreaFieldAliasName);
        bool AddCustomTextField(IFeatureClass fc, string CustomFieldName, string CustomFieldAliasName, int Length);
        bool AddDefinedCustomAreaField();
    }

    //实例:本地Shape文件PDJB.shp文件的相关操作
    public class PDJBTLocalShapeFileOperator : LocalShapeFileOperator
    {

        /// <summary>
        /// 重写虚态方法
        /// 创建PDJBTLocalShapeFile文件
        /// </summary>
        /// <returns></returns>
        public override bool CreateShapeFile()
        {
            bool rbc = false;
            try
            {
                string dir = this.getDir(this.LocalShapePathFileName);
                string name = this.getFileName(this.LocalShapePathFileName);

                ShapefileWorkspaceFactoryClass objwsf = new ShapefileWorkspaceFactoryClass();
                IWorkspace objws = objwsf.OpenFromFile(dir, 0);

                IFeatureWorkspace feaureWS = objws as IFeatureWorkspace;

                IFeatureClass fc = this.CreateFeatureClass(feaureWS, name, esriFeatureType.esriFTSimple, esriGeometryType.esriGeometryPolygon);
                if (fc != null)
                {
                    //Add 要素表字段结构(业务字段)
                    IField mField = new FieldClass();
                    IFieldEdit mFieldEdit = mField as IFieldEdit;
                    mFieldEdit.Name_2 = "PDJB";
                    mFieldEdit.AliasName_2 = "坡度级别";

                    mFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
                    mFieldEdit.Length_2 = 9;

                    fc.AddField(mField);

                    //--SLOPECODE
                    IField fd_slopecode = new FieldClass();
                    IFieldEdit mfd_slopecode = fd_slopecode as IFieldEdit;
                    mfd_slopecode.Name_2 = "SLOPECODE";
                    mfd_slopecode.AliasName_2 = "坡度代码";

                    mfd_slopecode.Type_2 = esriFieldType.esriFieldTypeInteger;
                    mfd_slopecode.Length_2 = 1;

                    fc.AddField(fd_slopecode);
                    //--

                    fc = null;
                    //
                    rbc = true;
                }
                feaureWS = null;
                objws = null;
                objwsf = null;

            }
            catch
            {
                rbc = false;
            }
            return rbc;
        }
    }


    //Raster 栅格操作类
    public interface IXpRasterInfoConfig
    {
        string XpRasterPathFile { get; set; }
        RasterLayerClass RasterLayerClass { get; }

        void Open();
        void Render();

        void RenderPJT();

        double[] getOraDataStringArray();
        double[] getDivEquestStringArray(int DivNum);
    }

    /// <summary>
    /// 栅格文件的相关操作功能类
    /// </summary>
    public class XpRasterInfoConfigClass : IXpRasterInfoConfig
    {

        #region IXpRasterInfoConfig 成员

        private string m_XpRasterPathFile = "";
        public string XpRasterPathFile
        {
            get
            {
                return m_XpRasterPathFile;
            }
            set
            {
                m_XpRasterPathFile = value;
            }
        }


        private RasterLayerClass m_RasterLayerClass = null;
        public RasterLayerClass RasterLayerClass
        {
            get { return m_RasterLayerClass; }
        }

        public double[] getOraDataStringArray()
        {
            List<double> arr = new List<double>();
            arr.Clear();
            if (this.RasterLayerClass != null &&
                this.RasterLayerClass.AttributeTable != null)
            {
                ITable tb = this.RasterLayerClass.AttributeTable as ITable;
                if (tb != null)
                {
                    object[] objArr = this.Unique(tb, "Value", "");
                    foreach (object obj in objArr)
                    {
                        arr.Add(double.Parse(obj.ToString()));
                    }
                    arr.Sort();
                }
            }
            else if (this.RasterLayerClass.AttributeTable == null)
            {
                //渲染方式获取Value值的方法
                //this.RasterLayerClass.Renderer = new RasterValueClass();
                for (int ib = 0; ib < this.RasterLayerClass.BandCount; ib++)
                {
                    IUniqueValues uv = new UniqueValuesClass();

                    IRasterCalcUniqueValues rcuv = new RasterCalcUniqueValuesClass();
                    rcuv.AddFromRaster(this.RasterLayerClass.Raster, ib, uv);

                    for (int i = 0; i < uv.Count; i++)
                    {
                        arr.Add(double.Parse(uv.get_UniqueValue(i).ToString()));
                    }
                }
                arr.Sort();
            }
            return arr.ToArray();
        }

        //N等份算法
        public double[] getDivEquestStringArray(int DivNum)
        {
            List<double> DivItemList = new List<double>();
            if (DivNum != 0)
            {
                //N等份
                double[] tt = this.getOraDataStringArray();
                if (tt.Length > 0)
                {
                    double lastValue = tt[tt.Length - 1];
                    double firstValue = tt[0];
                    double peritem = (lastValue - firstValue) / DivNum;
                    for (int i = 0; i <= DivNum; i++)
                    {
                        DivItemList.Add(firstValue + i * peritem);
                    }
                    DivItemList.Add(lastValue);
                }
            }
            return DivItemList.ToArray();
        }

        //新地科学分类方法 [没有使用]
        public double[] getDivEquestStringArray2(int breakNum)
        {
            List<double> DivItemList = new List<double>();
            if (breakNum != 0)
            {
                for (int ib = 0; ib < this.RasterLayerClass.BandCount; ib++)
                {
                    IUniqueValues uv = new UniqueValuesClass();

                    IRasterCalcUniqueValues rcuv = new RasterCalcUniqueValuesClass();
                    rcuv.AddFromRaster(this.RasterLayerClass.Raster, ib, uv);

                    object valueArray = null, freq = null;
                    ;
                    IClassify classify = null;


                    uv.GetHistogram(out valueArray, out freq);

                    //--
                    EqualIntervalClass eq = new EqualIntervalClass();
                    eq.Classify(valueArray, freq, ref breakNum);
                    classify = (IClassify)eq;
                    //object o = classify.ClassBreaks;
                    ////--
                    //Quantile qc = new QuantileClass();
                    //qc.Classify(valueArray, freq, ref breakNum);
                    //classify = (IClassify)qc;
                    //o = classify.ClassBreaks;
                    ////--
                    //NaturalBreaksClass nb = new NaturalBreaksClass();
                    //nb.Classify(valueArray, freq, ref breakNum);
                    //classify = nb as IClassify;
                    //o = classify.ClassBreaks;
                    ////--
                    ////DefinedIntervalClass di = new DefinedIntervalClass();
                    ////di.IntervalRange = this.m_classBreaksParam.Interval;
                    ////di.Classify(valueArray, freq, ref breakNum);
                    ////--
                    ////StandardDeviationClass sd = new StandardDeviationClass();
                    ////IStatisticsResults stat = histogram as IStatisticsResults;
                    ////classify = sd as IClassify;
                    ////classify.SetHistogramData(valueArray, freq);
                    ////IDeviationInterval di = sd as IDeviationInterval;
                    ////di.DeviationInterval = 1;
                    ////di.Mean = stat.Mean;
                    ////di.StandardDev = stat.StandardDeviation;
                    ////classify.Classify(ref breakNum);
                    ////--

                    object o = classify.ClassBreaks;
                    System.Array breakArray = o as System.Array;

                    for (int i = breakArray.Length - 1; i >= 0; i--)
                    {
                        DivItemList.Add(double.Parse(breakArray.GetValue(i).ToString()));
                    }
                }
            }
            return DivItemList.ToArray();
        }

        #endregion

        /// <summary>
        /// 打开栅格文件this.XpRasterPathFile
        /// 到m_RasterLayerClass栅格图层中
        /// </summary>
        public void Open()
        {
            if (this.XpRasterPathFile.Trim() != "")
            {
                this.m_RasterLayerClass = new RasterLayerClass();
                this.m_RasterLayerClass.CreateFromFilePath(this.XpRasterPathFile);

            }
        }

        //渲染建设用地适宜级栅格图
        public void Render()
        {
            if (this.RasterLayerClass != null)
            {
                RgbColorClass pFromColor = new RgbColorClass();
                pFromColor.Red = 255; //红色
                pFromColor.Green = 0;
                pFromColor.Blue = 0;

                RgbColorClass pToColor = new RgbColorClass();
                pToColor.Red = 0;
                pToColor.Green = 255; //绿色
                pToColor.Blue = 0;

                bool ok = false;

                AlgorithmicColorRampClass acrc = new AlgorithmicColorRampClass();
                acrc.Size = 255;
                acrc.FromColor = pFromColor as IColor;
                acrc.ToColor = pToColor as IColor;
                acrc.CreateRamp(out ok);

                RasterStretchColorRampRendererClass scr = new RasterStretchColorRampRendererClass();
                scr.Raster = this.RasterLayerClass.Raster;   //显示图例
                scr.Update();                                //显示图例
                scr.BandIndex = 0;
                scr.ColorRamp = acrc as IColorRamp;
                scr.Update();                                //显示图例

                this.RasterLayerClass.Renderer = scr;

            }
        }

        //渲染评价图
        public void RenderPJT()
        {
            if (this.RasterLayerClass != null)
            {
                RasterUniqueValueRendererClass rUV = new RasterUniqueValueRendererClass();

                rUV.Raster = this.RasterLayerClass.Raster;
                rUV.Update();
                rUV.HeadingCount = 1;
                rUV.set_ClassCount(0, 4);
                rUV.Field = "Value";

                //不适宜类
                rUV.AddValue(0, 0, 0);
                RgbColorClass fc1 = new RgbColorClass();
                fc1.Red = 255; //红色
                fc1.Green = 0;
                fc1.Blue = 0;
                rUV.set_Label(0, 0, "不适宜类");
                IFillSymbol cs1 = new SimpleFillSymbol();
                cs1.Color = fc1 as IColor;
                rUV.set_Symbol(0, 0, cs1 as ISymbol);

                //基本适宜类
                rUV.AddValue(0, 1, 1);
                RgbColorClass fc2 = new RgbColorClass();
                fc2.Red = 255; //
                fc2.Green = 128;
                fc2.Blue = 64;
                rUV.set_Label(0, 1, "基本适宜类");
                IFillSymbol cs2 = new SimpleFillSymbol();
                cs2.Color = fc2 as IColor;
                rUV.set_Symbol(0, 1, cs2 as ISymbol);

                //中度适宜类
                rUV.AddValue(0, 2, 2);
                RgbColorClass fc3 = new RgbColorClass();
                fc3.Red = 141; //
                fc3.Green = 233;
                fc3.Blue = 86;
                rUV.set_Label(0, 2, "中度适宜类");
                IFillSymbol cs3 = new SimpleFillSymbol();
                cs3.Color = fc3 as IColor;
                rUV.set_Symbol(0, 2, cs3 as ISymbol);

                //高度适宜类
                rUV.AddValue(0, 3, 3);
                RgbColorClass fc4 = new RgbColorClass();
                fc4.Red = 0; //
                fc4.Green = 128;
                fc4.Blue = 0;
                rUV.set_Label(0, 3, "高度适宜类");
                IFillSymbol cs4 = new SimpleFillSymbol();
                cs4.Color = fc4 as IColor;
                rUV.set_Symbol(0, 3, cs4 as ISymbol);

                this.RasterLayerClass.Renderer = rUV;
            }
        }

        public object[] Unique(ITable table, string fieldName, string whereClause)
        {
            int index = table.FindField(fieldName);
            if (index == -1)
            {
                throw new Exception("Invaild field name!");
            }

            ICursor cursor = table.Search(null, false);

            ArrayList uniqueVals = new ArrayList();
            IRow row = cursor.NextRow();
            while (row != null)
            {
                if (uniqueVals.Contains(row.get_Value(index)) == false)
                {
                    uniqueVals.Add(row.get_Value(index));
                }
                row = cursor.NextRow();
            }

            return uniqueVals.ToArray();
        }
        public IQueryDef CreateQuery(ITable table, string whereClause)
        {
            IDataset dataset = table as IDataset;
            IFeatureWorkspace featureWorkspace = dataset.Workspace as IFeatureWorkspace;
            IQueryDef query = featureWorkspace.CreateQueryDef();

            query.Tables = dataset.Name;

            whereClause = whereClause.Trim();
            if (whereClause.Length != 0)
            {
                query.WhereClause = whereClause;
            }
            return query;
        }
    }
    //

    public class ConverageFileOperator : LocalShapeFileOperator
    {
        //PCCoverageWorkspaceFactoryClass
        public override IFeatureClass getIFeatureClass()
        {
            string dir = this.getDir(this.LocalShapePathFileName);
            if (System.IO.Directory.Exists(dir))
            {
                dir = this.getDir(this.LocalShapePathFileName);
                string name = this.getFileNameEx(this.LocalShapePathFileName);

                ESRI.ArcGIS.esriSystem.IPropertySet propertySet = new ESRI.ArcGIS.esriSystem.PropertySetClass();
                // path to coverage workspace directory        
                propertySet.SetProperty("DATABASE", dir);

                PCCoverageWorkspaceFactoryClass objwsf = new PCCoverageWorkspaceFactoryClass();
                //propertySet =objwsf.ReadConnectionPropertiesFromFile(this.LocalShapePathFileName);
                IWorkspace objws = objwsf.Open(propertySet, 0);  //dir

                IFeatureWorkspace feaureWS = objws as IFeatureWorkspace;

                if (feaureWS != null)
                {
                    return feaureWS.OpenFeatureClass(name);
                }
            }

            return null;
        }
    }
}
