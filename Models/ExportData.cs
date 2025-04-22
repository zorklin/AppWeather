namespace AppWeather.Models
{
    public class ExportData
    {
        public string Title { get; set; } = string.Empty;
        public List<string> Headers { get; set; } = [];
        public List<List<string>> Rows { get; set; } = [];
        public List<string> AdditionalValues { get; set; } = [];
    }
}