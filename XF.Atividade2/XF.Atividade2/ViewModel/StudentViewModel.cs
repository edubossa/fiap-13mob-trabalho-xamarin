using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.Forms;
using XF.Atividade2.Model;
using XF.Atividade2.View;
namespace XF.Atividade2.ViewModel
{
    public class StudentViewModel
    {

        #region Propriedades
        //public string RM { get; set; }
        //public string Name { get; set; }
        //public string Email { get; set; }
        public Student StudentModel { get; set; }
        public ObservableCollection<Student> Students { get; set; } = new ObservableCollection<Student>();

        // Eventos da UI
        public OnAddStudentCMD OnAddStudentCMD { get; } 
        public ICommand OnNewCMD { get; private set; }  
        public ICommand OnExitCMD { get; private set; } 
        #endregion

        public StudentViewModel()
        {
            //this.RM = student.RM;
            //this.Name = student.Name;
            //this.Email = student.Email;

            StudentModel = new Student();
            OnAddStudentCMD = new OnAddStudentCMD(this);
            OnExitCMD = new Command(OnExit);
            OnNewCMD = new Command(OnNew);
            addDefaultStudent();
        }

        public void addDefaultStudent()
        {
            var student1 = new Student()
            {
                Id = Guid.NewGuid(),
                RM = "31773",
                Name = "Eduardo Wallace",
                Email = "edubossa@gmail.com"
            };

            var student2 = new Student()
            {
                Id = Guid.NewGuid(),
                RM = "35605",
                Name = "Washington Alexandre",
                Email = "was.alexandre42@gmail.com "
            };

            this.Students.Add(student1);
            this.Students.Add(student2);
        }

        public void Add(Student paramStudent)
        {
            try
            {
                if (paramStudent.Name.Equals("") 
                    || paramStudent.RM.Equals("") 
                    || paramStudent.Email.Equals("")) {
                    App.Current.MainPage.DisplayAlert("Campos Obrigatorios", "Todos os campos devem ser preenchidos", "OK");
                    return;
                }

                if (paramStudent == null)
                    throw new Exception("Aluno invalido");

                paramStudent.Id = Guid.NewGuid();
                Students.Add(paramStudent);
                App.Current.MainPage.Navigation.PopAsync();
            }
            catch (Exception)
            {
                App.Current.MainPage.DisplayAlert("Error", "Ocorreu um erro inesperado ", "OK");
            }
        }

        private async void OnExit()
        {
            await App.Current.MainPage.Navigation.PopAsync();
        }

        private async void OnNew()
        {
            await App.Current.MainPage.Navigation.PushAsync(new NewStudentView() { BindingContext = App.StudentVM });
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void EventPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

    }

    public class OnAddStudentCMD : ICommand
    {
        private StudentViewModel studentVM;
        public OnAddStudentCMD(StudentViewModel paramVM)
        {
            studentVM = paramVM;
        }
        public event EventHandler CanExecuteChanged;
        public void DeleteCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        public bool CanExecute(object parameter)
        {
            if (parameter != null) return true;

            return false;
        }
        public void Execute(object parameter)
        {
            studentVM.Add(parameter as Student);
        }
    }

}