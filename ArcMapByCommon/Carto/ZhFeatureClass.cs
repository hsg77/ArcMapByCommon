using System;
using System.Collections.Generic;
using System.Text;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using System.Windows.Forms;

namespace ArcMapByCommon
{    
    public abstract class ZhFeatureClass
    {
        protected IFeatureClass featureClass;
        protected IFeatureCursor featureCursor;
        protected IGeometry pGeometry = null;

        private IWorkspace oWs = null;
        private IWorkspaceEdit iwe = null;
        private string mAreaAnalyseFieldName = "";

        protected ZhFeatureClass(IFeatureClass featureClass)
        {
            this.featureClass = featureClass;
            this.init(this.featureClass);
        }
        public virtual void init(IFeatureClass featureClass)
        {
            if (featureClass != null)
            {   //
                this.SystemFieldsList.Clear();
                string OIDFieldName = featureClass.OIDFieldName;
                string GeometryFieldName = featureClass.ShapeFieldName;
                OIDFieldName = OIDFieldName.ToUpper();            //转为大写
                GeometryFieldName = GeometryFieldName.ToUpper();  //转为大写
                //
                this.SystemFieldsList.Add(GeometryFieldName);
                this.SystemFieldsList.Add(GeometryFieldName + ".LEN");
                this.SystemFieldsList.Add(GeometryFieldName + ".LENG");
                this.SystemFieldsList.Add(GeometryFieldName + ".AREA");
                this.SystemFieldsList.Add(GeometryFieldName + "_AREA");
                this.SystemFieldsList.Add(GeometryFieldName + "_LENGTH");
                this.SystemFieldsList.Add(OIDFieldName);
                if (OIDFieldName != "FID")
                {
                    this.SystemFieldsList.Add("FID");
                }
                if (featureClass.FeatureType == esriFeatureType.esriFTAnnotation)
                {   //注记系统字段
                    this.SystemFieldsList.Add("FeatureID".ToUpper());
                    this.SystemFieldsList.Add("ZOrder".ToUpper());
                    this.SystemFieldsList.Add("AnnotationClassID".ToUpper());
                    this.SystemFieldsList.Add("Element".ToUpper());
                    this.SystemFieldsList.Add("SymbolID".ToUpper());
                    this.SystemFieldsList.Add("Status".ToUpper());
                }
            }
        }

        #region GetFeatures
        public virtual ZhFeature[] GetFeatures(IGeometry geometry, esriSpatialRelEnum pesriSpatialRelEnum)
        {
            //List<ZHFeature> pFeatures = new List<ZHFeature>();
            //try
            //{
            //    pFeatures.Clear();
            if (geometry is ITopologicalOperator)
            {
                (geometry as ITopologicalOperator).Simplify();
            }

            ISpatialFilter psf = new SpatialFilterClass();
            psf.Geometry = geometry;
            psf.GeometryField = this.featureClass.ShapeFieldName;
            psf.SpatialRel = pesriSpatialRelEnum;

            IFeatureCursor pFeatCur = this.featureClass.Search(psf, false);
            return GetFeatures(pFeatCur);
            //    IFeature feat = pFeatCur.NextFeature();
            //    while (feat != null)
            //    {
            //        pFeatures.Add(this.CreateZHFeature(feat));
            //        feat = pFeatCur.NextFeature();
            //    }
            //    feat = null;
            //    pFeatCur = null;
            //    psf = null;
            //}
            //catch
            //{
            //    pFeatures.Clear();
            //}
            //return pFeatures.ToArray();
        }

        public virtual ZhFeature[] GetFeatures(ZhFeature[] zhFeatures, esriSpatialRelEnum pesriSpatialRelEnum)
        {
            List<ZhFeature> pMuliFeatures = new List<ZhFeature>();
            ZhFeature[] pFeatures = null;
            try
            {
                pMuliFeatures.Clear();
                for (int i = 0; i < zhFeatures.Length; i++)
                {
                    pFeatures = this.GetFeatures(zhFeatures[i].GetBufferedGeometry(), pesriSpatialRelEnum);
                    for (int j = 0; j < pFeatures.Length; j++)
                    {
                        if (pMuliFeatures.Contains(pFeatures[j]) == false)
                        {
                            pMuliFeatures.Add(pFeatures[j]);
                        }
                    }
                }
            }
            catch
            {
                pMuliFeatures.Clear();
            }
            pFeatures = null;
            return pMuliFeatures.ToArray();

        }

