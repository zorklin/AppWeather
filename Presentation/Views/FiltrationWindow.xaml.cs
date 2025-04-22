using System.Windows;
using AppWeather.Presentation.ViewModels;

namespace AppWeather.Presentation.Views
{
    public partial class FiltrationWindow : Window
    {
        public FiltrationWindow(FiltrationViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
