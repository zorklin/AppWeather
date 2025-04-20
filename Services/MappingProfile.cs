using AutoMapper;
using AppWeather.Models;
using System.Globalization;
using AppWeather.Utilities;

namespace AppWeather.Services
{
    public interface IWeatherMapper
    {
        WeatherForecastViewModel MapToViewModel(WeatherForecast forecast);
        WeatherForecast MapToModel(WeatherForecastViewModel viewModel);
    }

    public class WeatherMapper : IWeatherMapper
    {
        private readonly IMapper _mapper;

        public WeatherMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<WeatherMappingProfile>();
            });
            _mapper = config.CreateMapper();
        }

        public WeatherForecastViewModel MapToViewModel(WeatherForecast forecast)
            => _mapper.Map<WeatherForecastViewModel>(forecast);

        public WeatherForecast MapToModel(WeatherForecastViewModel viewModel)
            => _mapper.Map<WeatherForecast>(viewModel);
    }

    public class WeatherMappingProfile : Profile
    {
        public WeatherMappingProfile()
        {
            CreateMap<WeatherForecast, WeatherForecastViewModel>()
                .ForMember(dest => dest.Date,
                    opt => opt.MapFrom(src => src.Date.HasValue
                        ? src.Date.Value.ToString("yyyy-MM-dd")
                        : ""))
                .ForMember(dest => dest.Temperature,
                    opt => opt.MapFrom(src => src.Temperature.HasValue
                        ? $"{src.Temperature:F1}"
                        : ""))
                .ForMember(dest => dest.Precipitation,
                    opt => opt.MapFrom(src => src.Precipitation.HasValue
                        ? (src.Precipitation.Value ? "Так" : "Ні")
                        : ""))
                .ForMember(dest => dest.Pressure,
                    opt => opt.MapFrom(src => src.Pressure.HasValue
                        ? $"{src.Pressure:F1}"
                        : ""));

            CreateMap<WeatherForecastViewModel, WeatherForecast>()
                .ForMember(dest => dest.Date,
                    opt => opt.MapFrom(src => !string.IsNullOrWhiteSpace(src.Date)
                        ? DateOnly.ParseExact(src.Date, "yyyy-MM-dd")
                        : (DateOnly?)null))
                .ForMember(dest => dest.Temperature,
                    opt => opt.MapFrom(src => !string.IsNullOrWhiteSpace(src.Temperature)
                        ? float.Parse(src.Temperature, CultureInfo.InvariantCulture)
                        : (float?)null))
                .ForMember(dest => dest.Precipitation,
                    opt => opt.MapFrom(src =>
                        src.Precipitation == "Так" ? true :
                        src.Precipitation == "Ні" ? false :
                        (bool?)null))
                .ForMember(dest => dest.Pressure,
                    opt => opt.MapFrom(src => !string.IsNullOrWhiteSpace(src.Pressure)
                        ? float.Parse(src.Pressure, CultureInfo.InvariantCulture)
                        : (float?)null));
        }
    }
}