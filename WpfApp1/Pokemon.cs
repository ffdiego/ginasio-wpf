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
        private uint level;
        public event PropertyChangedEventHandler PropertyChanged;

        public Pokemon() { }
        public Pokemon(string name) 
        {
            this.name = name;
            this.level = 0;
        }
        private void Notify([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public string Name { get { return name; } set { name = value; Notify(); } }
        public string SpriteFront { get { return spriteFront; } set { spriteFront = value; Notify(); } }
        public string SpriteBack { get { return spriteBack; } set { spriteBack = value; Notify(); } }
        public uint Level { get { return level; } set { level = value; Notify(); } }
        public string Type { get { return type; } set { type = value; Notify(); } }
    }
}
