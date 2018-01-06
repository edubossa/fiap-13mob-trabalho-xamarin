using Android.Content;
using Android.Net;
using Android.Telephony;
using System.Linq;
using Xamarin.Forms;
using XF.Atividade5.App_Code;
using XF.Atividade5.Droid.App_Code;

[assembly: Dependency(typeof(Telefone_Android))]
namespace XF.Atividade5.Droid.App_Code
{
    public class Telefone_Android : ITelefone
    {

        public bool Ligar(string numero) {
            var context = Forms.Context;
            if (context == null) {
                return false;
            }
            var intent = new Intent(Intent.ActionCall);
            intent.SetData(Uri.Parse("tel:" + numero));
            if (IsDisponivel(context, intent)) {
                context.StartActivity(intent);
                return true;
            }
            return false;
        }

        public static bool IsDisponivel(Context context, Intent intent) {
            var packageManager = context.PackageManager;
            var lista = packageManager.QueryIntentServices(intent, 0).Union(packageManager.QueryIntentActivities(intent, 0));
            if (lista.Any()) {
                return true;
            } 
            return TelephonyManager.FromContext(context).PhoneType != PhoneType.None;
        }
    }

} 
