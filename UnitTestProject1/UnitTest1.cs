using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestCalcul()
        {
            // List de tests @TODO rajouter des tests pour chaque bug rencontré
            Dictionary<String, String> testsNonRegression = new Dictionary<String, String>();
            testsNonRegression.Add("5+3+(3*4+2)*(5*3+6)+1-3", "300");
            testsNonRegression.Add("5*6/3+(3*2+1)+3*4+(5*6+(6/9)*5+5-(4+6+(4+5)))", "48,3333333333333");
            testsNonRegression.Add("5*6/3+(3*2+1)+3*4", "29");
            testsNonRegression.Add("5+3*4+2", "19");
            testsNonRegression.Add("5,4543+3,454+(3,565*4,3+2,0034)*(5,43*3+6)+1,34-3", "393,598641");
            testsNonRegression.Add("-5+3*4+2", "9");
            testsNonRegression.Add("5*6/3+4(-3*2+1)3+3*4", "-38");
            testsNonRegression.Add("9+(6+3)*9", "90");
            testsNonRegression.Add("cos(60)+3", "2,04758701958484");
            testsNonRegression.Add("9+sin(30)", "8,01196837590714");
            testsNonRegression.Add("9+cos(60+3)*9", "17,8730692342429");
            testsNonRegression.Add("9+sin(6+30)*9", "0,0739903190119566");
            testsNonRegression.Add("5*6/3+(3*2+1)+3*4+sin(5*6+(6/9)*5+5-(4+6+(4+5)))", "29,4651264181338");
            testsNonRegression.Add("5,4543+3,454+(3,565*4,3+2,0034)*cos(5,43*3+6)+1,34-3", "-9,31632416572502");
            testsNonRegression.Add("9+-sin(6+30)*9", "17,926009680988");
            testsNonRegression.Add("9+tan(60+3)*9", "10,5277477687442");
            testsNonRegression.Add("9+exp(6+3)*9", "72936,7553481784");
            testsNonRegression.Add("9+log(60+3)*9", "46,2882125375238");
            testsNonRegression.Add("sqrt(4)", "2");
            testsNonRegression.Add("sin(cos(tan(exp(log(8)))))", "0,764032254013142");
            testsNonRegression.Add("abs(-4)", "4");
            testsNonRegression.Add("4/-5+6", "5,2");
            testsNonRegression.Add("9+exp(60+3)*9", "2,06440484352265E+28");
            testsNonRegression.Add("(3*4+2)*cos(5*3+6)+1", "-6,66820964313975");
            testsNonRegression.Add("log(0)*log(0)", "∞");
            testsNonRegression.Add("9/0", "NaN");

            foreach (KeyValuePair<String, String> row in testsNonRegression)
            {
                AssertCalcul(row.Key, row.Value);
            }
        }

        [TestMethod]
        public void TestReplaceNegative()
        {
            String str = Calculatrice.Service.Calculateur.replaceNegativeNumber("-9,31632416572502");

            Assert.AreEqual("N9,31632416572502", str);

            str = Calculatrice.Service.Calculateur.replaceNegativeNumber("--9,31632416572502");

            Assert.AreEqual("NN9,31632416572502", str);

            str = Calculatrice.Service.Calculateur.replaceNegativeNumber("---9,31632416572502");

            Assert.AreEqual("NNN9,31632416572502", str);


            str = Calculatrice.Service.Calculateur.replaceNegativeNumber("9-9,31632416572502");

            Assert.AreEqual("9-9,31632416572502", str);
        }

        [TestMethod]
        public void TestReplaceBigNumber()
        {
            String str = Calculatrice.Service.Calculateur.replaceBigNumber("2+2,06440484352265E+28");

            Assert.AreEqual("2+2,06440484352265E28", str); 
        }

        [TestMethod]
        public void TestReplaceForgotOperande()
        {
            String str = Calculatrice.Service.Calculateur.replaceForgotOperande("5*6/3+4(-3*2+1)3+3*4");

            Assert.AreEqual("5*6/3+4*(-3*2+1)*3+3*4", str);
        }

        [TestMethod]
        public void TestReplaceSinCosTan()
        {
            String str = Calculatrice.Service.Calculateur.replaceSinCosTan("sin(cos(tan(exp(log(8)))))");

            Assert.AreEqual("0,764032254013142", str);

            str = Calculatrice.Service.Calculateur.replaceSinCosTan("log(0)*log(0)");

            Assert.AreEqual("N∞*N∞", str);
        }

        private void AssertCalcul(String strCalcul, String expected)
        {
            Calculatrice.Service.Calculateur.replaceNegativeNumber(strCalcul);

            strCalcul = Calculatrice.Service.Calculateur.replaceForgotOperande(strCalcul);
            strCalcul = Calculatrice.Service.Calculateur.replaceSinCosTan(strCalcul);
            strCalcul = Calculatrice.Service.Calculateur.replaceBigNumber(strCalcul);
            Calculatrice.Model.Operation op = Calculatrice.Service.Calculateur.buildOperationsTree(strCalcul);

            Assert.AreEqual(expected, Calculatrice.Service.Calculateur.calcul(op).ToString());
        }
    }
}
