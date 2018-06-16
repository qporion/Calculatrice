using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculatrice.Model
{
    class BindingCalcul : INotifyPropertyChanged
    {
        private String strCalcul;

        public BindingCalcul()
        {
            this.strCalcul = "";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public String StrCalcul
        {
            get
            {
                return this.strCalcul;
            }

            set
            {
                if (value != this.strCalcul)
                {
                    this.strCalcul = value;
                    NotifyPropertyChanged(nameof(strCalcul));
                }
            }
        }

        private ObservableCollection<String> history = new ObservableCollection<String>();
        public ObservableCollection<String> History
        {
            get
            {
                return this.history;
            }

            set
            {
                if (value != this.history)
                {
                    this.history = value;
                    NotifyPropertyChanged(nameof(history));
                }
            }
        }

        private void NotifyPropertyChanged(String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

    }
}
