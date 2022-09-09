namespace WpfApp1
{

    public class Pokemon
    {
        public string name;
        private string spriteFront;
        private string spriteBack;
        private string type;
        private uint level;

        public Pokemon() { }
        public Pokemon(string name) 
        {
            this.name = name;
            this.level = 0;
        }

        public string Name { get { return name; } set { name = value; } }
        public string SpriteFront { get { return spriteFront; } set { spriteFront = value; } }
        public string SpriteBack { get { return spriteBack; } set { spriteBack = value; } }
        public uint Level { get { return level; } set { level = value; } }
        public string Type { get { return type; } set { type = value; } }
    }
}
