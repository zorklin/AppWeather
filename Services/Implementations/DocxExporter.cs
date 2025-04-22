// OpenXML SDK + Extensions

using AppWeather.Models;
using Xceed.Words.NET;
using Xceed.Document.NET;
using AppWeather.Services.Interfaces;

namespace AppWeather.Services.Implementations
{
    public class DocxExporter : IExporter
    {
        private readonly IWeatherMapper _mapper;
        public DocxExporter(IWeatherMapper mapper)
        {
            _mapper = mapper;
        }
        public void Export(List<WeatherGui> viewModelData, string filePath, float avgTemp, float avgPressure)
        {
            using var doc = DocX.Create(filePath);
            doc.InsertParagraph("Weather").FontSize(16).Bold();

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