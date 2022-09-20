using MySqlConnector;
using Npgsql;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Remoting.Messaging;
using System.Windows;

namespace WpfApp1
{
    public class MARIAdb : IDatabase
    {
        private readonly string connectionString;
        private MySqlConnection connection;
        MySqlCommand cmd;
        MySqlDataReader reader;
        public MARIAdb()
        {
            connectionString = "Server=127.0.0.1;Port=8000;User ID=root;Password=my-secret-pw;Database=test";
        }
        public MARIAdb(string db)
        {
            connectionString = $"Server=127.0.0.1;Port=8000;User ID=root;Password=my-secret-pw;Database={db}";
        }
        public List<Trainer> GetAllTrainers()
        {
            Dictionary<string, double> FDados = new Dictionary<string, double>();
            
            try
            {
                Connect();
                List<Trainer> result = _readTrainers();
                foreach (Trainer trainer in result)
                {
                    trainer.SetPokemons(_readPokemonsOf(trainer.Id));
                }
                Disconnect();
                return result;
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

        public void InsertPokemon(Pokemon pokemon)
        {
            try
            {
                Connect();
                _createPokemon(pokemon);
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

        public void UpdateTrainer(Trainer t)
        {
            try
            {
                Connect();
                _updateTrainer(t);
                t.SetPokemons(_readPokemonsOf(t.Id));
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
                    Trainer trainer = new Trainer()
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1)
                    };
                    list.Add(trainer);
                }
                return list;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                cmd.Dispose();
                reader.Dispose();
            }
        }
        // Create
        private void _createTrainer(Trainer trainer)
        {
            try
            {
                cmd = new MySqlCommand("INSERT INTO ginasio(name) VALUES(@n) RETURNING id", connection);
                cmd.Parameters.AddWithValue("n", trainer.Name);
                reader = cmd.ExecuteReader();
                reader.Read();
                trainer.Id = reader.GetInt16(0);
            }
            catch (Exception e)
            {
                throw e;
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
                cmd = new MySqlCommand("INSERT INTO pokemon(id, name, type, sprite_front, sprite_back) VALUES(@i, @n, @t, @f, @b) RETURNING id", connection);
                cmd.Parameters.AddWithValue("i", pokemon.Id);
                cmd.Parameters.AddWithValue("n", pokemon.Name);
                cmd.Parameters.AddWithValue("t", pokemon.Type);
                cmd.Parameters.AddWithValue("f", pokemon.SpriteFront);
                cmd.Parameters.AddWithValue("b", pokemon.SpriteBack);
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                cmd.Dispose();
            }
        }
        private bool _attachPokemon(Trainer trainer, Pokemon pokemon)
        {
            bool result = false;
            try
            {
                cmd = new MySqlCommand("INSERT INTO trainers2pokemons (trainer_id, pokemon_id) VALUES (@t,@p)", connection);
                cmd.Parameters.AddWithValue("t", trainer.Id);
                cmd.Parameters.AddWithValue("p", pokemon.Id);
                reader = cmd.ExecuteReader();
            }
            catch (Exception e)
            {
                throw e;
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
                cmd = new MySqlCommand("SELECT id, name, type, sprite_front, sprite_back FROM pokemon WHERE name = @i LIMIT 1", connection);
                cmd.Parameters.AddWithValue("id", pokemon.Id);
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                        result = new Pokemon()
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Type = reader.GetString(2),
                            SpriteFront = new MemoryStream(Convert.FromBase64String(reader.GetString(3))),
                            SpriteBack = new MemoryStream(Convert.FromBase64String(reader.GetString(4)))
                        };
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                cmd.Dispose();
                reader.Dispose();
            }
            return result;
        }
        private List<Pokemon> _readPokemonsOf(int id)
        {
            List<Pokemon> result = new List<Pokemon>();
            try
            {
                cmd = new MySqlCommand("SELECT pokemon.id, name, type, sprite_front, sprite_back FROM pokemon pokemon " +
                                    "INNER JOIN trainers2pokemons t2p " +
                                    "ON pokemon.id = t2p.pokemon_id " +
                                    "WHERE t2p.trainer_id = (@id)", connection);
                cmd.Parameters.AddWithValue("id", id);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Pokemon p = new Pokemon()
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Type = reader.GetString(2),
                        SpriteFront = new MemoryStream(Convert.FromBase64String(reader.GetString(3))),
                        SpriteBack = new MemoryStream(Convert.FromBase64String(reader.GetString(4)))
                    };
                    result.Add(p);
                }
                return result;
            }
            catch (Exception e)
            {
                throw e;
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
                throw e;
            }
            finally
            {
                cmd.Dispose();
            }
        }
        // Delete
        private void _deleteTrainer(Trainer trainer)
        {
            try
            {
                cmd = new MySqlCommand("DELETE FROM ginasio WHERE id = @i;", connection);
                cmd.Parameters.AddWithValue("i", trainer.Id);
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw e;
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
                throw e;
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
                throw e;
            }
        }
        private void Disconnect()
        {
            connection.Close();
        }

        public void ResetTables()
        {
            throw new NotImplementedException();
        }
    }
}

