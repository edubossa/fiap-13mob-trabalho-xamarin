using System;
using System.Collections.Generic;
using XF.Atividade3.ViewModel;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XF.Atividade3.View.Student
{   
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewStudentView : ContentPage
    {
        public static StudentViewModel StudentVM { get; set; }

        public NewStudentView()
        {
            InitializeComponent();
        }
    }
}
