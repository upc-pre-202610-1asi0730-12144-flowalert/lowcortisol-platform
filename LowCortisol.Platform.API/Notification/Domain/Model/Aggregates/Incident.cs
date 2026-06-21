using LowCortisol.Platform.API.Notification.Domain.Model.Entities;
using LowCortisol.Platform.API.Notification.Domain.Model.ValueObjects;
using LowCortisol.Platform.API.Shared.Domain.Model;

namespace LowCortisol.Platform.API.Notification.Domain.Model.Aggregates;

public class Incident : IEntity, IAuditableEntity
{
    private readonly List<IncidentAction> _actions = [];
    private readonly List<IncidentAssignment> _assignments = [];

    private Incident()
    {
        Title = string.Empty;
        Description = string.Empty;
        Actions = _actions;
        Assignments = _assignments;
    }

    public Incident(
        Guid id,
        Guid alertId,
        Guid siteId,
        Guid roomId,
        Guid deviceGroupId,
        Guid deviceId,
        Guid sensorId,
        IncidentPriority priority,
        string title,
        string description)
    {
        if (alertId == Guid.Empty) throw new ArgumentException("Alert id is required.", nameof(alertId));
        if (siteId == Guid.Empty) throw new ArgumentException("Site id is required.", nameof(siteId));
        if (string.IsNullOrWhiteSpace(title)) throw new ArgumentException("Incident title is required.", nameof(title));

        Id = id == Guid.Empty ? Guid.NewGuid() : id;
        AlertId = alertId;
        SiteId = siteId;
        RoomId = roomId;
        DeviceGroupId = deviceGroupId;
        DeviceId = deviceId;
        SensorId = sensorId;
        Priority = priority;
        Status = IncidentStatus.Open;
        Title = title.Trim();
        Description = (description ?? string.Empty).Trim();
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = CreatedAt;
        Actions = _actions;
        Assignments = _assignments;
    }

    public Guid Id { get; private set; }
    public Guid AlertId { get; private set; }
    public Guid SiteId { get; private set; }
    public Guid RoomId { get; private set; }
    public Guid DeviceGroupId { get; private set; }
    public Guid DeviceId { get; private set; }
    public Guid SensorId { get; private set; }
    public IncidentPriority Priority { get; private set; }
    public IncidentStatus Status { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public DateTime? ResolvedAt { get; private set; }
    public DateTime? ClosedAt { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public IReadOnlyCollection<IncidentAction> Actions { get; private set; }
    public IReadOnlyCollection<IncidentAssignment> Assignments { get; private set; }

    public IncidentAssignment Assign(string assigneeId, string assigneeName)
    {
        if (Status == IncidentStatus.Closed)
            throw new InvalidOperationException("Closed incidents cannot be assigned.");

        foreach (var assignment in _assignments.Where(assignment => assignment.IsActive))
        {
            assignment.Deactivate();
        }

        var newAssignment = new IncidentAssignment(Guid.NewGuid(), Id, assigneeId, assigneeName, DateTime.UtcNow);
        _assignments.Add(newAssignment);
        Status = IncidentStatus.Assigned;
        UpdatedAt = DateTime.UtcNow;

        return newAssignment;
    }

    public IncidentAction RegisterAction(string actionType, string description, string performedBy)
    {
        if (Status == IncidentStatus.Closed)
            throw new InvalidOperationException("Closed incidents cannot receive actions.");

        var action = new IncidentAction(Guid.NewGuid(), Id, actionType, description, performedBy, DateTime.UtcNow);
        _actions.Add(action);
        if (Status == IncidentStatus.Open || Status == IncidentStatus.Assigned)
        {
            Status = IncidentStatus.InProgress;
        }

        UpdatedAt = DateTime.UtcNow;

        return action;
    }

    public void MarkInProgress()
    {
        if (Status == IncidentStatus.Closed)
            throw new InvalidOperationException("Closed incidents cannot return to progress.");
        if (Status == IncidentStatus.Resolved)
            throw new InvalidOperationException("Resolved incidents cannot return to progress.");

        Status = IncidentStatus.InProgress;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Resolve(string resolvedBy, string resolution)
    {
        if (Status == IncidentStatus.Closed)
            throw new InvalidOperationException("Closed incidents cannot be resolved.");

        RegisterAction("resolution", resolution, resolvedBy);
        Status = IncidentStatus.Resolved;
        ResolvedAt = DateTime.UtcNow;
        UpdatedAt = ResolvedAt.Value;
    }

    public void Close(string closedBy, string closingNote)
    {
        if (Status == IncidentStatus.Closed) return;
        if (Status != IncidentStatus.Resolved && !_actions.Any())
            throw new InvalidOperationException("Incident must be resolved or have a final action before closing.");

        if (Status != IncidentStatus.Resolved)
        {
            RegisterAction("closing", closingNote, closedBy);
        }

        Status = IncidentStatus.Closed;
        ClosedAt = DateTime.UtcNow;
        UpdatedAt = ClosedAt.Value;
    }

    public static Incident FromAlert(Alert alert)
    {
        if (!alert.CanCreateIncident())
            throw new InvalidOperationException("Only open or acknowledged critical alerts can create incidents.");

        return new Incident(
            Guid.NewGuid(),
            alert.Id,
            alert.SiteId,
            alert.RoomId,
            alert.DeviceGroupId,
            alert.DeviceId,
            alert.SensorId,
            IncidentPriority.Critical,
            $"Incident for {alert.Message.Title}",
            alert.Message.Description);
    }
}
