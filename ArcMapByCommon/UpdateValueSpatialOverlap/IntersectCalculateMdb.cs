using System;
using System.Collections.Generic;
using System.Text;
using ESRI.ArcGIS.Geoprocessing;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.AnalysisTools;

namespace ArcMapByCommon
{
    /// <summary>
    /// 相交运算扩展部件  输出mdb
    /// </summary>
    public class t_IntersectCalculateMdb
    {
        //输入参数
        //输出参数
        public string Message = "";
        /// <summary>
        /// 执行相交运算
        /// </summary>
        /// <param name="pLayerName1">图层名称1</param>
        /// <param name="pLayerName2">图层名称2</param>
        /// <param name="outputMdbPath">...xxx.mdb\xzq</param>
        /// <param name="output_type">INPUT;LINE;POINT</param>
        /// <param name="cluster_tolerance">"0.0001 Meters"</param>
        /// <param name="map">IMap</param>
        /// <returns></returns>
        public bool Execute(string pLayerName1, string pLayerName2, string outputMdbPath, string output_type, object cluster_tolerance, IMap map)
        {
            bool rbc = false;
            //[pLayerName1]与[pLayerName2]相交分析 生成 mdb file
            //GPUtilitiesClass gpUtil = new GPUtilitiesClass();
            //gpUtil.SetInternalMap(map);
            //object in_features = pLayerName1 + " #;" + pLayerName2 + " #";
            //string output_type = "LINE";
            //string join_attributes = "ALL";
            // "0.0001 Meters"; // Meters
            IntersectCalculateMdbEx interclass = new IntersectCalculateMdbEx();
            bool ret = interclass.Execute(pLayerName1, pLayerName2, outputMdbPath, output_type, cluster_tolerance, map);
            //IntersectClass cls = new IntersectClass();
            //cls.SetValue(in_features, outputMdbPath, output_type, join_attributes, cluster_tolerance);
            if (ret == true)
            {   //执行成功
                rbc = true;
            }
            else
            {
                rbc = false;
            }
            this.Message = interclass.Message;
            return rbc;
        }

        public bool Execute(IFeatureClass fc1, IFeatureClass fc2, string outputMdbPath, string output_type, object cluster_tolerance)
        {
            bool rbc = false;

            MapClass map = new MapClass();
            IFeatureLayer fL1 = AddFeatureClassToMap(map, fc1);
            IFeatureLayer fL2 = AddFeatureClassToMap(map, fc2);
            string pLayerName1 = fL1.Name;
            string pLayerName2 = fL2.Name;
            //[pLayerName1]与[pLayerName2]相交分析 生成 mdb file

            //GPUtilitiesClass gpUtil = new GPUtilitiesClass();
            //gpUtil.SetInternalMap(map);
            //object in_features = pLayerName1 + " #;" + pLayerName2 + " #";
            //string output_type = "LINE";
            //string join_attributes = "ALL";
            // "0.0001 Meters"; // Meters
            IntersectCalculateMdbEx interclass = new IntersectCalculateMdbEx();
            bool ret = interclass.Execute(pLayerName1, pLayerName2, outputMdbPath, output_type, cluster_tolerance, map);
            //IntersectClass cls = new IntersectClass();
            //cls.SetValue(in_features, outputMdbPath, output_type, join_attributes, cluster_tolerance);
            if (ret == true)
            {   //执行成功
                rbc = true;
            }
            else
            {
                rbc = false;
            }
            this.Message = interclass.Message;
            return rbc;
        }

        //添加IfeatureClass到Imap中
        public static IFeatureLayer AddFeatureClassToMap(IMap map, IFeatureClass featureClass)
        {
            IFeatureLayer fl = new FeatureLayerClass();
            fl.FeatureClass = featureClass;
            fl.Name = featureClass.AliasName;
            if (featureClass is IDataset)
            {
                fl.Name = TokayWorkspace.GetDatasetName(featureClass as IDataset);
            }
            map.AddLayer(fl as ILayer);
            return fl;
        }
    }

