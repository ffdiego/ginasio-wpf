using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1;
using WpfApp1.ViewModels;

namespace TestProject1
{
    class GymTests
    {
        private Gym gym;
        Trainer trainer1 = new()
        {
            Name = "Diego"
        };
        private readonly Pokemon pokemon1 = new()
        {
            Id = 1,
            Name = "Bulbasaur",
            Type = "Grass",
            SpriteFront = new MemoryStream(new byte()),
            SpriteBack = new MemoryStream(new byte())
        };
        public GymTests()
        {
            DBManager.SetDB(DBType.PostGRES);
            DBManager.ResetTables();
            gym = new Gym();
        }

        [Test]
        public void AddATrainer()
        {

            gym.Add(trainer1);
            Assert.That(gym.Trainers.Contains(trainer1), Is.True);
        }
        [Test]
        public void InsertPokemon()
        {
            trainer1.AttachPokemon(pokemon1);
            Assert.That(trainer1.Pokemons.Where(p => p.Id == pokemon1.Id).Any(), Is.True);
        }
        [Test]
        public void InsertPokemonIgnoreDuplicated()
        {
            trainer1.AttachPokemon(pokemon1);
            Assert.That(trainer1.Pokemons.Count(), Is.EqualTo(1));
        }
        [Test]
        public void RemovePokemon()
        {
            trainer1.DetachPokemon(pokemon1);
            Assert.That(trainer1.Pokemons.Count(), Is.EqualTo(0));
        }
    }
}
