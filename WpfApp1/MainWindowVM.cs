﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace WpfApp1
{
    public class MainWindowVM : INotifyPropertyChanged
    {
        public ObservableCollection<Treinador> listaDeTreinadores { get; set; }
        public Treinador treinadorSelecionado;
        public Pokemon pokemonSelecionado;
        // Relay Commands
        public RelayCommand Add { get; private set; }
        public RelayCommand Edit { get; private set; }
        public RelayCommand Remove { get; private set; }
        public RelayCommand NewPokemon { get; private set; }

        private EditarTreinador tela;
        public MainWindowVM()
        {
            listaDeTreinadores = new ObservableCollection<Treinador>();
            listaDeTreinadores.Add(new Treinador("Marcos"));
            listaDeTreinadores.Add(new Treinador("Letícia"));
            listaDeTreinadores[1].AddPokemon("Sapinho");
            listaDeTreinadores[1].AddPokemon("Pikachu");

            Console.WriteLine("iniciando tudo!");
            IniciaComandos();
        }
        public void IniciaComandos()
        {
            Add = new RelayCommand( (object _) => {
                Treinador treinador = new Treinador();
                tela = new EditarTreinador();
                tela.DataContext = treinador;
                tela.Title = "Adicionar Treinador";
                tela.ShowDialog();
                if (tela.DialogResult == true)
                {
                    listaDeTreinadores.Add(treinador);
                    MessageBox.Show("Treinador Salvo!", "", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            });
            Edit = new RelayCommand((object _) => {
                //  todo: resolver cópia superficial
                tela = new EditarTreinador();
                Treinador treinador = new Treinador(TreinadorSelecionado);
                tela.DataContext = treinador;
                tela.Title = "Editar Treinador";
                tela.ShowDialog();
                if(tela.DialogResult == true)
                {
                    this.treinadorSelecionado.CopyFrom(treinador);
                    MessageBox.Show("Informações Atualizadas!", "", MessageBoxButton.OK, MessageBoxImage.Information);
                    Notify("PokemonSelecionado");
                }
            });
            Remove = new RelayCommand((object _) => {
                listaDeTreinadores.Remove(TreinadorSelecionado);
            });
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void Notify([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public Treinador TreinadorSelecionado
        {
            get { return treinadorSelecionado; }
            set
            {
                treinadorSelecionado = value;
                Notify();
            }
        }
        public Pokemon PokemonSelecionado
        {
            get { return pokemonSelecionado; }
            set
            {
                if (value == null) return;
                pokemonSelecionado = value;
                Console.WriteLine("Troquei para o pokemon: " + value.Name);
                if(value.SpriteFront == null)
                {
                    PokeApi.ApplyPokemonAPIInfo(value.Name, pokemonSelecionado);
                }
                Notify();
            }
        }  
    }
}
