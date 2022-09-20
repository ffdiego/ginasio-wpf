using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfApp1.Db;

namespace WpfApp1
{
    public class Gym
    {
        public Gym()
        {
            try
            {
                DBManager.SetDB(DBType.PostGRES);
                Trainers = new ObservableCollection<Trainer>(DBManager.GetAllTrainers());
            }
            catch(Exception)
            {
                throw;
            }
        }
        public ObservableCollection<Trainer> Trainers { get; }
        public void Add(Trainer trainer)
        {
            try
            {
                DBManager.AddTrainer(trainer);
                Trainers.Add(trainer);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void Remove(Trainer trainer)
        {
            try
            {
                DBManager.RemoveTrainer(trainer);
                Trainers.Remove(trainer);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
