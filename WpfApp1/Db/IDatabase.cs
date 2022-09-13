using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    interface IDatabase
    {
        // Trainer
        List<Trainer> GetAllTrainers();
        void InsertTrainer(Trainer trainer);
        void UpdateTrainer(int id, Trainer treinador);
        void RemoveTrainer(Trainer trainer);

        // Pokemon
        void InsertPokemon(Trainer trainer, Pokemon pokemon);
        void UpdatePokemon(int id, Pokemon pokemon);
        void RemovePokemon(int id);
    }
}
