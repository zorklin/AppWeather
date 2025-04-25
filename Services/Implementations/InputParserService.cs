using AppWeather.Common;
using AppWeather.Services.Interfaces;
using System.Globalization;

namespace AppWeather.Services.Implementations
{
    public class InputParserService : IInputParserService
    {
        public bool TryParseDate(string input, out DateOnly result, out string? error)
        {
            result = default;
            error = null;

            if (!DateTime.TryParse(input, out var dt))
            {
                error = "Невірний формат дати. Використовуйте РРРР-ММ-ДД.";
                return false;
            }

            result = DateOnly.FromDateTime(dt);
            return true;
        }

        public bool TryParseFloat(string input, out float? result, out string? error)
        {
            result = null;
            error = null;

            if (string.IsNullOrWhiteSpace(input)) return true;

            if (!float.TryParse(input, NumberStyles.Float, CultureInfo.InvariantCulture, out float temp))
            {
                error = "Невірний формат числа.";
                return false;
            }

            result = temp;
            return true;
        }

        public bool TryParseBool(string input, out bool? result, out string? error)
        {
            result = null;
            error = null;

            try
            {
                result = BoolParser.ParseNullableBool(input);
                return true;
            }
            catch (FormatException ex)
            {
                error = ex.Message;
                return false;
            }
        }
    }

}
