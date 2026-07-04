using LowCortisol.Platform.API.Plan.Domain.Model.ValueObjects;
using LowCortisol.Platform.API.Shared.Domain.Model;

namespace LowCortisol.Platform.API.Plan.Domain.Model.Entities;

public class ServiceRequest : IEntity, IAuditableEntity
{
    private ServiceRequest()
    {
    }

    public ServiceRequest(Guid id, Guid subscriptionId, string type, string description, ServiceRequestStatus status = ServiceRequestStatus.Open)
    {
        if (subscriptionId == Guid.Empty) throw new ArgumentException("Subscription id is required.", nameof(subscriptionId));
        if (string.IsNullOrWhiteSpace(type)) throw new ArgumentException("Service request type is required.", nameof(type));

        Id = id == Guid.Empty ? Guid.NewGuid() : id;
        SubscriptionId = subscriptionId;
        Type = type.Trim();
        Description = description?.Trim() ?? string.Empty;
        Status = status;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = CreatedAt;
    }

    public Guid Id { get; private set; }
    public Guid SubscriptionId { get; private set; }
    public string Type { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public ServiceRequestStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    public void Resolve()
    {
        if (Status == ServiceRequestStatus.Closed) throw new InvalidOperationException("Closed requests cannot be resolved.");
        Status = ServiceRequestStatus.Resolved;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Close()
    {
        if (Status == ServiceRequestStatus.Closed) return;
        Status = ServiceRequestStatus.Closed;
        UpdatedAt = DateTime.UtcNow;
    }
}
