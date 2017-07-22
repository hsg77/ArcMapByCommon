using System;
using System.Collections.Generic;
using System.Text;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Display;
using System.Windows.Forms;
using System.Runtime.CompilerServices;
using System.Drawing;

namespace ArcMapByJBNT
{       
    public abstract class ZhFeature : IZhFeature
    {

        private IFeature mpFeature = null;
        public IFeature pFeature
        {
            get { return mpFeature; }
            set { mpFeature = value; }
        }

        protected ITopologicalOperator Topoperator = null;
        protected ITopologicalOperator2 Topoperator2 = null;
        protected IRelationalOperator RelationalOperator = null;

        private List<string> FeatureFieldNames = new List<string>();
        public List<string> FieldNames
        {
            get { return FeatureFieldNames; }
            set { FeatureFieldNames = value; }
        }

        public string AreaFieldName
        {
            get
            {
                if (this.mpFeature != null && this.mpFeature.Class is IFeatureClass)
                {
                    IFeatureClass feaClass = this.mpFeature.Class as IFeatureClass;
                    if (feaClass != null && feaClass.AreaField != null)
                        return feaClass.AreaField.Name;
                }

                if (mpFeature != null)
                {
                    if (mpFeature.Fields.FindField("Shape_Area") != -1)
                        return "Shape_Area";
                }

                return string.Empty;
            }
        }

        private object mTag = null;
        public object Tag
        {
            get { return mTag; }
            set { mTag = value; }
        }

        private double mPercent = 0.0;
        public double Percent
        {
            get { return mPercent; }
            set { mPercent = value; }
        }

        //--线宽度或点半径
        private double mdistance = 0.0;
        public double distance
        {
            get { return mdistance; }
            set { mdistance = value; }
        }
        //--

        public ZhFeature()
        {
        }

        public ZhFeature(IFeature feature)
        {
            this.pFeature = feature;
        }

        /// <summary>
        /// 独立存在于内存中的IFeature实例对象
        /// </summary>
        /// <param name="Geometry"></param>
        public ZhFeature(IGeometry Geometry)
        {
            this.ZHGeometry = Geometry;
        }

        /// <summary>
        /// IGeometry内存对象
        /// </summary>
        private IGeometry mZHGeometry = null;
        public IGeometry ZHGeometry
        {
            get
            {
                return this.mZHGeometry;
            }
            set
            {
                this.mZHGeometry = value;
            }
        }

        public abstract int getZHFeatureType { get; }

        public virtual void AnalyseLogic(IGeometry MidGeo, ref double Area)
        {
            Area = 0.0;
        }
        public virtual void AnalyseLogicTK(IGeometry MidGeo, ref double Area)
        {
            Area = 0.0;
        }

        public virtual IGeometry IntersectsSameDim(IGeometry pGeo, esriGeometryDimension pesriGeometryDimension)
        {
            IGeometry retGeo = null;
            retGeo = this.IntersectsSameDim(this.pFeature.Shape, pGeo, pesriGeometryDimension);
            return retGeo;
        }
        public virtual IGeometry IntersectsSameDim(IGeometry byGeo, IGeometry pGeo, esriGeometryDimension pesriGeometryDimension)
        {
            IGeometry retGeo = null;
            IGeometry TopGeometry1 = byGeo;
            IGeometry TopGeometry2 = pGeo;
            if (TopGeometry1 == null || TopGeometry2 == null || TopGeometry1.IsEmpty == true || TopGeometry2.IsEmpty == true)
            {
                retGeo = null;
                MessageBox.Show("空间叠加有一对象为空", "空间分析错误");
                return retGeo;
            }
            if (TopGeometry1.Dimension != TopGeometry2.Dimension)
            {
                retGeo = null;
                MessageBox.Show("空间叠加两对象维数不同不能空间叠加分析", "空间分析错误");
                return retGeo;
            }
            if (!(TopGeometry1 is ITopologicalOperator) && !(TopGeometry2 is ITopologicalOperator))
            {
                retGeo = null;
                MessageBox.Show("空间叠加有一对象不是高级对象(Point,Polygon,Polyline,MuliPoint,GemetryBag)", "空间分析错误");
                return retGeo;
            }
            Topoperator = (ITopologicalOperator)TopGeometry2;
            Topoperator.Simplify();
            Topoperator = (ITopologicalOperator)TopGeometry1;
            Topoperator.Simplify();
            try
            {
                retGeo = Topoperator.Intersect(TopGeometry2, pesriGeometryDimension);
            }
            catch (Exception ee)
            {
                retGeo = null;
                System.Diagnostics.Debug.WriteLine(ee.ToString());
            }
            return retGeo;
        }

