using AppWeather.Models;

namespace AppWeather.Services.Interfaces
{
    public interface IExporter
    {
        void Export(ExportData data, string filePath);
    }
}