        public virtual ZhFeature[] GetFeatures(IQueryFilter queryFilter)
        {
            //List<ZHFeature> pFeatures = new List<ZHFeature>();
            //try
            //{
            //    pFeatures.Clear();
            //    if (pGeometry is ITopologicalOperator)
            //    {
            //        (pGeometry as ITopologicalOperator).Simplify();
            //    }
            IFeatureCursor pFeatCur = this.featureClass.Search(queryFilter, false);
            return GetFeatures(pFeatCur);
            //    IFeature feat = pFeatCur.NextFeature();
            //    while (feat != null)
            //    {
            //        pFeatures.Add(this.CreateZHFeature(feat));
            //        feat = pFeatCur.NextFeature();
            //    }
            //    feat = null;
            //    pFeatCur = null;
            //}
            //catch
            //{
            //    pFeatures.Clear();
            //}
            //return pFeatures.ToArray();
        }

        public virtual ZhFeature[] GetFeatures(IFeatureCursor cursor)
        {
            List<ZhFeature> pFeatures = new List<ZhFeature>();
            try
            {
                pFeatures.Clear();

                IFeature feat = cursor.NextFeature();
                while (feat != null)
                {
                    pFeatures.Add(this.CreateFeature(feat));
                    feat = cursor.NextFeature();
                }
                feat = null;
            }
            catch
            {
                pFeatures.Clear();
            }
            return pFeatures.ToArray();
        }

        public virtual ZhFeature[] GetFeatures()
        {
            //List<ZHFeature> pFeatures = new List<ZHFeature>();
            //try
            //{
            //    pFeatures.Clear();

            IFeatureCursor pFeatCur = this.featureClass.Search(null, false);
            return GetFeatures(pFeatCur);
            //    IFeature feat = pFeatCur.NextFeature();
            //    while (feat != null)
            //    {
            //        pFeatures.Add(this.CreateZHFeature(feat));
            //        feat = pFeatCur.NextFeature();
            //    }
            //    feat = null;
            //    System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeatCur);
            //    pFeatCur = null;

            //}
            //catch
            //{
            //    pFeatures.Clear();
            //}
            //return pFeatures.ToArray();

        }
        #endregion

