
namespace TestProject1
{
    public class TestDB
    {
        [SetUp]
        public void Setup()
        {
            
        }
        [Test]
        public void ShouldAddAPokemon()
        {
            // Arrange
            Trainer trainer = new Trainer();

            // Act
            trainer.AddPokemon(new Pokemon("Pikachu"));

            // Assert
            Assert.IsTrue(trainer.Pokemons[0].Name == "Pikachu");
        }   
        public void ShouldConsultDBFirst()
        {    
            
        }
    }
}