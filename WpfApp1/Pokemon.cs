using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace WpfApp1
{

    public class Pokemon
    {
        public string name;
        private string spriteFront;
        private string spriteBack;
        private string type;
        private uint level;
        // HTTP Client
        private static readonly HttpClient client = new HttpClient();

        public Pokemon() { }
        public Pokemon(string name) 
        {
            this.name = name;
            this.level = 0;
        }

        public async Task ApplyPokemonAPIInfo(string pokemonName)
        {
            Console.WriteLine("Tentando conectar!");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");
            var response = await client.GetAsync("https://pokeapi.co/api/v2/pokemon/" + pokemonName.ToLower());
            if (response.IsSuccessStatusCode)
            {
                var stringTask = await response.Content.ReadAsStreamAsync();
                var msg = await JsonDocument.ParseAsync(stringTask);
                var pokemonDetails = msg.RootElement;

                JsonElement sprites = pokemonDetails.GetProperty("sprites").GetProperty("versions").GetProperty("generation-v").GetProperty("black-white").GetProperty("animated");

                this.Name = pokemonDetails.GetProperty("name").ToString();
                this.Name = Char.ToUpper(this.Name[0]) + this.Name.Substring(1);
                this.Type = pokemonDetails.GetProperty("types")[0].GetProperty("type").GetProperty("name").ToString();
                this.SpriteFront = sprites.GetProperty("front_default").ToString();
                this.SpriteBack = sprites.GetProperty("back_default").ToString();
            }
            else
            {
                Console.WriteLine("Pokemon não encontrado!");
                this.SpriteFront = "";
                this.SpriteBack = "";
                this.Type = "";
            }

        }

        public string Name { get { return name; } set { name = value; } }
        public string SpriteFront { get { return spriteFront; } set { spriteFront = value; } }
        public string SpriteBack { get { return spriteBack; } set { spriteBack = value; } }
        public uint Level { get { return level; } set { level = value; } }
        public string Type { get { return type; } set { type = value; } }
    }
}
