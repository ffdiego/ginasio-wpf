using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Db;

namespace WpfApp1.ViewModels
{
    public class EditTrainerVM
    {
        private ObservableCollection<Trainer> trainers;
        private Trainer source, editTrainer;
        public Pokemon HighlightedPokemon {get; set; }
        private EditTrainer screen;
        public RelayCommand AddPokemon { get; private set; }
        public RelayCommand RemovePokemon { get; private set; }
        private EditTrainerVM()
        {
        }
        public EditTrainerVM(ObservableCollection<Trainer> list)
        {
            trainers = list;
            editTrainer = new Trainer();
            DisplayScreen();
        }
        public EditTrainerVM(ObservableCollection<Trainer> list, Trainer treinador)
        {
            trainers = list;
            source = treinador;
            editTrainer = new Trainer(treinador);
            DisplayScreen();
        }

        private void DisplayScreen()
        {
            InitializeCommands();
            screen = new EditTrainer();
            screen.DataContext = this;
            screen.ShowDialog();
            if (screen.DialogResult == true)
            {
                if (source != null) //I'm editing
                {
                    editTrainer.Id = source.Id;
                    DBManager.UpdateTrainer(editTrainer);
                    source.CopyFrom(editTrainer);
                }
                else
                {
                    DBManager.AddTrainer(editTrainer);
                    trainers.Add(editTrainer);
                }
            }
        }
        private void InitializeCommands()
        {
            AddPokemon = new RelayCommand((object _) =>
            {
                EditPokemonVM vm = new EditPokemonVM(Treinador);
            });
            RemovePokemon = new RelayCommand((object _) =>
            {
                DBManager.RemovePokemon(Treinador, HighlightedPokemon);
                Treinador.Pokemons.Remove(HighlightedPokemon);
            }, (object _) => HighlightedPokemon != null);

        }
        public Trainer Treinador { get { return editTrainer; } set { editTrainer = value; } }
    }
}