        public virtual IGeometry IntersectsMuliDim(IGeometry pGeo)
        {
            IGeometry retGeo = null;
            retGeo = this.IntersectsMuliDim(this.pFeature.Shape, pGeo);
            return retGeo;
        }
        public virtual IGeometry IntersectsMuliDim(IGeometry byGeo, IGeometry pGeo)
        {

            IGeometry retGeo = null;
            IGeometry TopGeometry1 = byGeo;
            IGeometry TopGeometry2 = pGeo;
            if (TopGeometry1 == null || TopGeometry2 == null || TopGeometry1.IsEmpty == true || TopGeometry2.IsEmpty == true)
            {
                retGeo = null;
                MessageBox.Show("空间叠加有一对象为空", "空间分析错误");
                return retGeo;
            }
            if (!(TopGeometry1 is ITopologicalOperator2) && !(TopGeometry2 is ITopologicalOperator2))
            {
                retGeo = null;
                MessageBox.Show("空间叠加有一对象不是高级对象(Polygon,Polyline,MuliPoint)", "空间分析错误");
                return retGeo;
            }

            try
            {
                Topoperator2 = (ITopologicalOperator2)TopGeometry2;
                Topoperator2.Simplify();
                Topoperator2 = (ITopologicalOperator2)TopGeometry1;
                Topoperator2.Simplify();

                retGeo = Topoperator2.IntersectMultidimension(TopGeometry2);
            }
            catch (Exception ee)
            {
                retGeo = null;
                System.Diagnostics.Debug.WriteLine(ee.ToString());
                //ZH.Error.Write.Line("ZHFeature.IntersectsMuliDim():<Topoperator2.IntersectMultidimension>出错:<" + this.Tag.ToString() + ">", ee.Message);
            }
            return retGeo;
        }

