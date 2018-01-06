using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.Forms;
using XF.Atividade3.Model;


namespace XF.Atividade3
{
    public class StudentViewModel : INotifyPropertyChanged
    {

        #region Propriedades
        public Student StudentModel { get; set; }
        public string Name { get { return App.UserVM.Name; } }

        private Student selected;
        public Student Selected {
            get { return selected; }
            set {
                selected = value as Student;
                EventPropertyChanged();
            }
        }

        private string findByName;
        public string FindByName {
            get { return findByName; }
            set {
                if (value == findByName) return;
                findByName = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FindByName)));
                ApplyFilter();
            }
        }

        public List<Student> CopyListStudents;
        public ObservableCollection<Student> Students { get; set; } = new ObservableCollection<Student>();

        // Eventos UI
        public OnAddStudentCMD OnAddStudentCMD { get; }
        public OnEditStudentCMD OnEditStudentCMD { get; }
        public OnDeleteStudentCMD OnDeleteStudentCMD { get; }
        public ICommand OnExitCMD { get; private set; }
        public ICommand OnNewCMD { get; private set; }
        #endregion

        public StudentViewModel() {
            StudentRepository repository = StudentRepository.Instance;
            OnAddStudentCMD = new OnAddStudentCMD(this);
            OnEditStudentCMD = new OnEditStudentCMD(this);
            OnDeleteStudentCMD = new OnDeleteStudentCMD(this);
            OnExitCMD = new Command(OnExit);
            OnNewCMD = new Command(OnNew);
            CopyListStudents = new List<Student>();
            load();
        }

        public void load() {
            CopyListStudents = StudentRepository.GetStudents().ToList();
            ApplyFilter();
        }

        private void ApplyFilter() {
            if (findByName == null) {
                findByName = "";
            }
            var resultado = CopyListStudents.Where(n => n.Name.ToLowerInvariant()
                .Contains(FindByName.ToLowerInvariant().Trim())).ToList();
            var removeList = Students.Except(resultado).ToList();
            foreach (var item in removeList) {
                Students.Remove(item);
            }
            for (int index = 0; index < resultado.Count; index++) {
                var item = resultado[index];
                if (index + 1 > Students.Count || !Students[index].Equals(item)) {
                    Students.Insert(index, item);
                }
            }
        }

        public void Add(Student paramStudent) {
            if ((paramStudent == null) || (string.IsNullOrWhiteSpace(paramStudent.Name))) {
                App.Current.MainPage.DisplayAlert("Atencao", "O campo nome a obrigatorio", "OK");
            } else if (StudentRepository.SaveStudent(paramStudent) > 0) {
                App.Current.MainPage.Navigation.PopAsync();
            } else {
                App.Current.MainPage.DisplayAlert("Falhou", "Desculpe, ocorreu um erro inesperado =(", "OK");
            }
        }

        public async void Edit()
        {
            await App.Current.MainPage.Navigation.PushAsync(
                new View.Student.NewStudentView() { BindingContext = App.StudentVM});
        }

        public async void Remove() {
            if (await App.Current.MainPage.DisplayAlert("Atencao?",
                string.Format("Tem certeza que deseja remover o {0}?", Selected.Name), "Sim", "Nao")) {
                if (StudentRepository.RemoveStudent(Selected.Id) > 0) {
                    CopyListStudents.Remove(Selected);
                    load();
                } else {
                    await App.Current.MainPage.DisplayAlert("Falhou", "Desculpe, ocorreu um erro inesperado =(", "OK");
                }             
            }
        }

        private async void OnExit() {
            await App.Current.MainPage.Navigation.PopAsync();
        }

        private void OnNew() {
            App.StudentVM.Selected = new Model.Student();
            App.Current.MainPage.Navigation.PushAsync( 
                  new View.Student.NewStudentView() { BindingContext = App.StudentVM });
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void EventPropertyChanged([CallerMemberName] string propertyName = null) {
            if (this.PropertyChanged != null) {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    public class OnAddStudentCMD : ICommand {
        private StudentViewModel studentVM;
        public OnAddStudentCMD(StudentViewModel paramVM) {
            studentVM = paramVM;
        }
        public event EventHandler CanExecuteChanged;
        public void AdicionarCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        public bool CanExecute(object parameter) => true;
        public void Execute(object parameter)
        {
            studentVM.Add(parameter as Student);
        }
    }

    public class OnEditStudentCMD : ICommand {
        private StudentViewModel studentVM;
        public OnEditStudentCMD(StudentViewModel paramVM) {
            studentVM = paramVM;
        }
        public event EventHandler CanExecuteChanged;
        public void EditarCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        public bool CanExecute(object parameter) => (parameter != null);
        public void Execute(object parameter) {
            App.StudentVM.Selected = parameter as Student;
            studentVM.Edit();
        }
    }

    public class OnDeleteStudentCMD : ICommand {
        private StudentViewModel studentVM;
        public OnDeleteStudentCMD(StudentViewModel paramVM) {
            studentVM = paramVM;
        }
        public event EventHandler CanExecuteChanged;
        public void DeleteCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        public bool CanExecute(object parameter) => (parameter != null);
        public void Execute(object parameter)
        {
            App.StudentVM.Selected = parameter as Student;
            studentVM.Remove();
        }
    }

}