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

namespace Calculatrice
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;

            var strCalcul = entryCalcul.Text + button.Content;

            if(verifyString(strCalcul))
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
                    entryCalcul.Text = calculStr(entryCalcul.Text).ToString();
                    break;
            } 
        }

        private bool verifyString(String strCalcul) //^(\w |\-)+((\w |\-)+(,|\+|\(|\*|\)))+(\w +)$
        {
            foreach (String str in getListWithString(strCalcul))
            {
                if (str.Count( c => c == ',') > 1)
                {
                    return false;
                }
            }

            return true;
        }

        private List<String> getListWithString(String str)
        {
            List<String> listStrCalcul = new List<String>();
            List<char> listPattern = new List<char>();
            listPattern.Add('-');
            listPattern.Add('+');
            listPattern.Add('/');
            listPattern.Add('*');

            var tmpStr = "";
            for (var i = 0; i < str.Length; i++)
            {
                if (listPattern.Contains(str.ElementAt(i)))
                {
                    if (Char.Equals(str.ElementAt(i), '-') && !(Char.IsNumber(str.ElementAtOrDefault(i-1)) || Char.Equals(str.ElementAtOrDefault(i - 1), ')')))
                    {
                        tmpStr += str.ElementAt(i);
                    }
                    else
                    {
                        listStrCalcul.Add(tmpStr);
                        listStrCalcul.Add("" + str.ElementAt(i));
                        tmpStr = "";
                    }
                }
                else
                {
                    tmpStr += str.ElementAt(i);
                }
            }

            listStrCalcul.Add(tmpStr);

            return listStrCalcul;
        }

        private float calculStr(String str)
        {
            List<String> listCalcul = getListWithString(str);

            foreach (String strList in listCalcul)
            {
                
            }

            return 0;
        }
    }
}
