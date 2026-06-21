using LowCortisol.Platform.API.DeviceControl.Interfaces.Rest.Transform;
using LowCortisol.Platform.API.Notification.Application.Results;
using LowCortisol.Platform.API.Notification.Interfaces.Rest.Resources;

namespace LowCortisol.Platform.API.Notification.Interfaces.Rest.Transform;

public static class IncidentMitigationActionResourceFromResultAssembler
{
    public static IncidentMitigationActionResource ToResourceFromResult(IncidentMitigationResult result) =>
        new(
            IncidentResourceFromEntityAssembler.ToResourceFromEntity(result.Incident),
            DeviceControlMitigationResultResourceFromResultAssembler.ToResourceFromResult(result.Mitigation));
}
