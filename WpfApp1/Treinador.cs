using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace WpfApp1
{
    public class Treinador
    {
        private string name;
        private List<Pokemon> pokemons;
        public RelayCommand AddRandomPokemonCommand { get; private set; }
        public Treinador() { 
            pokemons = new List<Pokemon>();
            InitializeCommands();
        }
        public Treinador(Treinador treinador)
        {
            this.name = treinador.Name;
            this.pokemons = treinador.pokemons;
            InitializeCommands();
        }
        public Treinador(string name) 
        {
            pokemons = new List<Pokemon>();
            this.name = name;
            InitializeCommands();
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
            PokeApi.ApplyPokemonAPIInfo(random.Next(151).ToString(), pokemon);
        }
        public void Set(Treinador treinador)
        {
            this.pokemons = treinador.pokemons;
            this.name = treinador.Name;
        }

        private void InitializeCommands()
        {
            this.AddRandomPokemonCommand = new RelayCommand((object _) =>
            {
                this.AddRandomPokemon();
            });
        }
        public string Name { get { return name; } set { name = value; } }
        public List<Pokemon> Pokemons { get { return pokemons; } set { pokemons = value; } }
    }
}
