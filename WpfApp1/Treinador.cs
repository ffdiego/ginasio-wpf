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
        private uint level;
        private List<Pokemon> pokemons;
        // HTTP Client
        private static readonly HttpClient client = new HttpClient();
        public Treinador() { 
            pokemons = new List<Pokemon>();
        }
        public Treinador(string name)
        {
            pokemons = new List<Pokemon>();
            this.name = name;
            this.level = 0;
        }
        public Treinador(string name, uint level)
        {
            pokemons = new List<Pokemon>();
            this.name = name;
            this.level = level;
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
            pokemon.ApplyPokemonAPIInfo(random.Next(151).ToString());
        }

        public string Name { get { return name; } set { name = value; } }
        public uint Level { get { return level; } set { level = value; } }
        public List<Pokemon> Pokemons { get { return pokemons; } }
    }
}