        public virtual ZhFeature[] GetFeaturesNoExtend(IGeometry pGeometry, esriSpatialRelEnum pesriSpatialRelEnum)
        {
            //List<ZHFeature> pFeatures = new List<ZHFeature>();
            //try
            //{
            //    pFeatures.Clear();
            if (pGeometry is ITopologicalOperator)
            {
                (pGeometry as ITopologicalOperator).Simplify();
            }

            ISpatialFilter psf = new SpatialFilterClass();
            psf.Geometry = pGeometry;
            psf.GeometryField = this.featureClass.ShapeFieldName;
            psf.SpatialRel = pesriSpatialRelEnum;

            //属性先搜索，再空间搜索
            psf.SearchOrder = esriSearchOrder.esriSearchOrderAttribute;

            IFeatureCursor pFeatCur = this.featureClass.Search(psf, false);
            return GetFeatures(pFeatCur);
            //    IFeature feat = pFeatCur.NextFeature();
            //    while (feat != null)
            //    {
            //        pFeatures.Add(this.CreateZHFeature(feat));
            //        feat = pFeatCur.NextFeature();
            //    }
            //    feat = null;
            //    pFeatCur = null;
            //    psf = null;
            //}
            //catch
            //{
            //    pFeatures.Clear();
            //}
            //return pFeatures.ToArray();
        }
        //分割面
        public virtual ZhFeature[] SplitPolygon(ZhFeature[] zhFeatures, ZhFeature[] pUsedZHFeatures)
        {
            List<ZhFeature> pMuliFeatures = new List<ZhFeature>();
            List<IGeometry> GeoList = new List<IGeometry>();
            GeoList.Clear();
            IGeometry[] byCutGeoArray = null;
            IGeometry[] cgGeoArray = null;
            //几何对象分割
            foreach (ZhFeature tpByFeat in zhFeatures)
            {
                byCutGeoArray = null;
                cgGeoArray = null;
                GeoList.Clear();
                GeoList.Add(tpByFeat.pFeature.ShapeCopy);
                byCutGeoArray = GeoList.ToArray();
                foreach (ZhFeature tpUsedFeat in pUsedZHFeatures)
                {
                    cgGeoArray = this.MuliCutPolygon(tpByFeat, byCutGeoArray, tpUsedFeat.pFeature.ShapeCopy);
                    byCutGeoArray = null;
                    byCutGeoArray = cgGeoArray;
                }
                //byCutGeoArray
                //清除原对象
                GeoList.Clear();
                //加入分割后对象集合
                foreach (IGeometry ogeo in byCutGeoArray)
                {
                    GeoList.Add(ogeo);
                }

                //创建分割后对象 ZHFeatureByGeoList
                ZhFeature tpFeat = null;
                foreach (IGeometry pGeo in GeoList)
                {
                    tpFeat = this.CreateFeature();
                    //属性拷贝(含GHDM赋码)
                    tpByFeat.CopyField(ref tpFeat);
                    tpFeat.pFeature.Shape = pGeo;
                    //保存
                    tpFeat.pFeature.Store();
                    pMuliFeatures.Add(tpFeat);
                }

            }
            return pMuliFeatures.ToArray();
        }

