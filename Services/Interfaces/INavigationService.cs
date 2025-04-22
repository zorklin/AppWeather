using System.Windows;

namespace AppWeather.Services.Interfaces
{
    public interface INavigationService
    {
        void NavigateTo<TWindow>() where TWindow : Window;
    }
}
