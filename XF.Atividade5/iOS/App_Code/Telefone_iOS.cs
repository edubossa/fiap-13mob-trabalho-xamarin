using Foundation;
using UIKit;
using Xamarin.Forms;
using XF.Atividade5.App_Code;
using XF.Atividade5.iOS.App_Code;

[assembly: Dependency(typeof(Telefone_iOS))]
namespace XF.Atividade5.iOS.App_Code
{   
    public class Telefone_iOS : ITelefone
    {
        public bool Ligar(string numero) {
            return UIApplication.SharedApplication.OpenUrl( new NSUrl("tel:" + numero));
        }
    }
}
