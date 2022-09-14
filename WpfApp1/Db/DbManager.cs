using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Db
{
    static class DBManager
    {
        static IDatabase db;

        static public void SetDB(DBType type)
        {
            switch (type)
            {
                case DBType.PostGRES:
                    db = new PGSQLdb();
                    break;
                case DBType.MariaDB:
                    break;
                default:
                    break;
            }
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

        static public void AddTrainer(Trainer trainer)
        {
            db.InsertTrainer(trainer);
        }
        static public void RemoveTrainer(Trainer trainer)
        {
            db.RemoveTrainer(trainer);
        }
        static public void AddPokemon(Trainer t, Pokemon p)
        {
            db.InsertPokemon(t, p);
        }
        static public void RemovePokemon(Trainer t, Pokemon p)
        {
            db.DetachPokemon(t, p);
        }
        static public void UpdatePokemon(Pokemon pokemon)
        {
            db.UpdatePokemon(pokemon);
        }
        static public void UpdateTrainer(Trainer trainer)
        {
            db.UpdateTrainer(trainer);
        }
    }
}

internal enum DBType
{
    PostGRES,
    MariaDB
}