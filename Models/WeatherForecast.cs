namespace AppWeather.Models
{
    public class WeatherForecast
    {
        public DateOnly? Date { get; set; }
        public float? Temperature { get; set; }
        public bool? Precipitation { get; set; }
        public float? Pressure { get; set; }
    }
}