using System.Windows;
using AppWeather.Presentation.ViewModels;

namespace AppWeather.Presentation.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow(MainViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
