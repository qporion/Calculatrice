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
        private String strCalcul, erreur;
        private bool affErreur;

        public BindingCalcul()
        {
            this.strCalcul = "";
            this.erreur = "";
            this.affErreur = false;
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

        public String Erreur
        {
            get
            {
                return this.erreur;
            }

            set
            {
                if (value != this.erreur)
                {
                    this.erreur = value;
                    NotifyPropertyChanged(nameof(erreur));
                }
            }
        }

        public bool AffErreur
        {
            get
            {
                return this.affErreur;
            }

            set
            {
                if (value != this.affErreur)
                {
                    this.affErreur = value;
                    NotifyPropertyChanged(nameof(affErreur));
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
