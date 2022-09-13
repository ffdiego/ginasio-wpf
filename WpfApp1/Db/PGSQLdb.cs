using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp1
{
    public class PGSQLdb : IDatabase
    {
        private readonly string connectionString = "Host=127.0.0.1;Username=postgres;Password=docker";
        private NpgsqlCommand cmd;
        private NpgsqlDataReader reader;
        private NpgsqlConnection connection;
        public PGSQLdb()
        {
            
        }

        public List<Trainer> GetAllTrainers()
        {
            List<Trainer> trainers = new List<Trainer>();
            Connect();
            cmd = new NpgsqlCommand("SELECT * FROM ginasio", connection);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                trainers.Add(new Trainer(reader.GetString(2)));
            }
            Disconnect();
            return trainers;
        }

        public void InsertPokemon(Pokemon pokemon)
        {
            throw new NotImplementedException();
        }

        public void InsertTrainer(Trainer trainer)
        {
            throw new NotImplementedException();
        }

        public void RemovePokemon(int id)
        {
            throw new NotImplementedException();
        }

        public void RemoveTrainer(int id)
        {
            throw new NotImplementedException();
        }

        public void UpdatePokemon(int id, Pokemon pokemon)
        {
            throw new NotImplementedException();
        }

        public void UpdateTrainer(int id, Trainer treinador)
        {
            throw new NotImplementedException();
        }

        private void Connect()
        {
            connection = new NpgsqlConnection(connectionString);
            try
            {
                connection.Open();
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    Console.WriteLine("Success open postgreSQL connection");
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        private void Disconnect()
        {
            connection.Close();
        }
    }
}
