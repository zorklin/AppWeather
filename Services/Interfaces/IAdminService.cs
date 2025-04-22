using AppWeather.Models;

namespace AppWeather.Services.Interfaces
{
    public interface IAdminService
    {
        Task<bool> IsAdminValidAsync(Admin admin);
    }
}
