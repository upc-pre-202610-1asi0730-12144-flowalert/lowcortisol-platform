using LowCortisol.Platform.API.Notification.Domain.Model.Commands;
using LowCortisol.Platform.API.Notification.Interfaces.Rest.Resources;

namespace LowCortisol.Platform.API.Notification.Interfaces.Rest.Transform;

public static class RequestIncidentMitigationCommandFromResourceAssembler
{
    public static RequestIncidentMitigationCommand ToCommandFromResource(
        Guid incidentId,
        CloseValveForIncidentResource resource) =>
        new(
            incidentId,
            resource.DeviceId,
            resource.ValveId,
            resource.RequestedBy,
            resource.Reason ?? string.Empty);
}
