using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using AppWeather.Common;
using AppWeather.Models;
using AppWeather.Presentation.Views;
using AppWeather.Services.Implementations;
using AppWeather.Services.Interfaces;
using AutoMapper;
using CommunityToolkit.Mvvm;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace AppWeather.Presentation.ViewModels
{
    public partial class MainViewModel : ObservableRecipient
    {
        private readonly IAdminService _adminService;
        private readonly IWeatherService _weatherService;
        private readonly INavigationService _navigationService;
        private readonly IMessageService _messageService;
        private readonly IWeatherMapper _weatherMapper;
        private readonly IExporter _exporter;
        private readonly IFileDialogService _fileDialogService;
        private readonly IUserSessionService _userSessionService;

        [ObservableProperty]
        private ObservableCollection<WeatherGui> _weatherForecasts = new ObservableCollection<WeatherGui>();

        [ObservableProperty]
        private bool isAdmin;

        public MainViewModel(
            IAdminService adminService,
            IWeatherService weatherService,
            INavigationService navigationService,
            IMessageService messageService,
            IWeatherMapper weatherMapper,
            IExporter exporter,
            IFileDialogService fileDialogService,
            IUserSessionService sessionService)
        {
            _weatherService = weatherService ?? throw new ArgumentNullException(nameof(weatherService));
            _adminService = adminService ?? throw new ArgumentNullException(nameof(adminService));
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            _messageService = messageService ?? throw new ArgumentNullException(nameof(messageService));
            _weatherMapper = weatherMapper ?? throw new ArgumentNullException(nameof(weatherMapper));
            _exporter = exporter ?? throw new ArgumentNullException(nameof(exporter));
            _fileDialogService = fileDialogService ?? throw new ArgumentNullException(nameof(fileDialogService));
            _userSessionService = sessionService ?? throw new ArgumentNullException(nameof(sessionService));

            WeatherForecasts = new ObservableCollection<WeatherGui>();

            IsAdmin = _userSessionService.IsAdmin;

            WeakReferenceMessenger.Default.Register<UserSessionChangedMessage>(this, (r, m) =>
            {
                IsAdmin = m.Value.IsAdmin;
            });

            CheckDatabaseConnectionAsync();
        }

        private async void CheckDatabaseConnectionAsync()
        {
            while (true)
            {
                bool isConnected = await _weatherService.CheckConnectionAsync();
                if (isConnected)
                {
                    await FetchFromServer();
                    _messageService.ShowMessage("Підключення до бази даних встановлено.", "Успіх");
                    break;
                }
                else
                {
                    _messageService.ShowMessage("Не вдалося підключитися до бази даних. Натисність щоб спробувати знову.", "Помилка");
                    await Task.Delay(5000);
                }
            }
        }

        [RelayCommand]
        private async Task FetchFromServer()
        {
            try
            {
                var forecasts = await _weatherService.GetAllAsync();
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
                _messageService.ShowMessage($"Помилка при отриманні даних із сервера: {ex.Message}", "Помилка");
            }
        }


        [RelayCommand]
        private void SaveLocally()
        {
            try
            {
                var filePath = _fileDialogService.ShowSaveDialog("Save File", "Word Documents (*.docx)|*.docx", ".docx");
                if (string.IsNullOrEmpty(filePath))
                {
                    _messageService.ShowMessage("Операція збереження скасована.", "Інформація");
                    return;
                }

                var avgTemp = WeatherForecasts
                    .Where(f => float.TryParse(f.Temperature, out _))
                    .Average(f => float.Parse(f.Temperature));

                var avgPressure = WeatherForecasts
                    .Where(f => float.TryParse(f.Pressure, out _))
                    .Average(f => float.Parse(f.Pressure));


                var exportData = new ExportData
                {
                    Title = "Звіт Погоди",
                    Headers = new List<string> { "#", "Дата", "Температура", "Опади", "Тиск" },
                    Rows = WeatherForecasts.Select((forecast, index) => new List<string>
                    {
                        (index + 1).ToString(),
                        forecast.Weather_Date,
                        forecast.Temperature,
                        forecast.Precipitation,
                        forecast.Pressure
                    }).ToList(),
                    AdditionalValues = new List<string>
                    {
                        $"Середня температура: {avgTemp:F1} °C",
                        $"Середній тиск: {avgPressure:F1} mmHg"
                    }
                };

                _exporter.Export(exportData, filePath);
                _messageService.ShowMessage("Дані успішно збережено локально.", "Успіх");
            }
            catch (Exception ex)
            {
                _messageService.ShowMessage($"Помилка при збереженні даних: {ex.Message}", "Помилка");
            }
        }

        [RelayCommand]
        private void OpenSearchWindow()
        {
            _navigationService.NavigateTo<FiltrationWindow>();
        }

        [RelayCommand]
        private void OpenAuthorizationWindow()
        {
            _navigationService.NavigateTo<AuthorizationWindow>();
        }

        [RelayCommand]
        private void AddData()
        {
            _navigationService.NavigateTo<AddDataWindow>();
        }

        [RelayCommand]
        private void ChangeData()
        {
            _navigationService.NavigateTo<ChangeDataWindow>();
        }

        [RelayCommand]
        private void DeleteData()
        {
            _navigationService.NavigateTo<DeleteDataWindow>();
        }
    }
}