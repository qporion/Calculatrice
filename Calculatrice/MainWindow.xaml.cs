using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Calculatrice.Model;

namespace Calculatrice
{
    enum Operande { Moins = '-', Plus = '+', Multiplier = '*', Diviser = '/', Modulo = '%'};
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<char> listPattern;

        BindingCalcul bindingCalcul = new BindingCalcul();

        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = bindingCalcul;
            this.listPattern = new List<char>();
            listPattern.Add('-');
            listPattern.Add('+');
            listPattern.Add('/');
            listPattern.Add('*');
            listPattern.Add('%');

            // List de tests @TODO rajouter des tests pour chaque bug rencontré
            List<String> testsNonRegression = new List<String>();
            testsNonRegression.Add("5+3+(3*4+2)*(5*3+6)+1-3");
            testsNonRegression.Add("5*6/3+(3*2+1)+3*4+(5*6+(6/9)*5+5-(4+6+(4+5)))");
            testsNonRegression.Add("5*6/3+(3*2+1)+3*4");
            testsNonRegression.Add("5+3*4+2");
            testsNonRegression.Add("5,4543+3,454+(3,565*4,3+2,0034)*(5,43*3+6)+1,34-3");
            testsNonRegression.Add("-5+3*4+2");
            testsNonRegression.Add("5*6/3+4(-3*2+1)3+3*4");
            testsNonRegression.Add("9+(6+3)*9");
            testsNonRegression.Add("cos(60)+3");
            testsNonRegression.Add("9+sin(30)");
            testsNonRegression.Add("9+cos(60+3)*9");
            testsNonRegression.Add("9+sin(6+30)*9");
            testsNonRegression.Add("5*6/3+(3*2+1)+3*4+sin(5*6+(6/9)*5+5-(4+6+(4+5)))");
            testsNonRegression.Add("5,4543+3,454+(3,565*4,3+2,0034)*cos(5,43*3+6)+1,34-3");

            foreach (String str in testsNonRegression)
            {
                CalculStr(str);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;

            String str = bindingCalcul.StrCalcul + button.Content;

            if (button.Content.Equals("sin") || button.Content.Equals("cos"))
            {
                str += "(";
            }
            
            AddCharToCalculStr(str);
        }
       
        private void Erase_Button(object sender, RoutedEventArgs e)
        {
            bindingCalcul.StrCalcul = "";
        }

        private void Del_Button(object sender, RoutedEventArgs e)
        {
            if (bindingCalcul.StrCalcul.Length > 0) {
                bindingCalcul.StrCalcul = bindingCalcul.StrCalcul.Substring(0, bindingCalcul.StrCalcul.Length - 1);
            }
        }

        private void ClearHistory_Button(object sender, RoutedEventArgs e)
        {
            bindingCalcul.StrCalcul = "";
            bindingCalcul.History.Clear();
        }

        private void Calcul_Click(object sender, RoutedEventArgs e)
        {
            String str = bindingCalcul.StrCalcul;

            CalculStr(str);
        }

        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            var str = bindingCalcul.StrCalcul;

