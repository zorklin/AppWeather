namespace AppWeather.Models
{
    public class WeatherFilter
    {
        public DateOnly? StartDate { get; set; } = null;
        public DateOnly? EndDate { get; set; } = null;
        public float? MinPressure { get; set; } = null;
        public float? MaxPressure { get; set; } = null;
        public bool? Precipitation { get; set; } = null;
        public float? MinTemperature { get; set; } = null;
        public float? MaxTemperature { get; set; } = null;
    }
}
