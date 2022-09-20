using WpfApp1.ViewModels;

namespace TestProject1
{
    class MainVMTest
    {
        [SetUp]
        public void Setup()
        {
            
        }

        [Test]
        public void First()
        {
            Trainer t = new Trainer();
            t.Name = "Diego";
            EditPokemonVM vm = new EditPokemonVM(t);
            Assert.IsTrue(vm.owner.Name == t.Name);
        }
    }
}