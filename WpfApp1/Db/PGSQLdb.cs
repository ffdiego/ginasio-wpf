using Npgsql;
using System;
using System.Collections.Generic;
using System.Windows;

namespace WpfApp1
{
    public class PGSQLdb : IDatabase
    {
        private readonly string connectionString = "Host=127.0.0.1;Username=postgres;Password=docker";
        private NpgsqlConnection connection;
        NpgsqlCommand cmd;
        NpgsqlDataReader reader;
        public PGSQLdb()
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
            }
            catch (Exception e)
            {
                throw e;
            }
            return trainers;
        }
        public Pokemon GetPokemon(Pokemon pokemon)
        {
            Connect();
            Pokemon result = _searchPokemon(pokemon);
            Disconnect();
            return result;
        }

        public void InsertPokemon(Trainer trainer, Pokemon pokemon)
        {
            Connect();
            try
            {
                Pokemon search = _searchPokemon(pokemon);
                if (search == null)
                    _createPokemon(pokemon);
                else
                    pokemon.CopyFrom(search);
                _attachPokemon(trainer, pokemon);
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
            Disconnect();
        }

        public void InsertTrainer(Trainer trainer)
        {
            Connect();
            _createTrainer(trainer);
            foreach (Pokemon pokemon in trainer.Pokemons)
            {
                if(_searchPokemon(pokemon) == null)
                    _createPokemon(pokemon);
                _attachPokemon(trainer, pokemon);
            }
            Disconnect();
        }

        public void RemoveTrainer(Trainer trainer)
        {
            Connect();
            _deleteTrainer(trainer);
            foreach (Pokemon pokemon in trainer.Pokemons)
            {
                _detachPokemon(trainer, pokemon);
            }
            Disconnect();
        }
        public void DetachPokemon(Trainer trainer, Pokemon pokemon)
        {
            Connect();
            _detachPokemon(trainer, pokemon);
            Disconnect();
        }
        public void AttachPokemon(Trainer trainer, Pokemon pokemon)
        {
            Connect();
            _attachPokemon(trainer, pokemon);
            Disconnect();
        }

        public void UpdatePokemon(Pokemon pokemon)
        {
            Connect();
            _updatePokemon(pokemon);
            Disconnect();
        }

        public void UpdateTrainer(Trainer treinador)
        {
            Connect();
            _updateTrainer(treinador);
            Disconnect();
        }
        private List<Trainer> _readTrainers()
        {
            List<Trainer> list = new List<Trainer>();
            try
            {
                cmd = new NpgsqlCommand("SELECT * FROM ginasio", connection);
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
                cmd?.Dispose();
                reader?.Dispose();
            }
        }
        private void _createPokemon(Pokemon pokemon)
        {
            try
            {
                cmd = new NpgsqlCommand("INSERT INTO pokemon(name) VALUES($1) RETURNING id", connection);
                cmd.Parameters.AddWithValue(pokemon.Name);
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
                cmd = new NpgsqlCommand("INSERT INTO trainers2pokemons (trainer_id, pokemon_id) VALUES ($1,$2)", connection);
                cmd.Parameters.AddWithValue(trainer.Id);
                cmd.Parameters.AddWithValue(pokemon.Id);
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
                cmd = new NpgsqlCommand("SELECT * FROM pokemon WHERE name = $1 LIMIT 1", connection);
                cmd.Parameters.AddWithValue(pokemon.Name);
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
                cmd = new NpgsqlCommand("SELECT pokemon.id, name, type, sprite_front, sprite_back FROM pokemon pokemon " +
                                    "INNER JOIN trainers2pokemons t2p " +
                                    "ON pokemon.id = t2p.pokemon_id " +
                                    "WHERE t2p.trainer_id = ($1)", connection);
                cmd.Parameters.AddWithValue(trainer.Id);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Pokemon p = new Pokemon()
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Type = reader.GetString(2),
                        SpriteFront = reader.GetString(3),
                        SpriteBack = reader.GetString(4),
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
                cmd = new NpgsqlCommand("UPDATE ginasio " +
                    "SET name = $1 " +
                    "WHERE id = $2", connection);
                cmd.Parameters.AddWithValue(t.Name);
                cmd.Parameters.AddWithValue(t.Id);
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
                cmd = new NpgsqlCommand("UPDATE pokemon " +
                    "SET name = $1, type = $2, sprite_front = $3, sprite_back = $4 " +
                    "WHERE id = $5", connection);
                cmd.Parameters.AddWithValue(p.Name);
                cmd.Parameters.AddWithValue(p.Type);
                cmd.Parameters.AddWithValue(p.SpriteFront);
                cmd.Parameters.AddWithValue(p.SpriteBack);
                cmd.Parameters.AddWithValue(p.Id);

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
                cmd = new NpgsqlCommand("DELETE FROM ginasio WHERE id = $1;", connection);
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
                cmd.Dispose();
                reader.Dispose();
            }
        }
        private void _detachPokemon(Trainer trainer, Pokemon pokemon)
        {
            try
            {
                cmd = new NpgsqlCommand("DELETE FROM trainers2pokemons WHERE trainer_id = $1 AND pokemon_id = $2", connection);
                cmd.Parameters.AddWithValue(trainer.Id);
                cmd.Parameters.AddWithValue(pokemon.Id);
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
