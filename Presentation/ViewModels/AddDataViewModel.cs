using AppWeather.Common;
using AppWeather.Models;
using AppWeather.Services.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Threading.Tasks;

namespace AppWeather.Presentation.ViewModels
{
    public partial class AddDataViewModel : ObservableObject
    {
        private readonly IWeatherService _weatherService;
        private readonly IMessageService _messageService;
        private readonly IInputParserService _parserService;
        private readonly MainViewModel _mainViewModel;

        [ObservableProperty] private string date = "";
        [ObservableProperty] private string temperature = "";
        [ObservableProperty] private string precipitation = "";
        [ObservableProperty] private string pressure = "";

        [ObservableProperty] private string? dateError;
        [ObservableProperty] private string? temperatureError;
        [ObservableProperty] private string? pressureError;
        [ObservableProperty] private string? precipitationError;

        public AddDataViewModel(
            IWeatherService weatherService,
            IMessageService messageService,
            MainViewModel mainViewModel,
            IInputParserService parserService)
        {
            _weatherService = weatherService;
            _messageService = messageService;
            _mainViewModel = mainViewModel;
            _parserService = parserService;
        }

        [RelayCommand]
        private async Task AddAsync()
        {
            if (string.IsNullOrWhiteSpace(Date) ||
                (string.IsNullOrWhiteSpace(Temperature)
                && string.IsNullOrWhiteSpace(Precipitation)
                && string.IsNullOrWhiteSpace(Pressure)))
            {
                _messageService.ShowMessage("Заповніть дату та хоча б одне поле з даними.", "Помилка");
                return;
            }

            string? dateErr = null;
            string? tempErr = null;
            string? pressureErr = null;
            string? precipErr = null;

            if (!_parserService.TryParseDate(Date, out var parsedDate, out dateErr) ||
                !_parserService.TryParseFloat(Temperature, out var parsedTemp, out tempErr) ||
                !_parserService.TryParseFloat(Pressure, out var parsedPressure, out pressureErr) ||
                !_parserService.TryParseBool(Precipitation, out var parsedPrecipitation, out precipErr))
            {
                _messageService.ShowMessage(dateErr ?? tempErr ?? pressureErr ?? precipErr!, "Помилка");
                return;
            }

            var weather = new Weather
            {
                Id = Guid.NewGuid(),
                Weather_Date = parsedDate,
                Temperature = parsedTemp,
                Pressure = parsedPressure,
                Precipitation = parsedPrecipitation
            };

            var added = await _weatherService.AddAsync(weather);
            if (!added)
            {
                _messageService.ShowMessage("Дані на цю дату вже існують.", "Помилка");
                return;
            }

            _messageService.ShowMessage("Дані успішно додано!", "Успіх");
            _mainViewModel.FetchFromServerCommand.Execute(null);
        }

        partial void OnDateChanged(string value)
        {
            _parserService.TryParseDate(value, out _, out var error);
            DateError = error;
        }

        partial void OnTemperatureChanged(string value)
        {
            _parserService.TryParseFloat(value, out _, out var error);
            TemperatureError = error;
        }

        partial void OnPressureChanged(string value)
        {
            _parserService.TryParseFloat(value, out _, out var error);
            PressureError = error;
        }

        partial void OnPrecipitationChanged(string value)
        {
            _parserService.TryParseBool(value, out _, out var error);
            PrecipitationError = error;
        }
    }
}
