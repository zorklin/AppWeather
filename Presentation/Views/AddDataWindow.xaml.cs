using AppWeather.Presentation.ViewModels;
using System.Windows;

namespace AppWeather.Presentation.Views
{
    public partial class AddDataWindow : Window
    {
        public AddDataWindow(AddDataViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
