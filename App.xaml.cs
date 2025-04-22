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
            try
            {
                base.OnStartup(e);

                var serviceCollection = new ServiceCollection();
                ServiceProvider = DependencyInjection.ConfigureServices(serviceCollection).BuildServiceProvider();
                var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
                mainWindow.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred during startup: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown();
            }
        }
    }
}
