using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WpfApp1
{

    public class Pokemon : INotifyPropertyChanged
    {
        private string name;
        private string spriteFront;
        private string spriteBack;
        private string type;
        public int Id { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        public Pokemon() { }
        public Pokemon(string name) 
        {
            this.name = name;
        }
        public Pokemon(int id, string name)
        {
            this.name = name;
            this.Id = id;
        }
        public Pokemon(Pokemon pokemon)
        {
            this.CopyFrom(pokemon);
        }

        public void CopyFrom(Pokemon pokemon)
        {
            Name = pokemon.Name;
            SpriteFront = pokemon.SpriteFront;
            SpriteBack = pokemon.SpriteBack;
            Type = pokemon.Type;
            Id = pokemon.Id;
        }

        public void FetchData()
        {
            if(Name == null || Type == null || SpriteFront == null || spriteBack == null)
            {
                _ = PokeApi.ApplyPokemonAPIInfo(this.name, this);

            }
        }

        private void Notify([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public string Name { get { return name; } set { name = value; Notify(); } }
        public string SpriteFront { get { return spriteFront; } set { spriteFront = value; Notify(); } }
        public string SpriteBack { get { return spriteBack; } set { spriteBack = value; Notify(); } }
        public string Type { get { return type; } set { type = value; Notify(); } }
    }
}
