using LowCortisol.Platform.API.Plan.Domain.Model.ValueObjects;
using LowCortisol.Platform.API.Shared.Domain.Model;

namespace LowCortisol.Platform.API.Plan.Domain.Model.Aggregates;

public class Subscription : IEntity, IAuditableEntity
{
    private Subscription()
    {
    }

    public Subscription(Guid id, string userId, string workplaceId, Guid planId, bool autoRenew = true)
    {
        if (string.IsNullOrWhiteSpace(userId)) throw new ArgumentException("User id is required.", nameof(userId));
        if (planId == Guid.Empty) throw new ArgumentException("Plan id is required.", nameof(planId));

        Id = id == Guid.Empty ? Guid.NewGuid() : id;
        UserId = userId.Trim();
        WorkplaceId = string.IsNullOrWhiteSpace(workplaceId) ? "WORKPLACE-001" : workplaceId.Trim();
        PlanId = planId;
        Status = SubscriptionStatus.Active;
        StartedAt = DateTime.UtcNow;
        AutoRenew = autoRenew;
        CreatedAt = StartedAt;
        UpdatedAt = StartedAt;
    }

    public Guid Id { get; private set; }
    public string UserId { get; private set; } = string.Empty;
    public string WorkplaceId { get; private set; } = "WORKPLACE-001";
    public Guid PlanId { get; private set; }
    public SubscriptionStatus Status { get; private set; }
    public DateTime StartedAt { get; private set; }
    public DateTime? ExpiresAt { get; private set; }
    public bool AutoRenew { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    public void ChangePlan(Guid planId)
    {
        if (Status != SubscriptionStatus.Active) throw new InvalidOperationException("Only active subscriptions can change plan.");
        if (planId == Guid.Empty) throw new ArgumentException("Plan id is required.", nameof(planId));

        PlanId = planId;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Cancel()
    {
        if (Status == SubscriptionStatus.Cancelled) return;
        Status = SubscriptionStatus.Cancelled;
        AutoRenew = false;
        ExpiresAt = DateTime.UtcNow;
        UpdatedAt = ExpiresAt.Value;
    }
}
