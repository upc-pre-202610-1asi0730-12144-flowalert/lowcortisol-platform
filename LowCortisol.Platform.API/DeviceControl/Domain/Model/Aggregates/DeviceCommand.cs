using LowCortisol.Platform.API.DeviceControl.Domain.Model.Entities;
using LowCortisol.Platform.API.DeviceControl.Domain.Model.ValueObjects;
using LowCortisol.Platform.API.Shared.Domain.Model;

namespace LowCortisol.Platform.API.DeviceControl.Domain.Model.Aggregates;

public class DeviceCommand : IEntity, IAuditableEntity
{
    private readonly List<CommandExecution> _executions = [];
    private readonly List<CommandAuditEntry> _auditEntries = [];

    private DeviceCommand()
    {
        Reason = string.Empty;
        RequestedBy = string.Empty;
        FailureReason = string.Empty;
        Executions = _executions;
        AuditEntries = _auditEntries;
    }

    public DeviceCommand(
        Guid id,
        PhysicalTarget target,
        Guid? incidentId,
        DeviceCommandType commandType,
        CommandSource source,
        string reason,
        string requestedBy,
        DateTime? requestedAt = null)
    {
        if (!target.HasDevice)
            throw new ArgumentException("Device id is required.", nameof(target));
        if (commandType == DeviceCommandType.CloseValve && !target.HasValve)
            throw new ArgumentException("Close valve command requires valve id.", nameof(target));
        if (source == CommandSource.Incident && (!incidentId.HasValue || incidentId.Value == Guid.Empty))
            throw new ArgumentException("Incident source commands require incident id.", nameof(incidentId));
        if (string.IsNullOrWhiteSpace(reason))
            throw new ArgumentException("Command reason is required.", nameof(reason));

        Id = id == Guid.Empty ? Guid.NewGuid() : id;
        DeviceId = target.DeviceId;
        ValveId = target.ValveId;
        SiteId = target.SiteId;
        RoomId = target.RoomId;
        DeviceGroupId = target.DeviceGroupId;
        IncidentId = incidentId;
        CommandType = commandType;
        Status = DeviceCommandStatus.Pending;
        Source = source;
        Reason = reason.Trim();
        RequestedBy = string.IsNullOrWhiteSpace(requestedBy) ? "Operations" : requestedBy.Trim();
        RequestedAt = requestedAt ?? DateTime.UtcNow;
        CreatedAt = RequestedAt;
        UpdatedAt = RequestedAt;
        FailureReason = string.Empty;
        Executions = _executions;
        AuditEntries = _auditEntries;
    }

    public Guid Id { get; private set; }
    public Guid DeviceId { get; private set; }
    public Guid? ValveId { get; private set; }
    public Guid SiteId { get; private set; }
    public Guid RoomId { get; private set; }
    public Guid DeviceGroupId { get; private set; }
    public Guid? IncidentId { get; private set; }
    public DeviceCommandType CommandType { get; private set; }
    public DeviceCommandStatus Status { get; private set; }
    public CommandSource Source { get; private set; }
    public string Reason { get; private set; }
    public string RequestedBy { get; private set; }
    public DateTime RequestedAt { get; private set; }
    public DateTime? ExecutedAt { get; private set; }
    public DateTime? FailedAt { get; private set; }
    public string FailureReason { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public IReadOnlyCollection<CommandExecution> Executions { get; private set; }
    public IReadOnlyCollection<CommandAuditEntry> AuditEntries { get; private set; }

    public bool IsExecutable() => Status == DeviceCommandStatus.Pending;

    public void MarkAsExecuted(string resultMessage = "Command executed.")
    {
        if (!IsExecutable())
            throw new InvalidOperationException("Only pending commands can be executed.");

        var now = DateTime.UtcNow;
        Status = DeviceCommandStatus.Executed;
        ExecutedAt = now;
        UpdatedAt = now;
        RegisterExecution(DeviceCommandStatus.Executed, resultMessage, null, RequestedAt, now);
        RegisterAuditEntry("executed", resultMessage, RequestedBy, now);
    }

    public void MarkAsFailed(string reason)
    {
        if (string.IsNullOrWhiteSpace(reason))
            throw new ArgumentException("Failure reason is required.", nameof(reason));
        if (Status == DeviceCommandStatus.Executed)
            throw new InvalidOperationException("Executed commands cannot be failed.");

        var now = DateTime.UtcNow;
        Status = DeviceCommandStatus.Failed;
        FailedAt = now;
        FailureReason = reason.Trim();
        UpdatedAt = now;
        RegisterExecution(DeviceCommandStatus.Failed, "Command failed.", FailureReason, RequestedAt, now);
        RegisterAuditEntry("failed", FailureReason, RequestedBy, now);
    }

    public void Cancel(string reason)
    {
        if (string.IsNullOrWhiteSpace(reason))
            throw new ArgumentException("Cancellation reason is required.", nameof(reason));
        if (!IsExecutable())
            throw new InvalidOperationException("Only pending commands can be cancelled.");

        var now = DateTime.UtcNow;
        Status = DeviceCommandStatus.Cancelled;
        FailureReason = reason.Trim();
        UpdatedAt = now;
        RegisterAuditEntry("cancelled", FailureReason, RequestedBy, now);
    }

    public CommandExecution RegisterExecution(
        DeviceCommandStatus status,
        string resultMessage,
        string? failureReason,
        DateTime startedAt,
        DateTime? finishedAt)
    {
        var execution = new CommandExecution(
            Guid.NewGuid(),
            Id,
            status,
            startedAt,
            finishedAt,
            resultMessage,
            failureReason);

        _executions.Add(execution);
        return execution;
    }

    public CommandAuditEntry RegisterAuditEntry(
        string action,
        string description,
        string performedBy,
        DateTime? performedAt = null)
    {
        var entry = new CommandAuditEntry(
            Guid.NewGuid(),
            Id,
            action,
            description,
            performedBy,
            performedAt ?? DateTime.UtcNow);

        _auditEntries.Add(entry);
        return entry;
    }
}
