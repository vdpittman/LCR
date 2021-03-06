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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LCR
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Simulation _viewModel;

        /// <summary>
        /// Initializes an object of this class
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            DataContext = _viewModel = new Simulation();
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.RunSimulation();
        }
    }
}
