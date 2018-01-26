using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XF.Atividade6.View
{   
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewAtividadeView : ContentPage
    {
        public static string txtDataCadastro = "";
        public static string txtDataEntrega = "";

        public NewAtividadeView()
        {
            InitializeComponent();
        }

        private void OnDataCadastroSelected(object sender, DateChangedEventArgs e)
        {
            txtDataCadastro = e.NewDate.Day + "/" + e.NewDate.Month + "/" + e.NewDate.Year;
        }

        private void OnDataEntregaSelected(object sender, DateChangedEventArgs e)
        {
            txtDataEntrega = e.NewDate.Day + "/" + e.NewDate.Month + "/" + e.NewDate.Year;
        }

    }
}
