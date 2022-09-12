﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Views;

namespace WpfApp1.ViewModels
{
    public class VMEditPokemon
    {
        private EditPokemon screen;
        private Pokemon source, editPokemon;
        private Treinador owner;
        public string Name { get; set; }
        public string ErrorMSG { get; private set; }
        public RelayCommand Fetch { get; private set; }
        public RelayCommand OK { get; private set; }
        public RelayCommand Cancel { get; private set; }

        private VMEditPokemon()
        {
            
        }
        public VMEditPokemon(Treinador treinador)
        {
            owner = treinador;
            this.editPokemon = new Pokemon();
            DisplayScreen();
        }
        public VMEditPokemon(Treinador treinador, Pokemon pokemon)
        {
            owner = treinador;
            source = pokemon;
            editPokemon = new Pokemon(pokemon);
            DisplayScreen();
        }
        private void DisplayScreen()
        {
            InitializeCommands();
            this.screen = new EditPokemon();
            this.screen.DataContext = this;
            screen.ShowDialog();
            if (screen.DialogResult == true)
            {
                Console.WriteLine("Retornei true!");
                if(this.source != null)
                {
                    this.source.CopyFrom(editPokemon);
                } else
                {
                    owner.AddPokemon(EditPokemon);
                }
            }
        }
        private void InitializeCommands()
        {
            Fetch = new RelayCommand((object _) =>
            {
                _ = PokeApi.ApplyPokemonAPIInfo(Name, EditPokemon);
            });
            OK = new RelayCommand((object _) =>
            {
                this.screen.DialogResult = true;
            });
            Cancel = new RelayCommand((object _) =>
            {
                this.screen.DialogResult = false;
            });
        }
        public Pokemon EditPokemon { get { return editPokemon; } set { editPokemon = value; } }
    }
}