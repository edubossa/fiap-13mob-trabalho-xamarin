using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XF.Atividade6.ViewModel;

namespace XF.Atividade6.View
{   
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AtividadeView : ContentPage
    {
        public AtividadeView()
        {
            BindingContext = AtividadeViewModel.Instancia;
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            listaAtividades.IsRefreshing = !listaAtividades.IsRefreshing;
            await AtividadeViewModel.Instancia.Carregar();
            listaAtividades.IsRefreshing = !listaAtividades.IsRefreshing;
        }

    }
}
