using LowCortisol.Platform.API.DeviceControl.Domain.Model.ValueObjects;
using LowCortisol.Platform.API.Shared.Domain.Model;

namespace LowCortisol.Platform.API.DeviceControl.Domain.Model.Entities;

public class CommandExecution : IEntity, IAuditableEntity
{
    private CommandExecution()
    {
        ResultMessage = string.Empty;
        FailureReason = string.Empty;
    }

    internal CommandExecution(
        Guid id,
        Guid deviceCommandId,
        DeviceCommandStatus status,
        DateTime startedAt,
        DateTime? finishedAt,
        string resultMessage,
        string? failureReason)
    {
        if (deviceCommandId == Guid.Empty)
            throw new ArgumentException("Device command id is required.", nameof(deviceCommandId));
        if (status == DeviceCommandStatus.Failed && string.IsNullOrWhiteSpace(failureReason))
            throw new ArgumentException("Failed command executions require a failure reason.", nameof(failureReason));

        Id = id == Guid.Empty ? Guid.NewGuid() : id;
        DeviceCommandId = deviceCommandId;
        Status = status;
        StartedAt = startedAt;
        FinishedAt = finishedAt;
        ResultMessage = (resultMessage ?? string.Empty).Trim();
        FailureReason = (failureReason ?? string.Empty).Trim();
        CreatedAt = startedAt;
        UpdatedAt = finishedAt ?? startedAt;
    }

    public Guid Id { get; private set; }
    public Guid DeviceCommandId { get; private set; }
    public DeviceCommandStatus Status { get; private set; }
    public DateTime StartedAt { get; private set; }
    public DateTime? FinishedAt { get; private set; }
    public string ResultMessage { get; private set; }
    public string FailureReason { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
}
