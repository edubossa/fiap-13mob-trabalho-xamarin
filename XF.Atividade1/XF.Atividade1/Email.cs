using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace XF.Atividade1
{
    public class Email : INotifyPropertyChanged
    {

        public bool active;
        public bool Active
        {
            get {return active;}
            set {
                active = value;
                if (!active) AccountEmail = string.Empty;
            }
        }

        private string accountEmail;
        public string AccountEmail
        {
            get {return accountEmail;}
            set {
                accountEmail = value;
                EventPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void EventPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }


    }
}