        public virtual bool IsIntersects(ZhFeature uZHFeature)
        {
            bool rbc = false;
            rbc = this.IsIntersects(uZHFeature.pFeature.Shape);
            return rbc;
        }
        public virtual bool IsIntersects(IGeometry uGeometry)
        {
            return IsIntersects(this.pFeature.ShapeCopy, uGeometry);
        }
        public virtual bool IsIntersects(IGeometry byGeometry, IGeometry uGeometry)
        {
            bool rbc = false;
            IGeometry mGeo = null;
            if (byGeometry.Dimension == uGeometry.Dimension)
            {
                #region SameDimension 同维操作
                switch (uGeometry.Dimension)
                {
                    case esriGeometryDimension.esriGeometry2Dimension:  //面
                        this.RelationalOperator = byGeometry as IRelationalOperator;
                        rbc = this.RelationalOperator.Overlaps(uGeometry);
                        if (rbc == false)
                        {
                            rbc = this.RelationalOperator.Equals(uGeometry);
                            if (rbc == false)
                            {
                                rbc = this.RelationalOperator.Contains(uGeometry);
                                if (rbc == false)
                                {
                                    rbc = this.RelationalOperator.Within(uGeometry);
                                    if (rbc == false)
                                    {
                                        rbc = this.RelationalOperator.Touches(uGeometry);
                                        if (rbc == false)
                                        {
                                            rbc = this.RelationalOperator.Disjoint(uGeometry);
                                        }
                                    }
                                }
                            }
                        }
                        this.RelationalOperator = null;
                        break;
                    case esriGeometryDimension.esriGeometry1Dimension:  //线
                        this.RelationalOperator = byGeometry as IRelationalOperator;
                        rbc = this.RelationalOperator.Crosses(uGeometry);
                        if (rbc == false)
                        {
                            rbc = this.RelationalOperator.Overlaps(uGeometry);
                            if (rbc == false)
                            {
                                rbc = this.RelationalOperator.Contains(uGeometry);
                                if (rbc == false)
                                {
                                    rbc = this.RelationalOperator.Within(uGeometry);
                                    if (rbc == false)
                                    {
                                        rbc = this.RelationalOperator.Equals(uGeometry);
                                        if (rbc == false)
                                        {
                                            rbc = this.RelationalOperator.Touches(uGeometry);
                                            if (rbc == false)
                                            {
                                                rbc = this.RelationalOperator.Disjoint(uGeometry);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        this.RelationalOperator = null;
                        break;
                    case esriGeometryDimension.esriGeometry0Dimension:  //点
                        this.RelationalOperator = byGeometry as IRelationalOperator;
                        rbc = this.RelationalOperator.Within(uGeometry);
                        if (rbc == false)
                        {
                            rbc = this.RelationalOperator.Contains(uGeometry);
                            if (rbc == false)
                            {
                                rbc = this.RelationalOperator.Equals(uGeometry);
                                if (rbc == false)
                                {
                                    rbc = this.RelationalOperator.Overlaps(uGeometry);
                                    if (rbc == false)
                                    {
                                        rbc = this.RelationalOperator.Disjoint(uGeometry);
                                    }
                                }
                            }
                        }
                        this.RelationalOperator = null;
                        break;
                    default:
                        this.RelationalOperator = byGeometry as IRelationalOperator;
                        rbc = this.RelationalOperator.Disjoint(uGeometry);
                        this.RelationalOperator = null;
                        break;
                }
                #endregion
            }
            else
            {
                #region MuliDimension 异维操作
                switch (uGeometry.Dimension)
                {
                    case esriGeometryDimension.esriGeometry2Dimension:  //面    //面  同维已解决
                        switch (byGeometry.Dimension)
                        {
                            case esriGeometryDimension.esriGeometry0Dimension:  //点
                                this.RelationalOperator = byGeometry as IRelationalOperator;
                                rbc = this.RelationalOperator.Touches(uGeometry);
                                if (rbc == false)
                                {
                                    rbc = this.RelationalOperator.Within(uGeometry);
                                    if (rbc == false)
                                    {
                                        rbc = this.RelationalOperator.Disjoint(uGeometry);
                                    }
                                }
                                this.RelationalOperator = null;
                                break;
                            case esriGeometryDimension.esriGeometry1Dimension:  //线
                                mGeo = this.IntersectsMuliDim(uGeometry);
                                if (mGeo != null)
                                {
                                    rbc = true;
                                }
                                else
                                {
                                    rbc = false;
                                }
                                break;
                        }
                        break;
                    case esriGeometryDimension.esriGeometry1Dimension:  //线    //线 同维已解决
                        switch (byGeometry.Dimension)
                        {
                            case esriGeometryDimension.esriGeometry0Dimension:  //点
                                this.RelationalOperator = byGeometry as IRelationalOperator;
                                rbc = this.RelationalOperator.Within(uGeometry);
                                if (rbc == false)
                                {
                                    rbc = this.RelationalOperator.Touches(uGeometry);
                                    if (rbc == false)
                                    {
                                        rbc = this.RelationalOperator.Disjoint(uGeometry);
                                    }
                                }
                                this.RelationalOperator = null;
                                break;
                            case esriGeometryDimension.esriGeometry2Dimension:  //面
                                mGeo = this.IntersectsMuliDim(uGeometry);
                                if (mGeo != null)
                                {
                                    rbc = true;
                                }
                                else
                                {
                                    rbc = false;
                                }
                                break;
                        }
                        break;
                    case esriGeometryDimension.esriGeometry0Dimension:  //点  //点 同维已解决
                        switch (byGeometry.Dimension)
                        {
                            case esriGeometryDimension.esriGeometry0Dimension:  //线
                                this.RelationalOperator = byGeometry as IRelationalOperator;
                                rbc = this.RelationalOperator.Contains(uGeometry);
                                if (rbc == false)
                                {
                                    rbc = this.RelationalOperator.Touches(uGeometry);
                                    if (rbc == false)
                                    {
                                        rbc = this.RelationalOperator.Disjoint(uGeometry);
                                    }
                                }
                                this.RelationalOperator = null;
                                break;
                            case esriGeometryDimension.esriGeometry2Dimension:  //面
                                this.RelationalOperator = byGeometry as IRelationalOperator;
                                rbc = this.RelationalOperator.Contains(uGeometry);
                                if (rbc == false)
                                {
                                    rbc = this.RelationalOperator.Touches(uGeometry);
                                    if (rbc == false)
                                    {
                                        rbc = this.RelationalOperator.Disjoint(uGeometry);
                                    }
                                }
                                this.RelationalOperator = null;
                                break;
                        }
                        break;
                    default:
                        this.RelationalOperator = byGeometry as IRelationalOperator;
                        rbc = this.RelationalOperator.Disjoint(uGeometry);
                        this.RelationalOperator = null;
                        break;
                }
                #endregion
            }
            return rbc;
        }

        public virtual IGeometry Intersects(IGeometry byGeometry, IGeometry uGeometry)
        {
            //bool rbc = false;
            IGeometry pGeo = null;
            if (byGeometry.Dimension == uGeometry.Dimension)
            {
                pGeo = this.IntersectsSameDim(byGeometry, uGeometry, uGeometry.Dimension);
            }
            else
            {
                pGeo = this.IntersectsMuliDim(byGeometry, uGeometry);
            }
            return pGeo;
        }
        public virtual IGeometry Intersects(IGeometry uGeometry)
        {
            //bool rbc = false;
            IGeometry pGeo = null;
            if (this.pFeature.Shape.Dimension == uGeometry.Dimension)
            {
                pGeo = this.IntersectsSameDim(uGeometry, uGeometry.Dimension);
            }
            else
            {
                pGeo = this.IntersectsMuliDim(uGeometry);
            }
            return pGeo;
        }

        public IGeometry GetBufferByGeometry(IGeometry pGeometry, double distance)
        {
            IGeometry oGeo = null;
            if (pGeometry is ITopologicalOperator)
            {
                ITopologicalOperator TopOper = pGeometry as ITopologicalOperator;
                TopOper.Simplify();
                oGeo = TopOper.Buffer(distance);
            }
            return oGeo;
        }

        public virtual IGeometry GetBufferedGeometry()
        {
            return this.pFeature.ShapeCopy;
        }

        //使用本要素IGeometry进行分割
        public virtual void Cut(IPolyline cutter, out IGeometry leftGeom, out IGeometry rightGeom)
        {
            this.Topoperator = this.pFeature.ShapeCopy as ITopologicalOperator;
            this.Topoperator.Cut(cutter, out leftGeom, out rightGeom);
        }
        public virtual void Cut(IGeometry cutter, out IGeometry leftGeom, out IGeometry rightGeom)
        {
            this.Topoperator = this.pFeature.ShapeCopy as ITopologicalOperator;
            IPolyline pcutter = cutter as IPolyline;
            this.Topoperator.Cut(pcutter, out leftGeom, out rightGeom);
        }
        public virtual IGeometry[] Cut(IGeometry Cutter)
        {
            List<IGeometry> GeoList = new List<IGeometry>();
            GeoList.Clear();
            IGeometry leftGeom = null;
            IGeometry rightGeom = null;
            this.Cut(Cutter, out leftGeom, out rightGeom);
            if (leftGeom != null && leftGeom.IsEmpty != true)
            {
                GeoList.Add(leftGeom);
            }
            if (rightGeom != null && rightGeom.IsEmpty != true)
            {
                GeoList.Add(rightGeom);
            }
            return GeoList.ToArray();
        }
        //使用外部要素IGeometry进行分割
        public virtual void Cut(IGeometry GeoByCut, IPolyline cutter, out IGeometry leftGeom, out IGeometry rightGeom)
        {
            ITopologicalOperator Top = GeoByCut as ITopologicalOperator;
            Top.Cut(cutter, out leftGeom, out rightGeom);
        }
        public virtual void Cut(IGeometry GeoByCut, IGeometry cutter, out IGeometry leftGeom, out IGeometry rightGeom)
        {
            ITopologicalOperator Top = cutter as ITopologicalOperator;
            Top.Simplify();
            IPolyline pcutter = Top.Boundary as IPolyline;

            Top = GeoByCut as ITopologicalOperator;
            try
            {
                Top.Cut(pcutter, out leftGeom, out rightGeom);
            }
            catch (Exception ee)
            {
                leftGeom = null;
                rightGeom = null;
                System.Diagnostics.Debug.WriteLine(ee.ToString());
            }
        }
        public virtual IGeometry[] Cut(IGeometry GeoByCut, IGeometry Cutter)
        {
            List<IGeometry> GeoList = new List<IGeometry>();
            GeoList.Clear();
            try
            {
                IGeometry leftGeom = null;
                IGeometry rightGeom = null;
                this.Cut(GeoByCut, Cutter, out leftGeom, out rightGeom);

                if (leftGeom != null && leftGeom.IsEmpty != true)
                {
                    GeoList.Add(leftGeom);
                }
                if (rightGeom != null && rightGeom.IsEmpty != true)
                {
                    GeoList.Add(rightGeom);
                }
                if (leftGeom == null && rightGeom == null)
                {
                    GeoList.Add(GeoByCut);
                }
                if ((leftGeom != null && leftGeom.IsEmpty == true) && (rightGeom != null && rightGeom.IsEmpty == true))
                {
                    GeoList.Add(GeoByCut);
                }

            }
            catch (Exception ee)
            {
                GeoList.Clear();
                System.Diagnostics.Debug.WriteLine(ee.ToString());
            }
            return GeoList.ToArray();
        }

        /// <summary>
        ///  source与other 对象比较不同的地方
        ///  return source-other;
        /// </summary>
        /// <param name="source"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        public virtual IGeometry[] Erase(IGeometry source, IGeometry other)
        {
            ITopologicalOperator topoOper = source as ITopologicalOperator;
            if (!topoOper.IsSimple)
            {
                topoOper.Simplify();
            }

            IGeometry geo = topoOper.Difference(other);
            topoOper = geo as ITopologicalOperator;
            if (!topoOper.IsSimple)
            {
                topoOper.Simplify();
            }

            return new IGeometry[] { geo };
        }

        public virtual IGeometry[] Erase(IGeometry other)
        {
            return Erase(this.mpFeature.ShapeCopy, other);
        }

        /// <summary>
        /// 拷贝属性字段 this.Fields->tpObjectZHFeature.Fields
        /// </summary>
        /// <param name="tpObjectZHFeature"></param>
        public virtual void CopyField(ref ZhFeature tpObjectZHFeature)
        {
            //属性
            this.preGetFeatureFieldNames();
            foreach (string fdName in this.FieldNames)
            {
                if (fdName != "OBJECTID" &&
                    fdName != "FID" &&
                    this.getFieldValue(fdName) != null)
                {
                    tpObjectZHFeature.setFieldValue(fdName, this.getFieldValue(fdName));
                }
            }
        }
        /// <summary>
        /// 拷贝属性字段 this.Fields->tpObjectFeature.Fields
        /// </summary>
        /// <param name="tpObjectZHFeature"></param>
        public virtual void CopyField(ref IFeature tpObjectFeature)
        {
            //属性
            this.preGetFeatureFieldNames();
            ZHFeaturePoint tpObjectZHFeature = new ZHFeaturePoint(tpObjectFeature);
            foreach (string fdName in this.FieldNames)
            {
                if (fdName != "OBJECTID" &&
                    fdName != "FID" &&
                    this.getFieldValue(fdName) != null)
                {
                    tpObjectZHFeature.setFieldValue(fdName, this.getFieldValue(fdName));
                }
            }
        }
        /// <summary>
        ///  拷贝属性字段 this.Fields->tpObjectFeatureBuffer.Fields
        /// </summary>
        /// <param name="tpObjectFeatureBuffer"></param>
        public virtual void CopyField(ref IFeatureBuffer tpObjectFeatureBuffer)
        {
            //属性
            this.preGetFeatureFieldNames();
            ZhFeatureBuffer tpObjectZHFeatureBuffer = new ZhFeatureBuffer(tpObjectFeatureBuffer);
            foreach (string fdName in this.FieldNames)
            {
                if (fdName != "OBJECTID" && fdName != "FID" && this.getFieldValue(fdName) != null)
                {
                    tpObjectZHFeatureBuffer.setFieldValue(fdName, this.getFieldValue(fdName));
                }
            }
        }
        /// <summary>
        ///  拷贝属性字段 this.Fields->tpObjectZhFeatureBuffer.Fields
        /// </summary>
        /// <param name="tpObjectZhFeatureBuffer"></param>
        public virtual void CopyField(ref ZhFeatureBuffer tpObjectZhFeatureBuffer)
        {
            //属性
            this.preGetFeatureFieldNames();
            foreach (string fdName in this.FieldNames)
            {
                if (fdName != "OBJECTID" && fdName != "FID" && this.getFieldValue(fdName) != null)
                {
                    tpObjectZhFeatureBuffer.setFieldValue(fdName, this.getFieldValue(fdName));
                }
            }
        }

        /// <summary>
        /// 预处理获取所有字段操作 存入this.FieldNames中
        /// </summary>
        public virtual void preGetFeatureFieldNames()
        {
            this.FieldNames.Clear();
            string OIDFieldName = (this.pFeature.Class as IFeatureClass).OIDFieldName;
            string GeometryFieldName = (this.pFeature.Class as IFeatureClass).ShapeFieldName;
            //
            OIDFieldName = OIDFieldName.ToUpper();
            GeometryFieldName = GeometryFieldName.ToUpper();
            //           
            string FName = "";
            for (int i = 0; i < this.pFeature.Fields.FieldCount; i++)
            {
                FName = this.pFeature.Fields.get_Field(i).Name.ToString();
                if (FName.ToUpper().Trim() == OIDFieldName ||
                    FName.ToUpper().Trim() == GeometryFieldName ||
                    FName.ToUpper().Trim() == GeometryFieldName + ".LEN" ||
                    FName.ToUpper().Trim() == GeometryFieldName + ".LENG" ||
                    FName.ToUpper().Trim() == GeometryFieldName + ".AREA" ||
                    FName.ToUpper().Trim() == GeometryFieldName + "_LENGTH" ||
                    FName.ToUpper().Trim() == GeometryFieldName + "_LENG" ||
                    FName.ToUpper().Trim() == GeometryFieldName + "_AREA")
                {
                }
                else
                {
                    this.FieldNames.Add(FName);
                }
            }
        }
        /// <summary>
        /// 图形面积return (pFeature.Shape as IArea).Area   else 0;
        /// </summary>
        public virtual double GeometryArea
        {
            get
            {
                IArea area = this.pFeature.Shape as IArea;
                if (area != null)
                {
                    return area.Area;
                }
                return 0.0;
            }
        }
        /// <summary>
        /// 图形长度 return  this.pFeature.Shape as ICurve.Length  else 0.0;
        /// </summary>
        public virtual double GeometryLength
        {
            get
            {
                ICurve curve = this.pFeature.Shape as ICurve;
                if (curve != null)
                {
                    return curve.Length;
                }
                return 0.0;
            }
        }

        public virtual object getFieldValue(string FieldName)
        {
            #region information
            object rbc = "";
            int colindex = this.pFeature.Fields.FindField(FieldName);
            if (colindex >= 0)
            {
                rbc = RuntimeHelpers.GetObjectValue(this.pFeature.get_Value(colindex));
            }
            return rbc;
            #endregion
        }
        public virtual string getObjectID()
        {
            string rbc = "-1";
            if (this.pFeature.HasOID == true)
            {
                rbc = this.pFeature.OID.ToString();
            }
            return rbc;
        }
        public virtual void setFieldValue(string FieldName, object value)
        {
            #region information
            int colindex = this.pFeature.Fields.FindField(FieldName);
            if (colindex >= 0)
            {
                if (value is System.DBNull)
                {
                }
                else
                {
                    this.pFeature.set_Value(colindex, value);
                }
            }
            #endregion
        }
        public virtual void SaveFeature()
        {
            if (this.pFeature != null)
            {
                this.pFeature.Store();
            }
        }

        /// <summary>
        /// 是OID字段吗？
        /// </summary>
        /// <param name="FieldName"></param>
        /// <returns></returns>
        public virtual bool IsOIDField(string FieldName)
        {
            bool rbc = false;
            if (this.pFeature != null)
            {
                if (this.pFeature.Class.OIDFieldName == FieldName)
                {
                    rbc = true;
                }
            }
            return rbc;
        }
        /// <summary>
        /// 求最近距离
        /// ZHFeature.pFeature.ShapeCope 与pGeometry之间的最近距离
        /// </summary>
        /// <param name="pGeometry"></param>
        /// <returns></returns>
        public double NearestDistance(IGeometry pGeometry)
        {
            double Distance = 0;
            IProximityOperator prox = null;
            if (this.pFeature.ShapeCopy is IProximityOperator)
            {
                prox = this.pFeature.ShapeCopy as IProximityOperator;
                if (pGeometry != null && pGeometry is IProximityOperator)
                {
                    Distance = prox.ReturnDistance(pGeometry);
                }
            }
            return Distance;
        }

        #region IZHFeature 成员


        public bool setSelfMaxMinXYValue()
        {
            if (this.pFeature == null)
            {
                return false;
            }

            if (this.pFeature.Shape.GeometryType == esriGeometryType.esriGeometryPolyline ||
                this.pFeature.Shape.GeometryType == esriGeometryType.esriGeometryPoint ||
                this.pFeature.Shape.GeometryType == esriGeometryType.esriGeometryPolygon ||
                this.pFeature.FeatureType == esriFeatureType.esriFTAnnotation)
            {

                this.setFieldValue("XMIN", this.pFeature.Shape.Envelope.XMin);
                this.setFieldValue("XMAX", this.pFeature.Shape.Envelope.XMax);
                this.setFieldValue("YMIN", this.pFeature.Shape.Envelope.YMin);
                this.setFieldValue("YMAX", this.pFeature.Shape.Envelope.YMax);

                return true;
            }
            else
            {
                return false;
            }
        }

        public virtual bool Move(double dx, double dy)
        {
            bool rbc = false;
            if (this.pFeature != null)
            {
                if (this.pFeature is IAnnotationFeature)
                {   //是注记要素
                    IAnnotationFeature anFeat = this.pFeature as IAnnotationFeature;
                    IElement TextEl = (IElement)((IClone)anFeat.Annotation).Clone();
                    ITransform2D trans = TextEl as ITransform2D;
                    if (trans != null)
                    {
                        trans.Move(dx, dy);
                    }
                    anFeat.Annotation = TextEl;
                }
                else
                {   //是一般要素
                    IGeometry g = this.pFeature.ShapeCopy;
                    if (g is ITransform2D)
                    {
                        (g as ITransform2D).Move(dx, dy);
                        this.pFeature.Shape = g;
                        rbc = true;
                    }
                }
            }
            return rbc;
        }

        public virtual bool Zoom(double zoom) //???未写完
        {
            bool rbc = false;
            if (this.pFeature != null)
            {
                if (this.pFeature is IAnnotationFeature)
                {   //是注记要素
                    IAnnotationFeature anFeat = this.pFeature as IAnnotationFeature;
                    IElement TextEl = (IElement)((IClone)anFeat.Annotation).Clone();
                    ITransform2D trans = TextEl as ITransform2D;
                    //trans.Move(dx, dy);
                    anFeat.Annotation = TextEl;
                }
                else
                {   //是一般要素
                    //if (this.pFeature.Shape.GeometryType== esriGeometryType.esriGeometryPolygon)
                    //{
                    double x = 0;
                    double y = 0;
                    IPoint p = null;
                    IGeometry geo = this.pFeature.ShapeCopy;
                    IPointCollection pn = geo as IPointCollection;
                    for (int i = 0; i < pn.PointCount; i++)
                    {
                        p = pn.get_Point(i);
                        x = p.X;
                        y = p.Y;
                        //???
                    }
                    rbc = true;
                    //}
                }
            }
            return rbc;
        }


        #endregion

        #region static 方法
        /// <summary>
        /// 设置字段值
        /// </summary>
        /// <param name="feature"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        static public bool SetFeatureField(IFeature feature, string field, object value)
        {
            if (feature == null || field == null || value == null)
            {
                return false;
            }
            int index = -1;
            index = feature.Fields.FindField(field);
            if (index >= 0)
            {
                feature.set_Value(index, value);
            }
            else
            {
                return false;
            }
            return true;
        }

        static public object GetFeatureField(IFeature feature, string field)
        {
            if (feature == null || field == null)
            {
                return null;
            }
            int index = -1;
            index = feature.Fields.FindField(field);
            if (index >= 0)
            {
                return (feature.get_Value(index));
            }
            else
            {
                return null;
            }
        }

        static public bool SetMinMaxValue(IFeature feature)
        {
            if (feature == null)
            {
                return false;
            }
            if (feature.FeatureType == esriFeatureType.esriFTAnnotation)
            {
                return false;
            }
            if (feature.Shape.GeometryType == esriGeometryType.esriGeometryPolyline ||
                feature.Shape.GeometryType == esriGeometryType.esriGeometryPoint ||
                feature.Shape.GeometryType == esriGeometryType.esriGeometryPolygon)
            {

                int index = feature.Fields.FindField("XMIN");
                if (index >= 0)
                {
                    feature.set_Value(index, (object)feature.Shape.Envelope.XMin);
                }
                else
                {
                    return false;
                }
                index = feature.Fields.FindField("XMAX");
                if (index >= 0)
                {
                    feature.set_Value(index, (object)feature.Shape.Envelope.XMax);
                }
                else
                {
                    return false;
                }
                index = feature.Fields.FindField("YMIN");
                if (index >= 0)
                {
                    feature.set_Value(index, (object)feature.Shape.Envelope.YMin);
                }
                else
                {
                    return false;
                }
                index = feature.Fields.FindField("YMAX");
                if (index >= 0)
                {
                    feature.set_Value(index, (object)feature.Shape.Envelope.YMax);
                }
                else
                {
                    return false;
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        static public bool Simplify(IFeature feature)
        {
            bool rbc = false;
            if (feature.Shape != null && feature.Shape is ITopologicalOperator)
            {
                (feature.Shape as ITopologicalOperator).Simplify();
                rbc = true;
            }
            return rbc;
        }

        static public int GetObjectID(IFeature feat)
        {
            int rbc = -1;
            if (feat.HasOID == true)
            {
                rbc = feat.OID;
            }
            return rbc;
        }

        static public bool Move(IGeometry byMoveGeo, double dx, double dy)
        {
            bool rbc = false;
            if (byMoveGeo != null)
            {
                if (byMoveGeo is ITransform2D)
                {
                    (byMoveGeo as ITransform2D).Move(dx, dy);
                    rbc = true;
                }
            }
            return rbc;
        }

        static public IColor GetColor(Color tpColor)
        {
            return TokayWorkspace.GetColor(tpColor);
        }
        static public stdole.IFontDisp GetFontDisp(string fontName, float fontSize)
        {
            return TokayWorkspace.GetFontDisp(fontName, fontSize);
        }
        #endregion

    }

    public interface IZhFeature
    {
        void AnalyseLogic(IGeometry MidGeo, ref double Area);
        void AnalyseLogicTK(IGeometry MidGeo, ref double Area);
        void CopyField(ref ZhFeature tpObjectZHFeature);
        void Cut(IPolyline cutter, out IGeometry leftGeom, out IGeometry rightGeom);
        void Cut(IGeometry GeoByCut, IPolyline cutter, out IGeometry leftGeom, out IGeometry rightGeom);
        void Cut(IGeometry cutter, out IGeometry leftGeom, out IGeometry rightGeom);
        IGeometry[] Cut(IGeometry Cutter);
        IGeometry[] Cut(IGeometry GeoByCut, IGeometry Cutter);
        void Cut(IGeometry GeoByCut, IGeometry cutter, out IGeometry leftGeom, out IGeometry rightGeom);
        double distance { get; set; }
        List<string> FieldNames { get; set; }
        double GeometryArea { get; }
        double GeometryLength { get; }
        IGeometry GetBufferedGeometry();
        object getFieldValue(string FieldName);
        int getZHFeatureType { get; }
        string getObjectID();
        IGeometry Intersects(IGeometry uGeometry);
        IGeometry Intersects(IGeometry byGeometry, IGeometry uGeometry);
        IGeometry IntersectsMuliDim(IGeometry pGeo);
        IGeometry IntersectsMuliDim(IGeometry byGeo, IGeometry pGeo);
        IGeometry IntersectsSameDim(IGeometry pGeo, esriGeometryDimension pesriGeometryDimension);
        IGeometry IntersectsSameDim(IGeometry byGeo, IGeometry pGeo, esriGeometryDimension pesriGeometryDimension);
        bool IsIntersects(IGeometry uGeometry);
        bool IsIntersects(ZhFeature uZHFeature);
        bool IsIntersects(IGeometry byGeometry, IGeometry uGeometry);
        double Percent { get; set; }
        IFeature pFeature { get; set; }
        void preGetFeatureFieldNames();
        void SaveFeature();
        void setFieldValue(string FieldName, object value);
        object Tag { get; set; }

        IGeometry ZHGeometry { get; set; }
        double NearestDistance(IGeometry pGeometry);

        bool setSelfMaxMinXYValue();
    }

    public class ZHFeaturePoint : ZhFeature
    {
        public ZHFeaturePoint()
            : base()
        {
        }
        public ZHFeaturePoint(IFeature feature)
            : base(feature)
        {
        }
        public ZHFeaturePoint(IGeometry Geometry)
            : base(Geometry)
        {
        }
        public override int getZHFeatureType
        {
            get { return (int)ZhLayerType.Point; }
        }

        //获取点的缓冲区IGeometry对象
        public override IGeometry GetBufferedGeometry()
        {
            double r = 0;
            try
            {
                r = Convert.ToDouble(this.getFieldValue("YDMJ").ToString()) / 3.14159265358979;
                r = Math.Sqrt(r);
            }
            catch
            {
            }

            return base.GetBufferByGeometry(this.pFeature.Shape, r);
        }
    }

    public class ZHFeaturePolyLine : ZhFeature
    {
        public ZHFeaturePolyLine()
            : base()
        {
        }
        public ZHFeaturePolyLine(IFeature feature)
            : base(feature)
        {
        }
        public ZHFeaturePolyLine(IGeometry Geometry)
            : base(Geometry)
        {
        }
        public override int getZHFeatureType
        {
            get { return (int)ZhLayerType.PolyLine; }
        }

        //获取线的缓冲区IGeometry对象
        public override IGeometry GetBufferedGeometry()
        {
            double r = 0;
            double kd = 0;
            try
            {
                r = Convert.ToDouble(this.getFieldValue("YDMJ").ToString());
                double len = (this.pFeature.ShapeCopy as ICurve).Length;
                if (len > 0)
                {
                    kd = r / len;
                }
                //r = Convert.ToDouble(this.getFieldValue("DWKD").ToString());  //??
            }
            catch
            {

            }

            return base.GetBufferByGeometry(this.pFeature.ShapeCopy, kd);
        }
    }

    public class ZHFeaturePolygon : ZhFeature
    {
        public ZHFeaturePolygon()
            : base()
        {
        }
        public ZHFeaturePolygon(IFeature feature)
            : base(feature)
        {
        }
        public ZHFeaturePolygon(IGeometry Geometry)
            : base(Geometry)
        {
        }
        public override int getZHFeatureType
        {
            get { return (int)ZhLayerType.Polygon; }
        }
    }
    public class ZHFeatureAnnotation : ZhFeature
    {
        public ZHFeatureAnnotation()
            : base()
        {
        }
        public ZHFeatureAnnotation(IFeature feature)
            : base(feature)
        {
        }
        public ZHFeatureAnnotation(IGeometry Geometry)
            : base(Geometry)
        {
        }
        public override int getZHFeatureType
        {
            get { return (int)ZhLayerType.Annotation; }
        }

        public override bool Move(double dx, double dy)
        {
            bool rbc = false;
            IAnnotationFeature anFeat = this.pFeature as IAnnotationFeature;
            IElement TextEl = (IElement)((IClone)anFeat.Annotation).Clone();
            ITransform2D trans = TextEl as ITransform2D;
            trans.Move(dx, dy);
            anFeat.Annotation = TextEl;
            rbc = true;
            return rbc;
        }
    }
}
