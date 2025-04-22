using AppWeather.Models;

namespace AppWeather.Services.Interfaces
{
    public interface IWeatherMapper
    {
        WeatherGui MapToViewModel(Weather forecast);
        Weather MapToModel(WeatherGui viewModel);
    }
}
