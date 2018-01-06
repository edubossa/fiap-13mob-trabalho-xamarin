using XF.Atividade5.Model;
using XF.Atividade5.ViewModel;
using Xamarin.Contacts;
using Xamarin.Forms;
using XF.Atividade5.iOS;
using UIKit;


[assembly: Dependency(typeof(AddressBook_iOS))]
namespace XF.Atividade5.iOS
{
    public class AddressBook_iOS : IContato
    {

        public object Bitmap { get; private set; }

        public async void GetMobileContacts(ContatoViewModel vm) {
            var book = new Xamarin.Contacts.AddressBook();
            if (await book.RequestPermission()) {
                foreach (Contact contact in book) {
                    AdicionarContato(contact, vm);
                }
            }
            else
            {
                var message = "Permissao negada. Habite acesso a lista de contatos";
                UIAlertView avAlert = new UIAlertView("Autorizacao", message, null, "OK");
                avAlert.Show();
            }
        }

        void AdicionarContato(Contact paramContato, ContatoViewModel vm)
        {
            var contato = new Contato() {
                Nome = paramContato.FirstName,
                SobreNome = paramContato.LastName,
                DisplayName = paramContato.DisplayName
            };

            foreach (var item in paramContato.Phones) {
                var telefone = new Telefone() {
                    Descricao = item.Label,
                    Numero = item.Number
                };
                switch (item.Type)
                {
                    case Xamarin.Contacts.PhoneType.Home:
                        telefone.Tipo = Model.PhoneType.Home;
                        break;
                    case Xamarin.Contacts.PhoneType.HomeFax:
                        telefone.Tipo = Model.PhoneType.HomeFax;
                        break;
                    case Xamarin.Contacts.PhoneType.Work:
                        telefone.Tipo = Model.PhoneType.Work;
                        break;
                    case Xamarin.Contacts.PhoneType.WorkFax:
                        telefone.Tipo = Model.PhoneType.WorkFax;
                        break;
                    case Xamarin.Contacts.PhoneType.Pager:
                        telefone.Tipo = Model.PhoneType.Pager;
                        break;
                    case Xamarin.Contacts.PhoneType.Mobile:
                        telefone.Tipo = Model.PhoneType.Mobile;
                        break;
                    case Xamarin.Contacts.PhoneType.Other:
                        telefone.Tipo = Model.PhoneType.Other;
                        break;
                    default:
                        break;
                }
                contato.Telefones.Add(telefone);
            }
            vm.Contatos.Add(contato);
        }
    }
}
