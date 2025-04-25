using AppWeather.Presentation.ViewModels;
using System.Windows;

namespace AppWeather.Presentation.Views
{
    public partial class DeleteDataWindow : Window
    {
        public DeleteDataWindow(DeleteDataViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
