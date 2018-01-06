using System;
using Xamarin.Forms;

namespace XF.Atividade1
{
    public partial class XF_Atividade1Page : ContentPage
    {
        Email emailModel;

        public XF_Atividade1Page()
        {
            InitializeComponent();

            if (this.emailModel == null) this.emailModel = new Email();
        }

        void SendEmail_Clicked(object sender, EventArgs e)
        {
            //DisplayAlert("Event", "SendEmail_Clicked", "OK");
            if (this.emailModel.Active &&  !string.IsNullOrEmpty(this.emailModel.AccountEmail) ) {
                    DisplayAlert("Mensagem", $"E-mail enviado para {emailModel.AccountEmail}", "Ok");
            } else {
                DisplayAlert("Mensagem", "E-mail não autorizado", "Ok");
            }

        }

        void Config_Clicked(object sender, EventArgs e)
        {
            //DisplayAlert("Event", "Config_Clicked", "OK");
            Navigation.PushAsync(new ConfigPage() { BindingContext = emailModel });
        }

    }
}
