using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp1
{
    public class PGSQLdb : IDatabase
    {
        private readonly string connectionString = "Host=127.0.0.1;Username=postgres;Password=docker";
        private NpgsqlConnection connection;
        public PGSQLdb()
        {
            
        }

        public List<Trainer> GetAllTrainers()
        {
            List<Trainer> trainers = _getTrainers();
            foreach (Trainer trainer in trainers)
            {
                _getPokemonsOf(trainer);
            }
            return trainers;
        }

        public void InsertPokemon(Trainer trainer, Pokemon pokemon)
        {
            Connect();
            NpgsqlCommand cmd;
            NpgsqlDataReader reader;
            try
            {
                //todo:
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                throw;
            }
            finally
            {
                //cmd?.Dispose();
                //reader?.Dispose();
            }
            Disconnect();
        }

        public void InsertTrainer(Trainer trainer)
        {
            Connect();
            NpgsqlCommand cmd;
            NpgsqlDataReader reader;
            try
            {
                cmd = new NpgsqlCommand("INSERT INTO ginasio(name) VALUES($1) RETURNING id", connection);
                cmd.Parameters.AddWithValue(trainer.Name);
                reader = cmd.ExecuteReader();
                reader.Read();
                trainer.Id = reader.GetInt16(0);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                throw;
            }
            finally
            {
                //cmd?.Dispose();
                //reader?.Dispose();
            }
            Disconnect();
        }

        public void RemovePokemon(int id)
        {
            throw new NotImplementedException();
        }

        public void RemoveTrainer(Trainer trainer)
        {
            Connect();
            NpgsqlCommand cmd;
            try
            {
                cmd = new NpgsqlCommand("DELETE FROM ginasio WHERE id = $1", connection);
                cmd.Parameters.AddWithValue(trainer.Id);
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                throw;
            }
            finally
            {
                //cmd?.Dispose();
                //reader?.Dispose();
            }
            Disconnect();
        }

        public void UpdatePokemon(int id, Pokemon pokemon)
        {
            throw new NotImplementedException();
        }

        public void UpdateTrainer(int id, Trainer treinador)
        {
            throw new NotImplementedException();
        }
        //todo: resolver nomenclaturas
        private List<Trainer> _getTrainers()
        {
            Connect();
            NpgsqlCommand cmd;
            NpgsqlDataReader reader;
            List<Trainer> list = new List<Trainer>();
            try
            {
                cmd = new NpgsqlCommand("SELECT * FROM ginasio", connection);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Trainer trainer = new Trainer();
                    trainer.Id = reader.GetInt32(0);
                    trainer.Name = reader.GetString(1);
                    list.Add(trainer);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                throw;
            } finally
            {
                //cmd?.Dispose();
                //reader?.Dispose();
            }
            Disconnect();
            return list;
        }

        private void _getPokemonsOf(Trainer trainer)
        {
            Connect();
            NpgsqlCommand cmd;
            NpgsqlDataReader reader;
            try
            {
                cmd = new NpgsqlCommand("SELECT name, type FROM pokemon p " +
                                    "INNER JOIN trainers2pokemons t2p " +
                                    "ON p.id = t2p.pokemon_id " +
                                    "WHERE t2p.trainer_id = ($1)", connection);
                cmd.Parameters.AddWithValue(trainer.Id);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Pokemon p = new Pokemon(reader.GetString(0));
                    trainer.AddPokemon(p);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                throw;
            }
            finally
            {
                //cmd?.Dispose();
                //reader?.Dispose();
                Disconnect();
            }
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
