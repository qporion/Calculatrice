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
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;

            var strCalcul = entryCalcul.Text + button.Content;

            if(verifyString(getOperations(strCalcul)))
            {
                entryCalcul.Text += button.Content;
            }
        }

        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Delete:
                    entryCalcul.Text = "";
                    break;
                case Key.Enter:
                    var op = getOperations(entryCalcul.Text);
                    entryCalcul.Text = calculOperation(op).ToString();
                    break;
            } 
        }

        private bool verifyString(Operation listCalcul) //^(\w |\-)+((\w |\-)+(,|\+|\(|\*|\)))+(\w +)$
        {
            /*var isValid = true;
            foreach (Object item in listCalcul)
            {
                if (item.GetType() == typeof(List<Object>))
                {
                    isValid = verifyString(item as List<Object>);
                } else
                {
                    var str = item as String;
                    if (str.Count(c => c == ',') > 1)
                    {
                        return false;
                    }

                    if (Char.Equals(str.ElementAt(0), ','))
                    {
                        return false;
                    }
                }
            }*/

            return true;
        }

        private Operation getOperations(String str, Operation leftOp = null)
        {
            Operation op = new Operation();
            Operation leftValue = leftOp;
            Operation rightvalue;

            //leftValue = paraanthèses
            // right value right text after operande

            if ( Char.Equals(str.ElementAt(0), '('))
            {

                var tmp = str.Substring(1, str.LastIndexOf(')') - 1);

                op.valueLeft = getOperations(tmp);

                var idxStart = 0;
                for (int i = str.LastIndexOf(')'); i < str.Length; i++)
                {
                    if (this.listPattern.Contains(str.ElementAt(i)))
                    {
                        idxStart = i;
                        break;
                    }
                }

                op.operande = ""+str.ElementAt(idxStart);
                op.valueRight = getOperations(str.Substring(idxStart+1));
            } else
            {
                for (var idx = 0; idx < str.Length; idx++)
                {
                    switch (str.ElementAt(idx))
                    {
                        case '+':
                        case '-':

                            if (leftValue == null)
                            {
                                leftValue = getOperations(str.Substring(0, idx));

                            }
                            rightvalue = getOperations(str.Substring(idx + 1));
                            op.valueRight = rightvalue;

                            op.valueLeft = leftValue;
                            op.operande = "" + str.ElementAt(idx);

                            return op;
                            break;
                        case '*':
                        case '/':

                            if (leftValue == null)
                            {
                                leftValue = getOperations(str.Substring(0, idx));
                            }
                            op.valueLeft = leftValue;

                            op.operande = "" + str.ElementAt(idx);

                            int idxEnd = 0;
                            for (int i = idx + 1; i < str.Length; i++)
                            {
                                if (!Char.IsDigit(str.ElementAt(i)) && !Char.Equals(str.ElementAt(i), ','))
                                {
                                    idxEnd = i;
                                    break;
                                }
                            }
                            rightvalue = new Operation();

                            if (idxEnd == 0)
                            {
                                var tmp = str.Substring(idx + 1);
                                rightvalue.value = Convert.ToDouble(tmp);
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
                            return getOperations(str.Substring(idxEnd), op);

                            break;
                    }
                }

                op.value = Convert.ToDouble(str);
            }

            return op;
        }

        private double calculOperation(Operation ops)
        {
            if (ops.valueLeft == null && ops.valueRight == null)
            {
                return ops.value;
            }
            else
            {
                double left = calculOperation(ops.valueLeft);
                double right = calculOperation(ops.valueRight);

                switch(ops.operande)
                {
                    case "+":
                        return left + right;
                        break;
                    case "-":
                        return left - right;
                        break;
                    case "*":
                        return left * right;
                        break;
                    case "/":
                        return left / right;
                        break;
                    default:
                        return 0;
                }
            }
        }
    }
}
