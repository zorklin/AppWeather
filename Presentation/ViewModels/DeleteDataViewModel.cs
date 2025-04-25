using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AppWeather.Services.Interfaces;

namespace AppWeather.Presentation.ViewModels
{
    public partial class DeleteDataViewModel : ObservableObject
    {
        private readonly IWeatherService _weatherService;
        private readonly IMessageService _messageService;
        private readonly IInputParserService _parserService;
        private readonly MainViewModel _mainViewModel;

        [ObservableProperty]
        private string date = "";

        [ObservableProperty]
        private string? dateError;

        public DeleteDataViewModel(
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
        private async Task DeleteAsync()
        {
            if (!_parserService.TryParseRequiredDate(Date, out var parsedDate, out var error))
            {
                DateError = error;
                return;
            }

            var success = await _weatherService.DeleteAsync(parsedDate);
            if (!success)
            {
                _messageService.ShowMessage("Даних за цією датою не знайдено.", "Помилка");
                return;
            }

            _mainViewModel.FetchFromServerCommand.Execute(null);
            _messageService.ShowMessage("Дані успішно видалено.", "Інформація");
        }

        partial void OnDateChanged(string value)
        {
            _parserService.TryParseRequiredDate(value, out _, out var error);
            DateError = error;
        }
    }
}
