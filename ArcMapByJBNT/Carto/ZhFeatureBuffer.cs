using System;
using System.Collections.Generic;
using System.Text;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using System.Runtime.CompilerServices;

namespace ArcMapByJBNT
{    
    public class ZhFeatureBuffer : IZhFeatureBuffer
    {
        private IFeatureBuffer FeatureBuffer = null;
        private IFeatureCursor FeatureCursor = null;

        public ZhFeatureBuffer()
        {
        }

        public ZhFeatureBuffer(IFeatureBuffer pFeatureBuffer)
        {
            this.FeatureBuffer = pFeatureBuffer;
        }

        public ZhFeatureBuffer(IFeatureCursor pFeatureCursor, IFeatureBuffer pFeatureBuffer)
        {
            this.FeatureCursor = pFeatureCursor;
            this.FeatureBuffer = pFeatureBuffer;
        }



        #region IZhFeatureBuffer 成员
        private List<string> FeatureFieldNames = new List<string>();
        public List<string> FieldNames
        {
            get { return FeatureFieldNames; }
            set { FeatureFieldNames = value; }
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

        public virtual double GeometryArea
        {
            get
            {
                IArea area = this.FeatureBuffer.Shape as IArea;
                if (area != null)
                {
                    return area.Area;
                }
                return 0.0;
            }
        }

        public virtual double GeometryLength
        {
            get
            {
                ICurve curve = this.FeatureBuffer.Shape as ICurve;
                if (curve != null)
                {
                    return curve.Length;
                }
                return 0.0;
            }
        }

        protected IGeometry GetBufferByGeometry(IGeometry pGeometry, double distance)
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

        public IGeometry GetBufferedGeometry()
        {
            return this.FeatureBuffer.Shape;
        }

        public object getFieldValue(string FieldName)
        {
            #region information
            object rbc = "";
            int colindex = this.FeatureBuffer.Fields.FindField(FieldName);
            if (colindex >= 0)
            {
                rbc = RuntimeHelpers.GetObjectValue(this.FeatureBuffer.get_Value(colindex));
            }
            return rbc;
            #endregion
        }

        public IFeatureBuffer pFeatureBuffer
        {
            get
            {
                return FeatureBuffer;
            }
            set
            {
                FeatureBuffer = value;
            }
        }

        public void preGetFeatureFieldNames()
        {
            this.FieldNames.Clear();
            string FName = "";
            for (int i = 0; i < this.FeatureBuffer.Fields.FieldCount; i++)
            {
                FName = this.FeatureBuffer.Fields.get_Field(i).Name.ToString();
                if (FName.ToUpper().Trim() == "SHAPE" ||
                    FName.ToUpper().Trim() == "SHAPE.LEN" ||
                    FName.ToUpper().Trim() == "SHAPE.AREA" ||
                    FName.ToUpper().Trim() == "SHAPE_LENGTH" ||
                    FName.ToUpper().Trim() == "SHAPE_AREA")
                {
                }
                else
                {
                    this.FieldNames.Add(FName);
                }
            }
        }

        public void setFieldValue(string FieldName, object value)
        {
            #region information
            int colindex = this.FeatureBuffer.Fields.FindField(FieldName);
            if (colindex >= 0)
            {
                if (value is System.DBNull)
                {
                    this.FeatureBuffer.set_Value(colindex, null);
                }
                else
                {
                    this.FeatureBuffer.set_Value(colindex, value);
                }
            }
            #endregion
        }

        /// <summary>
        /// 拷贝属性字段 this.Fields->tpObjectZHFeature.Fields
        /// </summary>
        /// <param name="tpObjectZHFeature"></param>
        public void CopyField(ref ZhFeature tpObjectZHFeature)
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
        /// 
        /// </summary>
        /// <param name="tpObjectZHFeatureBuffer"></param>
        public void CopyField(ref ZhFeatureBuffer tpObjectZHFeatureBuffer)
        {
            //属性
            this.preGetFeatureFieldNames();
            foreach (string fdName in this.FieldNames)
            {
                if (fdName != "OBJECTID" &&
                    fdName != "FID" &&
                    this.getFieldValue(fdName) != null)
                {
                    tpObjectZHFeatureBuffer.setFieldValue(fdName, this.getFieldValue(fdName));
                }
            }
        }

        public virtual void SaveFeatureBuffer()
        {
            if (this.FeatureCursor != null && this.FeatureBuffer != null)
            {
                this.FeatureCursor.InsertFeature(this.FeatureBuffer);
            }
        }

        #endregion
    }

    public interface IZhFeatureBuffer
    {
        List<string> FieldNames { get; set; }
        object Tag { get; set; }
        double Percent { get; set; }
        double distance { get; set; }


        double GeometryArea { get; }
        double GeometryLength { get; }
        IGeometry GetBufferedGeometry();
        object getFieldValue(string FieldName);

        IFeatureBuffer pFeatureBuffer { get; set; }
        void preGetFeatureFieldNames();
        void setFieldValue(string FieldName, object value);

        void CopyField(ref ZhFeature tpObjectZHFeature);
        void CopyField(ref ZhFeatureBuffer tpObjectZHFeatureBuffer);

        void SaveFeatureBuffer();

    }
}
