using LowCortisol.Platform.API.Notification.Domain.Model.Entities;
using LowCortisol.Platform.API.Notification.Domain.Model.ValueObjects;
using LowCortisol.Platform.API.Shared.Domain.Model;

namespace LowCortisol.Platform.API.Notification.Domain.Model.Aggregates;

public class Alert : IEntity, IAuditableEntity
{
    private readonly List<AlertDelivery> _deliveries = [];

    private Alert()
    {
        Message = new AlertMessage("Alert", string.Empty);
        Deliveries = _deliveries;
    }

    public Alert(
        Guid id,
        AlertMessage message,
        AlertSeverity severity,
        RiskSourceType sourceType,
        Guid? anomalyId,
        Guid? readingId,
        Guid siteId,
        Guid roomId,
        Guid deviceGroupId,
        Guid deviceId,
        Guid sensorId,
        string resourceType,
        ResponseSla? responseSla = null)
    {
        if (sourceType == RiskSourceType.Anomaly && (anomalyId is null || anomalyId == Guid.Empty))
            throw new ArgumentException("Anomaly alerts must include anomaly id.", nameof(anomalyId));
        if (siteId == Guid.Empty) throw new ArgumentException("Site id is required.", nameof(siteId));
        if (string.IsNullOrWhiteSpace(resourceType))
            throw new ArgumentException("Resource type is required.", nameof(resourceType));

        Id = id == Guid.Empty ? Guid.NewGuid() : id;
        Message = message ?? throw new ArgumentNullException(nameof(message));
        Severity = severity;
        Status = AlertStatus.Open;
        SourceType = sourceType;
        AnomalyId = anomalyId;
        ReadingId = readingId;
        SiteId = siteId;
        RoomId = roomId;
        DeviceGroupId = deviceGroupId;
        DeviceId = deviceId;
        SensorId = sensorId;
        ResourceType = resourceType.Trim();
        ResponseSla = responseSla ?? ResponseSla.ForSeverity(severity);
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = CreatedAt;
        Deliveries = _deliveries;
    }

    public Guid Id { get; private set; }
    public AlertMessage Message { get; private set; }
    public AlertSeverity Severity { get; private set; }
    public AlertStatus Status { get; private set; }
    public RiskSourceType SourceType { get; private set; }
    public Guid? AnomalyId { get; private set; }
    public Guid? ReadingId { get; private set; }
    public Guid SiteId { get; private set; }
    public Guid RoomId { get; private set; }
    public Guid DeviceGroupId { get; private set; }
    public Guid DeviceId { get; private set; }
    public Guid SensorId { get; private set; }
    public string ResourceType { get; private set; } = string.Empty;
    public ResponseSla ResponseSla { get; private set; } = ResponseSla.ForSeverity(AlertSeverity.Warning);
    public DateTime? AcknowledgedAt { get; private set; }
    public DateTime? ResolvedAt { get; private set; }
    public DateTime? ClosedAt { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public IReadOnlyCollection<AlertDelivery> Deliveries { get; private set; }

    public bool IsCritical() => Severity == AlertSeverity.Critical;

    public bool CanCreateIncident() => IsCritical() && Status is AlertStatus.Open or AlertStatus.Acknowledged;

    public void Acknowledge(string acknowledgedBy)
    {
        if (Status == AlertStatus.Closed)
            throw new InvalidOperationException("Closed alerts cannot be acknowledged.");
        if (Status is AlertStatus.Resolved)
            throw new InvalidOperationException("Resolved alerts cannot be acknowledged.");
        if (Status == AlertStatus.Acknowledged) return;

        Status = AlertStatus.Acknowledged;
        AcknowledgedAt = DateTime.UtcNow;
        UpdatedAt = AcknowledgedAt.Value;
    }

    public void Resolve(string resolvedBy, string note)
    {
        if (Status == AlertStatus.Closed)
            throw new InvalidOperationException("Closed alerts cannot be resolved.");
        if (Status == AlertStatus.Resolved) return;

        Status = AlertStatus.Resolved;
        ResolvedAt = DateTime.UtcNow;
        UpdatedAt = ResolvedAt.Value;
    }

    public void Close(string closedBy, string note)
    {
        if (Status == AlertStatus.Closed) return;
        if (Status != AlertStatus.Resolved)
            throw new InvalidOperationException("Alert must be resolved before it can be closed.");

        Status = AlertStatus.Closed;
        ClosedAt = DateTime.UtcNow;
        UpdatedAt = ClosedAt.Value;
    }

    public AlertDelivery RegisterDelivery(NotificationChannel channel, Recipient recipient)
    {
        if (!channel.CanDeliver(this))
            throw new InvalidOperationException("Notification channel cannot deliver this alert.");

        var delivery = new AlertDelivery(
            Guid.NewGuid(),
            Id,
            channel.Id,
            channel.Type,
            recipient,
            Message,
            DateTime.UtcNow);

        if (channel.Type == NotificationChannelType.InApp)
        {
            delivery.MarkAsSent();
        }

        _deliveries.Add(delivery);
        UpdatedAt = DateTime.UtcNow;

        return delivery;
    }

    public static Alert FromCriticalAnomaly(
        Guid anomalyId,
        Guid readingId,
        Guid siteId,
        Guid roomId,
        Guid deviceGroupId,
        Guid deviceId,
        Guid sensorId,
        string resourceType,
        decimal value,
        decimal limitValue,
        string unit,
        DateTime detectedAt)
    {
        var message = new AlertMessage(
            "Critical anomaly detected",
            $"Reading {value} {unit} exceeded critical limit {limitValue} {unit}.");

        return new Alert(
            Guid.NewGuid(),
            message,
            AlertSeverity.Critical,
            RiskSourceType.Anomaly,
            anomalyId,
            readingId,
            siteId,
            roomId,
            deviceGroupId,
            deviceId,
            sensorId,
            resourceType);
    }
}
