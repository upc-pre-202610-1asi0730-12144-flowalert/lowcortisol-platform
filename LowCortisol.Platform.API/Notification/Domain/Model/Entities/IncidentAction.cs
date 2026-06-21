using LowCortisol.Platform.API.Shared.Domain.Model;

namespace LowCortisol.Platform.API.Notification.Domain.Model.Entities;

public class IncidentAction : IEntity, IAuditableEntity
{
    private IncidentAction()
    {
        ActionType = string.Empty;
        Description = string.Empty;
        PerformedBy = string.Empty;
    }

    internal IncidentAction(
        Guid id,
        Guid incidentId,
        string actionType,
        string description,
        string performedBy,
        DateTime performedAt)
    {
        if (string.IsNullOrWhiteSpace(actionType))
            throw new ArgumentException("Incident action type is required.", nameof(actionType));
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Incident action description is required.", nameof(description));

        Id = id == Guid.Empty ? Guid.NewGuid() : id;
        IncidentId = incidentId == Guid.Empty
            ? throw new ArgumentException("Incident id is required.", nameof(incidentId))
            : incidentId;
        ActionType = actionType.Trim();
        Description = description.Trim();
        PerformedBy = string.IsNullOrWhiteSpace(performedBy) ? "Operations" : performedBy.Trim();
        PerformedAt = performedAt;
        CreatedAt = performedAt;
        UpdatedAt = CreatedAt;
    }

    public Guid Id { get; private set; }
    public Guid IncidentId { get; private set; }
    public string ActionType { get; private set; }
    public string Description { get; private set; }
    public string PerformedBy { get; private set; }
    public DateTime PerformedAt { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
}
