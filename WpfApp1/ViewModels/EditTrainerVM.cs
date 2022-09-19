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
        private Gym gym;
        private Trainer source, editTrainer;
        private EditTrainer screen;
        public ObservableCollection<Trainer> trainers { get; private set; }
        public Trainer Treinador { get { return editTrainer; } set { editTrainer = value; } }
        public Pokemon HighlightedPokemon {get; set; }
        public RelayCommand AddPokemon { get; private set; }
        public RelayCommand RemovePokemon { get; private set; }
        private EditTrainerVM()
        {
        }
        public EditTrainerVM(Gym gym)
        {
            this.gym = gym;
            trainers = gym.Trainers;
            editTrainer = new Trainer();
            DisplayScreen();
        }
        public EditTrainerVM(Gym gym, Trainer treinador)
        {
            this.gym = gym;
            trainers = gym.Trainers;
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
                    source.CopyFrom(editTrainer);
                }
                else
                {
                    gym.Add(editTrainer);
                }
            }
        }
        private void InitializeCommands()
        {
            AddPokemon = new RelayCommand((object _) =>
            {
                EditPokemonVM vm = new EditPokemonVM(Treinador);
            }, (object _) => source != null);
            RemovePokemon = new RelayCommand((object _) =>
            {
                Treinador.DetachPokemon(HighlightedPokemon);
            }, (object _) => HighlightedPokemon != null);

        }

    }
}
