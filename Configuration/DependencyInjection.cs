using Microsoft.Extensions.DependencyInjection;
using AppWeather.Services;
using AppWeather.Utilities;
using AppWeather.Presentation.ViewModels;
using AppWeather.Presentation.Views;

namespace AppWeather
{
    public static class DependencyInjection
    {
        public static IServiceCollection ConfigureServices(IServiceCollection services)
        {
            // Register services
            services.AddSingleton<IMessageService, MessageService>();
            services.AddSingleton<INavigationService, NavigationService>();
            services.AddSingleton<IWeatherMapper, WeatherMapper>();
            services.AddSingleton<DatabaseService, DatabaseService>();
            services.AddSingleton<IExporter, DocxExporter>();

            // Register windows
            services.AddTransient<MainWindow>();
            services.AddTransient<AuthorizationWindow>();
            services.AddTransient<AddDataWindow>();
            services.AddTransient<ChangeDataWindow>();
            services.AddTransient<DeleteDataWindow>();
            services.AddTransient<FiltrationWindow>();

            // Register ViewModels
            services.AddTransient<MainViewModel>();
            services.AddTransient<AuthorizationViewModel>();
            services.AddTransient<AddDataViewModel>();
            services.AddTransient<ChangeDataViewModel>();
            services.AddTransient<DeleteDataViewModel>();
            services.AddTransient<FiltrationViewModel>();

            return services;
        }
    }
}
