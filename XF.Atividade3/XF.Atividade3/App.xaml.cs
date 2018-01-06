using Xamarin.Forms;
using XF.Atividade3.ViewModel;

namespace XF.Atividade3
{
    public partial class App : Application
    {

        #region ViewModels
        public static StudentViewModel StudentVM { get; set; }
        public static UserViewModel UserVM { get; set; }
        #endregion

        public App()
        {
            InitializeComponent();
            InitializeApplication();

            //MainPage = new XF_Atividade3Page();
            //MainPage = new NavigationPage(new View.Student.MainPage() {  });
            MainPage = new NavigationPage(new View.Login.LoginView() { BindingContext = App.UserVM });
        }

        private void InitializeApplication()
        {
            if (StudentVM == null) {
                StudentVM = new StudentViewModel();
            }
            if (UserVM == null) {
                UserVM = new UserViewModel();
            }
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