        private IGeometry[] MuliCutPolygon(ZhFeature tpFeat, IGeometry[] byCutGeoArray, IGeometry tpUsedGeo)
        {
            List<IGeometry> GeoList = new List<IGeometry>();
            GeoList.Clear();
            IGeometry[] tpcgGeoArray = null;
            foreach (IGeometry tpGeo in byCutGeoArray)
            {
                tpcgGeoArray = tpFeat.Cut(tpGeo, tpUsedGeo);
                foreach (IGeometry ogeo in tpcgGeoArray)
                {
                    GeoList.Add(ogeo);
                }
            }
            return GeoList.ToArray();
        }
        //分割线 
        public virtual ZhFeature[] SplitPolyline(ZhFeature[] zhFeatures, ZhFeature[] pUsedZHFeatures)
        {
            List<ZhFeature> pMuliFeatures = new List<ZhFeature>();
            List<IGeometry> GeoList = new List<IGeometry>();
            GeoList.Clear();
            List<IGeometry> CutterGeoList = new List<IGeometry>();
            IPointCollection pMultipoint = null;
            IGeometry[] cgGeoArray = null;
            //几何对象分割
            foreach (ZhFeature tpByFeat in zhFeatures)
            {
                CutterGeoList.Clear();
                cgGeoArray = null;

                object obj = Type.Missing;
                IPolyline cutterPolyline = new PolylineClass();
                pMultipoint = (IPointCollection)cutterPolyline;

                //求相交点的所有集合
                foreach (ZhFeature tpUsedFeat in pUsedZHFeatures)
                {
                    //求相交点集合
                    cgGeoArray = this.MuliCutPolyline(tpByFeat, tpUsedFeat);
                    foreach (IGeometry tGeo in cgGeoArray)
                    {
                        if (((IRelationalOperator)pMultipoint).Contains(tGeo) == false)
                        {
                            pMultipoint.AddPoint((IPoint)tGeo, ref obj, ref obj);
                        }
                    }
                }

                //添加原线端点
                //pMultipoint.AddPointCollection((IPointCollection)tpByFeat.pFeature.ShapeCopy);

                //拓朴化处理  去掉重复点操作
                //ITopologicalOperator pTop = pMultipoint as ITopologicalOperator;
                //pTop.Simplify();
                //pMultipoint = pTop as IPointCollection;

                //清除原对象
                GeoList.Clear();

                //用相交点集合分割线
                if (tpByFeat.pFeature.Shape is IPolycurve2)
                {
                    //IPoint outVertex = null;
                    //IPoint preVertex = null;

                    //int outPartIndex = 0;
                    //int vertexIndex = 0;
                    IPolycurve2 Curve2 = tpByFeat.pFeature.Shape as IPolycurve2;
                    IEnumVertex SplitPoints = pMultipoint.EnumVertices;
                    Curve2.SplitAtPoints(SplitPoints, true, true, -1);
                    IGeometryCollection pcgGeoColl = Curve2 as IGeometryCollection;
                    for (int i = 0; i < pcgGeoColl.GeometryCount; i++)
                    {
                        IGeometry tpcgGeo = pcgGeoColl.get_Geometry(i);

                        IGeometryCollection oGeoCol = new PolylineClass();
                        oGeoCol.AddGeometries(1, ref tpcgGeo);

                        if (((ITopologicalOperator)oGeoCol).IsSimple == false)
                        {
                            ((ITopologicalOperator)oGeoCol).Simplify();
                        }

                        GeoList.Add(oGeoCol as IGeometry);
                    }
                    #region IEnumSplitPoint
                    //IEnumSplitPoint cgEnumPoint=Curve2.SplitAtPoints(SplitPoints,false,false,-1);
                    //cgEnumPoint.Reset();
                    //cgEnumPoint.Next(out outVertex, out outPartIndex, out vertexIndex);
                    //while (outVertex != null && outVertex.IsEmpty != true && cgEnumPoint.IsLastInPart()!=true)
                    //{
                    //    preVertex=outVertex;
                    //    cgEnumPoint.Next(out outVertex, out outPartIndex, out vertexIndex);
                    //    if (preVertex != null && outVertex != null && outVertex.IsEmpty!=true)
                    //    {
                    //        IPolyline pcgPolyline = new PolylineClass();
                    //        (pcgPolyline as IPointCollection).AddPoint(preVertex, ref obj, ref obj);
                    //        (pcgPolyline as IPointCollection).AddPoint(outVertex, ref obj, ref obj);
                    //        if (pcgPolyline.IsEmpty != true)
                    //        {
                    //            GeoList.Add(pcgPolyline);
                    //        }
                    //    }
                    //}
                    #endregion
                }

                //创建分割后对象 ZHFeatureByGeoList
                ZhFeature tpFeat = null;
                foreach (IGeometry pGeo in GeoList)
                {
                    if (pGeo.IsEmpty != true)
                    {
                        tpFeat = this.CreateFeature();
                        //属性拷贝(含GHDM赋码)
                        tpByFeat.CopyField(ref tpFeat);
                        tpFeat.pFeature.Shape = pGeo;
                        //保存
                        //tpFeat.pFeature.Store();
                        pMuliFeatures.Add(tpFeat);
                    }
                }

            }
            return pMuliFeatures.ToArray();
        }
        //求相交点集合
        public IGeometry[] MuliCutPolyline(ZhFeature tpFeat, ZhFeature tpUsedFeat)
        {
            List<IGeometry> GeoList = new List<IGeometry>();
            GeoList.Clear();
            IGeometry tpGeo = null;
            IPolyline pcutter = null;
            switch (tpUsedFeat.pFeature.Shape.Dimension)
            {
                case esriGeometryDimension.esriGeometry2Dimension:
                    ITopologicalOperator pTop = tpUsedFeat.pFeature.ShapeCopy as ITopologicalOperator;
                    pTop.Simplify();
                    pcutter = pTop.Boundary as IPolyline;
                    break;
                case esriGeometryDimension.esriGeometry1Dimension:
                    pcutter = tpUsedFeat.pFeature.ShapeCopy as IPolyline;
                    break;
            }
            //线线相交
            tpGeo = tpFeat.IntersectsSameDim(pcutter, esriGeometryDimension.esriGeometry0Dimension);
            if (tpGeo is IMultipoint)
            {
                IMultipoint Multigeos = tpGeo as IMultipoint;
                IGeometryCollection geoColl = (IGeometryCollection)Multigeos;
                IGeometry geo = null;
                for (int i = 0; i < geoColl.GeometryCount; i++)
                {
                    geo = geoColl.get_Geometry(i);
                    if (geo is IPoint)
                    {
                        (geo as ITopologicalOperator).Simplify();
                        GeoList.Add(geo);
                    }
                }
            }
            return GeoList.ToArray();
        }

