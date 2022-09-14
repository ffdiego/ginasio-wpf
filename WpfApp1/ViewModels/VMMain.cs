using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using WpfApp1.Db;
using WpfApp1.ViewModels;

namespace WpfApp1
{
    public class VMMain : INotifyPropertyChanged
    {
        public ObservableCollection<Trainer> trainers { get; set; }
        private Trainer highlightedTrainer;
        public Pokemon highlightedPokemon;
        // Relay Commands
        public RelayCommand Add { get; private set; }
        public RelayCommand Edit { get; private set; }
        public RelayCommand Remove { get; private set; }
        public RelayCommand NewPokemon { get; private set; }
        public VMMain()
        {
            Console.WriteLine("Iniciando tudo!");
            IniciaComandos();
            DBManager.SetDB(DBType.PostGRES);
            try
            {
                trainers = new ObservableCollection<Trainer>(DBManager.GetAllTrainers());
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        public void IniciaComandos()
        {
            Add = new RelayCommand( (object _) => {
                VMEditTrainer vm = new VMEditTrainer(trainers);
            });
            Edit = new RelayCommand((object _) => {
                VMEditTrainer vm = new VMEditTrainer(trainers, highlightedTrainer);
            }, (object _) => this.highlightedTrainer != null);
            Remove = new RelayCommand((object _) => {
                DBManager.RemoveTrainer(HighlightedTrainer);
                trainers.Remove(highlightedTrainer);
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
                Notify();
            }
        }
        public Pokemon HighlightedPokemon
        {
            get { return highlightedPokemon; }
            set
            {
                if (value == null) return;
                highlightedPokemon = value;
                Console.WriteLine("Troquei para o pokemon: " + value.Name);
                if(value.SpriteFront == null)
                {
                    _ = PokeApi.ApplyPokemonAPIInfo(value.Name, highlightedPokemon);
                }
                Notify();
            }
        }  
    }
}
