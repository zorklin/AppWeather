public static class BoolParser
{
    public static bool? ParseNullableBool(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return null;
        input = input.Trim().ToLower();

        if (input is "так" or "true")
            return true;

        if (input is "ні" or "false")
            return false;

        throw new FormatException($"Неможливо конвертувати рядок '{input}' в логічне значення.");
    }
}
