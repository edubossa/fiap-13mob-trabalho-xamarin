using System.Threading.Tasks;
using Xamarin.Forms;
using XF.Atividade5.App_Code;
using XF.Atividade5.iOS.App_Code;

using Xamarin.Geolocation;

[assembly: Dependency(typeof(Localizacao_iOS))]
namespace XF.Atividade5.iOS.App_Code
{   
    public class Localizacao_iOS : ILocalizacao
    {
        public void GetCoordenada() {
            var locator = new Geolocator { DesiredAccuracy = 50 };
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
