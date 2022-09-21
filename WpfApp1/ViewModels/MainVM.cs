using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using WpfApp1.Db;
using WpfApp1.ViewModels;

namespace WpfApp1
{
    public class MainVM : INotifyPropertyChanged
    {
        public ObservableCollection<Trainer> trainers { get; set; }
        Gym gym;
        private Trainer highlightedTrainer;
        public Pokemon highlightedPokemon;
        // Relay Commands
        public RelayCommand Add { get; private set; }
        public RelayCommand Edit { get; private set; }
        public RelayCommand Remove { get; private set; }
        public RelayCommand NewPokemon { get; private set; }
        public MainVM()
        {
            DBManager.SetDB(new PGSQLdb());
            try
            {
                gym = new Gym();
                trainers = gym.Trainers;
            } catch(Exception e)
            {
                MessageBox.Show(e.Message);
                Application.Current.Shutdown();
            }
            IniciaComandos();
        }
        public void IniciaComandos()
        {
            Add = new RelayCommand( (object _) => {
                EditTrainerVM vm = new EditTrainerVM(gym);
                vm.ShowWindow();
            });
            Edit = new RelayCommand((object _) => {
                EditTrainerVM vm = new EditTrainerVM(gym, highlightedTrainer);
                vm.ShowWindow();
            }, (object _) => this.highlightedTrainer != null);
            Remove = new RelayCommand((object _) => {
                gym.Remove(highlightedTrainer);
            }, (object _) => this.highlightedTrainer != null);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void Notify([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public Trainer HighlightedTrainer
        {
            get { return highlightedTrainer; }
            set
            {
                highlightedTrainer = value;
                if (value != null)
                    HighlightedPokemon = (value.Pokemons.Count > 0) ? value.Pokemons[0] : null;
                Notify();
            }
        }
        public Pokemon HighlightedPokemon
        {
            get { return highlightedPokemon; }
            set
            {
                highlightedPokemon = value;
                Notify();
            }
        }  
    }
}
