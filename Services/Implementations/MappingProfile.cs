using AutoMapper;
using AppWeather.Models;
using System.Globalization;
using AppWeather.Services.Interfaces;

namespace AppWeather.Services.Implementations
{
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

        public WeatherGui MapToViewModel(Weather forecast)
            => _mapper.Map<WeatherGui>(forecast);

        public Weather MapToModel(WeatherGui viewModel)
            => _mapper.Map<Weather>(viewModel);
    }

    public class WeatherMappingProfile : Profile
    {
        public WeatherMappingProfile()
        {
            CreateMap<Weather, WeatherGui>()
                .ForMember(dest => dest.Date,
                    opt => opt.MapFrom(src => src.Date != default
                        ? src.Date.ToString("dd MMMM yyyy", new CultureInfo("uk-UA"))
                        : ""))
                .ForMember(dest => dest.Temperature,
                    opt => opt.MapFrom(src => src.Temperature.HasValue
                        ? $"{src.Temperature:F1}"
                        : ""))
                .ForMember(dest => dest.Precipitation,
                    opt => opt.MapFrom(src => src.Precipitation.HasValue
                        ? src.Precipitation.Value ? "Так" : "Ні"
                        : ""))
                .ForMember(dest => dest.Pressure,
                    opt => opt.MapFrom(src => src.Pressure.HasValue
                        ? $"{src.Pressure:F1}"
                        : ""));

            CreateMap<WeatherGui, Weather>()
                .ForMember(dest => dest.Date,
                    opt => opt.MapFrom(src => !string.IsNullOrWhiteSpace(src.Date)
                        ? DateOnly.ParseExact(src.Date, "dd MMMM yyyy", new CultureInfo("uk-UA", false), DateTimeStyles.None)
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