using System;
using System.Net.Http;
using System.Windows.Input;
using XF.Atividade3.Model;
namespace XF.Atividade3.ViewModel
{
    public class UserViewModel
    {
        #region Propriedade
        public User UserModel { get; set; }
        public string Name { get; set; }
        public string Stream { get; set; }

        // Eventos UI
        public IsAutenticarCMD IsAutenticarCMD { get; }
        #endregion

        public UserViewModel() {
            UserModel = new User();
            IsAutenticarCMD = new IsAutenticarCMD(this);
            this.GetUsuarios("http://wopek.com.spiraea.arvixe.com/xml/usuarios.xml");
        }

        public void IsAutenticar(User paramUser) {
            this.Name = paramUser.Username;
            if (UserRepository.IsAutorizado(paramUser)) {
                App.Current.MainPage.Navigation.PushAsync(
                    new View.Student.MainPage() { BindingContext = App.StudentVM });
            } else {
                App.Current.MainPage.DisplayAlert("Atencao", "Usuario nao autorizado", "Ok");
            }
        }

        private async void GetUsuarios(string paramURL) {
            var httpRequest = new HttpClient();
            Stream = await httpRequest.GetStringAsync(paramURL);
        }
    }

    public class IsAutenticarCMD : ICommand {
        private UserViewModel userVM;
        public IsAutenticarCMD(UserViewModel paramVM) {
            userVM = paramVM;
        }
        public event EventHandler CanExecuteChanged;
        public void DeleteCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        public bool CanExecute(object parameter) {
            if (parameter != null) return true;
            return false;
        }
        public void Execute(object parameter) {
            userVM.IsAutenticar(parameter as User);
        }
    }

}
