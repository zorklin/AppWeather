using AppWeather.Presentation.ViewModels;
using System.Windows;

namespace AppWeather.Presentation.Views
{
    public partial class ChangeDataWindow : Window
    {
        public ChangeDataWindow(ChangeDataViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
