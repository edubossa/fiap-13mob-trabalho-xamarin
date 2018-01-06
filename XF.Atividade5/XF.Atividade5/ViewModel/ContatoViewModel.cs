using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.Forms;
using XF.Atividade5.App_Code;
using XF.Atividade5.Model;

namespace XF.Atividade5.ViewModel
{
    public class ContatoViewModel
    {
        private Contato contato;
        public Contato Contato {
            get { return contato; }
            set {
                contato = value as Contato;
                OnLigarCMD.LigarCanExecuteChanged();
                EventPropertyChanged();
            }
        }
        public ObservableCollection<Contato> Contatos { get; set; } = new ObservableCollection<Contato>();
        // Eventos da Interfce
        public OnDetalheCMD OnDetalheCMD { get; }
        public OnLigarCMD OnLigarCMD { get; }

        public ContatoViewModel() {
            OnDetalheCMD = new OnDetalheCMD(this);
            OnLigarCMD = new OnLigarCMD(this);
        }

        public async void Ligar(Contato paramContato) {
            if (!string.IsNullOrWhiteSpace(paramContato.Numero)) {
                if (await App.Current.MainPage.DisplayAlert("Ligando ... ", "Ligar para " + paramContato.Numero + "?", "Sim", "Nao")) {
                    var phone = DependencyService.Get<ITelefone>();
                    if (phone != null) {
                        phone.Ligar(paramContato.Numero);
                    }
                }
            }
        }

        public void GetDetalhe(Contato paramContato) {
            if (paramContato != null) {
                App.Current.MainPage.Navigation.PushAsync(
                    new View.DetalheView() { BindingContext = paramContato });
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void EventPropertyChanged([CallerMemberName] string propertyName = null) {
            if (this.PropertyChanged != null) {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }    
    }

    public class OnLigarCMD : ICommand {
        private ContatoViewModel contatoVM;
        public OnLigarCMD(ContatoViewModel paramVM) {
            contatoVM = paramVM;
        }
        public event EventHandler CanExecuteChanged;
        public void LigarCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        public bool CanExecute(object parameter) {
            if (parameter != null) {
                return true;
            }
            return false;
        }
        public void Execute(object parameter) {
            contatoVM.Ligar(parameter as Contato);
        }
    }

    public class OnDetalheCMD : ICommand {
        private ContatoViewModel contatoVM;
        public OnDetalheCMD(ContatoViewModel paramVM) {
            contatoVM = paramVM;
        }
        public event EventHandler CanExecuteChanged;
        public void DetalheCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        public bool CanExecute(object parameter) {
            if (parameter != null) {
                return true;
            }
            return false;
        }
        public void Execute(object parameter) {
            contatoVM.GetDetalhe(parameter as Contato);
        }
    }

}
