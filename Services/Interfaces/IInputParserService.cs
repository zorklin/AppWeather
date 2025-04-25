namespace AppWeather.Services.Interfaces
{
    public interface IInputParserService
    {
        bool TryParseRequiredDate(string input, out DateOnly result, out string? error);
        bool TryParseNullableDate(string input, out DateOnly? result, out string? error);
        bool TryParseRequiredFloat(string input, out float result, out string? error);
        bool TryParseNullableFloat(string input, out float? result, out string? error);
        bool TryParseRequiredBool(string input, out bool result, out string? error);
        bool TryParseNullableBool(string input, out bool? result, out string? error);
    }
}
