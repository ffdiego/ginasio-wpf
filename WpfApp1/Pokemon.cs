using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{

    public class Pokemon
    {
        private string name;
        private string gifUrl;
        private string type;
        private uint level;

        public Pokemon() { }
        public Pokemon(string name) 
        {
            this.name = name;
            this.level = 0;
        }

        public string Name { get { return name; } set { name = value; } }
        public string GifUrl { get { return gifUrl; } set { gifUrl = value; } }
        public uint Level { get { return level; } set { level = value; } }
    }

}
