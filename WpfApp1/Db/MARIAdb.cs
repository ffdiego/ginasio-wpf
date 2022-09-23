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
        public void ResetTables()
        {
            Connect();
            _dropTables();
            _createTables();
            Disconnect();
        }
        public List<Trainer> GetAllTrainers()
        {
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
            catch (Exception)
            {
                throw;
            }
        }
        public Pokemon SearchPokemon(string name)
        {
            Connect();
            Pokemon pokemon = _getPokemon(name);
            Disconnect();
            return pokemon;
        }
        public void InsertPokemon(Pokemon pokemon)
        {
            try
            {
                Connect();
                if (_pokemonExistsInDb(pokemon) == false)
                    _createPokemon(pokemon);
                Disconnect();
            }
            catch (Exception)
            {
                throw;
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
                    if (_pokemonExistsInDb(pokemon) == false)
                        _createPokemon(pokemon);
                    _attachPokemon(trainer, pokemon);
                }
                Disconnect();
            }
            catch (Exception)
            {

                throw;
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
            catch (Exception)
            {

                throw;
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
            catch (Exception)
            {
                throw;
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
            catch (Exception)
            {
                throw;
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
            catch (Exception)
            {
                throw;
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
            catch (Exception)
            {
                throw;
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
            catch (Exception)
            {
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
                cmd = new MySqlCommand("INSERT INTO pokemon(id, name, type, sprite_front, sprite_back) VALUES(@i, @n, @t, @f, @b) RETURNING id", connection);
                cmd.Parameters.AddWithValue("i", pokemon.Id);
                cmd.Parameters.AddWithValue("n", pokemon.Name);
                cmd.Parameters.AddWithValue("t", pokemon.Type);
                cmd.Parameters.AddWithValue("f", Convert.ToBase64String(pokemon.SpriteFront.ToArray()));
                cmd.Parameters.AddWithValue("b", Convert.ToBase64String(pokemon.SpriteBack.ToArray()));
                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
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
            catch (Exception)
            {
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
        private bool _pokemonExistsInDb(Pokemon pokemon)
        {
            bool result = false;
            try
            {
                cmd = new MySqlCommand("SELECT id FROM pokemon WHERE id = @i LIMIT 1", connection);
                cmd.Parameters.AddWithValue("i", pokemon.Id);
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                        result = true;
                }
                return result;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                cmd.Dispose();
                reader.Dispose();
            }

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
            catch (Exception)
            {
                throw;
            }
            finally
            {
                cmd.Dispose();
                reader.Dispose();
            }
        }
        private Pokemon _getPokemon(string nameOrId)
        {
            Pokemon result = new Pokemon();
            int id;
            try
            {
                if (int.TryParse(nameOrId, out id))
                {
                    cmd = new MySqlCommand(@"SELECT pokemon.id, name, type, sprite_front, sprite_back FROM pokemon
                                          WHERE id = @id", connection);
                    cmd.Parameters.AddWithValue("id", id);
                }
                else
                {
                    cmd = new MySqlCommand(@"SELECT pokemon.id, name, type, sprite_front, sprite_back FROM pokemon
                                          WHERE LOWER(name) = @name", connection);
                    cmd.Parameters.AddWithValue("name", nameOrId.ToLower());
                }

                reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    result = new Pokemon()
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Type = reader.GetString(2),
                        SpriteFront = new MemoryStream(Convert.FromBase64String(reader.GetString(3))),
                        SpriteBack = new MemoryStream(Convert.FromBase64String(reader.GetString(4)))
                    };
                }
                return (result.Id != 0) ? result : null;
            }
            catch (Exception)
            {
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
            catch (Exception)
            {
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
            try
            {
                cmd = new MySqlCommand("DELETE FROM ginasio WHERE id = @i;", connection);
                cmd.Parameters.AddWithValue("i", trainer.Id);
                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
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
            catch (Exception)
            {
                throw;
            }
            finally
            {
                cmd.Dispose();
                reader.Dispose();
            }
        }
        private void _dropTables()
        {
            try
            {
                cmd = new MySqlCommand(@"  DROP TABLE IF EXISTS trainers2pokemons;
                                            DROP TABLE IF EXISTS ginasio;
                                            DROP TABLE IF EXISTS pokemon;", connection);
                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                cmd.Dispose();
            }
        }
        private void _createTables()
        {
            try
            {
                cmd = new MySqlCommand(@"
                        CREATE TABLE ginasio (
                            id SERIAL NOT NULL PRIMARY KEY,
                            name VARCHAR(255) NOT NULL
                        );
                        CREATE TABLE pokemon (
                            id SERIAL NOT NULL PRIMARY KEY,
                            name VARCHAR(255) NOT NULL,
                            type VARCHAR(255) NOT NULL,
                            sprite_front TEXT NOT NULL,
                            sprite_back TEXT NOT NULL
                        );
                        CREATE TABLE trainers2pokemons (
                            id serial NOT NULL PRIMARY KEY,
                            trainer_id SERIAL REFERENCES ginasio(id) ON DELETE CASCADE ON UPDATE CASCADE,
                            pokemon_id SERIAL REFERENCES pokemon(id) ON DELETE CASCADE ON UPDATE CASCADE
                        );", connection);
                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                cmd.Dispose();
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
                    Console.WriteLine("Success: PostgreSQL connection opened");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        private void Disconnect()
        {
            connection.Close();
        }
    }
}

