using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculatrice.Ressources
{
    public partial class Buttons : ResourceDictionary
    {
        public Buttons()
        {
            
        }

        void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(sender.ToString());
        }
    }
}
