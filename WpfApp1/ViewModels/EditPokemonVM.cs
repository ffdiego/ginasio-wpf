using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Db;
using WpfApp1.Views;

namespace WpfApp1.ViewModels
{
    public class EditPokemonVM
    {
        private EditPokemon screen;
        private Pokemon editPokemon;
        private Trainer owner;
        public string Name { get; set; }
        public Pokemon EditPokemon { get { return editPokemon; } set { editPokemon = value; } }
        public string ErrorMSG { get; private set; }
        public RelayCommand Fetch { get; private set; }
        public RelayCommand OK { get; private set; }
        public RelayCommand Cancel { get; private set; }

        private EditPokemonVM()
        {
            
        }
        public EditPokemonVM(Trainer treinador)
        {
            owner = treinador;
            editPokemon = new Pokemon();
        }
        public void ShowWindow()
        {
            InitializeCommands();
            screen = new EditPokemon();
            screen.DataContext = this;
            screen.ShowDialog();
            if (screen.DialogResult == true)
                owner.AttachPokemon(EditPokemon);
        }
        private void InitializeCommands()
        {
            Fetch = new RelayCommand(async (object _) =>
            {
                Pokemon result;
                (result, ErrorMSG) = await PokeApi.GetPokemonAPIInfo(Name);
                if(result != null)
                    EditPokemon.CopyFrom(result);

            });
            OK = new RelayCommand((object _) =>
            {
                screen.DialogResult = true;
            });
            Cancel = new RelayCommand((object _) =>
            {
                screen.DialogResult = false;
            });
        }
        
    }
}
