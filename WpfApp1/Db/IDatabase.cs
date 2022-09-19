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
        void UpdateTrainer(Trainer treinador);
        void RemoveTrainer(Trainer trainer);

        // Pokemon
        Pokemon GetPokemon(Pokemon pokemon);
        void InsertPokemon(Pokemon pokemon);
        void AttachPokemon(Trainer trainer, Pokemon pokemon);
        void DetachPokemon(Trainer trainer, Pokemon pokemon);
    }
}
