using AppWeather.Models;

namespace AppWeather.Services.Interfaces
{
    public interface IExporter
    {
        void Export(List<WeatherGui> viewModelData, string filePath, float avgTemp, float avgPressure);
    }
}
