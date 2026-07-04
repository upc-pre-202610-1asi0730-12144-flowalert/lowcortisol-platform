using LowCortisol.Platform.API.Plan.Domain.Model.ValueObjects;
using LowCortisol.Platform.API.Shared.Domain.Model;

namespace LowCortisol.Platform.API.Plan.Domain.Model.Entities;

public class Payment : IEntity, IAuditableEntity
{
    private Payment()
    {
    }

    public Payment(
        Guid id,
        Guid subscriptionId,
        decimal amount,
        string currency,
        string method,
        PaymentStatus status = PaymentStatus.Paid)
    {
        if (subscriptionId == Guid.Empty) throw new ArgumentException("Subscription id is required.", nameof(subscriptionId));
        if (amount < 0) throw new ArgumentException("Payment amount cannot be negative.", nameof(amount));

        Id = id == Guid.Empty ? Guid.NewGuid() : id;
        SubscriptionId = subscriptionId;
        Amount = amount;
        Currency = string.IsNullOrWhiteSpace(currency) ? "PEN" : currency.Trim();
        Method = string.IsNullOrWhiteSpace(method) ? "card" : method.Trim();
        Status = status;
        PaidAt = status == PaymentStatus.Paid ? DateTime.UtcNow : null;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = CreatedAt;
    }

    public Guid Id { get; private set; }
    public Guid SubscriptionId { get; private set; }
    public decimal Amount { get; private set; }
    public string Currency { get; private set; } = "PEN";
    public string Method { get; private set; } = "card";
    public PaymentStatus Status { get; private set; }
    public DateTime? PaidAt { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
}
