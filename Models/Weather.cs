namespace AppWeather.Models
{
    public class Weather
    {
        public Guid Id { get; set; }
        public DateOnly Date { get; set; }
        public float? Temperature { get; set; }
        public bool? Precipitation { get; set; }
        public float? Pressure { get; set; }
    }
}
