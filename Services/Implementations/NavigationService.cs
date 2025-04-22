using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using AppWeather.Services.Interfaces;

namespace AppWeather.Services.Implementations
{
    public class NavigationService : INavigationService
    {
        private readonly IServiceProvider _serviceProvider;

        public NavigationService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void NavigateTo<TWindow>() where TWindow : Window
        {
            var window = _serviceProvider.GetRequiredService<TWindow>();
            window.ShowDialog();
        }
    }
}
