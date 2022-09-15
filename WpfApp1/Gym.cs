using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Db;

namespace WpfApp1
{
    public class Gym
    {
        public ObservableCollection<Trainer> Trainers { get; set; }
        public Gym()
        {
            DBManager.SetDB(DBType.PostGRES);
            Trainers = new ObservableCollection<Trainer>(DBManager.GetAllTrainers());
        }


    }
}
