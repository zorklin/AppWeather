using AppWeather.Presentation.Views;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace AppWeather
{
    public partial class App : Application
    {
        public static IServiceProvider ServiceProvider { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var serviceCollection = new ServiceCollection();
            ServiceProvider = DependencyInjection.ConfigureServices(serviceCollection).BuildServiceProvider();

            var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }
    }
}
