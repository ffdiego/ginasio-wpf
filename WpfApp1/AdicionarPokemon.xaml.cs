using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Lógica interna para AdicionarPokemon.xaml
    /// </summary>
    public partial class AdicionarPokemon : Window
    {
        public Pokemon Pokemon { get; set; }
        public AdicionarPokemon()
        {
            InitializeComponent();
            Pokemon = new Pokemon();
            DataContext = Pokemon;
        }
        public AdicionarPokemon(Pokemon pokemon)
        {
            InitializeComponent();
            Pokemon = pokemon;
            DataContext = pokemon;
        }

        private void FetchAPI_Btn(object sender, RoutedEventArgs e)
        {
            Pokemon.FetchAPIData();
        }
        private void Ok_Btn(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
        private void Cancel_Btn(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
