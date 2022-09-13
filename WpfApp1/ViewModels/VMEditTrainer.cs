﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.ViewModels
{
    public class VMEditTrainer
    {
        private ObservableCollection<Trainer> trainers;
        private Trainer source, editTrainer;
        public Pokemon HighlightedPokemon {get; set; }
        private EditTrainer screen;
        public RelayCommand AddPokemon { get; private set; }
        public RelayCommand RemovePokemon { get; private set; }
        private VMEditTrainer()
        {
        }
        public VMEditTrainer(ObservableCollection<Trainer> list)
        {
            trainers = list;
            editTrainer = new Trainer();
            DisplayScreen();
        }
        public VMEditTrainer(ObservableCollection<Trainer> list, Trainer treinador)
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
                    source.CopyFrom(editTrainer);
                }
                else
                {
                    trainers.Add(editTrainer);
                }
            }
        }
        private void InitializeCommands()
        {
            AddPokemon = new RelayCommand((object _) =>
            {
                Console.Write("Apertaste!");
                VMEditPokemon vm = new VMEditPokemon(Treinador);
            });
            RemovePokemon = new RelayCommand((object _) =>
            {
                Console.Write("Apertaste!");
                Treinador.Pokemons.Remove(HighlightedPokemon);
            }, (object _) => HighlightedPokemon != null);

        }
        public Trainer Treinador { get { return editTrainer; } set { editTrainer = value; } }
    }
}
