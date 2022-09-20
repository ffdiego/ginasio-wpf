using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1;
using WpfApp1.ViewModels;

namespace TestProject1
{
    class DbTest
    {
        [SetUp]
        public void Setup()
        {
            DBManager.SetDB(DBType.PostGRES);
            DBManager.ResetTables();
        }

        [Test]
        public void AddATrainer()
        {
            Trainer trainer = new Trainer()
            {
                Name = "Diego"
            };
            Trainer trainer2 = new Trainer()
            {
                Name = "Marcelinho"
            };
            DBManager.AddTrainer(trainer);
            var results = DBManager.GetAllTrainers();
            Assert.IsTrue(results.Contains(trainer));
        }

    }
}
