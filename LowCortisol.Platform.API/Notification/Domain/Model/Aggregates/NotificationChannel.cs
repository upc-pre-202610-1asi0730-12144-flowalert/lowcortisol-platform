using LowCortisol.Platform.API.Notification.Domain.Model.ValueObjects;
using LowCortisol.Platform.API.Shared.Domain.Model;

namespace LowCortisol.Platform.API.Notification.Domain.Model.Aggregates;

public class NotificationChannel : IEntity, IAuditableEntity
{
    private NotificationChannel()
    {
        Name = string.Empty;
    }

    public NotificationChannel(
        Guid id,
        string name,
        NotificationChannelType type,
        bool isActive = true)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Notification channel name is required.", nameof(name));

        Id = id == Guid.Empty ? Guid.NewGuid() : id;
        Name = name.Trim();
        Type = type;
        IsActive = isActive;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = CreatedAt;
    }

    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public NotificationChannelType Type { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    public void Activate()
    {
        if (IsActive) return;

        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        if (!IsActive) return;

        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }

    public bool CanDeliver(Alert alert) => IsActive && (Type == NotificationChannelType.InApp || alert.IsCritical());

    public static NotificationChannel DefaultInApp(Guid? id = null) =>
        new(id ?? Guid.NewGuid(), "In-app operations channel", NotificationChannelType.InApp);
}
