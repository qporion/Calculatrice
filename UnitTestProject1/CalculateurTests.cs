using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Calculatrice;

namespace TestsUnitaire
{
    [TestClass]
    public class CalculateurTest
    {
        public Dictionary<String, String> getData()
        {
            Dictionary<String, String> testsNonRegression = new Dictionary<String, String>();
            testsNonRegression.Add("5+3+(3*4+2)*(5*3+6)+1-3");
            testsNonRegression.Add("5*6/3+(3*2+1)+3*4+(5*6+(6/9)*5+5-(4+6+(4+5)))");
            testsNonRegression.Add("5*6/3+(3*2+1)+3*4");
            testsNonRegression.Add("5+3*4+2");
            testsNonRegression.Add("5,4543+3,454+(3,565*4,3+2,0034)*(5,43*3+6)+1,34-3");
            testsNonRegression.Add("-5+3*4+2");
            testsNonRegression.Add("5*6/3+4(-3*2+1)3+3*4");
            testsNonRegression.Add("9+(6+3)*9", "90");

            return testsNonRegression;
        }

        [TestMethod]
        public void testCalcul()
        {
            Dictionary<String, String> data = getData();

            foreach(KeyValuePair<String, String> row in data)
            {
                String str = Service.Calculateur.replaceNegativeNumber(row.Key);
                str = Service.Calculateur.replaceForgotOperande(str);
                var op = Service.Calculateur.buildOperationsTree(str);
                Assert.AreEqual(row.Value, Service.Calculateur.calcul(op).ToString());
            }
        }
    }
}
