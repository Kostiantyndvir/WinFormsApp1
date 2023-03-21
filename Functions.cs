using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1
{
    public static class Functions
    {
        public static double SquareX(double x) => Math.Pow(x, 2);
        public static double PowerX(double x) => Math.Pow(2, x);
        public static double OneDivX(double x) => 1/x;
        public static double ExpX(double x) => Math.Exp(x) * x;
        public static double xCos(double x) => Math.Pow(x,2)*Math.Sin(x);
            
    }
}
