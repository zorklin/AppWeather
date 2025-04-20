using System.Windows;

namespace AppWeather.Services
{
    public interface IMessageService
    {
        void ShowMessage(string message, string title);
    }

    class MessageService : IMessageService
    {
        public void ShowMessage(string message, string title)
        {
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
