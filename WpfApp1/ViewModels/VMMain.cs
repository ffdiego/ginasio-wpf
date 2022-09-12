using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using WpfApp1.ViewModels;

namespace WpfApp1
{
    public class VMMain : INotifyPropertyChanged
    {
        public ObservableCollection<Treinador> trainers { get; set; }
        private Treinador highlightedTrainer;
        public Pokemon highlightedPokemon;
        // Relay Commands
        public RelayCommand Add { get; private set; }
        public RelayCommand Edit { get; private set; }
        public RelayCommand Remove { get; private set; }
        public RelayCommand NewPokemon { get; private set; }
        public VMMain()
        {
            trainers = new ObservableCollection<Treinador>
            {
                new Treinador("Marcos"),
                new Treinador("Letícia")
            };
            trainers[0].AddPokemon("Bulbasaur");
            trainers[0].AddPokemon("Pikachu");
            trainers[0].AddPokemon("Voltorb");
            trainers[1].AddPokemon("Cubone");
            trainers[1].AddPokemon("Magnemite");

            Console.WriteLine("Iniciando tudo!");
            IniciaComandos();
        }
        public void IniciaComandos()
        {
            Add = new RelayCommand( (object _) => {
                VMEditTrainer vm = new VMEditTrainer(trainers);
            });
            Edit = new RelayCommand((object _) => {
                VMEditTrainer vm = new VMEditTrainer(trainers, highlightedTrainer);
            }, (object _) => this.highlightedTrainer != null);
            Remove = new RelayCommand((object _) => {
                trainers.Remove(highlightedTrainer);
            }, (object _) => this.highlightedTrainer != null);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void Notify([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public Treinador HighlightedTrainer
        {
            get { return highlightedTrainer; }
            set
            {
                highlightedTrainer = value;
                Notify();
            }
        }
        public Pokemon HighlightedPokemon
        {
            get { return highlightedPokemon; }
            set
            {
                if (value == null) return;
                highlightedPokemon = value;
                Console.WriteLine("Troquei para o pokemon: " + value.Name);
                if(value.SpriteFront == null)
                {
                    _ = PokeApi.ApplyPokemonAPIInfo(value.Name, highlightedPokemon);
                }
                Notify();
            }
        }  
    }
}
