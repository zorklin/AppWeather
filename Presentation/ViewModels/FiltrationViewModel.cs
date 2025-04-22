using AppWeather.Common;
using AppWeather.Models;
using AppWeather.Services.Interfaces;
using AppWeather.Services.Implementations;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using AppWeather.Presentation.Views;
using DocumentFormat.OpenXml.Wordprocessing;

namespace AppWeather.Presentation.ViewModels
{
    public class FiltrationViewModel : BaseViewModel
    {
        private readonly IWeatherService _weatherService;
        private readonly IMessageService _messageService;
        private readonly IWeatherMapper _weatherMapper;
        private readonly INavigationService _navigationService;
        private readonly MainViewModel _mainViewModel;

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
            FilterCommand = new RelayCommand(async _ => await FilterAsync());
        }
        public WeatherFilterGui FilterGui { get; set; } = new WeatherFilterGui();

        public RelayCommand FilterCommand { get; }

        private async Task FilterAsync()
            {
            var filter = new WeatherFilter();

            try
            {
                filter.StartDate = FilterGui.StartDate.HasValue ? DateOnly.FromDateTime(FilterGui.StartDate.Value) : null;
                filter.EndDate = FilterGui.EndDate.HasValue ? DateOnly.FromDateTime(FilterGui.EndDate.Value) : null;

                filter.MinTemperature = FilterGui.MinTemperature.HasValue ? FilterGui.MinTemperature.Value : null;
                filter.MaxTemperature = FilterGui.MaxTemperature.HasValue ? FilterGui.MaxTemperature.Value : null;

                filter.Precipitation = BoolParser.ParseNullableBool(FilterGui.Precipitation);

                filter.MinPressure = FilterGui.MinPressure.HasValue ? FilterGui.MinPressure.Value : null;
                filter.MaxPressure = FilterGui.MaxPressure.HasValue ? FilterGui.MaxPressure.Value : null;
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
