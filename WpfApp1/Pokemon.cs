using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;

namespace WpfApp1
{

    public class Pokemon : INotifyPropertyChanged
    {
        private string name;
        private MemoryStream spriteFront;
        private MemoryStream spriteBack;
        private string type;

        public event PropertyChangedEventHandler PropertyChanged;

        public Pokemon() { }

        public void CopyFrom(Pokemon pokemon)
        {
            Name = pokemon.Name;
            SpriteFront = pokemon.SpriteFront;
            SpriteBack = pokemon.SpriteBack;
            Type = pokemon.Type;
            Id = pokemon.Id;
        }

        private void Notify([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public int Id { get; set; }
        public string Name { get { return name; } set { name = value; Notify(); } }
        public MemoryStream SpriteFront { get { return spriteFront; } set { spriteFront = value; Notify(); } }
        public MemoryStream SpriteBack { get { return spriteBack; } set { spriteBack = value; Notify(); } }
        public string Type { get { return type; } set { type = value; Notify(); } }
    }
}
