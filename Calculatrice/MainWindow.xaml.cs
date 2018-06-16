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
        private List<char> listPattern;


        public MainWindow()
        {
            InitializeComponent();
            this.listPattern = new List<char>();
            listPattern.Add('-');
            listPattern.Add('+');
            listPattern.Add('/');
            listPattern.Add('*');
            listPattern.Add('%');

            /*entryCalcul.Text = "5+3+(3*4+2)*(5*3+6)+1-3";
            entryCalcul.Text = "5*6/3+(3*2+1)+3*4+(5*6+(6/9)*5+5-(4+6+(4+5)))";
            entryCalcul.Text = "5*6/3+(3*2+1)+3*4";
            entryCalcul.Text = "5+3*4+2";
            entryCalcul.Text = "5,4543+3,454+(3,565*4,3+2,0034)*(5,43*3+6)+1,34-3";
            entryCalcul.Text = "-5+3*4+2";
            entryCalcul.Text = "5*6/3+4(-3*2+1)3+3*4";
            */
            entryCalcul.Text = "5,4543+3,454+(3,565*4,3+2,0034)*(5,43*3+6)+1,34-3";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            
            var str = entryCalcul.Text + button.Content;
            AddCharToCalculStr(str);
        }
       
        private void Erase_Button(object sender, RoutedEventArgs e)
        {
            entryCalcul.Text = "";
        }

        private void Del_Button(object sender, RoutedEventArgs e)
        {
            if (entryCalcul.Text.Length > 0) {
                entryCalcul.Text = entryCalcul.Text.Substring(0, entryCalcul.Text.Length - 1);
            }
        }

        private void Calcul_Click(object sender, RoutedEventArgs e)
        {
            String str = entryCalcul.Text;

            CalculStr(str);
        }


        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            var str = entryCalcul.Text;

            switch (e.Key)
            {
                case Key.Delete:
                    entryCalcul.Text = "";
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

        private void AddCharToCalculStr(String str)
        {
            str = replaceNegativeNumber(str);

            if (verifyString(str))
            {
                str = str.Replace('N', '-');
                entryCalcul.Text = str;
            }
        }

        private void CalculStr(String str)
        {
            str = replaceNegativeNumber(str);
            if (verifyString(str))
            {
                if (validParantheses(str))
                {
                    str = replaceForgotOperande(str);
                    var op = buildOperationsTree(str);
                    entryCalcul.Text = calcul(op).ToString();
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
                    if (stack.Count == 0)
                    {
                        return false;
                    }
                    stack.Pop();
                }
            }

            return stack.Count == 0;
        }

        private String replaceNegativeNumber(String str)
        {
            String strReturn = "";
            for (var i = 0; i < str.Length; i++)
            {
                if (Char.Equals(str.ElementAt(i), '-') && !Char.IsNumber(str.ElementAtOrDefault(i-1)) && !Char.Equals(str.ElementAtOrDefault(i - 1), ')'))
                {
                    strReturn += 'N';
                } else
                {
                    strReturn += str.ElementAt(i);
                }
            }

            return strReturn;
        }

        private String replaceForgotOperande(String str)
        {
            var returnStr = "";

            for(var i=0; i<str.Length; i++)
            {
                if (Char.Equals(str.ElementAt(i), '(') && (Char.IsDigit(str.ElementAtOrDefault(i-1)) || Char.Equals(str.ElementAtOrDefault(i-1), ')')))
                {
                    returnStr += "*";
                }

                returnStr += str.ElementAt(i);

                if (Char.Equals(str.ElementAt(i), ')') && Char.IsDigit(str.ElementAtOrDefault(i + 1)))
                {
                    returnStr += "*";
                }
            }

            return returnStr;
        }

        private Operation buildOperationsTree(String str, Operation leftOp = null)
        {
            Operation op = new Operation();
            Operation leftValue = leftOp;
            Operation rightvalue;

            //leftValue = paraanthèses
            // right value right text after operande

            if ( Char.Equals(str.ElementAt(0), '('))
            {
                //Récupération de l'index de la première opérande après les parenthèses
                var idxStart = 0;
                var cptParantheses = 0;
                var idxEndParantheses = -1;
                for (int i = 0; i < str.Length; i++)
                {
                    if (Char.Equals(str.ElementAt(i), '('))
                    {
                        cptParantheses++;
                    }

                    if (this.listPattern.Contains(str.ElementAt(i)) && cptParantheses == 0)
                    {
                        idxStart = i;
                        break;
                    }

                    if (Char.Equals(str.ElementAt(i), ')'))
                    {
                        cptParantheses--;

                        if (cptParantheses == 0)
                        {
                            idxEndParantheses = i;
                        }
                    }
                }

                if (idxStart == 0)
                {
                    return buildOperationsTree(str.Substring(1, str.Length - 2));
                }
                op.operande = ""+str.ElementAt(idxStart);

                //La valeur de gauche est égale au contenu des parenthèses
                op.valueLeft = buildOperationsTree(str.Substring(1, idxEndParantheses - 1));

                //En cas de divisions ou de multiplications on met le reste dans l'enfant de droite et on envois l'Opération pour quelle soit bien placée
                //Pour less addition et soustractions on met le reste dans l'enfant de droite
                var strAfterParantheses = str.Substring(idxStart + 1);
                if ( Char.Equals((char)Operande.Diviser, op.operande.ElementAt(0)) || Char.Equals((char)Operande.Multiplier, op.operande.ElementAt(0))
                    || Char.Equals((char)Operande.Modulo, op.operande.ElementAt(0)))
                {
                    int idxEnd = 0;
                    idxStart = 0;
                    cptParantheses = 0;
                    for (int i = 0; i < strAfterParantheses.Length; i++)
                    {
                        if (Char.Equals(strAfterParantheses.ElementAt(i), '('))
                        {
                            cptParantheses++;
                            idxStart = i + 1;
                        }

                        if (!Char.IsDigit(strAfterParantheses.ElementAt(i)) && !Char.Equals(strAfterParantheses.ElementAt(i), ',') && cptParantheses == 0) 
                        {
                            idxEnd = i;
                            break;
                        }

                        if (Char.Equals(strAfterParantheses.ElementAt(i), ')'))
                        {
                            cptParantheses--;
                        }
                    }

                    op.valueRight = buildOperationsTree(strAfterParantheses.Substring(idxStart, idxEnd - (idxStart * 2)));
                    return buildOperationsTree(strAfterParantheses.Substring(idxEnd), op);
                } else
                {
                    op.valueRight = buildOperationsTree(strAfterParantheses);
                }
               
            } else
            {
                for (var idx = 0; idx < str.Length; idx++)
                {
                    switch ( str.ElementAt(idx))
                    {
                        case (char) Operande.Plus:
                        case (char) Operande.Moins:

                            //Si un enfant est envoyé c'est par les multiplication ou division, on le met à gauche
                            //Sinon on met tout ce qui est à gauche de l'opérande dans l'enfant de gauche et pareil pour la droite
                            if (leftValue == null)
                            {
                                leftValue = buildOperationsTree(str.Substring(0, idx));
                            }
                            
                            rightvalue = buildOperationsTree(str.Substring(idx + 1));
                            op.valueRight = rightvalue;

                            op.valueLeft = leftValue;
                            op.operande = "" + str.ElementAt(idx);

                            return op;
                        case (char) Operande.Multiplier:
                        case (char) Operande.Diviser:
                        case (char) Operande.Modulo:
                            // Pareil que les additions et soustractions sauf qu'on envois l'object à l'enfant qui deviendra parent de cet objet
                            if (leftValue == null)
                            {
                                leftValue = buildOperationsTree(str.Substring(0, idx));
                            }
                            op.valueLeft = leftValue;

                            op.operande = "" + str.ElementAt(idx);

                            bool isParentheses = false;
                            int idxEnd = 0;
                            for (int i = idx + 1; i < str.Length; i++)
                            {
                                if (!Char.IsDigit(str.ElementAt(i)) && !Char.Equals(str.ElementAt(i), ','))
                                {
                                    idxEnd = i;

                                    if (Char.Equals(str.ElementAt(i), '('))
                                    {

                                        var cptParantheses = 0;
                                        for (int j = i; j < str.Length; j++)
                                        {
                                            if (Char.Equals(str.ElementAt(j), '('))
                                            {
                                                cptParantheses++;
                                            }

                                            if (Char.Equals(str.ElementAt(j), ')'))
                                            {
                                                cptParantheses--;

                                                if (cptParantheses == 0)
                                                {
                                                    isParentheses = true;
                                                    idxEnd = j;
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    break;
                                }
                            }
                            rightvalue = new Operation();

                            if (idxEnd == 0)
                            {
                                rightvalue.value = Convert.ToDouble(str.Substring(idx + 1));
                            }
                            else if(isParentheses)
                            {
                                var tmp = str.Substring(idx + 1, idxEnd - (idx));
                                rightvalue = buildOperationsTree(tmp);
                            }
                            else
                            {
                                rightvalue.value = Convert.ToDouble(str.Substring(idx + 1, idxEnd - (idx + 1)));
                            }

                            op.valueRight = rightvalue;

                            if (idxEnd == 0)
                            {
                                return op;
                            }
                            return buildOperationsTree(str.Substring(idxEnd), op);
                    }
                }

                if ( Char.Equals('N' , str.ElementAt(0)))
                {
                    str = str.Replace('N', '-');
                }
                op.value = Convert.ToDouble(str);
            }

            return op;
        }

        private double calcul(Operation ops)
        {
            if (ops.valueLeft == null && ops.valueRight == null)
            {
                return ops.value;
            }
            else
            {
                double left = calcul(ops.valueLeft);
                double right = calcul(ops.valueRight);

                switch(ops.operande)
                {
                    case "+":
                        return left + right;
                    case "-":
                        return left - right;
                    case "*":
                        return left * right;
                    case "/":
                        if (right == 0) //@TODO gérer exception
                        {
                            return 0;
                        }
                        return left / right;
                    case "%":
                        return left % right;
                    default:
                        return 0;
                }
            }
        }
    }
}
