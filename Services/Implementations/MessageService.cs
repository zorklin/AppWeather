using System.Windows;
using AppWeather.Services.Interfaces;

namespace AppWeather.Services.Implementations
{
    class MessageService : IMessageService
    {
        public void ShowMessage(string message, string title)
        {
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
