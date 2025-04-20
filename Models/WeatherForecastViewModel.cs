using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppWeather.Models
{
    public class WeatherForecastViewModel
    {
        public string? Date { get; set; } = "";
        public string? Temperature { get; set; } = "";
        public string? Precipitation { get; set; } = "";
        public string? Pressure { get; set; } = "";
    }
}
