using Xamarin.Forms;
using XF.Atividade2.ViewModel;

namespace XF.Atividade2
{
    public partial class App : Application
    {
        #region ViewModels
        public static StudentViewModel StudentVM { get; set; }
        #endregion

        public App()
        {
            InitializeComponent();
            InitializeApplication();
            //Configuramos aqui a pagina principal do projeto
            //MainPage = new NavigationPage(new View.StudentView());
            MainPage = new NavigationPage(new View.StudentView() { BindingContext = App.StudentVM });
        }

        private void InitializeApplication()
        {
            if (StudentVM == null) StudentVM = new StudentViewModel();
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
