using Xamarin.Forms;
using Android.App;
using Xamarin.Geolocation; 
using System.Threading.Tasks;
using XF.Atividade5.Droid.App_Code;
using XF.Atividade5.App_Code;

[assembly: Dependency(typeof(Localizacao_Android))]
namespace XF.Atividade5.Droid.App_Code
{
    public class Localizacao_Android : ILocalizacao
    {
        public void GetCoordenada() {
            var context = Forms.Context as Activity;
            var locator = new Geolocator(context) { DesiredAccuracy = 50 };
            locator.GetPositionAsync(timeout: 10000).ContinueWith(t => {
                Coordenada(t.Result.Latitude, t.Result.Longitude);
                System.Diagnostics.Debug.WriteLine(t.Result.Latitude);
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        void Coordenada(double latitude, double longitude) {
            var coordenada = new Coordenada() {
                Latitude = latitude.ToString(),
                Longitude = longitude.ToString()
            };
            MessagingCenter.Send<ILocalizacao, Coordenada>(this, "coordenada", coordenada);
        }
    }
}
