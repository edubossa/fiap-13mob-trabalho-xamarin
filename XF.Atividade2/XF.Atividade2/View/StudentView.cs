using System;

using Xamarin.Forms;

namespace XF.Atividade2.View
{
    public class StudentView : ContentPage
    {
        public StudentView()
        {
            Content = new StackLayout
            {
                Children = {
                    new Label { Text = "Hello ContentPage" }
                }
            };
        }
    }
}

