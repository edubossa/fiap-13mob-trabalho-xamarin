using System;
using System.Threading.Tasks;
using Xamarin.Forms;

using XF.Atividade6.ViewModel;

namespace XF.Atividade6
{
    public partial class App : Application
    {

        #region ViewModels
        public static UserViewModel UserVM { get; set; }
        #endregion

        public App()
        {
            InitializeComponent();
            InitializeApplication();
            //MainPage = new XF_Atividade6Page();
            //MainPage = new NavigationPage(new View.AtividadeView());
            MainPage = new NavigationPage(new View.LoginView() { BindingContext = App.UserVM });
        }

        private void InitializeApplication()
        {
            if (UserVM == null)
            {
                UserVM = new UserViewModel();
            }
        }

        public async static Task NavigateToProfile(string message)
        {
            await App.Current.MainPage.Navigation.PushAsync(new View.AtividadeView());
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
