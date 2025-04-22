using AppWeather.Infrastructure;
using AppWeather.Models;
using AppWeather.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AppWeather.Services.Implementations
{
    public class WeatherService : IWeatherService
    {
        private readonly WeatherDbContext _context;

        public WeatherService(WeatherDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CheckConnectionAsync()
        {
            try
            {
                return await _context.Database.CanConnectAsync();
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<Weather>> GetAllAsync() =>
            await _context.Weather.ToListAsync();

        public async Task<bool> AddAsync(Weather weather)
        {
            var exists = await _context.Weather.AnyAsync(w => w.Weather_Date == weather.Weather_Date);
            if (exists) return false;

            _context.Weather.Add(weather);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAsync(Weather weather)
        {
            var existing = await _context.Weather.FirstOrDefaultAsync(w => w.Weather_Date == weather.Weather_Date);
            if (existing == null) return false;

            existing.Temperature = weather.Temperature;
            existing.Precipitation = weather.Precipitation;
            existing.Pressure = weather.Pressure;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(DateOnly date)
        {
            var existing = await _context.Weather.FirstOrDefaultAsync(w => w.Weather_Date == date);
            if (existing == null) return false;

            _context.Weather.Remove(existing);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Weather>> FilterAsync(WeatherFilter filter)
        {
            var query = _context.Weather.AsQueryable();

            if (filter.StartDate.HasValue)
                query = query.Where(w => w.Weather_Date >= filter.StartDate.Value);

            if (filter.EndDate.HasValue)
                query = query.Where(w => w.Weather_Date <= filter.EndDate);

            if (filter.MinTemperature.HasValue)
                query = query.Where(w => w.Temperature >= filter.MinTemperature);

            if (filter.MaxTemperature.HasValue)
                query = query.Where(w => w.Temperature <= filter.MaxTemperature);

            if (filter.MinPressure.HasValue)
                query = query.Where(w => w.Pressure >= filter.MinPressure);

            if (filter.MaxPressure.HasValue)
                query = query.Where(w => w.Pressure <= filter.MaxPressure);

            if (filter.Precipitation.HasValue)
                query = query.Where(w => w.Precipitation == filter.Precipitation);

            var sql = query.ToQueryString();
            return await query.ToListAsync();
        }
    }
}
