using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using AppWeather.Helpers;
using AppWeather.Models;
using AppWeather.Presentation.Views;
using AppWeather.Services;
using AppWeather.Utilities;
using AutoMapper;

namespace AppWeather.Presentation.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly DatabaseService _databaseService;
        private readonly INavigationService _navigationService;
        private readonly IMessageService _messageService;
        private readonly IWeatherMapper _weatherMapper;
        private readonly IExporter _exporter;
        private ObservableCollection<WeatherForecastViewModel> _weatherForecasts = new ObservableCollection<WeatherForecastViewModel>();

        public ICommand FetchFromServerCommand { get; }
        public ICommand SaveLocallyCommand { get; }
        public ICommand SearchCommand { get; }
        public ICommand AuthorizationCommand { get; }

        public MainViewModel(
            INavigationService navigationService,
            IMessageService messageService,
            IWeatherMapper weatherMapper,
            IExporter exporter,
            DatabaseService databaseService)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            _messageService = messageService ?? throw new ArgumentNullException(nameof(messageService));
            _weatherMapper = weatherMapper ?? throw new ArgumentNullException(nameof(weatherMapper));
            _exporter = exporter ?? throw new ArgumentNullException(nameof(exporter));
            _databaseService = databaseService ?? throw new ArgumentNullException(nameof(databaseService));

            WeatherForecasts = new ObservableCollection<WeatherForecastViewModel>();

            FetchFromServerCommand = new RelayCommand(_ => FetchFromServer());
            SaveLocallyCommand = new RelayCommand(_ => SaveLocally());
            SearchCommand = new RelayCommand(_ => OpenSearchWindow());
            AuthorizationCommand = new RelayCommand(_ => OpenAuthorizationWindow());

            CheckDatabaseConnectionAsync();
        }

        public ObservableCollection<WeatherForecastViewModel> WeatherForecasts
        {
            get => _weatherForecasts;
            set
            {
                _weatherForecasts = value;
                //OnPropertyChanged();
            }
        }

        private async void CheckDatabaseConnectionAsync()
        {
            while (true)
            {
                if (_databaseService.IsConnectionAvailable())
                {
                    _messageService.ShowMessage("Підключення до бази даних встановлено.", "Успіх");
                    FetchFromServer();
                    break;
                }
                else
                {
                    _messageService.ShowMessage("Не вдалося підключитися до бази даних. Спроба знову через 10 секунд.", "Помилка");
                    await Task.Delay(10000);
                }
            }
        }

        private void FetchFromServer()
        {
            try
            {
                var forecasts = _databaseService.GetAllForecasts();
                var viewModelForecasts = forecasts.Select(forecast => _weatherMapper.MapToViewModel(forecast)).ToList();

                Application.Current.Dispatcher.Invoke(() =>
                {
                    WeatherForecasts.Clear();
                    foreach (var forecast in viewModelForecasts)
                    {
                        WeatherForecasts.Add(forecast);
                    }
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка при отриманні даних із сервера: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SaveLocally()
        {
            //_exporter.Export(WeatherForecasts.ToList(), "weather_forecast.docx");  
        }

        private void OpenSearchWindow()
        {
            _navigationService.NavigateTo<FiltrationWindow>();
        }

        private void OpenAuthorizationWindow()
        {
            _navigationService.NavigateTo<AuthorizationWindow>();
        }
    }

}
