using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XF.Atividade5.Model;
using XF.Atividade5.ViewModel;

namespace XF.Atividade5.View
{   
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ContatosView : ContentPage
    {
        public ContatoViewModel contatosVM = new ContatoViewModel();
        public ContatosView() {
            BindingContext = contatosVM;
            InitializeComponent();
            Contatos(contatosVM);
        }

        private void Contatos(ContatoViewModel vm) {
            IContato lista = DependencyService.Get<IContato>();
            lista.GetMobileContacts(vm);
        }
    }
}
