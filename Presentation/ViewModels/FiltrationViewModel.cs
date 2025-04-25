using AppWeather.Common;
using AppWeather.Services.Interfaces;
using AppWeather.Presentation.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AppWeather.Models;
using DocumentFormat.OpenXml.Wordprocessing;

namespace AppWeather.Presentation.ViewModels;

public partial class FiltrationViewModel : ObservableObject
{
    private readonly IWeatherService _weatherService;
    private readonly IMessageService _messageService;
    private readonly IWeatherMapper _weatherMapper;
    private readonly INavigationService _navigationService;
    private readonly MainViewModel _mainViewModel;
    private readonly IInputParserService _parserService;

    public FiltrationViewModel(
        IWeatherService weatherService,
        IMessageService messageService,
        IWeatherMapper weatherMapper,
        INavigationService navigationService,
        MainViewModel mainViewModel,
        IInputParserService parserService)
    {
        _weatherService = weatherService;
        _messageService = messageService;
        _weatherMapper = weatherMapper;
        _navigationService = navigationService;
        _mainViewModel = mainViewModel;
        _parserService = parserService;
    }

    [ObservableProperty]
    private string startDate = "";

    [ObservableProperty]
    private string endDate = "";

    [ObservableProperty]
    private string minTemperature = "";

    [ObservableProperty]
    private string maxTemperature = "";

    [ObservableProperty]
    private string precipitation = "";

    [ObservableProperty]
    private string minPressure = "";

    [ObservableProperty]
    private string maxPressure = "";

    [ObservableProperty] private string? startDateError = "";
    [ObservableProperty] private string? endDateError = "";
    [ObservableProperty] private string? minTemperatureError = "";
    [ObservableProperty] private string? maxTemperatureError = "";
    [ObservableProperty] private string? precipitationError = "";
    [ObservableProperty] private string? minPressureError = "";
    [ObservableProperty] private string? maxPressureError = "";

    partial void OnStartDateChanged(string value)
    {
        _parserService.TryParseNullableDate(value, out _, out var error);
        StartDateError = error;
    }

    partial void OnEndDateChanged(string value)
    {
        _parserService.TryParseNullableDate(value, out _, out var error);
        EndDateError = error;
    }

    partial void OnMinTemperatureChanged(string value)
    {
        _parserService.TryParseNullableFloat(value, out _, out var error);
        MinTemperatureError = error;
    }

    partial void OnMaxTemperatureChanged(string value)
    {
        _parserService.TryParseNullableFloat(value, out _, out var error);
        MaxTemperatureError = error;
    }

    partial void OnPrecipitationChanged(string value)
    {
        _parserService.TryParseNullableBool(value, out _, out var error);
        PrecipitationError = error;
    }

    partial void OnMinPressureChanged(string value)
    {
        _parserService.TryParseNullableFloat(value, out _, out var error);
        MinPressureError = error;
    }

    partial void OnMaxPressureChanged(string value)
    {
        _parserService.TryParseNullableFloat(value, out _, out var error);
        MaxPressureError = error;
    }

    [RelayCommand]
    private async Task Filter()
    {
        if (!_parserService.TryParseNullableDate(StartDate, out var start, out var error))
        {
            _messageService.ShowMessage(error!, "Помилка у початковій даті");
            return;
        }

        if (!_parserService.TryParseNullableDate(EndDate, out var end, out error))
        {
            _messageService.ShowMessage(error!, "Помилка у кінцевій даті");
            return;
        }

        if (!_parserService.TryParseNullableFloat(MinTemperature, out var minTemp, out error))
        {
            _messageService.ShowMessage(error!, "Помилка у мінімальній температурі");
            return;
        }

        if (!_parserService.TryParseNullableFloat(MaxTemperature, out var maxTemp, out error))
        {
            _messageService.ShowMessage(error!, "Помилка у максимальній температурі");
            return;
        }

        if (!_parserService.TryParseNullableBool(Precipitation, out var rain, out error))
        {
            _messageService.ShowMessage(error!, "Помилка у полі опади");
            return;
        }

        if (!_parserService.TryParseNullableFloat(MinPressure, out var minPres, out error))
        {
            _messageService.ShowMessage(error!, "Помилка у мінімальному тиску");
            return;
        }

        if (!_parserService.TryParseNullableFloat(MaxPressure, out var maxPres, out error))
        {
            _messageService.ShowMessage(error!, "Помилка у максимальному тиску");
            return;
        }

        if (start is null && end is null && minTemp is null && maxTemp is null &&
            rain is null && minPres is null && maxPres is null)
        {
            _messageService.ShowMessage("Введіть хоча б один параметр для фільтрації.", "Попередження");
            return;
        }

        var filter = new WeatherFilter
        {
            StartDate = start,
            EndDate = end,
            MinTemperature = minTemp,
            MaxTemperature = maxTemp,
            Precipitation = rain,
            MinPressure = minPres,
            MaxPressure = maxPres
        };

        var results = await _weatherService.FilterAsync(filter);

        if (results.Count > 0)
        {
            _mainViewModel.WeatherForecasts.Clear();
            foreach (var weather in results)
                _mainViewModel.WeatherForecasts.Add(_weatherMapper.MapToViewModel(weather));

            _navigationService.NavigateBack<FiltrationWindow>();
        }
        else
        {
            _messageService.ShowMessage("За даними критеріями нічого не знайдено.", "Результат");
        }
    }
}
