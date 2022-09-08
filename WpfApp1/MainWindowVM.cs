using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfApp1
{
    public class MainWindowVM : INotifyPropertyChanged
    {
        public ObservableCollection<Treinador> listaDeTreinadores { get; set; }
        public Treinador treinadorSelecionado;
        public Pokemon pokemonSelecionado;
        // Relay Commands
        public RelayCommand Add { get; private set; }
        public RelayCommand Edit { get; private set; }
        public RelayCommand Remove { get; private set; }
        public RelayCommand Puxar { get; private set; }

        // HTTP Client
        private static readonly HttpClient client = new HttpClient();

        private EditarTreinador tela;

        public MainWindowVM()
        {
            listaDeTreinadores = new ObservableCollection<Treinador>();
            listaDeTreinadores.Add(new Treinador("Marcos"));
            listaDeTreinadores.Add(new Treinador("Letícia"));
            listaDeTreinadores[1].AddPokemon("Sapinho");
            listaDeTreinadores[1].AddPokemon("Pikachu");

            Console.WriteLine("iniciando tudo!");
            IniciaComandos();
        }
        public void IniciaComandos()
        {
            Add = new RelayCommand( (object _) => {
                Treinador treinador = new Treinador();
                tela = new EditarTreinador();
                tela.DataContext = treinador;
                tela.Title = "Adicionar Treinador";
                tela.ShowDialog();
                if (tela.DialogResult == true)
                {
                    listaDeTreinadores.Add(treinador);
                }
            });
            Edit = new RelayCommand((object _) => {
                tela = new EditarTreinador();
                tela.DataContext = TreinadorSelecionado;
                tela.Title = "Editar Treinador";
                tela.ShowDialog();
            });
            Remove = new RelayCommand((object _) => {
                listaDeTreinadores.Remove(TreinadorSelecionado);
            });
            Puxar = new RelayCommand(async (object _) =>
            {
                await ProcessRepositories();
            });
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public Treinador TreinadorSelecionado
        {
            get { return treinadorSelecionado; }
            set
            {
                treinadorSelecionado = value;
                OnPropertyChanged();
            }
        }
        public Pokemon PokemonSelecionado
        {
            get { return pokemonSelecionado; }
            set
            {
                pokemonSelecionado = value;
                Console.WriteLine("Troquei para o pokemon: " + value.Name);
                OnPropertyChanged("pokemonSelecionado");
            }
        }

        private static async Task ProcessRepositories()
        {
            
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");
            var stringTask = client.GetStringAsync("https://pokeapi.co/api/v2/pokemon/ditto");
            var msg = await stringTask;
            Console.Write(msg);
        }
    }
}
