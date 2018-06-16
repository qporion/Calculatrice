using Calculatrice.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculatrice.Service
{
    class Calculateur
    {  
        public static String replaceNegativeNumber(String str)
        {
            String strReturn = "";
            for (var i = 0; i < str.Length; i++)
            {
                if (Char.Equals(str.ElementAt(i), '-') && !Char.IsNumber(str.ElementAtOrDefault(i - 1)) && !Char.Equals(str.ElementAtOrDefault(i - 1), ')'))
                {
                    strReturn += 'N';
                }
                else
                {
                    strReturn += str.ElementAt(i);
                }
            }

            return strReturn;
        }

        public static String replaceForgotOperande(String str)
        {
            var returnStr = "";

            for (var i = 0; i < str.Length; i++)
            {
                if (Char.Equals(str.ElementAt(i), '(') && (Char.IsDigit(str.ElementAtOrDefault(i - 1)) || Char.Equals(str.ElementAtOrDefault(i - 1), ')')))
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

        public static Operation buildOperationsTree(String str, Operation leftOp = null)
        {
            List<char> listPattern = new List<char>();
            listPattern.Add('-');
            listPattern.Add('+');
            listPattern.Add('/');
            listPattern.Add('*');
            listPattern.Add('%');
            Operation op = new Operation();
            Operation leftValue = leftOp;
            Operation rightvalue;

            //leftValue = paranthèses
            // right value right text after operande

            if (Char.Equals(str.ElementAt(0), '('))
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

                    if (listPattern.Contains(str.ElementAt(i)) && cptParantheses == 0)
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
                op.operande = "" + str.ElementAt(idxStart);

                //La valeur de gauche est égale au contenu des parenthèses
                op.valueLeft = buildOperationsTree(str.Substring(1, idxEndParantheses - 1));

                //En cas de divisions ou de multiplications on met le reste dans l'enfant de droite et on envois l'Opération pour quelle soit bien placée
                //Pour less addition et soustractions on met le reste dans l'enfant de droite
                var strAfterParantheses = str.Substring(idxStart + 1);
                if (Char.Equals((char)Operande.Diviser, op.operande.ElementAt(0)) || Char.Equals((char)Operande.Multiplier, op.operande.ElementAt(0))
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

                    if (idxEnd == 0)
                    {
                        idxEnd = 1;
                        op.valueRight = buildOperationsTree(strAfterParantheses.Substring(idxStart, idxEnd - (idxStart * 2)));
                        return op;
                    }
                    op.valueRight = buildOperationsTree(strAfterParantheses.Substring(idxStart, idxEnd - (idxStart * 2)));
                    return buildOperationsTree(strAfterParantheses.Substring(idxEnd), op);
                }
                else
                {
                    op.valueRight = buildOperationsTree(strAfterParantheses);
                }

            }
            else
            {
                for (var idx = 0; idx < str.Length; idx++)
                {
                    switch (str.ElementAt(idx))
                    {
                        case (char)Operande.Plus:
                        case (char)Operande.Moins:

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
                        case (char)Operande.Multiplier:
                        case (char)Operande.Diviser:
                        case (char)Operande.Modulo:
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
                            else if (isParentheses)
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

                if (Char.Equals('N', str.ElementAt(0)))
                {
                    str = str.Replace('N', '-');
                }
                op.value = Convert.ToDouble(str);
            }

            return op;
        }

        public static double calcul(Operation ops)
        {
            if (ops.valueLeft == null && ops.valueRight == null)
            {
                return ops.value;
            }
            else
            {
                double left = calcul(ops.valueLeft);
                double right = calcul(ops.valueRight);

                switch (ops.operande)
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
