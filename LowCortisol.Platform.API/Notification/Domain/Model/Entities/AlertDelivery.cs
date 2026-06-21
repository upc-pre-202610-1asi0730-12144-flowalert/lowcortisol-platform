using LowCortisol.Platform.API.Notification.Domain.Model.ValueObjects;
using LowCortisol.Platform.API.Shared.Domain.Model;

namespace LowCortisol.Platform.API.Notification.Domain.Model.Entities;

public class AlertDelivery : IEntity, IAuditableEntity
{
    private AlertDelivery()
    {
        Recipient = Recipient.OperationsDesk();
        Message = new AlertMessage("Pending alert", string.Empty);
        FailureReason = string.Empty;
    }

    internal AlertDelivery(
        Guid id,
        Guid alertId,
        Guid channelId,
        NotificationChannelType channelType,
        Recipient recipient,
        AlertMessage message,
        DateTime attemptedAt)
    {
        Id = id == Guid.Empty ? Guid.NewGuid() : id;
        AlertId = alertId == Guid.Empty ? throw new ArgumentException("Alert id is required.", nameof(alertId)) : alertId;
        ChannelId = channelId == Guid.Empty ? throw new ArgumentException("Channel id is required.", nameof(channelId)) : channelId;
        ChannelType = channelType;
        Recipient = recipient ?? throw new ArgumentNullException(nameof(recipient));
        Message = message is null
            ? throw new ArgumentNullException(nameof(message))
            : new AlertMessage(message.Title, message.Description);
        Status = DeliveryStatus.Pending;
        AttemptedAt = attemptedAt;
        CreatedAt = attemptedAt;
        UpdatedAt = CreatedAt;
        FailureReason = string.Empty;
    }

    public Guid Id { get; private set; }
    public Guid AlertId { get; private set; }
    public Guid ChannelId { get; private set; }
    public NotificationChannelType ChannelType { get; private set; }
    public DeliveryStatus Status { get; private set; }
    public Recipient Recipient { get; private set; }
    public AlertMessage Message { get; private set; }
    public DateTime AttemptedAt { get; private set; }
    public DateTime? SentAt { get; private set; }
    public string FailureReason { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    public void MarkAsSent()
    {
        if (Status == DeliveryStatus.Sent) return;

        Status = DeliveryStatus.Sent;
        SentAt = DateTime.UtcNow;
        FailureReason = string.Empty;
        UpdatedAt = SentAt.Value;
    }

    public void MarkAsFailed(string reason)
    {
        Status = DeliveryStatus.Failed;
        FailureReason = string.IsNullOrWhiteSpace(reason) ? "Delivery failed." : reason.Trim();
        UpdatedAt = DateTime.UtcNow;
    }

    public void MarkAsSkipped(string reason)
    {
        Status = DeliveryStatus.Skipped;
        FailureReason = string.IsNullOrWhiteSpace(reason) ? "Delivery skipped." : reason.Trim();
        UpdatedAt = DateTime.UtcNow;
    }
}