        public virtual ZhFeature CreateFeature()
        {
            return this.CreateFeature(this.featureClass.CreateFeature());
        }

        public abstract ZhFeature CreateFeature(IFeature feature);

        public virtual ZhFeatureBuffer CreateZhFeatureBuffer()
        {
            IFeatureBuffer fb = this.FeatureClass.CreateFeatureBuffer();
            ZhFeatureBuffer zhfb = new ZhFeatureBuffer(fb);
            return zhfb;
        }

        public virtual IFeatureBuffer CreateFeatureBuffer()
        {
            IFeatureBuffer fb = this.FeatureClass.CreateFeatureBuffer();
            return fb;
        }

        public virtual void InsertFeatureBuffer(IFeatureCursor FeatureCursorInsert, IFeatureBuffer featBuf)
        {
            FeatureCursorInsert.InsertFeature(featBuf);
        }

        public virtual void InsertZhFeatureBuffer(IFeatureCursor FeatureCursorInsert, ZhFeatureBuffer ZhfeatBuf)
        {
            FeatureCursorInsert.InsertFeature(ZhfeatBuf.pFeatureBuffer);
        }


        public virtual IFeatureCursor GetFeatureCursorInsert(bool useBuffering)
        {
            return this.FeatureClass.Insert(useBuffering);
        }

        public abstract ZhLayerType LayerType { get; }

        public virtual void StartEdit()
        {
            this.oWs = this.Workspace;
            this.iwe = (IWorkspaceEdit)oWs;
            this.iwe.StartEditing(true);
            this.iwe.StartEditOperation();
        }

        public virtual void StopEdit()
        {
            if (iwe != null)
            {
                this.iwe.StopEditOperation();
                this.iwe.StopEditing(true);
                this.iwe = null;
            }
        }

        public virtual void StopEdit(bool saveEdited)
        {
            if (iwe != null)
            {
                this.iwe.StopEditOperation();
                this.iwe.StopEditing(saveEdited);
                this.iwe = null;
            }
        }

        /// <summary>
        /// 删除要素类中所有要素方法
        /// modify date:2008-12-18
        /// vp:hsg
        /// </summary>
        public virtual void DeleteAllFeatures()
        {
            #region 删除速度是最慢的一种方法   现已过时
            //IFeatureCursor pFeatCur = this.FeatureClass.Search(null, false);
            //IFeature pFeat = pFeatCur.NextFeature();
            //while (pFeat != null)
            //{
            //    pFeat.Delete();
            //    pFeat = pFeatCur.NextFeature();
            //}
            //pFeat = null;
            //pFeatCur = null;
            #endregion

            #region 删除速度是中间的一种方法   现已过时   供参考
            //IFeatureCursor pFeatureCursor = this.featureClass.Update(null, false);
            //IFeature pFeature = pFeatureCursor.NextFeature();
            //while (pFeature != null)
            //{
            //    pFeatureCursor.DeleteFeature();
            //    pFeature = pFeatureCursor.NextFeature();
            //}
            //System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeatureCursor);
            #endregion

            #region 删除速度是中间的一种方法   现启用  <=500,000  供参考
            ITable pTable = this.featureClass as ITable;
            pTable.DeleteSearchedRows(null);
            #endregion

            #region 删除速度是最快的一种方法   现已过时 <=500,000
            //string x = "delete from " + this.featureClass.AliasName;
            //(this.featureClass as IDataset).Workspace.ExecuteSQL(x);
            #endregion
        }

        /// <summary>
        /// 删除要素 by UpdateCursor更新游标方法 大于500,000条记录时用
        /// </summary>
        public virtual bool DeleteFeaturesByUpdateCursor(IQueryFilter qf)
        {
            int fCount = 0;
            IFeatureCursor pFeatureCursor = this.featureClass.Update(qf, false);
            IFeature pFeature = pFeatureCursor.NextFeature();
            while (pFeature != null)
            {
                fCount += 1;
                if (fCount % 1000 == 0)
                {
                    Application.DoEvents();
                }
                pFeatureCursor.DeleteFeature();
                pFeature = pFeatureCursor.NextFeature();
            }
            TokayWorkspace.ComRelease(pFeatureCursor);
            pFeatureCursor = null;
            return true;
        }

