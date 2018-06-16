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
            testsNonRegression.Add("9+-sin(6+30)*9");
            testsNonRegression.Add("9+tan(60+3)*9");
            testsNonRegression.Add("9+exp(6+3)*9");
            testsNonRegression.Add("9+log(60+3)*9");
            testsNonRegression.Add("sqrt(4)");
            testsNonRegression.Add("sin(cos(tan(exp(log(8)))))");
            testsNonRegression.Add("abs(-4)");

            foreach (String str in testsNonRegression)
            {
                CalculStr(str);
            }

            SetFocus();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;

            String str = bindingCalcul.StrCalcul + button.Content;

            if (button.Content.Equals("sin") || button.Content.Equals("cos") || button.Content.Equals("tan") || button.Content.Equals("exp")
                || button.Content.Equals("log") || button.Content.Equals("sqrt") || button.Content.Equals("abs"))
            {
                str += "(";
            }
            
            AddCharToCalculStr(str);
            SetFocus();
        }
       
        private void Erase_Button(object sender, RoutedEventArgs e)
        {
            bindingCalcul.StrCalcul = "";
            SetFocus();
        }

        private void Del_Button(object sender, RoutedEventArgs e)
        {
            if (bindingCalcul.StrCalcul.Length > 0) {
                bindingCalcul.StrCalcul = removeLastCharacter(bindingCalcul.StrCalcul);
            }
            SetFocus();
        }

        private void ClearHistory_Button(object sender, RoutedEventArgs e)
        {
            bindingCalcul.StrCalcul = "";
            bindingCalcul.History.Clear();
            SetFocus();
        }

        private void Calcul_Click(object sender, RoutedEventArgs e)
        {
            String str = bindingCalcul.StrCalcul;

            CalculStr(str);
            SetFocus();
        }

        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            var str = bindingCalcul.StrCalcul;

            switch (e.Key)
            {
                case Key.Delete:
                    bindingCalcul.StrCalcul = "";
                    break;
                case Key.Back:
                    if (bindingCalcul.StrCalcul.Length > 0)
                    {
                        bindingCalcul.StrCalcul = removeLastCharacter(bindingCalcul.StrCalcul);
                    }
                    break;
                case Key.Enter:
                    CalculStr(str);
                    break;
                case Key.Add:
                    str += "+";
                    AddCharToCalculStr(str);
                    break;
                case Key.Subtract:
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
                case Key.Oem4:
                    str += ")";
                    AddCharToCalculStr(str);
                    break;
                case Key.Oem3:
                    str += "%";
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
                    if (Keyboard.Modifiers == ModifierKeys.Shift || Key.NumPad5 == e.Key)
                    {
                        str += "5";
                    }
                    else
                    {
                        str += "(";
                    }
                    
                    AddCharToCalculStr(str);
                    break;
                case Key.NumPad6:
                case Key.D6:
                    if (Keyboard.Modifiers == ModifierKeys.Shift || Key.NumPad6 == e.Key)
                    {
                        str += "6";
                    }
                    else
                    {
                        str += "-";
                    }
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

                case Key.S:
                    str += "s";
                    AddCharToCalculStr(str);
                    break;
                case Key.I:
                    str += "in(";
                    AddCharToCalculStr(str);
                    break;
                case Key.Q:
                    str += "qrt(";
                    AddCharToCalculStr(str);
                    break;
                case Key.T:
                    str += "tan(";
                    AddCharToCalculStr(str);
                    break;
                case Key.C:
                    str += "cos(";
                    AddCharToCalculStr(str);
                    break;
                case Key.A:
                    str += "abs(";
                    AddCharToCalculStr(str);
                    break;
                case Key.E:
                    str += "exp(";
                    AddCharToCalculStr(str);
                    break;
                case Key.L:
                    str += "log(";
                    AddCharToCalculStr(str);
                    break;

            }

            SetFocus();
        }

        private void DoubleClickHistory_Event(object sender, RoutedEventArgs e)
        {
            ListBox list = (ListBox) sender;

            if (list.SelectedItem != null)
            {
                bindingCalcul.StrCalcul = (String) list.SelectedItem;
            }
        }

        private String removeLastCharacter(String str)
        {
            int idxEnd = 0;
            for (int i=str.Length-2; i>=0; i--)
            {
                if (Char.IsNumber(str.ElementAt(i)) || this.listPattern.Contains(str.ElementAt(i)) || Char.Equals(str.ElementAt(i), '(')
                    || Char.Equals(str.ElementAt(i), ')'))
                {
                    idxEnd = i+1;
                    break; 
                }
            }

            return str.Substring(0, idxEnd);
        }

        private void AddCharToCalculStr(String str)
        {
            bindingCalcul.AffErreur = false;

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

            if (str.Length > 0)
            {
                if (verifyString(str))
                {
                    if (validParantheses(str))
                    {
                        str = Service.Calculateur.replaceForgotOperande(str);
                        bindingCalcul.History.Insert(0, str.Replace('N', '-'));
                        str = Service.Calculateur.replaceSinCosTan(str);
                        Operation op = Service.Calculateur.buildOperationsTree(str);
                        bindingCalcul.StrCalcul = Service.Calculateur.calcul(op).ToString();
                    }
                }
            }
            else
            {
                bindingCalcul.AffErreur = true;
                bindingCalcul.Erreur = "Chaine vide";
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
                        bindingCalcul.AffErreur = true;
                        bindingCalcul.Erreur = "Virgule invalide";
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
                        bindingCalcul.AffErreur = true;
                        bindingCalcul.Erreur = "Opérande invalide";
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
                        bindingCalcul.AffErreur = true;
                        bindingCalcul.Erreur = "Parenthèses vide";
                        return false;
                    }

                    if (stack.Count == 0)
                    {
                        bindingCalcul.AffErreur = true;
                        bindingCalcul.Erreur = "Pas assez de parenthèses ouvrantes";
                        return false;
                    }
                    stack.Pop();
                }
            }

            return stack.Count == 0;
        }

        private void SetFocus()
        {
            this.Focusable = true;
            Keyboard.Focus(this);
        }
    }
}
