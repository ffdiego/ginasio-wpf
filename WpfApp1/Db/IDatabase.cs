using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    public interface IDatabase
    {
        void ResetTables();
        // Trainer
        List<Trainer> GetAllTrainers();
        void InsertTrainer(Trainer trainer);
        void UpdateTrainer(Trainer treinador);
        void RemoveTrainer(Trainer trainer);

        // Pokemon
        Pokemon SearchPokemon(string nameOrId);
        void InsertPokemon(Pokemon pokemon);
        void AttachPokemon(Trainer trainer, Pokemon pokemon);
        void DetachPokemon(Trainer trainer, Pokemon pokemon);
    }
}
