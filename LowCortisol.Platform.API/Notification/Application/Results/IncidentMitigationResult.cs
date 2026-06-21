using LowCortisol.Platform.API.DeviceControl.Application.Results;
using LowCortisol.Platform.API.Notification.Domain.Model.Aggregates;

namespace LowCortisol.Platform.API.Notification.Application.Results;

public record IncidentMitigationResult(
    Incident Incident,
    DeviceControlMitigationResult Mitigation);
