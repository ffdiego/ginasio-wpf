
using Autofac.Extras.Moq;
using WpfApp1;

namespace TestProject1
{
    public class PokeAPITests
    {
        Gym gym;
        public PokeAPITests()
        {
            DBManager.SetDB(new PGSQLdb());
            DBManager.ResetTables();
            gym = new Gym();
        }
        [Test]
        public async Task GetValidPokemon()
        {
            Pokemon p = new Pokemon();
            string Error;
            (p, Error) = await PokeApi.GetPokemonAPIInfo("1");
            Assert.That(p, Is.Not.Null);
            Assert.That(Error, Is.Empty);
        }
        [Test]
        public async Task GetInvalidPokemon()
        {
            Pokemon p = new Pokemon();
            string Error;
            (p, Error) = await PokeApi.GetPokemonAPIInfo("0");
            Assert.That(p, Is.Null);
            Assert.That(Error, Is.Not.Empty);
        }
        [Test]
        public async Task AddPokemonFromAPIToDB()
        {
            Pokemon p = new Pokemon();
            string Error;
            (p, Error) = await PokeApi.GetPokemonAPIInfo("1");
            DBManager.AddPokemon(p);
            Assert.That(p.Id, Is.EqualTo(DBManager.GetPokemon("1").Id));
        }
    }
}