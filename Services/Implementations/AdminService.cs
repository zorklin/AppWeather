using AppWeather.Infrastructure;
using AppWeather.Models;
using AppWeather.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AppWeather.Services.Implementations
{
    public class AdminService : IAdminService
    {
        private readonly WeatherDbContext _context;

        public AdminService(WeatherDbContext context)
        {
            _context = context;
        }

        public async Task<bool> IsAdminValidAsync(Admin admin)
        {
            return await _context.Admins
                .AnyAsync(a => a.Username == admin.Username && a.Password == admin.Password);
        }
    }
}
