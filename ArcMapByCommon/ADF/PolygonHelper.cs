using System;
using System.Collections.Generic;
using System.Text;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.esriSystem;

namespace ArcMapByCommon
{
    /// <summary>
    /// 定义：面对象辅助类
    /// </summary>
    public class PolygonHelper
    {
        /// <summary>
        /// 获取多边形的所有外环
        /// </summary>
        /// <param name="polygon">多边形</param>
        /// <returns></returns>
        public static IRing[] GetExteriorRings(IPolygon polygon)
        {
            IPolygon4 polygon4 = polygon as IPolygon4;
            List<IRing> exteriorRingList = new List<IRing>();

            IGeometryBag exteriorRings = polygon4.ExteriorRingBag;
            IEnumGeometry exteriorRingEnum = exteriorRings as IEnumGeometry;
            //exteriorRingEnum.Reset();
            IRing ring = exteriorRingEnum.Next() as IRing;

            while (ring != null)
            {
                exteriorRingList.Add(ring);

                ring = exteriorRingEnum.Next() as IRing;
            }

            return exteriorRingList.ToArray();
        }               
        /// <summary>
        /// 根据外环获取到内环
        /// </summary>
        /// <param name="ring">外环</param>
        /// <param name="polygon">多边形</param>
        /// <returns></returns>
        public static IRing[] GetInteriorRingsByExterior(IRing ring, IPolygon polygon)
        {
            List<IRing> interRingList = new List<IRing>();
            IPolygon4 polygon4 = polygon as IPolygon4;

            IGeometryBag interRings = polygon4.get_InteriorRingBag(ring);
            IEnumGeometry interRingsEnum = interRings as IEnumGeometry;
            interRingsEnum.Reset();

            IRing tmpring = interRingsEnum.Next() as IRing;

            while (tmpring != null)
            {
                interRingList.Add(tmpring);

                tmpring = interRingsEnum.Next() as IRing;
            }

            return interRingList.ToArray();
        }
        /// <summary>
        /// 获取多边形的所有环
        /// </summary>
        /// <param name="polygon">多边形</param>
        /// <returns></returns>
        public static Dictionary<string, IRing> GetAllRings(IPolygon polygon)
        {
            Dictionary<string, IRing> rings = new Dictionary<string, IRing>();

            int i = 0;
            foreach (IRing ring in  PolygonHelper.GetExteriorRings(polygon))
            {
                int j = 0;
                i++;
                rings.Add("外环" + i.ToString(), ring);
                foreach (IRing interring in PolygonHelper.GetInteriorRingsByExterior(ring, polygon))
                {
                    j++;
                    rings.Add("外环" + i.ToString() + "内环" + j.ToString(), interring);
                }
            }

            return rings;
        }

        /// <summary>
        /// 获取多边形的所有内外环
        /// </summary>
        /// <param name="polygon">多边形</param>
        /// <returns></returns>
        public static Dictionary<string, IRing> GetAllInteriorRings(IPolygon polygon)
        {
            Dictionary<string, IRing> rings = new Dictionary<string, IRing>();

            int i = 0;
            foreach (IRing ring in GetExteriorRings(polygon))
            {
                int j = 0;
                i++;
                rings.Add("外环" + i.ToString(), ring);
                foreach (IRing interring in GetInteriorRingsByExterior(ring, polygon))
                {
                    j++;
                    rings.Add("外环" + i.ToString() + "内环" + j.ToString(), interring);
                }
            }
            return rings;
        }
        /// <summary>
        /// 获取多边形的所有内环
        /// </summary>
        /// <param name="polygon">多边形</param>
        /// <returns></returns>
        public static Dictionary<string, IRing> GetAllOnlyInteriorRings(IPolygon polygon)
        {
            Dictionary<string, IRing> rings = new Dictionary<string, IRing>();

            int i = 0;
            foreach (IRing ring in GetExteriorRings(polygon))
            {
                int j = 0;
                i++;                
                foreach (IRing interring in GetInteriorRingsByExterior(ring, polygon))
                {
                    j++;
                    rings.Add("外环" + i.ToString() + "内环" + j.ToString(), interring);
                }
            }
            return rings;
        }

        /// <summary>
        /// 重新构造面对象
        /// </summary>
        /// <param name="pPolygon"></param>
        /// <returns></returns>
        public static PolygonClass ReNew(IPolygon geo)
        {
            object obj=Type.Missing;
            PolygonClass p = null;
            IRing[] ExtRingArray = GetExteriorRings(geo as IPolygon4);
            if (ExtRingArray != null && ExtRingArray.Length > 0)
            {
                p = new PolygonClass();
                foreach (IRing r in ExtRingArray)
                {
                    p.AddGeometry(r, ref obj, ref obj);
                    foreach (IRing ir in GetInteriorRingsByExterior(r, geo))
                    {
                        p.AddGeometry(ir, ref obj, ref obj);
                    }
                }
                p.SimplifyPreserveFromTo();
            }
            return p;
        }
        //

        /// <summary>
        /// 将pFeature的面积放大scale(如1.6)倍，生成个新图形
        /// </summary>
        /// <param name="geo"></param>
        /// <param name="scale"></param>
        public static IGeometry reScale(IPolygon geo, double scale)
        {
            IPolygon g = (geo as IClone).Clone() as IPolygon;
            double old_mj = (g as IArea).Area;
            double sx=Math.Sqrt(scale);
            double sy=sx;
            (g as ITransform2D).Scale((g as IArea).Centroid, sx, sy);
            double new_mj = (g as IArea).Area;
            double bl = new_mj / old_mj;
            return g;
        }
        /// <summary>
        /// 将pFeature的面积放大scale(如1.6)倍，生成个新图形
        /// </summary>
        /// <param name="geo"></param>
        /// <param name="scale"></param>
        public static IGeometry reScale(IPolygon geo, double scale,IPoint Origin)
        {
            IPolygon g = (geo as IClone).Clone() as IPolygon;
            double old_mj = (g as IArea).Area;
            double sx = Math.Sqrt(scale);
            double sy = sx;
            (g as ITransform2D).Scale(Origin, sx, sy);
            double new_mj = (g as IArea).Area;
            double bl = new_mj / old_mj;
            return g;
        }
    }
}
