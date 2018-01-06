using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XF.Atividade3.View.Login
{   
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginView : ContentPage
    {
        public LoginView()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            txtUser.Text = pwdPassword.Text = string.Empty;
            base.OnAppearing();
        } 

    }
}
