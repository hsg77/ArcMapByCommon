using System;
using System.Collections.Generic;
using System.Text;

namespace ArcMapByJBNT
{
    public class Maths
    {
        
        public static decimal Todecimal(double a)
        {
            decimal dec = (decimal)a;
            return dec;
        }
        public static double Todouble(decimal a)
        {
            double d = (double)a;
            return d;
        }

        public static decimal Cos(decimal a)
        {
            return Todecimal(Math.Cos(Todouble(a)));
        }

        public static decimal Sin(decimal a)
        {
            return Todecimal(Math.Sin(Todouble(a)));
        }

        public static decimal Tan(decimal a)
        {
            return Todecimal(Math.Tan(Todouble(a)));
        }

        public static decimal Pow(decimal x, decimal y)
        {
            return Todecimal(Math.Pow(Todouble(x),Todouble(y)));
        }

        public static decimal Sqrt(decimal a)
        {
            return Todecimal(Math.Sqrt(Todouble(a)));
        }
    }
}
