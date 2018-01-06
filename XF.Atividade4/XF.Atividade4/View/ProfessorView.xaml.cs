using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XF.Atividade4.ViewModel;

namespace XF.Atividade4.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfessorView : ContentPage
    {
        public ProfessorView() {
            BindingContext = ProfessorViewModel.Instancia;
            InitializeComponent();
        }

        protected override async void OnAppearing() {
            base.OnAppearing();
            listaProfessores.IsRefreshing = !listaProfessores.IsRefreshing;
            await ProfessorViewModel.Instancia.Carregar();
            listaProfessores.IsRefreshing = !listaProfessores.IsRefreshing;
        }
    }
}