    /// <summary>
    /// 相交运算扩展部件  输出mdb
    /// </summary>
    public class IntersectCalculateMdbEx
    {
        //输入参数
        //输出参数
        public string Message = "";
        /// <summary>
        /// 执行相交运算
        /// </summary>
        /// <param name="pLayerName1">图层名称1</param>
        /// <param name="pLayerName2">图层名称2</param>
        /// <param name="outputMdbPath">...xxx.mdb\xzq</param>
        /// <param name="output_type">INPUT;LINE;POINT</param>
        /// <param name="cluster_tolerance">"0.0001 Meters"</param>
        /// <param name="map">IMap</param>
        /// <returns></returns>
        public bool Execute(string pLayerName1, string pLayerName2, string outputMdbPath, string output_type, object cluster_tolerance, IMap map)
        {
            bool rbc = false;
            //[pLayerName1]与[pLayerName2]相交分析 生成 mdb file

            GPUtilitiesClass gpUtil = new GPUtilitiesClass();
            gpUtil.SetInternalMap(map);

            IntersectClass cls = new IntersectClass();

            object in_features = pLayerName1 + " #;" + pLayerName2 + " #";
            //string output_type = "LINE";
            string join_attributes = "ALL";
            // "0.0001 Meters"; // Meters
            cls.SetValue(in_features, outputMdbPath, output_type, join_attributes, cluster_tolerance);
            if (cls.Execute(null) == true)
            {   //执行成功
                rbc = true;
            }
            else
            {
                rbc = false;
            }
            this.Message = cls.Message;
            return rbc;
        }

        public bool Execute(IFeatureClass fc1, IFeatureClass fc2, string outputMdbPath, string output_type, object cluster_tolerance)
        {
            bool rbc = false;

            MapClass map = new MapClass();
            IFeatureLayer fL1 = AddFeatureClassToMap(map, fc1);
            IFeatureLayer fL2 = AddFeatureClassToMap(map, fc2);
            string pLayerName1 = fL1.Name;
            string pLayerName2 = fL2.Name;
            //[pLayerName1]与[pLayerName2]相交分析 生成 mdb file

            GPUtilitiesClass gpUtil = new GPUtilitiesClass();
            gpUtil.SetInternalMap(map);

            IntersectClass cls = new IntersectClass();

            object in_features = pLayerName1 + " #;" + pLayerName2 + " #";
            //string output_type = "LINE";
            string join_attributes = "ALL";
            // "0.0001 Meters"; // Meters
            cls.SetValue(in_features, outputMdbPath, output_type, join_attributes, cluster_tolerance);
            if (cls.Execute(null) == true)
            {   //执行成功
                rbc = true;
            }
            else
            {
                rbc = false;
            }
            this.Message = cls.Message;
            return rbc;
        }

        //添加IfeatureClass到Imap中
        public static IFeatureLayer AddFeatureClassToMap(IMap map, IFeatureClass featureClass)
        {
            IFeatureLayer fl = new FeatureLayerClass();
            fl.FeatureClass = featureClass;
            fl.Name = featureClass.AliasName;
            if (featureClass is IDataset)
            {
                fl.Name = TokayWorkspace.GetDatasetName(featureClass as IDataset);
            }
            map.AddLayer(fl as ILayer);
            return fl;
        }
    }

    /// <summary>
    /// 相交算法 IFeatureClass Overlap IFeatureClass
    /// out IFeatureClass
    /// 评价模型部件中使用
    /// 执行IGProcess接口功能
    /// </summary>
    public class IntersectClass : GeoProcessCommandClass, IIntersect
    {
        public IntersectClass()
        {
            this.m_process = new Intersect();
        }

        #region IIntersect 成员

        public void SetValue(object in_features, object out_feature_class, string output_type, string join_attributes, object cluster_tolerance)
        {
            Intersect cls = this.Process as Intersect;
            cls.in_features = in_features;  //"地类图斑 #;sde.DBO.C20_region #"
            cls.out_feature_class = out_feature_class;  //D:\tt\DLTB_C20_Intersect.shp
            cls.output_type = output_type;  //INPUT;LINE;POINT
            cls.join_attributes = join_attributes; //ALL;NO_FID;ONLY_FID
            cls.cluster_tolerance = cluster_tolerance;

        }

        #endregion
    }
    public interface IIntersect : IGeoProcessCommand
    {
        void SetValue(object in_features, object out_feature_class, string output_type, string join_attributes, object cluster_tolerance);
    }
}
