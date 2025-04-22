using AppWeather.Models;

namespace AppWeather.Services.Interfaces
{
    public interface IWeatherService
    {
        Task<bool> CheckConnectionAsync();
        Task<List<Weather>> GetAllAsync();
        Task<bool> AddAsync(Weather weather);
        Task<bool> UpdateAsync(Weather weather);
        Task<bool> DeleteAsync(DateOnly date);
        Task<List<Weather>> FilterAsync(WeatherFilter filter);
    }
}
