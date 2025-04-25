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
        string? dateErr = null;
        string? tempErr = null;
        string? pressureErr = null;
        string? precipErr = null;

        if (!_parserService.TryParseDate(Date, out var parsedDate, out dateErr) ||
            !_parserService.TryParseFloat(Temperature, out var parsedTemp, out tempErr) ||
            !_parserService.TryParseFloat(Pressure, out var parsedPressure, out pressureErr) ||
            !_parserService.TryParseBool(Precipitation, out var parsedPrecip, out precipErr))
        {
            _messageService.ShowMessage(dateErr ?? tempErr ?? pressureErr ?? precipErr!, "Помилка");
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
