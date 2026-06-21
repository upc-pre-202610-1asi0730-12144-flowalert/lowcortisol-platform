using LowCortisol.Platform.API.DeviceControl.Application.Results;
using LowCortisol.Platform.API.DeviceControl.Interfaces.Rest.Resources;

namespace LowCortisol.Platform.API.DeviceControl.Interfaces.Rest.Transform;

public static class DeviceControlMitigationResultResourceFromResultAssembler
{
    public static DeviceControlMitigationResultResource ToResourceFromResult(DeviceControlMitigationResult result) =>
        new(
            result.IncidentId,
            result.DeviceId,
            result.ValveId,
            result.DeviceCommandId,
            result.ValveOperationId,
            ToResourceValue(result.CommandStatus),
            ToResourceValue(result.ValveStatus),
            result.ExecutedAt);

    private static string ToResourceValue(string value)
    {
        var result = new List<char>(value.Length + 4);

        for (var index = 0; index < value.Length; index++)
        {
            var current = value[index];
            if (char.IsUpper(current) && index > 0) result.Add('_');
            result.Add(char.ToLowerInvariant(current));
        }

        return new string(result.ToArray());
    }
}
