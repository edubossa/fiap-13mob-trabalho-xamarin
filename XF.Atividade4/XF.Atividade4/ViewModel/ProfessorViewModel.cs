using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Windows.Input;
using XF.Atividade4.Model;

namespace XF.Atividade4.ViewModel
{
    public class ProfessorViewModel : INotifyPropertyChanged
    {
        #region Propriedades
        static ProfessorViewModel instancia = new ProfessorViewModel();
        public static ProfessorViewModel Instancia {
            get { return instancia; }
            private set { instancia = value; }
        }
        public Professor ProfessorModel { get; set; }
        private Professor selecionado;
        public Professor Selecionado { 
            get { return selecionado; }
            set {
                selecionado = value as Professor;
                EventPropertyChanged();
            }
        }

        private string pesquisaPorNome;
        public string PesquisaPorNome {
            get { return pesquisaPorNome; }
            set {
                if (value == pesquisaPorNome) return;
                pesquisaPorNome = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PesquisaPorNome)));
                Filtro();
            }
        }
        public ObservableCollection<Professor> Professores { get; set; } = new ObservableCollection<Professor>();
        public List<Professor> CopiarListaProfessores;
        // Eventos da Interface
        public OnEditarProfessorCMD OnEditarProfessorCMD { get; }
        public OnAdicionarProfessorCMD OnAdicionarProfessorCMD { get; }
        public OnDeleteProfessorCMD OnDeleteProfessorCMD { get; }
        public ICommand OnNovoCMD { get; private set; }
        public ICommand OnSairCMD { get; private set; }
        #endregion

        public ProfessorViewModel() {
            OnEditarProfessorCMD = new OnEditarProfessorCMD(this);
            OnAdicionarProfessorCMD = new OnAdicionarProfessorCMD(this);
            OnDeleteProfessorCMD = new OnDeleteProfessorCMD(this);
            OnNovoCMD = new Command(OnNovo);
            OnSairCMD = new Command(OnSair);
            CopiarListaProfessores = new List<Professor>();
        }

        public async Task Carregar() {
            await ProfessorRepository.GetProfessoresSqlAzureAsync().ContinueWith(retorno => {
                CopiarListaProfessores = retorno.Result.ToList();
            });
            Filtro();
        }

        public void Filtro() {
            if (pesquisaPorNome == null) {
                pesquisaPorNome = "";
            }
            var resultados = CopiarListaProfessores.Where(n => n.Nome.ToLowerInvariant().Contains(PesquisaPorNome.ToLowerInvariant().Trim())).ToList();
            var removerLista = Professores.Except(resultados).ToList();
            foreach (var item in removerLista) {
                Professores.Remove(item);
            }
            for (int index = 0; index < resultados.Count; index++) {
                var item = resultados[index];
                if (index + 1 > Professores.Count || !Professores[index].Equals(item))
                    Professores.Insert(index, item);
            }
        }

        public async void Adicionar(Professor paramProfessor) {
            if ((paramProfessor == null) || (string.IsNullOrWhiteSpace(paramProfessor.Nome))) {
                await App.Current.MainPage.DisplayAlert("Atenção", "O campo nome é obrigatório", "OK");
            } else if (await ProfessorRepository.PostProfessorSqlAzureAsync(paramProfessor)) {
                await App.Current.MainPage.Navigation.PopAsync();
            } else {
                await App.Current.MainPage.DisplayAlert("Falhou", "Desculpe, ocorreu um erro inesperado =(", "OK");
            }
        }

        public async void Editar() {
            await App.Current.MainPage.Navigation.PushAsync(new View.NovoProfessorView() { BindingContext = Instancia });
        }

        public async void Remover() {
            if (await App.Current.MainPage.DisplayAlert("Atenção?", string.Format("Tem certeza que deseja remover o {0}?", Selecionado.Nome), "Sim", "Não")) {
                if (await ProfessorRepository.DeleteProfessorSqlAzureAsync(Selecionado.Id.ToString())) {
                    CopiarListaProfessores.Remove(Selecionado);
                    await Carregar();
                } else {
                    await App.Current.MainPage.DisplayAlert("Falhou", "Desculpe, ocorreu um erro inesperado =(", "OK");
                }
            }
        }

        private void OnNovo() {
            Instancia.Selecionado = new Model.Professor();
            App.Current.MainPage.Navigation.PushAsync(new View.NovoProfessorView() { BindingContext = Instancia });
        }

        private async void OnSair() {
            await App.Current.MainPage.Navigation.PopAsync();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void EventPropertyChanged([CallerMemberName] string propertyName = null) {
            if (this.PropertyChanged != null) {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    public class OnAdicionarProfessorCMD : ICommand {
        private ProfessorViewModel professorVM;
        public OnAdicionarProfessorCMD(ProfessorViewModel paramVM) {
            professorVM = paramVM;
        }
        public event EventHandler CanExecuteChanged;
        public void AdicionarCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        public bool CanExecute(object parameter) => true;
        public void Execute(object parameter) {
            professorVM.Adicionar(parameter as Professor);
        }
    }

    public class OnEditarProfessorCMD : ICommand {
        private ProfessorViewModel professorVM;
        public OnEditarProfessorCMD(ProfessorViewModel paramVM) {
            professorVM = paramVM;
        }
        public event EventHandler CanExecuteChanged;
        public void EditarCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        public bool CanExecute(object parameter) => (parameter != null);
        public void Execute(object parameter) {
            ProfessorViewModel.Instancia.Selecionado = parameter as Professor;
            professorVM.Editar();
        }
    }

    public class OnDeleteProfessorCMD : ICommand {
        private ProfessorViewModel professorVM;
        public OnDeleteProfessorCMD(ProfessorViewModel paramVM) {
            professorVM = paramVM;
        }
        public event EventHandler CanExecuteChanged;
        public void DeleteCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        public bool CanExecute(object parameter) => (parameter != null);
        public void Execute(object parameter) {
            ProfessorViewModel.Instancia.Selecionado = parameter as Professor;
            professorVM.Remover();
        }
    }

}