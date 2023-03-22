using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
//using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace NumericalIntegration
{
    public partial class Form2 : Form
    {
        const int algorithms = 6;
        List<double> timesAll = new List<double>(); // 2
        List<double> mistakes = new List<double>(); // 1
        int maxT, maxM;
        public Form2()
        {
            InitializeComponent();
        }
        public Form2(double[] M, double[] T) 
        {
            InitializeComponent();
            timesAll = T.ToList();
            mistakes = M.ToList();
            maxM = (int)(mistakes.Max() * 1000);
            maxT = (int)(timesAll.Max() * 1000);

            progressBar1.Maximum = maxM;
            progressBar3.Maximum = maxM;
            progressBar5.Maximum = maxM;
            progressBar7.Maximum = maxM;
            progressBar9.Maximum = maxM;
            progressBar11.Maximum = maxM;

            progressBar2.Maximum = maxT;
            progressBar4.Maximum = maxT;
            progressBar6.Maximum = maxT;
            progressBar8.Maximum = maxT;
            progressBar10.Maximum = maxT;
            progressBar12.Maximum = maxT;

            progressBar1.Value = (int)(mistakes[0] * 1000);
            progressBar3.Value = (int)(mistakes[1] * 1000);
            progressBar5.Value = (int)(mistakes[2] * 1000);
            progressBar7.Value = (int)(mistakes[3] * 1000);
            progressBar9.Value = (int)(mistakes[4] * 1000);
            progressBar11.Value = (int)(mistakes[5] * 1000);

            progressBar2.Value = (int)(timesAll[0] * 1000);
            progressBar4.Value = (int)(timesAll[1] * 1000);
            progressBar6.Value = (int)(timesAll[2] * 1000);
            progressBar8.Value = (int)(timesAll[3] * 1000);
            progressBar10.Value = (int)(timesAll[4] * 1000);
            progressBar12.Value = (int)(timesAll[5] * 1000);
        }
    }
}
