using System.Windows;
using System.Windows.Input;
using AppWeather.Presentation.ViewModels;

namespace AppWeather.Presentation.Views
{
    public partial class AuthorizationWindow : Window
    {
        public AuthorizationWindow(AuthorizationViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter) return;

            if (sender == UsernameBox)
                PasswordBox.Focus();
            else if (sender == PasswordBox)
                ((AuthorizationViewModel)DataContext).LoginCommand.Execute(null);
        }

        private void OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            ((AuthorizationViewModel)DataContext).Password = PasswordBox.Password;
        }
    }
}
