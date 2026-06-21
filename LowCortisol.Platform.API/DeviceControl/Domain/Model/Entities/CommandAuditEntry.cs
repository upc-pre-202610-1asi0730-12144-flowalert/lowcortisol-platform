using LowCortisol.Platform.API.Shared.Domain.Model;

namespace LowCortisol.Platform.API.DeviceControl.Domain.Model.Entities;

public class CommandAuditEntry : IEntity, IAuditableEntity
{
    private CommandAuditEntry()
    {
        Action = string.Empty;
        Description = string.Empty;
        PerformedBy = string.Empty;
    }

    internal CommandAuditEntry(
        Guid id,
        Guid deviceCommandId,
        string action,
        string description,
        string performedBy,
        DateTime performedAt)
    {
        if (deviceCommandId == Guid.Empty)
            throw new ArgumentException("Device command id is required.", nameof(deviceCommandId));
        if (string.IsNullOrWhiteSpace(action))
            throw new ArgumentException("Command audit action is required.", nameof(action));
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Command audit description is required.", nameof(description));

        Id = id == Guid.Empty ? Guid.NewGuid() : id;
        DeviceCommandId = deviceCommandId;
        Action = action.Trim();
        Description = description.Trim();
        PerformedBy = string.IsNullOrWhiteSpace(performedBy) ? "Operations" : performedBy.Trim();
        PerformedAt = performedAt;
        CreatedAt = performedAt;
        UpdatedAt = performedAt;
    }

    public Guid Id { get; private set; }
    public Guid DeviceCommandId { get; private set; }
    public string Action { get; private set; }
    public string Description { get; private set; }
    public string PerformedBy { get; private set; }
    public DateTime PerformedAt { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
}
