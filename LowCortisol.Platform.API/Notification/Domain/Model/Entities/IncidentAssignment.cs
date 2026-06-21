using LowCortisol.Platform.API.Shared.Domain.Model;

namespace LowCortisol.Platform.API.Notification.Domain.Model.Entities;

public class IncidentAssignment : IEntity, IAuditableEntity
{
    private IncidentAssignment()
    {
        AssigneeId = string.Empty;
        AssigneeName = string.Empty;
    }

    internal IncidentAssignment(
        Guid id,
        Guid incidentId,
        string assigneeId,
        string assigneeName,
        DateTime assignedAt)
    {
        if (string.IsNullOrWhiteSpace(assigneeName))
            throw new ArgumentException("Assignee name is required.", nameof(assigneeName));

        Id = id == Guid.Empty ? Guid.NewGuid() : id;
        IncidentId = incidentId == Guid.Empty
            ? throw new ArgumentException("Incident id is required.", nameof(incidentId))
            : incidentId;
        AssigneeId = assigneeId?.Trim() ?? string.Empty;
        AssigneeName = assigneeName.Trim();
        AssignedAt = assignedAt;
        IsActive = true;
        CreatedAt = assignedAt;
        UpdatedAt = CreatedAt;
    }

    public Guid Id { get; private set; }
    public Guid IncidentId { get; private set; }
    public string AssigneeId { get; private set; }
    public string AssigneeName { get; private set; }
    public DateTime AssignedAt { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    internal void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }
}
