namespace LowCortisol.Platform.API.Monitoring.Application.Internal.Parsing;

internal static class MonitoringEnumParser
{
    public static bool TryParse<TEnum>(string? value, out TEnum result)
        where TEnum : struct, Enum
    {
        result = default;
        if (string.IsNullOrWhiteSpace(value)) return false;

        var normalizedValue = Normalize(value);

        foreach (var candidate in Enum.GetValues<TEnum>())
        {
            if (Normalize(candidate.ToString()).Equals(normalizedValue, StringComparison.OrdinalIgnoreCase))
            {
                result = candidate;
                return true;
            }
        }

        return false;
    }

    private static string Normalize(string value) =>
        value
            .Replace("_", string.Empty, StringComparison.Ordinal)
            .Replace("-", string.Empty, StringComparison.Ordinal)
            .Replace(" ", string.Empty, StringComparison.Ordinal)
            .Trim();
}
