
namespace TestProject1
{
    public class PokeAPITest
    {
        [SetUp]
        public void Setup()
        {
            
        }
        [Test]
        public async Task AddValidPokemon()
        {
            Pokemon p = new Pokemon();
            string Error = await PokeApi.ApplyPokemonAPIInfo("1", p);
            Console.WriteLine(p.Name);
            Assert.IsNotNull(p.SpriteFront);
            Assert.IsEmpty(Error);
        }
        [Test]
        public async Task AddInvalidPokemon()
        {
            Pokemon p = new Pokemon();
            string Error = await PokeApi.ApplyPokemonAPIInfo("0", p);
            Console.WriteLine(p.Name);
            Assert.IsNull(p.SpriteFront);
            Assert.IsNotEmpty(Error);
        }
    }
}