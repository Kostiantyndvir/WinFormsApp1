using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlTypes;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using static NumericalIntegration.IntegrationMethods;

namespace NumericalIntegration
{
    public partial class Form3 : Form
    {
        Dictionary<string, Func> dictionary= new ();
        
        string inputString;
        List<char> signs = new();
        List<int> numbers = new();
        List<string> functions = new();
        int[] outputState = new int[8];
        HashSet<char> signs1 = new() { '+', '-' };
        HashSet<char> signs2 = new() {'*','/','%'};
        
        public Form3()
        {
            InitializeComponent();
            dictionary.Add("sin", Math.Sin);
            dictionary.Add("cos", Math.Sin);
            dictionary.Add("exp", Math.Sin);
            dictionary.Add("log", Math.Sin);
            dictionary.Add("squarex", Functions.SquareX);
            dictionary.Add("powerx", Functions.PowerX);
            dictionary.Add("onedivx", Functions.OneDivX);
            dictionary.Add("expx", Functions.ExpX);
            dictionary.Add("xcos", Functions.xCos);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            outputState = new int[8];
            signs.Clear();
            numbers.Clear();
            functions.Clear();
            textBox3.Clear();
            Analiser();
            if (outputState.Sum() > 0)
                outputErrors();
            else
                outputResults();
        }
        void Analiser() 
        {
            int currentSymbol = 0;
            string currentNumber = "";
            string currentFunction = "";
            string temp = textBox1.Text;
            inputString = "";

            // remove spaces if there are any
            while (currentSymbol < temp.Length)
            {
                while (temp[currentSymbol] == ' ')
                    currentSymbol++;
                inputString += temp[currentSymbol];
                currentSymbol++;
            }

            currentSymbol = 0;

            if (inputString[0] != '+' && inputString[0] != '-')
                inputString = inputString.Insert(0, "+");

            while (currentSymbol < inputString.Length - 1)
            {
                char curr = inputString[currentSymbol];
                char next = inputString[currentSymbol + 1];
                // read +, -
                if (signs1.Contains(curr))
                {
                    signs.Add(curr);
                    if (!char.IsNumber(next) && !char.IsLetter(next))
                        outputState[5]++;
                    if (char.IsLetter(next))
                    {
                        numbers.Add(1);
                        signs.Add('*');
                    }
                }
                // reading numbers
                else if (char.IsNumber(curr))
                {
                    currentNumber += curr;
                    if (!char.IsNumber(next))
                    {
                        numbers.Add(Convert.ToInt32(currentNumber));
                        currentNumber = "";
                    }
                    if (!signs2.Contains(next) && !char.IsNumber(next))
                        outputState[2]++;
                    if (currentSymbol + 1 == inputString.Length - 1 && char.IsNumber(next))
                    {
                        currentNumber += next;
                        numbers.Add(Convert.ToInt32(currentNumber));
                        currentNumber = "";
                    }

                }
                // read *, /, %
                else if (signs2.Contains(curr))
                {
                    signs.Add(curr);
                    if (!char.IsLetter(next))
                        outputState[6]++;
                }
                else if (curr == 'X')
                {
                    if (next != ')')
                        outputState[7]++;
                }
                else if (curr == '(' && next != 'X')
                    outputState[3]++;
                else if (curr == ')' && !signs1.Contains(next))
                    outputState[4]++;
                // reading the function name
                else if (char.IsLetter(curr))
                {
                    currentFunction += curr;
                    if (!char.IsLetter(next))
                    {
                        functions.Add(currentFunction);
                        currentFunction = "";
                    }
                    if (!char.IsLetter(next) && next != '(')
                        outputState[1]++;
                }
                currentSymbol++;
            }
        }
        void outputErrors() 
        {
            textBox3.AppendText("Не вдалось обчислити\r\n");
            textBox3.AppendText("Виявлені помилки\r\n");
            if (outputState[1] > 0)
                textBox3.AppendText("після назви функції очікується ( - " + outputState[1].ToString() + "\r\n");
            if (outputState[2] > 0)
                textBox3.AppendText("після числа очікується [* \\ %] - " + outputState[2].ToString() + "\r\n");
            if (outputState[3] > 0)
                textBox3.AppendText("після ( повинен йти Х, тобто аргумент - " + outputState[3].ToString() + "\r\n");
            if (outputState[4] > 0)
                textBox3.AppendText("після ) має бути знак [+ -] - " + outputState[4].ToString() + "\r\n");
            if (outputState[5] > 0)
                textBox3.AppendText("після [+ -] має йти або число або назва функції - " + outputState[5].ToString() + "\r\n");
            if (outputState[6] > 0)
                textBox3.AppendText("після [* \\ %] має йти тільки назва функції - " + outputState[6].ToString() + "\r\n");
            if (outputState[7] > 0)
                textBox3.AppendText("після Х повинен йти ) - " + outputState[7].ToString() + "\r\n");
        }
        void outputResults() 
        {
            double a = (double)numericUpDown1.Value;
            double b = (double)numericUpDown2.Value;
            double n = (int)numericUpDown3.Value;
            
            if (a > b)
            {
                MessageBox.Show("Нижня межа більша верхньої, що не може бути");
                return;
            }
            if (n < 1)
            {
                MessageBox.Show("Кількість відрізків поділу не може бути від'ємна");
                return;
            }
            textBox3.AppendText(Calculate(LeftRect).ToString() + " - метод лівих" + "\r\n");
            textBox3.AppendText(Calculate(RightRect).ToString() + " - метод правих" + "\r\n");
            textBox3.AppendText(Calculate(MiddleRect).ToString() + " - метод середніх" + "\r\n");
            textBox3.AppendText(Calculate(Trap).ToString() + " - метод трапецій" + "\r\n");
            textBox3.AppendText(Calculate(Simpson).ToString() + " - метод сімпсона" + "\n\r");
            }
        double Calculate(Method M) 
        {
            double a = (double)numericUpDown1.Value;
            double b = (double)numericUpDown2.Value;
            int n = (int)numericUpDown3.Value;
            double result = 0;
            double current = 0;
            int count = numbers.Count();
            int count2 = functions.Count();
            for (int i = 0; i < count; i++ )
            {
                switch (signs[2 * i])
                {
                    case '+': current = numbers[i]; break;
                    default: current -= numbers[i]; break;
                }
                if (i < count2)
                {
                    switch (signs[2 * i + 1])
                    {
                        case '*': current *= M(a, b, n, dictionary[functions[i]]); break;
                        case '/': current /= M(a, b, n, dictionary[functions[i]]); break;
                        default: current %= M(a, b, n, dictionary[functions[i]]); break;
                    }
                }
                result += current;
            }
            return result; 
        }
    }
}