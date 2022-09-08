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
    /// Lógica interna para EditarTreinador.xaml
    /// </summary>
    public partial class EditarTreinador : Window
    {
        public EditarTreinador()
        {
            InitializeComponent();
        }

        public void SaveBtn(object sender, RoutedEventArgs e) 
        {
            DialogResult = true;
        }
    }
}
