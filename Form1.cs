using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static WinFormsApp1.IntegrationMethods;
using static WinFormsApp1.Functions;
namespace WinFormsApp1
{
    public delegate double Func(double x);
    public delegate double Method(double a, double b, int n, Func f);
    public partial class Form1 : Form
    {
        const int algorithms = 6;
        int countFunction = 7;

        string[] values = new string[algorithms] ; // масив значень інтегралів
        string[] times = new string[algorithms]; // масив часу обчислень
        // передаю далі
        double[] timesAll = new double[algorithms]; 
        double[] average = new double[algorithms]; //
        string[] averageString = new string[algorithms]; // масив для виведення
        // передаю далі
        double[] mistakes = new double[algorithms]; // масив похибок

        TimeOnly startTime, endTime;
        public Form1() { InitializeComponent(); }

        double CallIntMet(double a, double b, int n, int i, Func func, int function)
        {
            switch (i)
            {
                case 0: return PrecisionValue(a, b, function);
                case 1: return RightRect(a, b, n, func);
                case 2: return LeftRect(a, b, n, func);
                case 3: return MiddleRect(a, b, n, func);
                case 4: return Trap(a, b, n, func);
                case 5: return Simpson(a, b, n, func);
            }
            return 1;
        }

        // виведення при sin()
        void OutFunc(double a, double b, int n, Func func, int function)
        {
            
            for (int i = 0; i < algorithms; i++)
            {
                startTime = TimeOnly.FromDateTime(DateTime.Now);
                average[i] = CallIntMet(a, b, n, i, func, function);
                values[i] = Math.Round(average[i], 10).ToString(); 
                endTime = TimeOnly.FromDateTime(DateTime.Now);
                times[i] = Math.Round((endTime - startTime).TotalSeconds * 1000, 5).ToString();
                timesAll[i] += Math.Round((endTime - startTime).TotalSeconds * 1000, 5);
                // похибка у відсотках
                mistakes[i] += Math.Abs(Math.Round(100 * (average[0] - average[i]) / average[0], 4));
            }
            dataGridView1.Rows.Add(values);
            dataGridView1.Rows.Add(times);
        }

        void OutPrecision() 
        {
            for (int i = 0; i < algorithms; i++)
            {
                mistakes[i] = Math.Round(mistakes[i] / countFunction, 4);
                averageString[i] = mistakes[i].ToString();
            }
            dataGridView1.Rows.Add(averageString);

            for (int i = 0; i < algorithms; i++)
            {
                timesAll[i] = Math.Round(timesAll[i] / countFunction, 4);
                averageString[i] = timesAll[i].ToString();
            }
            dataGridView1.Rows.Add(averageString);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            new Form2(mistakes, timesAll).Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            new Form3().Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            dataGridView1.Rows.Clear();
            for (int i = 0; i < algorithms; i++)
            {
                mistakes[i] = 0;
                timesAll[i] = 0;
            }
            double a, b; // межі інтегрування та крок
            int n; // кількість проміжків розбиття

            a = (double)numericUpDown1.Value;
            b = (double)numericUpDown2.Value;
            n = (int)numericUpDown3.Value;

            OutFunc(a, b, n, Math.Sin, 1);
            OutFunc(a, b, n, Math.Cos, 2);
            OutFunc(a, b, n, Math.Exp, 3);
            OutFunc(a, b, n, SquareX, 4);
            OutFunc(a, b, n, PowerX, 5);
            OutFunc(a, b, n, OneDivX, 6);
            OutFunc(a, b, n, Math.Log, 7);

            OutPrecision();

        }
    }
}