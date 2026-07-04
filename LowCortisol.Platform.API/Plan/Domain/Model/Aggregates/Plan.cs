using LowCortisol.Platform.API.Plan.Domain.Model.Entities;
using LowCortisol.Platform.API.Shared.Domain.Model;

namespace LowCortisol.Platform.API.Plan.Domain.Model.Aggregates;

public class Plan : IEntity, IAuditableEntity
{
    private readonly List<PlanFeature> _features = [];

    private Plan()
    {
        Features = _features;
    }

    public Plan(
        Guid id,
        string code,
        string name,
        string description,
        decimal price,
        string currency,
        string billingPeriod,
        int maxSites,
        int maxDevices,
        bool isRecommended = false,
        bool isActive = true)
    {
        if (string.IsNullOrWhiteSpace(code)) throw new ArgumentException("Plan code is required.", nameof(code));
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Plan name is required.", nameof(name));
        if (price < 0) throw new ArgumentException("Plan price cannot be negative.", nameof(price));
        if (maxSites < 0) throw new ArgumentException("Max sites cannot be negative.", nameof(maxSites));
        if (maxDevices < 0) throw new ArgumentException("Max devices cannot be negative.", nameof(maxDevices));

        Id = id == Guid.Empty ? Guid.NewGuid() : id;
        Code = code.Trim().ToLowerInvariant();
        Name = name.Trim();
        Description = description?.Trim() ?? string.Empty;
        Price = price;
        Currency = string.IsNullOrWhiteSpace(currency) ? "PEN" : currency.Trim();
        BillingPeriod = string.IsNullOrWhiteSpace(billingPeriod) ? "monthly" : billingPeriod.Trim();
        MaxSites = maxSites;
        MaxDevices = maxDevices;
        IsRecommended = isRecommended;
        IsActive = isActive;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = CreatedAt;
        Features = _features;
    }

    public Guid Id { get; private set; }
    public string Code { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public decimal Price { get; private set; }
    public string Currency { get; private set; } = "PEN";
    public string BillingPeriod { get; private set; } = "monthly";
    public int MaxSites { get; private set; }
    public int MaxDevices { get; private set; }
    public bool IsRecommended { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public IReadOnlyCollection<PlanFeature> Features { get; private set; }

    public PlanFeature AddFeature(Guid id, string name, string description)
    {
        var feature = new PlanFeature(id, Id, name, description);
        _features.Add(feature);
        UpdatedAt = DateTime.UtcNow;
        return feature;
    }

    public bool CoversUsage(int usedSites, int usedDevices) =>
        IsActive &&
        (MaxSites == 0 || usedSites <= MaxSites) &&
        (MaxDevices == 0 || usedDevices <= MaxDevices);
}
