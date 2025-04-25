using AppWeather.Common;
using AppWeather.Models;
using AppWeather.Services.Interfaces;
using AppWeather.Presentation.Views;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AppWeather.Presentation.ViewModels
{
    public partial class FiltrationViewModel : ObservableRecipient
    {
        private readonly IWeatherService _weatherService;
        private readonly IMessageService _messageService;
        private readonly IWeatherMapper _weatherMapper;
        private readonly INavigationService _navigationService;
        private readonly MainViewModel _mainViewModel;
        public WeatherFilterGui FilterGui { get; set; } = new WeatherFilterGui();

        public FiltrationViewModel(
            IWeatherService weatherService,
            IMessageService messageService,
            IWeatherMapper weatherMapper,
            INavigationService navigationService,
            MainViewModel mainViewModel)
        {
            _weatherService = weatherService ?? throw new ArgumentNullException(nameof(weatherService));
            _messageService = messageService ?? throw new ArgumentNullException(nameof(messageService));
            _weatherMapper = weatherMapper ?? throw new ArgumentNullException(nameof(weatherMapper));
            _mainViewModel = mainViewModel ?? throw new ArgumentNullException(nameof(mainViewModel));
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));

        }

        [RelayCommand]
        private async Task Filter()
        {
            var filter = new WeatherFilter();

            try
            {
                filter.StartDate = FilterGui.StartDate is DateTime start ? DateOnly.FromDateTime(start) : null;
                filter.EndDate = FilterGui.EndDate is DateTime end ? DateOnly.FromDateTime(end) : null;

                filter.MinTemperature = FilterGui.MinTemperature;
                filter.MaxTemperature = FilterGui.MaxTemperature;

                filter.Precipitation = BoolParser.ParseNullableBool(FilterGui.Precipitation);

                filter.MinPressure = FilterGui.MinPressure;
                filter.MaxPressure = FilterGui.MaxPressure;
            }
            catch (FormatException ex)
            {
                _messageService.ShowMessage(ex.Message, "Помилка фільтрації");
                return;
            }

            var results = await _weatherService.FilterAsync(filter);

            if (results.Count > 0)
            {
                _mainViewModel.WeatherForecasts.Clear();
                foreach (var weather in results)
                {
                    var weatherGui = _weatherMapper.MapToViewModel(weather);
                    _mainViewModel.WeatherForecasts.Add(weatherGui);
                }
                _navigationService.NavigateBack<FiltrationWindow>();
            }
            else
            {
                _messageService.ShowMessage("За даними критеріями нічого не знайдено.", "Помилка");
            }
        }
    }
}