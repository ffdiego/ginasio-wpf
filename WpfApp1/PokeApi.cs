using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.Json;
using System.Windows;
using System.Net;
using System.IO;
using WpfApp1.Db;

namespace WpfApp1
{
    public static class PokeApi
    {
        private static readonly HttpClient client = new HttpClient();
        //todo: Pergunta: Isso deve existir?
        public static int ApiTimesCalled;
        public static async Task<(Pokemon, string)> GetPokemonAPIInfo(string nameOrNumber)
        {
            try
            {
                Pokemon result = DBManager.GetPokemon(nameOrNumber);
                if (result == null)
                { 
                    ApiTimesCalled++;
                    result = await getPokemonInfo(nameOrNumber);
                }
                return (result, "");
            }
            catch (Exception e)
            {
                return (null, e.Message);
            }
        }

        private static async Task<Pokemon> getPokemonInfo(string nameOrNumber)
        {
            // HeaderConfig
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");
            // HeaderConfig

            var response = await client.GetAsync("https://pokeapi.co/api/v2/pokemon/" + nameOrNumber.ToLower());
            if (response.IsSuccessStatusCode)
            {
                Stream stringTask = await response.Content.ReadAsStreamAsync();
                JsonDocument msg = await JsonDocument.ParseAsync(stringTask);
                JsonElement pokemonDetails = msg.RootElement;

                JsonElement sprites = pokemonDetails.GetProperty("sprites").GetProperty("versions").GetProperty("generation-v").GetProperty("black-white").GetProperty("animated");
                string SpriteFront = sprites.GetProperty("front_default").ToString();
                string SpriteBack = sprites.GetProperty("back_default").ToString();

                Pokemon target = new Pokemon()
                {
                    SpriteFront = new MemoryStream(new WebClient().DownloadData(SpriteFront)),
                    SpriteBack = new MemoryStream(new WebClient().DownloadData(SpriteBack)),
                    Id = pokemonDetails.GetProperty("id").GetInt32(),
                    Name = Capitalize(pokemonDetails.GetProperty("name").ToString()),
                    Type = Capitalize(pokemonDetails.GetProperty("types")[0].GetProperty("type").GetProperty("name").ToString())
                };

                return target;
            } else
            {
                throw new Exception("Error parsing pokemon details from API");
            }
        }

        private static string Capitalize(string text)
        {
            return Char.ToUpper(text[0]) + text.Substring(1);
        }
    }
}
