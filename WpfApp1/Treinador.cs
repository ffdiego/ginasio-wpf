using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    public class Treinador
    {
        private string name;
        private uint level;
        private List<Pokemon> pokemons;
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

        public string Name { get { return name; } set { name = value; } }
        public uint Level { get { return level; } set { level = value; } }
        public List<Pokemon> Pokemons { get { return pokemons; } }
    }
}
