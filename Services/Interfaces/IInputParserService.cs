namespace AppWeather.Services.Interfaces
{
    public interface IInputParserService
    {
        bool TryParseDate(string input, out DateOnly result, out string? error);
        bool TryParseFloat(string input, out float? result, out string? error);
        bool TryParseBool(string input, out bool? result, out string? error);
    }
}
