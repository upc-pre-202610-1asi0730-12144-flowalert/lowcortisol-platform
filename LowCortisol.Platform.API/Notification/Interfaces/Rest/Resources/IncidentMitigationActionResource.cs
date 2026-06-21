using LowCortisol.Platform.API.DeviceControl.Interfaces.Rest.Resources;

namespace LowCortisol.Platform.API.Notification.Interfaces.Rest.Resources;

public record IncidentMitigationActionResource(
    IncidentResource Incident,
    DeviceControlMitigationResultResource Mitigation);
