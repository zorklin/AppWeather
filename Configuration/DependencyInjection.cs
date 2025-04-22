using Microsoft.Extensions.DependencyInjection;
using AppWeather.Presentation.ViewModels;
using AppWeather.Presentation.Views;
using AppWeather.Infrastructure;
using Microsoft.EntityFrameworkCore;
using AppWeather.Services.Implementations;
using AppWeather.Services.Interfaces;

namespace AppWeather
{
    public static class DependencyInjection
    {
        public static IServiceCollection ConfigureServices(IServiceCollection services)
        {
            // absolute NOT safely, NEVER use in production
            // i use it only becouse its simple and
            // i dont want to create server conection
            var connectionString = "Server=localhost;Database=WEATHER_DB;Uid=root;Pwd=root;Port=3307;Connection Timeout=10;";
            //var connectionString = "Server = localhost; Database = weather_db; Uid = root; Pwd = root; Port = 3307";

            // Register DbContext and database services
            services.AddDbContext<WeatherDbContext>(options =>
                options.UseMySQL(connectionString));
            
            services.AddTransient<IWeatherService, WeatherService>();
            services.AddTransient<IAdminService, AdminService>();

            // Register services
            services.AddSingleton<IMessageService, MessageService>();
            services.AddSingleton<INavigationService, NavigationService>();
            services.AddSingleton<IFileDialogService, FileDialogService>();
            services.AddSingleton<IWeatherMapper, WeatherMapper>();
            services.AddSingleton<IExporter, DocxExporter>();

            // Register windows
            services.AddTransient<MainWindow>();
            services.AddTransient<AuthorizationWindow>();
            services.AddTransient<AddDataWindow>();
            services.AddTransient<ChangeDataWindow>();
            services.AddTransient<DeleteDataWindow>();
            services.AddTransient<FiltrationWindow>();

            // Register ViewModels
            services.AddSingleton<MainViewModel>();
            services.AddTransient<AuthorizationViewModel>();
            services.AddTransient<AddDataViewModel>();
            services.AddTransient<ChangeDataViewModel>();
            services.AddTransient<DeleteDataViewModel>();
            services.AddTransient<FiltrationViewModel>();

            return services;
        }
    }
}
