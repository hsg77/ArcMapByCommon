using System;
using System.Collections.Generic;
using System.Text;

namespace ArcMapByJBNT
{
    /// <summary>
    /// //经纬度类
    /// vp:hsg
    /// create date:2011-05-04
    /// </summary>
    public class LatLngClass
    {
        public double Lat = 0.0;
        public double Lng = 0.0;

        public LatLngClass()
        {
        }

        public LatLngClass(double pLat,double pLng)
        {
            Lat = pLat;
            Lng = pLng;
        }

        //获取两个经纬度坐标直接的距离
        public double GetDistance(LatLngClass BlatLng)
        {
            double rbc = 0;
            LatLngClass AlatLng = this;
            rbc = GetDistance(AlatLng, BlatLng);
            return rbc;
        }
        //获取两个经纬度坐标直接的距离
        public  double GetDistance(LatLngClass AlatLng,LatLngClass BlatLng)
        {
            double rbc = 0;
            double xgA=0;double ygA=0;
            BlToGs(ref xgA, ref ygA, AlatLng.Lng, AlatLng.Lat);

            double xgB = 0; double ygB = 0;
            BlToGs(ref xgB, ref ygB, BlatLng.Lng, BlatLng.Lat);

            rbc = GetDistance(xgA, ygA, xgB, ygB);
            
            return rbc;
        }
        //经纬度转为高斯平面真角坐标(bl单位度)  
        public void BlToGs(ref double xg,ref double yg, double lon, double lat)         
        {
            int dh;//带号   
            double temp1, temp2, a, b, c, d, e, f;
            double l1, l2;
            int n;
            n = (int)(lon / 6);
            dh = n + 1;
            l2 = 3 + 6 * n;
            l1 = lon - l2;
            temp1 = lat * Math.PI / 180;
            temp2 = l1 * Math.PI / 180;
            a = 6367558.497 * temp1;
            b = (16036.480 - 1597237.956 * Math.Pow(temp2, 2) - 268563.280 * Math.Pow(temp2, 4)) * Math.Sin(2 * temp1);
            c = (16.828 - 1340.831 * Math.Pow(temp2, 2) + 201450.536 * Math.Pow(temp2, 4)) * Math.Sin(4 * temp1);

            xg = a - b + c;
            d = (6383594.975 + 535998.795 * Math.Pow(temp2, 2) + 54206.791 * Math.Pow(temp2, 4)) * Math.Cos(temp1);
            e = (5356.713 - 534204.967 * Math.Pow(temp2, 2) - 134966.691 * Math.Pow(temp2, 4)) * Math.Cos(3 * temp1);
            f = (6.744 + 81276.496 * Math.Pow(temp2, 4)) * Math.Cos(5 * temp1);
            yg = (temp2) * (d - e + f) + 500000 + dh * 1000000;
        }

        //获取直角坐标系统距离
        public  double GetDistance(double Vx,double Vy,double mX,double mY)
        {
            return Math.Sqrt((mX - Vx) * (mX - Vx) + (mY - Vy) * (mY - Vy));
        }
    }
}
