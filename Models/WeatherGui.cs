using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AppWeather.Models
{
    public class WeatherGui
    {
        public string Date { get; set; } = string.Empty;
        public string Temperature { get; set; } = string.Empty;
        public string Precipitation { get; set; } = string.Empty;
        public string Pressure { get; set; } = string.Empty;
        public Weather weather { get; set; } 
    }
}
