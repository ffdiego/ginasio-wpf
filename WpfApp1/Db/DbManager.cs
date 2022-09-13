using System;
using System.Collections.Generic;
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

    }
}

internal enum Type
{
    PostGRES,
    MariaDB
}