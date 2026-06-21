using LowCortisol.Platform.API.DeviceControl.Domain.Model.ValueObjects;
using LowCortisol.Platform.API.Shared.Domain.Model;

namespace LowCortisol.Platform.API.DeviceControl.Domain.Model.Aggregates;

public class ValveOperation : IEntity, IAuditableEntity
{
    private ValveOperation()
    {
        FailureReason = string.Empty;
    }

    public ValveOperation(
        Guid id,
        PhysicalTarget target,
        Guid? incidentId,
        DeviceResourceType resourceType,
        ValveStatus previousStatus,
        ValveStatus targetStatus,
        ValveOperationReason reason,
        CommandSource source,
        DateTime? requestedAt = null)
    {
        if (!target.HasValve)
            throw new ArgumentException("Valve id is required.", nameof(target));
        if (!target.HasDevice)
            throw new ArgumentException("Device id is required.", nameof(target));
        if (source == CommandSource.Incident && (!incidentId.HasValue || incidentId.Value == Guid.Empty))
            throw new ArgumentException("Incident source valve operations require incident id.", nameof(incidentId));

        Id = id == Guid.Empty ? Guid.NewGuid() : id;
        ValveId = target.ValveId!.Value;
        DeviceId = target.DeviceId;
        SiteId = target.SiteId;
        RoomId = target.RoomId;
        DeviceGroupId = target.DeviceGroupId;
        IncidentId = incidentId;
        ResourceType = resourceType;
        PreviousStatus = previousStatus;
        TargetStatus = targetStatus;
        Reason = reason;
        Source = source;
        Status = DeviceCommandStatus.Pending;
        RequestedAt = requestedAt ?? DateTime.UtcNow;
        CreatedAt = RequestedAt;
        UpdatedAt = RequestedAt;
        FailureReason = string.Empty;
    }

    public Guid Id { get; private set; }
    public Guid ValveId { get; private set; }
    public Guid DeviceId { get; private set; }
    public Guid SiteId { get; private set; }
    public Guid RoomId { get; private set; }
    public Guid DeviceGroupId { get; private set; }
    public Guid? IncidentId { get; private set; }
    public DeviceResourceType ResourceType { get; private set; }
    public ValveStatus PreviousStatus { get; private set; }
    public ValveStatus TargetStatus { get; private set; }
    public ValveOperationReason Reason { get; private set; }
    public CommandSource Source { get; private set; }
    public DeviceCommandStatus Status { get; private set; }
    public DateTime RequestedAt { get; private set; }
    public DateTime? CompletedAt { get; private set; }
    public DateTime? FailedAt { get; private set; }
    public string FailureReason { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    public void Complete()
    {
        if (Status == DeviceCommandStatus.Failed)
            throw new InvalidOperationException("Failed valve operations cannot be completed.");
        if (Status == DeviceCommandStatus.Executed)
            return;

        CompletedAt = DateTime.UtcNow;
        Status = DeviceCommandStatus.Executed;
        UpdatedAt = CompletedAt.Value;
    }

    public void Fail(string reason)
    {
        if (string.IsNullOrWhiteSpace(reason))
            throw new ArgumentException("Failure reason is required.", nameof(reason));
        if (Status == DeviceCommandStatus.Executed)
            throw new InvalidOperationException("Completed valve operations cannot be failed.");

        FailedAt = DateTime.UtcNow;
        FailureReason = reason.Trim();
        Status = DeviceCommandStatus.Failed;
        UpdatedAt = FailedAt.Value;
    }
}
