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
using WpfApp1.Db;
using System.Windows;

namespace WpfApp1
{
    public class Trainer : INotifyPropertyChanged
    {

        private string name;
        private ObservableCollection<Pokemon> pokemons;
        public ObservableCollection<Pokemon> Pokemons { get { return pokemons; } set { pokemons = value; } }
        public event PropertyChangedEventHandler PropertyChanged;
        public string Name { get { return name; } set { name = value; } }
        public int Id { get; set; }
        public Trainer() { 
            pokemons = new ObservableCollection<Pokemon>();
        }
        public Trainer(Trainer treinador)
        {
            name = treinador.Name;
            Id = treinador.Id;
            pokemons = new ObservableCollection<Pokemon>(treinador.pokemons);
        }
        public Trainer(int id, string name, List<Pokemon> list)
        {
            Name = name;
            Id = id;
            pokemons = new ObservableCollection<Pokemon>(list);
        }
        public void AttachPokemon(Pokemon pokemon)
        {
            bool alreadyContains = Pokemons.Where(p => p.Id == pokemon.Id).Any();
            if (alreadyContains)
                return;
            pokemons.Add(pokemon);
            DBManager.AddPokemon(pokemon);
            DBManager.AttachPokemon(this, pokemon);
        }
        public void DetachPokemon(Pokemon pokemon)
        {
            pokemons.Remove(pokemon);
            DBManager.DetachPokemon(this, pokemon);
        }
        public void CopyFrom(Trainer treinador)
        {
            this.pokemons = treinador.pokemons;
            this.name = treinador.Name;
            Notify(nameof(name));
            Notify(nameof(Pokemons));
            DBManager.UpdateTrainer(this);
        }
        public void SetPokemons(List<Pokemon> list)
        {
            Pokemons = new ObservableCollection<Pokemon>(list);
        }
        private void Notify([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

    }
}
