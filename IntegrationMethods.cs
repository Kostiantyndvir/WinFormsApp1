using Microsoft.VisualBasic.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1
{
    public static class IntegrationMethods
    {

        // функція обчислення інтегралу на проміжку [a,b] з кроком step, 
        // що визначився на основі кількості проміжків розбиття
        // функція передається як об'єкт делегату - для того щоб можна було
        // в одному методі обчислювати різні підінтегральні функції
        public static double LeftRect(double a, double b, int n, Func f)
        {
            double result1 = 0;
            double step = (b - a) / n;
            for (int i = 0; i < n; i++)
                result1 += step * f(a + i * step);
            return result1;
        }

        // правих
        public static double RightRect(double a, double b, int n, Func f)
        {
            double result2 = 0;
            double step = (b - a) / n;
            for (int i = 1; i <= n; i++)
                result2 += step * f(a + i * step);
            return result2;
        }

        // середніх
        public static double MiddleRect(double a, double b, int n, Func f)
        {
            double result3 = 0;
            double step = (b - a) / n;
            for (int i = 1; i <= n; i++)
                result3 += step * f(a + i * step - step/2);
            return result3;
        }

        // обчислення інтегралу тобто підінтегральної функції методом трапецій
        public static double Trap(double a, double b, int n, Func f)
        {
            double result4 = 0;
            double step = (b - a) / n;
            for (int i = 1; i <= n; i++) 
                result4 += step * (f(a + i * step) + f(a + (i - 1) * step)) / 2;
            return result4;
        }
        // обчислення інтегралу тобто підінтегральної функції методом Сімпсона
        public static double Simpson(double a, double b, int n, Func func)
        {
            double step = (b - a) / n;
            double s = (func(a) + func(b)) * 0.5;
            for (int i = 1; i <= n - 1; i++)
            {
                double xk = a + step * i; //xk
                double xk1 = a + step * (i - 1); //Xk-1
                s += func(xk) + 2 * func((xk1 + xk) / 2);
            }
            var x = a + step * n; //xk
            var x1 = a + step * (n - 1); //Xk-1
            s += 2 * func((x1 + x) / 2);

            return s * step / 3.0;
        }

        // Ньютон-Лейбніц
        public static double PrecisionValue(double a, double b, int number)
        {
            switch (number)
            {
                case 1: return -Math.Cos(b) + Math.Cos(a);
                case 2: return Math.Sin(b) - Math.Sin(a);
                case 3: return Math.Exp(b) - Math.Exp(a);
                case 4: return Math.Pow(b, 3) / 3 - Math.Pow(a, 3) / 3;
                case 5: return Math.Pow(2, b) / Math.Log(2) - Math.Pow(2, a) / Math.Log(2);
                case 6: return Math.Log(b) - Math.Log(a);
                case 7: return b * Math.Log(b) - b - a * Math.Log(a) + a;
            }
            return 1; 
        }
    }
}
