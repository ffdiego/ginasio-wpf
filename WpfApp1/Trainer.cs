using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Net.Http;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;

namespace WpfApp1
{
    public class Trainer : INotifyPropertyChanged
    {

        private string name;
        private ObservableCollection<Pokemon> pokemons;
        public event PropertyChangedEventHandler PropertyChanged;
        public RelayCommand AddRandomPokemonCommand { get; private set; }
        public string Name { get { return name; } set { name = value; } }
        public int Id { get; set; }
        public ObservableCollection<Pokemon> Pokemons { get { return pokemons; } set { pokemons = value; } }
        public Trainer() { 
            pokemons = new ObservableCollection<Pokemon>();
        }
        public Trainer(Trainer treinador)
        {
            name = treinador.Name;
            pokemons = new ObservableCollection<Pokemon>(treinador.pokemons);
            Id = treinador.Id;
        }
        public void AddPokemon(string name)
        {
            this.pokemons.Add(new Pokemon(name));
        }
        public void AddPokemon(Pokemon pokemon)
        {
            this.pokemons.Add(pokemon);
        }
        public void AddRandomPokemon()
        {
            Random random = new Random();
            Pokemon pokemon = new Pokemon();
            this.AddPokemon(pokemon);
            //the api call takes a pokemon number as argument
            //here we request a random pokemon number between 0 and 150 
            _ = PokeApi.ApplyPokemonAPIInfo(random.Next(151).ToString(), pokemon);
        }
        public void CopyFrom(Trainer treinador)
        {
            this.pokemons = treinador.pokemons;
            this.name = treinador.Name;
            this.Id = treinador.Id;
            Notify("Name");
            Notify("Pokemons");
        }

        private void Notify([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

    }
}