        /// <summary>
        /// 删除要素 by 执行SQL语句方法 小于500,000条记录时用
        /// </summary>
        public virtual bool DeleteFeaturesByExecuteSQL(string pWhere)
        {
            string x = "delete from " + this.featureClass.AliasName + " where " + pWhere;
            if (this.featureClass is IDataset)
            {
                (this.featureClass as IDataset).Workspace.ExecuteSQL(x);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 删除要素 by DeleteSearchedRows方法 小于500,000条记录时用
        /// </summary>
        public virtual bool DeleteFeaturesByDeleteSearchedRows(string pWhere)
        {
            IQueryFilter pQueryFilter = new QueryFilterClass();
            pQueryFilter.WhereClause = pWhere;
            ITable pTable = this.featureClass as ITable;
            pTable.DeleteSearchedRows(pQueryFilter);
            return true;
        }

        public virtual void Update(string WhereClause, string fieldNames, object[] objValues)
        {
            IQueryFilter oQf = new QueryFilterClass();
            oQf.WhereClause = WhereClause;
            oQf.SubFields = fieldNames;

            IFeatureCursor oFCursor = this.featureClass.Update(oQf, false);
            IFeature pFeat = oFCursor.NextFeature();
            ZhFeature pZHFeat = null;
            while (pFeat != null)
            {
                pZHFeat = new ZHFeaturePoint();
                pZHFeat.pFeature = pFeat;
                this.Update(pZHFeat, fieldNames, objValues);
                pFeat = oFCursor.NextFeature();
            }
            pFeat = null;
            pZHFeat = null;
            oFCursor = null;
            oQf = null;
        }

        public virtual void Update(ZhFeature pZHFeat, string fieldNames, object[] objValues)
        {
            if (pZHFeat == null) return;

            string[] FNamesArray = null;
            FNamesArray = fieldNames.Split(new char[] { ',' });

            for (int i = 0; i < FNamesArray.Length; i++)
            {
                pZHFeat.setFieldValue(FNamesArray[i], objValues[i]);
            }
            pZHFeat.SaveFeature();
            FNamesArray = null;
        }

        public virtual ZhFeature[] SelectUpdateFeatures(string whereClause, string fieldNames)
        {
            List<ZhFeature> UpdateFeatures = new List<ZhFeature>();
            UpdateFeatures.Clear();

            IQueryFilter oQf = new QueryFilterClass();
            oQf.WhereClause = whereClause;
            oQf.SubFields = fieldNames;

            IFeatureCursor oFCursor = this.featureClass.Update(oQf, false);
            IFeature pFeat = oFCursor.NextFeature();
            while (pFeat != null)
            {
                UpdateFeatures.Add(this.CreateFeature(pFeat));
                pFeat = oFCursor.NextFeature();
            }
            pFeat = null;
            oFCursor = null;
            oQf = null;
            return UpdateFeatures.ToArray();
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

        public IQueryDef CreateQuery(string whereClause)
        {
            IDataset dataset = this.FeatureClass as IDataset;
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

        #region Statistic
        public object[] Unique(string fieldName, string whereClause)
        {
            return CustomStatisticClass.Unique(FeatureClass as ITable, fieldName, whereClause);
        }

        public double Sum(ITable table, string fieldName, string whereClause)
        {
            return CustomStatisticClass.Sum(table, fieldName, whereClause);
        }

        public double Sum(string fileName, string whereClause)
        {
            return Sum(FeatureClass as ITable, fileName, whereClause);
        }

        public object Max(ITable table, string fieldName, string whereClause)
        {
            return CustomStatisticClass.Max(table, fieldName, whereClause);
        }

        public object Max(string fieldName, string whereClause)
        {
            return CustomStatisticClass.Max(this.OriginalObject as ITable, fieldName, whereClause);
        }

        public object Min(string fieldName, string whereClause)
        {
            return CustomStatisticClass.Min(this.OriginalObject as ITable, fieldName, whereClause);
        }
        public object Min(ITable table, string fieldName, string whereClause)
        {
            return CustomStatisticClass.Min(table, fieldName, whereClause);
        }

        public int Count(ITable table, string whereClause)
        {
            return CustomStatisticClass.Count(table, whereClause);
        }

        public int Count(string whereClause)
        {
            return this.Count(FeatureClass as ITable, whereClause);
        }
        #endregion

        public void CopyFieldsToObjectFeatureClass(IFeatureClass ObjectFeatureClass)
        {
            IFields Fields = this.featureClass.Fields;
            IField field = null;

            IField mField;
            IFieldEdit mFieldEdit;

            for (int j = 0; j < Fields.FieldCount; j++)
            {
                field = Fields.get_Field(j);
                if (ObjectFeatureClass.Fields.FindField(field.Name) <= -1 && (SystemFieldsList.Contains(field.Name.ToUpper()) == false))
                {
                    if (field.Type != esriFieldType.esriFieldTypeOID &&
                        field.Type != esriFieldType.esriFieldTypeGeometry)
                    {
                        if (field.Type != esriFieldType.esriFieldTypeBlob)
                        {
                            //创建一个新的字段类型
                            mField = new FieldClass();
                            mFieldEdit = mField as IFieldEdit;
                            mFieldEdit.Name_2 = field.Name;
                            mFieldEdit.AliasName_2 = field.AliasName;

                            mFieldEdit.Type_2 = field.Type;

                            mFieldEdit.Length_2 = field.Length;
                            mFieldEdit.Precision_2 = field.Precision;
                            mFieldEdit.Scale_2 = field.Scale;

                            mFieldEdit.Editable_2 = field.Editable;
                            mFieldEdit.IsNullable_2 = field.IsNullable;
                            mFieldEdit.DefaultValue_2 = field.DefaultValue;
                            mFieldEdit.Domain_2 = field.Domain;
                            mFieldEdit.DomainFixed_2 = field.DomainFixed;

                            ObjectFeatureClass.AddField(mField);
                        }
                    }
                }
            }
        }

        public int FeaturesCount(IQueryFilter filter)
        {
            int count = 0;
            if (this.featureClass != null)
            {
                count = this.featureClass.FeatureCount(filter);
            }
            return count;
        }

        public IField GetIField(string FieldName)
        {
            IField fd = null;
            int index = this.FeatureClass.Fields.FindField(FieldName);
            if (index >= 0)
            {
                fd = this.FeatureClass.Fields.get_Field(index);
            }
            return fd;
        }


        #region 属性
        public List<string> SystemFieldsList = new List<string>();
        public List<string> GetFieldNames()
        {
            List<string> fieldNames = new List<string>();

            string FName = "";
            for (int i = 0; i < this.FeatureClass.Fields.FieldCount; i++)
            {
                FName = this.FeatureClass.Fields.get_Field(i).Name.ToString();
                //if (FName.ToUpper().Trim() == "SHAPE" ||
                //    FName.ToUpper().Trim() == "SHAPE.LEN" ||
                //    FName.ToUpper().Trim() == "SHAPE.AREA" ||
                //    FName.ToUpper().Trim() == "SHAPE_LENGTH" ||
                //    FName.ToUpper().Trim() == "SHAPE_AREA")
                //{
                if (SystemFieldsList.Contains(FName.ToUpper().Trim()) == false)
                {
                    fieldNames.Add(FName);
                }
            }
            return fieldNames;
        }

        /// <summary>
        /// 获取面积字段集合
        /// </summary>
        /// <returns></returns>
        public List<string> GetAreaFieldNames()
        {
            List<string> fieldNames = new List<string>();

            string FName = "";
            for (int i = 0; i < this.FeatureClass.Fields.FieldCount; i++)
            {
                switch (this.FeatureClass.Fields.get_Field(i).Type)
                {
                    case esriFieldType.esriFieldTypeDouble:
                    case esriFieldType.esriFieldTypeInteger:
                    case esriFieldType.esriFieldTypeSingle:
                    case esriFieldType.esriFieldTypeSmallInteger:
                        {
                            FName = this.FeatureClass.Fields.get_Field(i).Name.ToString();
                            fieldNames.Add(FName);
                        }
                        break;
                }
            }
            return fieldNames;
        }

        public string OIDFieldName
        {
            get
            {
                if (this.featureClass.HasOID)
                    return this.featureClass.OIDFieldName;
                else
                    return string.Empty;
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
            if (this.featureClass != null)
            {
                if (this.featureClass.OIDFieldName == FieldName)
                {
                    rbc = true;
                }
            }
            return rbc;
        }

        public IWorkspace Workspace
        {
            get
            {
                if (this.featureClass == null) return null;

                IDataset oDataset;
                oDataset = (IDataset)this.featureClass;

                if (oDataset == null)
                    return null;
                else
                    return oDataset.Workspace;
            }
        }

        public IFeatureClass FeatureClass
        {
            get
            {
                return this.featureClass;
            }
        }
        #endregion

        //kez 2009-6-10 add 用于坐标转换
        public ZhFeature[] getSelectedFeaturesByQueryFilter(IQueryFilter query)
        {
            IFeatureCursor pFeatureCursor = null;
            List<ZhFeature> zhFeatures = new List<ZhFeature>();
            try
            {
                pFeatureCursor = this.featureClass.Search(query, false);
                if (pFeatureCursor != null)
                {
                    IFeature pFeature = pFeatureCursor.NextFeature();
                    while (pFeature != null)
                    {
                        zhFeatures.Add(this.CreateFeature(pFeature));
                        pFeature = pFeatureCursor.NextFeature();
                    }
                }

                return zhFeatures.ToArray();
            }
            catch
            {
                zhFeatures.Clear();
                return zhFeatures.ToArray();
            }
            finally
            {
                if (pFeatureCursor != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeatureCursor);
                    pFeatureCursor = null;
                }
            }
        }

        public bool getExistsXMinYMin_XMaxYMax()
        {
            bool rbc = false;
            if (this.featureClass != null
                && this.featureClass.Fields.FindField("XMIN") > 0
                && this.featureClass.Fields.FindField("XMAX") > 0
                && this.featureClass.Fields.FindField("YMIN") > 0
                && this.featureClass.Fields.FindField("YMAX") > 0)
            {
                rbc = true;
            }
            else
            {
                rbc = false;
            }

            return rbc;

        }

        public IFeatureClass OriginalObject
        {
            get
            {
                return this.featureClass;
            }
        }

    }

    /// <summary>
    /// (8,9,10)
    /// </summary>
    public enum ZhLayerType : int
    {
        PolyLine = 8,
        Point = 9,
        Polygon = 10,
        Annotation = 11,

    }

    public class ZhPointFeatureClass : ZhFeatureClass
    {
        public ZhPointFeatureClass(IFeatureClass featureclass)
            : base(featureclass)
        {
        }


        public override ZhFeature CreateFeature(IFeature feature)
        {
            return new ZHFeaturePolyLine(feature);
        }

        public override ZhLayerType LayerType
        {
            get { return ZhLayerType.Point; }
        }
    }

    public class ZhPolylineFeatureClass : ZhFeatureClass
    {
        public ZhPolylineFeatureClass(IFeatureClass featureclass)
            : base(featureclass)
        {
        }


        public override ZhFeature CreateFeature(IFeature feature)
        {
            return new ZHFeaturePolyLine(feature);
        }

        public override ZhLayerType LayerType
        {
            get { return ZhLayerType.PolyLine; }
        }
    }

    public class ZhPolygonFeatureClass : ZhFeatureClass
    {
        public ZhPolygonFeatureClass(IFeatureClass featureclass)
            : base(featureclass)
        {
        }


        public override ZhFeature CreateFeature(IFeature feature)
        {
            return new ZHFeaturePolygon(feature);
        }

        public override ZhLayerType LayerType
        {
            get { return ZhLayerType.Polygon; }
        }
    }
}
