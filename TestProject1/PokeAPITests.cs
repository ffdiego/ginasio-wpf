
namespace TestProject1
{
    public class PokeAPITests
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
            Assert.That(p.SpriteFront, Is.Not.Null);
            Assert.That(Error, Is.Empty);
        }
        [Test]
        public async Task AddInvalidPokemon()
        {
            Pokemon p = new Pokemon();
            string Error = await PokeApi.ApplyPokemonAPIInfo("0", p);
            Console.WriteLine(p.Name);
            Assert.That(p.SpriteFront, Is.Null);
            Assert.That(Error, Is.Not.Empty);
        }
    }
}