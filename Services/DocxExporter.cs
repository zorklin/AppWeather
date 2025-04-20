using AppWeather.Models;
using Xceed.Words.NET;
using Xceed.Document.NET;

namespace AppWeather.Services
{
    public interface IExporter
    {
        void Export(List<WeatherForecast> data, string filePath);
    }
    public class DocxExporter : IExporter
    {
        private readonly IWeatherMapper _mapper;
        public DocxExporter(IWeatherMapper mapper)
        {
            _mapper = mapper;
        }
        public void Export(List<WeatherForecast> data, string filePath)
        {
            var avgTemp = data.Where(f => f.Temperature.HasValue).Average(f => f.Temperature) ?? 0;
            var avgPressure = data.Where(f => f.Pressure.HasValue).Average(f => f.Pressure) ?? 0;

            var viewModelData = data.Select(forecast => _mapper.MapToViewModel(forecast)).ToList();

            using var doc = DocX.Create(filePath);
            doc.InsertParagraph("Weather Forecast").FontSize(16).Bold();

            var table = doc.AddTable(viewModelData.Count + 1, 5);
            table.Design = TableDesign.MediumGrid1Accent2;

            table.Rows[0].Cells[0].Paragraphs[0].Append("#");
            table.Rows[0].Cells[1].Paragraphs[0].Append("Date");
            table.Rows[0].Cells[2].Paragraphs[0].Append("Temperature");
            table.Rows[0].Cells[3].Paragraphs[0].Append("Precipitation");
            table.Rows[0].Cells[4].Paragraphs[0].Append("Pressure");

            for (int i = 0; i < viewModelData.Count; i++)
            {
                var forecast = viewModelData[i];
                table.Rows[i + 1].Cells[0].Paragraphs[0].Append((i + 1).ToString());
                table.Rows[i + 1].Cells[1].Paragraphs[0].Append(forecast.Date ?? "");
                table.Rows[i + 1].Cells[2].Paragraphs[0].Append(forecast.Temperature ?? "");
                table.Rows[i + 1].Cells[3].Paragraphs[0].Append(forecast.Precipitation ?? "");
                table.Rows[i + 1].Cells[4].Paragraphs[0].Append(forecast.Pressure ?? "");
            }

            doc.InsertParagraph($"Average Temperature: {avgTemp:F1} °C");
            doc.InsertParagraph($"Average Pressure: {avgPressure:F0} mmHg");

            doc.Save();
        }
    }
}