            switch (e.Key)
            {
                case Key.Delete:
                    bindingCalcul.StrCalcul = "";
                    break;
                case Key.Enter:
                    CalculStr(str);
                    break;
                case Key.OemPlus:
                    str += "+";
                    AddCharToCalculStr(str);
                    break;
                case Key.OemMinus:
                    str += "-";
                    AddCharToCalculStr(str);
                    break;
                case Key.Divide:
                    str += "/";
                    AddCharToCalculStr(str);
                    break;
                case Key.Multiply:
                    str += "*";
                    AddCharToCalculStr(str);
                    break;
                case Key.OemComma:
                    str += ",";
                    AddCharToCalculStr(str);
                    break;
                case Key.NumPad1:
                case Key.D1:
                    str += "1";
                    AddCharToCalculStr(str);
                    break;
                case Key.NumPad2:
                case Key.D2:
                    str += "2";
                    AddCharToCalculStr(str);
                    break;
                case Key.NumPad3:
                case Key.D3:
                    str += "3";
                    AddCharToCalculStr(str);
                    break;
                case Key.NumPad4:
                case Key.D4:
                    str += "4";
                    AddCharToCalculStr(str);
                    break;
                case Key.NumPad5:
                case Key.D5:
                    str += "5";
                    AddCharToCalculStr(str);
                    break;
                case Key.NumPad6:
                case Key.D6:
                    str += "6";
                    AddCharToCalculStr(str);
                    break;
                case Key.NumPad7:
                case Key.D7:
                    str += "7";
                    AddCharToCalculStr(str);
                    break;
                case Key.NumPad8:
                case Key.D8:
                    str += "8";
                    AddCharToCalculStr(str);
                    break;
                case Key.NumPad9:
                case Key.D9:
                    str += "9";
                    AddCharToCalculStr(str);
                    break;
            }
        }

        private void DoubleClickHistory_Event(object sender, RoutedEventArgs e)
        {
            ListBox list = (ListBox) sender;

            if (list.SelectedItem != null)
            {
                bindingCalcul.StrCalcul = (String) list.SelectedItem;
            }
        }

        private void AddCharToCalculStr(String str)
        {
            str = Service.Calculateur.replaceNegativeNumber(str);
            int cptParantheses = 0;

            for (int i=0; i<str.Length; i++)
            {
                if (Char.Equals(str.ElementAt(i), '('))
                {
                    cptParantheses++;
                }
                if (Char.Equals(str.ElementAtOrDefault(i), ')'))
                {
                    cptParantheses--;
                }

                if (cptParantheses < 0)
                {
                    break;
                }
            }

            if (verifyString(str) && cptParantheses >= 0) 
            {
                str = str.Replace('N', '-');
                bindingCalcul.StrCalcul = str;
            }
        }

        private void CalculStr(String str)
        {
            str = Service.Calculateur.replaceNegativeNumber(str);
            if (verifyString(str))
            {
                if (validParantheses(str))
                {
                    str = Service.Calculateur.replaceForgotOperande(str);
                    bindingCalcul.History.Insert(0, str.Replace('N', '-'));
                    str = Service.Calculateur.replaceSinCos(str);
                    Operation op = Service.Calculateur.buildOperationsTree(str);
                    bindingCalcul.StrCalcul = Service.Calculateur.calcul(op).ToString();
                }
            }
        }

        private bool verifyString(String str)
        {
            var isValid = true;
            isValid = validComma(str);
            isValid = validOperande(str) && isValid;

            return isValid;
        }

        private bool validComma(String str)
        {
            var cptComma = 0;
            for(var i=0; i<str.Length; i++)
            {
                if(Char.Equals(str.ElementAt(i), ','))
                {
                    cptComma++;

                    if(cptComma > 1 || !Char.IsDigit(str.ElementAtOrDefault(i-1)))
                    {
                        return false;
                    }
                }

                if (this.listPattern.Contains(str.ElementAt(i)))
                {
                    cptComma = 0;
                }
            }

            return true;
        }

        private bool validOperande(String str)
        {
            for (var i = 0; i < str.Length; i++)
            {
                if (this.listPattern.Contains(str.ElementAt(i)))
                {
                    if (!Char.IsDigit(str.ElementAtOrDefault(i - 1)) && !Char.Equals(str.ElementAtOrDefault(i-1), ')'))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private bool validParantheses(String str)
        {
            Stack<String> stack = new Stack<String>();

            for (var i = 0; i < str.Length; i++)
            {
                if (Char.Equals(str.ElementAt(i), '('))
                {
                    stack.Push(":D");
                }

                if (Char.Equals(str.ElementAt(i), ')'))
                {
                    if (Char.Equals(str.ElementAt(i-1), '('))
                    {
                        return false;
                    }

                    if (stack.Count == 0)
                    {
                        return false;
                    }
                    stack.Pop();
                }
            }

            return stack.Count == 0;
        }
    }
}
