using AppWeather.Common;
using AppWeather.Services.Interfaces;
using System.Globalization;

namespace AppWeather.Services.Implementations
{
    public class InputParserService : IInputParserService
    {
        public bool TryParseRequiredDate(string input, out DateOnly result, out string? error)
        {
            result = default;
            error = null;

            if (string.IsNullOrWhiteSpace(input))
            {
                error = "Дата є обов'язковою.";
                return false;
            }

            if (!DateTime.TryParse(input, out var dt))
            {
                error = "Невірний формат дати. Використовуйте РРРР-ММ-ДД.";
                return false;
            }

            result = DateOnly.FromDateTime(dt);
            return true;
        }

        public bool TryParseNullableDate(string input, out DateOnly? result, out string? error)
        {
            result = null;
            error = null;

            if (string.IsNullOrWhiteSpace(input))
                return true;

            if (!DateTime.TryParse(input, out var dt))
            {
                error = "Невірний формат дати. Використовуйте РРРР-ММ-ДД.";
                return false;
            }

            result = DateOnly.FromDateTime(dt);
            return true;
        }

        public bool TryParseRequiredFloat(string input, out float result, out string? error)
        {
            result = default;
            error = null;

            if (string.IsNullOrWhiteSpace(input))
            {
                error = "Поле є обов'язковим.";
                return false;
            }

            if (!float.TryParse(input, NumberStyles.Float, CultureInfo.InvariantCulture, out result))
            {
                error = "Невірний формат числа.";
                return false;
            }

            return true;
        }

        public bool TryParseNullableFloat(string input, out float? result, out string? error)
        {
            result = null;
            error = null;

            if (string.IsNullOrWhiteSpace(input))
                return true;

            if (!float.TryParse(input, NumberStyles.Float, CultureInfo.InvariantCulture, out var temp))
            {
                error = "Невірний формат числа.";
                return false;
            }

            result = temp;
            return true;
        }

        public bool TryParseRequiredBool(string input, out bool result, out string? error)
        {
            result = default;
            error = null;

            if (string.IsNullOrWhiteSpace(input))
            {
                error = "Поле є обов'язковим.";
                return false;
            }

            try
            {
                var parsed = BoolParser.ParseNullableBool(input);
                if (parsed == null)
                {
                    error = "Невідоме значення.";
                    return false;
                }

                result = parsed.Value;
                return true;
            }
            catch (FormatException ex)
            {
                error = ex.Message;
                return false;
            }
        }

        public bool TryParseNullableBool(string input, out bool? result, out string? error)
        {
            result = null;
            error = null;

            if (string.IsNullOrWhiteSpace(input))
                return true;

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
