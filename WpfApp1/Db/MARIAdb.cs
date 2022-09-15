using MySqlConnector;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using System.Windows;

namespace WpfApp1
{
    public class MARIAdb : IDatabase
    {
        private readonly string connectionString = "Server=127.0.0.1;Port=8000;User ID=root;Password=my-secret-pw;Database=test";
        private MySqlConnection connection;
        MySqlCommand cmd;
        MySqlDataReader reader;
        public MARIAdb()
        {
            
        }

        public List<Trainer> GetAllTrainers()
        {
            List<Trainer> trainers = null;
            try
            {
                Connect();
                trainers = _readTrainers();
                foreach (Trainer trainer in trainers)
                {
                    _readPokemonsOf(trainer);
                }
                Disconnect();
                return trainers;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public Pokemon GetPokemon(Pokemon pokemon)
        {
            try
            {
                Connect();
                Pokemon result = _searchPokemon(pokemon);
                Disconnect();
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void InsertPokemon(Trainer trainer, Pokemon pokemon)
        {
            try
            {
                Connect();
                Pokemon search = _searchPokemon(pokemon);
                if (search == null)
                    _createPokemon(pokemon);
                else
                    pokemon.CopyFrom(search);
                _attachPokemon(trainer, pokemon);
                Disconnect();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                throw e;
            }          
        }

        public void InsertTrainer(Trainer trainer)
        {
            try
            {
                Connect();
                _createTrainer(trainer);
                foreach (Pokemon pokemon in trainer.Pokemons)
                {
                    if (_searchPokemon(pokemon) == null)
                        _createPokemon(pokemon);
                    _attachPokemon(trainer, pokemon);
                }
                Disconnect();
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public void RemoveTrainer(Trainer trainer)
        {
            try
            {
                Connect();
                _deleteTrainer(trainer);
                foreach (Pokemon pokemon in trainer.Pokemons)
                {
                    _detachPokemon(trainer, pokemon);
                }
                Disconnect();
            }
            catch (Exception e)
            {

                throw e;
            }
        }
        public void DetachPokemon(Trainer trainer, Pokemon pokemon)
        {
            try
            {
                Connect();
                _detachPokemon(trainer, pokemon);
                Disconnect();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public void AttachPokemon(Trainer trainer, Pokemon pokemon)
        {
            try
            {
                Connect();
                _attachPokemon(trainer, pokemon);
                Disconnect();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void UpdatePokemon(Pokemon pokemon)
        {
            try
            {
                Connect();
                _updatePokemon(pokemon);
                Disconnect();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void UpdateTrainer(Trainer treinador)
        {
            try
            {
                Connect();
                _updateTrainer(treinador);
                Disconnect();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        private List<Trainer> _readTrainers()
        {
            List<Trainer> list = new List<Trainer>();
            try
            {
                cmd = new MySqlCommand("SELECT * FROM ginasio", connection);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Trainer trainer = new Trainer
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1)
                    };
                    list.Add(trainer);
                }
            }
            catch (Exception e)
            {
                throw e;
            } finally
            {
                cmd.Dispose();
                reader.Dispose();
            }
            return list;
        }
        // Create
        private void _createTrainer(Trainer trainer)
        {
            try
            {
                cmd = new MySqlCommand("INSERT INTO ginasio(name) VALUES(@t) RETURNING id", connection);
                cmd.Parameters.AddWithValue("t", trainer.Name);
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
                cmd?.Dispose();
                reader?.Dispose();
            }
        }
        private void _createPokemon(Pokemon pokemon)
        {
            try
            {
                cmd = new MySqlCommand("INSERT INTO pokemon(name) VALUES(@p) RETURNING id", connection);
                cmd.Parameters.AddWithValue("p", pokemon.Name);
                reader = cmd.ExecuteReader();
                reader.Read();
                pokemon.Id = reader.GetInt16(0);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                throw;
            }
            finally
            {
                cmd.Dispose();
                reader.Dispose();
            }
        }
        private bool _attachPokemon(Trainer trainer, Pokemon pokemon)
        {
            bool result = false;
            try
            {
                cmd = new MySqlCommand("INSERT INTO trainers2pokemons (trainer_id, pokemon_id) VALUES (@t,@p)", connection);
                cmd.Parameters.AddWithValue("t",trainer.Id);
                cmd.Parameters.AddWithValue("p",pokemon.Id);
                reader = cmd.ExecuteReader();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                throw;
            }
            finally
            {
                cmd.Dispose();
                reader.Dispose();
            }
            return result;
        }
        // Read
        private Pokemon _searchPokemon(Pokemon pokemon)
        {
            Pokemon result = null;
            try
            {
                cmd = new MySqlCommand("SELECT * FROM pokemon WHERE name = @p LIMIT 1", connection);
                cmd.Parameters.AddWithValue("p", pokemon.Name);
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                        result = new Pokemon()
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1)
                        };
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                throw;
            }
            finally
            {
                cmd.Dispose();
                reader.Dispose();
            }
            return result;
        }
        private void _readPokemonsOf(Trainer trainer)
        {
            try
            {
                cmd = new MySqlCommand("SELECT pokemon.id, name, type, sprite_front, sprite_back FROM pokemon pokemon " +
                                    "INNER JOIN trainers2pokemons t2p " +
                                    "ON pokemon.id = t2p.pokemon_id " +
                                    "WHERE t2p.trainer_id = (@t)", connection);
                cmd.Parameters.AddWithValue("t", trainer.Id);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Pokemon p = new Pokemon()
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        //Type = (reader.GetString(2).GetType() == typeof(string)) ? reader.GetString(2) : ""
                        //SpriteFront = reader.GetString(2) != string) ? "" : reader.GetString(3),
                        //SpriteBack = reader.GetString(2) != string) ? "" : reader.GetString(4),
                    };
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
                cmd.Dispose();
                reader.Dispose();
            }
        }
        // Update
        private void _updateTrainer(Trainer t)
        {
            try
            {
                cmd = new MySqlCommand("UPDATE ginasio " +
                    "SET name = @n " +
                    "WHERE id = @i", connection);
                cmd.Parameters.AddWithValue("n", t.Name);
                cmd.Parameters.AddWithValue("i", t.Id);
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                throw;
            }
            finally
            {
                cmd.Dispose();
            }
        }
        private void _updatePokemon(Pokemon p)
        {
            try
            {
                cmd = new MySqlCommand("UPDATE pokemon " +
                    "SET name = @n, type = @t, sprite_front = @f, sprite_back = @b " +
                    "WHERE id = @i", connection);
                cmd.Parameters.AddWithValue("n", p.Name);
                cmd.Parameters.AddWithValue("t",p.Type);
                cmd.Parameters.AddWithValue("f",p.SpriteFront);
                cmd.Parameters.AddWithValue("b",p.SpriteBack);
                cmd.Parameters.AddWithValue("i",p.Id);

                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                throw;
            }
            finally
            {
                cmd.Dispose();
            }
        }
        // Delete
        private void _deleteTrainer(Trainer trainer)
        {
            // todo: Estudar OnDeleteCascade
            try
            {
                cmd = new MySqlCommand("DELETE FROM ginasio WHERE id = @t;", connection);
                cmd.Parameters.AddWithValue("t", trainer.Id);
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                throw;
            }
            finally
            {
                cmd.Dispose();
                reader.Dispose();
            }
        }
        private void _detachPokemon(Trainer trainer, Pokemon pokemon)
        {
            try
            {
                cmd = new MySqlCommand("DELETE FROM trainers2pokemons WHERE trainer_id = @t AND pokemon_id = @p", connection);
                cmd.Parameters.AddWithValue("t", trainer.Id);
                cmd.Parameters.AddWithValue("p", pokemon.Id);
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                throw;
            }
            finally
            {
                cmd.Dispose();
                reader.Dispose();
            }
        }
        private void Connect()
        {
            connection = new MySqlConnection(connectionString);
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
                connection.Dispose();
                throw e;
            }
        }
        private void Disconnect()
        {
            connection.Close();
        }

    }
}
