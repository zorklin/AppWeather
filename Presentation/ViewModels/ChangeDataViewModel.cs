using AppWeather.Models;
using AppWeather.Services.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AppWeather.Presentation.ViewModels;

public partial class ChangeDataViewModel : ObservableObject
{
    private readonly IWeatherService _weatherService;
    private readonly IMessageService _messageService;
    private readonly IInputParserService _parserService;
    private readonly MainViewModel _mainViewModel;

    [ObservableProperty] private string date = "";
    [ObservableProperty] private string temperature = "";
    [ObservableProperty] private string pressure = "";
    [ObservableProperty] private string precipitation = "";

    [ObservableProperty] private string? dateError;
    [ObservableProperty] private string? temperatureError;
    [ObservableProperty] private string? pressureError;
    [ObservableProperty] private string? precipitationError;

    public ChangeDataViewModel(
        IWeatherService weatherService,
        IMessageService messageService,
        IInputParserService parserService,
        MainViewModel mainViewModel)
    {
        _weatherService = weatherService;
        _messageService = messageService;
        _parserService = parserService;
        _mainViewModel = mainViewModel;
    }

    [RelayCommand]
    private async Task ChangeAsync()
    {
        if (!_parserService.TryParseRequiredDate(Date, out var parsedDate, out var dateErr))
        {
            _messageService.ShowMessage(dateErr!, "Помилка в полі дати");
            return;
        }

        _parserService.TryParseNullableFloat(Temperature, out var parsedTemp, out var tempErr);
        _parserService.TryParseNullableFloat(Pressure, out var parsedPressure, out var pressureErr);
        _parserService.TryParseNullableBool(Precipitation, out var parsedPrecip, out var precipErr);

        if (parsedTemp is null && parsedPressure is null && parsedPrecip is null)
        {
            _messageService.ShowMessage("Вкажіть хоча б одне з полів: температура, тиск або опади.", "Помилка");
            return;
        }

        if (!string.IsNullOrWhiteSpace(tempErr) || !string.IsNullOrWhiteSpace(pressureErr) || !string.IsNullOrWhiteSpace(precipErr))
        {
            var error = tempErr ?? pressureErr ?? precipErr;
            _messageService.ShowMessage(error!, "Помилка у введених значеннях");
            return;
        }

        var weather = new Weather
        {
            Weather_Date = parsedDate,
            Temperature = parsedTemp,
            Pressure = parsedPressure,
            Precipitation = parsedPrecip
        };

        var updated = await _weatherService.UpdateAsync(weather);
        if (!updated)
        {
            _messageService.ShowMessage("Дані на цю дату не знайдено.", "Помилка");
            return;
        }

        _messageService.ShowMessage("Дані успішно оновлено!", "Успіх");
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
