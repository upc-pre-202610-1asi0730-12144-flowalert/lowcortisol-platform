namespace LowCortisol.Platform.API.Notification.Application.Internal.Parsing;

public static class NotificationEnumParser
{
    public static bool TryParse<TEnum>(string value, out TEnum parsed)
        where TEnum : struct, Enum
    {
        var normalized = value.Replace("_", string.Empty, StringComparison.Ordinal)
            .Replace("-", string.Empty, StringComparison.Ordinal);

        foreach (var enumValue in Enum.GetValues<TEnum>())
        {
            if (enumValue.ToString().Equals(normalized, StringComparison.OrdinalIgnoreCase))
            {
                parsed = enumValue;
                return true;
            }
        }

        parsed = default;
        return false;
    }
}
