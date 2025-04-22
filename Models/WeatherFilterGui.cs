namespace AppWeather.Models
{
    public class WeatherFilterGui
    {
        public DateTime? StartDate { get; set; } = null;
        public DateTime? EndDate { get; set; } = null;
        public float? MinPressure { get; set; } = null;
        public float? MaxPressure { get; set; } = null;
        public string? Precipitation { get; set; } = null;
        public float? MinTemperature { get; set; } = null;
        public float? MaxTemperature { get; set; } = null;
    }
}
