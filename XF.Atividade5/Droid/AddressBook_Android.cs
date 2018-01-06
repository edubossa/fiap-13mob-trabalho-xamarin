using System.IO;
using Android.Graphics;
using Android.App;
using Xamarin.Forms;
using Xamarin.Contacts;
using XF.Atividade5.Droid;
using XF.Atividade5.Model;
using XF.Atividade5.ViewModel;


[assembly: Dependency(typeof(AddressBook_Android))]
namespace XF.Atividade5.Droid
{
    public class AddressBook_Android : IContato
    {

        public async void GetMobileContacts(ContatoViewModel vm) {
            var context = Forms.Context as Activity;
            var book = new Xamarin.Contacts.AddressBook(context);
            if (await book.RequestPermission()) {
                foreach (Contact contact in book) {
                    AdicionarContato(contact, vm);
                }
            } else {
                AlertDialog.Builder messageUI = new AlertDialog.Builder(context);
                messageUI.SetMessage("Permissao negada. Habite acesso a lista de contatos");
                messageUI.SetTitle("Autorizacao");
                messageUI.Create().Show();
            }
        }

        void AdicionarContato(Contact paramContato, ContatoViewModel vm) {
            var image = paramContato.GetThumbnail();
            ImageSource imgSource = null;
            if (image != null) {
                byte[] imageFile = new byte[image.Width * image.Height * 4];
                MemoryStream stream = new MemoryStream(imageFile);
                image.Compress(Bitmap.CompressFormat.Png, 100, stream);
                stream.Flush();
                imgSource = ImageSource.FromStream(() => new MemoryStream(imageFile));
            } else {
                imgSource = ImageSource.FromFile("contacts.png");
            }
                
            var contato = new Contato() {
                Nome = paramContato.FirstName,
                SobreNome = paramContato.LastName,
                DisplayName = paramContato.DisplayName,
                Foto = imgSource
            };

            foreach (var item in paramContato.Phones) {
                var telefone = new Telefone() {
                    Descricao = item.Label,
                    Numero = item.Number
                };
                switch (item.Type) {
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
