using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Windows.Input;
using XF.Atividade6.Model;
using XF.Atividade6.View;

namespace XF.Atividade6.ViewModel
{
    public class AtividadeViewModel : INotifyPropertyChanged
    {
        #region Propriedades
        static AtividadeViewModel instancia = new AtividadeViewModel();
        public static AtividadeViewModel Instancia
        {
            get { return instancia; }
            private set { instancia = value; }
        }
        public Atividade AtividadeModel { get; set; }
        private Atividade selecionado;
        public Atividade Selecionado
        {
            get { return selecionado; }
            set
            {
                selecionado = value as Atividade;
                EventPropertyChanged();
            }
        }

        private string pesquisaPorNome;
        public string PesquisaPorNome
        {
            get { return pesquisaPorNome; }
            set
            {
                if (value == pesquisaPorNome) return;
                pesquisaPorNome = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PesquisaPorNome)));
                Filtro();
            }
        }
        public ObservableCollection<Atividade> Atividades { get; set; } = new ObservableCollection<Atividade>();
        public List<Atividade> CopiarListaAtividades;
        // Eventos da Interface
        public OnEditarAtividadeCMD OnEditarAtividadeCMD { get; }
        public OnAdicionarAtividadeCMD OnAdicionarAtividadeCMD { get; }
        public OnDeleteAtividadeCMD OnDeleteAtividadeCMD { get; }
        public ICommand OnNovoCMD { get; private set; }
        public ICommand OnSairCMD { get; private set; }
        #endregion

        public AtividadeViewModel()
        {
            OnEditarAtividadeCMD = new OnEditarAtividadeCMD(this);
            OnAdicionarAtividadeCMD = new OnAdicionarAtividadeCMD(this);
            OnDeleteAtividadeCMD = new OnDeleteAtividadeCMD(this);
            OnNovoCMD = new Command(OnNovo);
            OnSairCMD = new Command(OnSair);
            CopiarListaAtividades = new List<Atividade>();
        }

        public async Task Carregar()
        {
            await AtividadeRepository.GetAtividades().ContinueWith(retorno => {
                CopiarListaAtividades = retorno.Result.ToList();
            });
            Filtro();
        }

        public void Filtro()
        {
            if (pesquisaPorNome == null)
            {
                pesquisaPorNome = "";
            }
            var resultados = CopiarListaAtividades.Where(n => n.descricao.ToLowerInvariant().Contains(PesquisaPorNome.ToLowerInvariant().Trim())).ToList();
            var removerLista = Atividades.Except(resultados).ToList();
            foreach (var item in removerLista)
            {
                Atividades.Remove(item);
            }
            for (int index = 0; index < resultados.Count; index++)
            {
                var item = resultados[index];
                if (index + 1 > Atividades.Count || !Atividades[index].Equals(item))
                    Atividades.Insert(index, item);
            }
        }

        public async void Adicionar(Atividade paramAtividade)
        {
            paramAtividade.dataCadastro = NewAtividadeView.txtDataCadastro;
            paramAtividade.dataEntrega = NewAtividadeView.txtDataEntrega;

            if ((paramAtividade == null) || (string.IsNullOrWhiteSpace(paramAtividade.descricao)))
            {
                await App.Current.MainPage.DisplayAlert("Atenção", "O campo descricao é obrigatório", "OK");
            }
            else if (await AtividadeRepository.PostAtividade(paramAtividade))
            {
                await App.Current.MainPage.Navigation.PopAsync();
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Falhou", "Desculpe, ocorreu um erro inesperado =(", "OK");
            }
        }


        public async void Editar()
        {
            await App.Current.MainPage.Navigation.PushAsync(new View.NewAtividadeView() { BindingContext = Instancia });
        }

        public async void Remover()
        {
            if (await App.Current.MainPage.DisplayAlert("Atenção?", string.Format("Tem certeza que deseja remover o {0}?", Selecionado.descricao), "Sim", "Não"))
            {
                if (await AtividadeRepository.DeleteAtividade(Selecionado.token))
                {
                    CopiarListaAtividades.Remove(Selecionado);
                    await Carregar();
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Falhou", "Desculpe, ocorreu um erro inesperado =(", "OK");
                }
            }
        }

        private void OnNovo()
        {
            Instancia.Selecionado = new Model.Atividade();
            App.Current.MainPage.Navigation.PushAsync(new View.NewAtividadeView() { BindingContext = Instancia });
        }

        private async void OnSair()
        {
            await App.Current.MainPage.Navigation.PopAsync();
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


    public class OnAdicionarAtividadeCMD : ICommand
    {
        private AtividadeViewModel atividadeVM;
        public OnAdicionarAtividadeCMD(AtividadeViewModel paramVM)
        {
            atividadeVM = paramVM;
        }
        public event EventHandler CanExecuteChanged;
        public void AdicionarCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        public bool CanExecute(object parameter) => true;
        public void Execute(object parameter)
        {
            atividadeVM.Adicionar(parameter as Atividade);
        }
    }

    public class OnEditarAtividadeCMD : ICommand
    {
        private AtividadeViewModel atividadeVM;
        public OnEditarAtividadeCMD(AtividadeViewModel paramVM)
        {
            atividadeVM = paramVM;
        }
        public event EventHandler CanExecuteChanged;
        public void EditarCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        public bool CanExecute(object parameter) => (parameter != null);
        public void Execute(object parameter)
        {
            AtividadeViewModel.Instancia.Selecionado = parameter as Atividade;
            atividadeVM.Editar();
        }
    }

    public class OnDeleteAtividadeCMD : ICommand
    {
        private AtividadeViewModel atividadeVM;
        public OnDeleteAtividadeCMD(AtividadeViewModel paramVM)
        {
            atividadeVM = paramVM;
        }
        public event EventHandler CanExecuteChanged;
        public void DeleteCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        public bool CanExecute(object parameter) => (parameter != null);
        public void Execute(object parameter)
        {
            AtividadeViewModel.Instancia.Selecionado = parameter as Atividade;
            atividadeVM.Remover();
        }
    }


}
