using AppWeather.Common;
using AppWeather.Models;
using AppWeather.Services.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

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
            if (!_parserService.TryParseRequiredDate(Date, out var parsedDate, out var dateErr))
            {
                _messageService.ShowMessage(dateErr!, "Помилка у даті");
                return;
            }

            _parserService.TryParseNullableFloat(Temperature, out var parsedTemp, out var tempErr);
            _parserService.TryParseNullableFloat(Pressure, out var parsedPressure, out var pressureErr);
            _parserService.TryParseNullableBool(Precipitation, out var parsedPrecipitation, out var precipErr);

            if (parsedTemp is null && parsedPressure is null && parsedPrecipitation is null)
            {
                _messageService.ShowMessage("Заповніть хоча б одне поле: температура, тиск або опади.", "Помилка");
                return;
            }

            if (!string.IsNullOrWhiteSpace(tempErr) ||!string.IsNullOrWhiteSpace(pressureErr) || !string.IsNullOrWhiteSpace(precipErr))
            {
                _messageService.ShowMessage(tempErr ?? pressureErr ?? precipErr!, "Помилка у введенні");
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
            _parserService.TryParseRequiredDate(value, out _, out var error);
            DateError = error;
        }

        partial void OnTemperatureChanged(string value)
        {
            _parserService.TryParseNullableFloat(value, out _, out var error);
            TemperatureError = error;
        }

        partial void OnPressureChanged(string value)
        {
            _parserService.TryParseNullableFloat(value, out _, out var error);
            PressureError = error;
        }

        partial void OnPrecipitationChanged(string value)
        {
            _parserService.TryParseNullableBool(value, out _, out var error);
            PrecipitationError = error;
        }
    }
}
