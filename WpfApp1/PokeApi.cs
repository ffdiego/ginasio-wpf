using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.Json;
using System.Runtime.CompilerServices;
using WpfApp1.Db;

namespace WpfApp1
{
    static class PokeApi
    {
        private static readonly HttpClient client = new HttpClient();
        public static async Task ApplyPokemonAPIInfo(string pokemonName, Pokemon target)
        {
            Console.WriteLine("Tentando conectar!");

            // HeaderConfig
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");
            // HeaderConfig

            var response = await client.GetAsync("https://pokeapi.co/api/v2/pokemon/" + pokemonName.ToLower());
            if (response.IsSuccessStatusCode)
            {
                var stringTask = await response.Content.ReadAsStreamAsync();
                var msg = await JsonDocument.ParseAsync(stringTask);
                var pokemonDetails = msg.RootElement;

                JsonElement sprites = pokemonDetails.GetProperty("sprites").GetProperty("versions").GetProperty("generation-v").GetProperty("black-white").GetProperty("animated");

                target.Name = Capitalize(pokemonDetails.GetProperty("name").ToString());
                target.Type = Capitalize(pokemonDetails.GetProperty("types")[0].GetProperty("type").GetProperty("name").ToString());
                target.SpriteFront = sprites.GetProperty("front_default").ToString();
                target.SpriteBack = sprites.GetProperty("back_default").ToString();
                Console.WriteLine("Dados Obtidos!");
            }
            else
            {
                Console.WriteLine("HighlightedPokemon não encontrado!");
                target.SpriteFront = "";
                target.SpriteBack = "";
                target.Type = "";
            }

        }

        private static string Capitalize(string text)
        {
            return Char.ToUpper(text[0]) + text.Substring(1);
        }
    }
}
