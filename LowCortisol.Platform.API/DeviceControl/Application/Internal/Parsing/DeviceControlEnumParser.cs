using LowCortisol.Platform.API.DeviceControl.Domain.Model.ValueObjects;

namespace LowCortisol.Platform.API.DeviceControl.Application.Internal.Parsing;

public static class DeviceControlEnumParser
{
    public static bool TryParseCommandType(string value, out DeviceCommandType commandType) =>
        TryParseNormalized(value, out commandType);

    public static bool TryParseCommandSource(string value, out CommandSource commandSource) =>
        TryParseNormalized(value, out commandSource);

    public static bool TryParseValveOperationReason(string value, out ValveOperationReason reason) =>
        TryParseNormalized(value, out reason);

    private static bool TryParseNormalized<TEnum>(string value, out TEnum result)
        where TEnum : struct
    {
        var normalized = value.Replace("_", string.Empty).Replace("-", string.Empty);
        return Enum.TryParse(normalized, ignoreCase: true, out result);
    }
}
