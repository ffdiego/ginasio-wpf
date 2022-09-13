using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Db
{
    class DbManager
    {
        private Type type;
        private IDatabase db;

        public DbManager()
        {

        }
        public DbManager(Type type)
        {
            this.type = type;
            switch (type)
            {
                case Type.PostGRES:
                    this.db = new PGSQLdb();
                    break;
                case Type.MariaDB:
                    break;
                default:
                    break;
            }
        }

        public List<Trainer> GetAllTrainers()
        {
            return this.db.GetAllTrainers();
        }

        public void AddTrainer(Trainer trainer)
        {
            this.db.InsertTrainer(trainer);
        }
        public void RemoveTrainer(Trainer trainer)
        {
            this.db.RemoveTrainer(trainer);
        }
    }
}

internal enum Type
{
    PostGRES,
    MariaDB
}