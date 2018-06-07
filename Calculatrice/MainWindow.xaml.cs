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
    enum Operande { Moins = '-', Plus = '+', Multiplier = '*', Diviser = '/'};
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

            entryCalcul.Text = "5+3+(3*4+2)*(5*3+6)+1-3";
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

                    if (Char.Equals(str.ElementAt(i), ')'))
                    {
                        cptParantheses--;

                        if (cptParantheses == 0)
                        {
                            idxEndParantheses = i;
                        }
                    }

                    if (this.listPattern.Contains(str.ElementAt(i)) && cptParantheses == 0)
                    {
                        idxStart = i;
                        break;
                    }
                }

                op.operande = ""+str.ElementAt(idxStart);

                //La valeur de gauche est égale au contenu des parenthèses
                op.valueLeft = getOperations(str.Substring(1, idxEndParantheses - 1));

                //En cas de divisions ou de multiplications on met le reste dans l'enfant de droite et on envois l'Opération pour quelle soit bien placée
                //Pour less addition et soustractions on met le reste dans l'enfant de droite
                var strAfterParantheses = str.Substring(idxStart + 1);
                if ( Char.Equals((char)Operande.Diviser, op.operande.ElementAt(0)) || Char.Equals((char)Operande.Multiplier, op.operande.ElementAt(0)))
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

                    var tmp = strAfterParantheses.Substring(idxStart, idxEnd-(idxStart*2));
                    var tmp2 = strAfterParantheses.Substring(idxEnd);
                    op.valueRight = getOperations(tmp);
                    return getOperations(strAfterParantheses.Substring(idxEnd), op);
                } else
                {
                    op.valueRight = getOperations(strAfterParantheses);
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
                                var tmp3 = str.Substring(0, idx);
                                leftValue = getOperations(tmp3);

                            }

                            var tmp4 = str.Substring(idx + 1);
                            rightvalue = getOperations(tmp4);
                            op.valueRight = rightvalue;

                            op.valueLeft = leftValue;
                            op.operande = "" + str.ElementAt(idx);

                            return op;
                        case (char) Operande.Multiplier:
                        case (char) Operande.Diviser:
                            // Pareil que les additions et soustractions sauf qu'on envois l'object à l'enfant qui deviendra parent de cet objet
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
                    case "-":
                        return left - right;
                    case "*":
                        return left * right;
                    case "/":
                        return left / right;
                    default:
                        return 0;
                }
            }
        }
    }
}
