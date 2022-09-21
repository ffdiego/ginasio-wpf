using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Db
{
    static public class DBManager
    {
        static IDatabase db;
        static public void SetDB(IDatabase p_db)
        {
            db = p_db;
        }

        static public List<Trainer> GetAllTrainers()
        {
            try
            {
                return db.GetAllTrainers();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        static public void ResetTables()
        {
            db.ResetTables();
        }
        static public void AddTrainer(Trainer trainer)
        {
            db.InsertTrainer(trainer);
        }
        static public void RemoveTrainer(Trainer trainer)
        {
            db.RemoveTrainer(trainer);
        }
        static public Pokemon GetPokemon(string nameOrId)
        {
            return db.SearchPokemon(nameOrId);
        }
        static public void AddPokemon(Pokemon p)
        {
            db.InsertPokemon(p);
        }
        static public void AttachPokemon(Trainer t, Pokemon p)
        {
            db.AttachPokemon(t, p);
        }
        static public void DetachPokemon(Trainer t, Pokemon p)
        {
            db.DetachPokemon(t, p);
        }
        static public void UpdateTrainer(Trainer trainer)
        {
            db.UpdateTrainer(trainer);
        }
    }
}