using LowCortisol.Platform.API.Shared.Domain.Model;

namespace LowCortisol.Platform.API.Plan.Domain.Model.Entities;

public class PlanFeature : IEntity, IAuditableEntity
{
    private PlanFeature()
    {
    }

    public PlanFeature(Guid id, Guid planId, string name, string description)
    {
        if (planId == Guid.Empty) throw new ArgumentException("Plan id is required.", nameof(planId));
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Feature name is required.", nameof(name));

        Id = id == Guid.Empty ? Guid.NewGuid() : id;
        PlanId = planId;
        Name = name.Trim();
        Description = description?.Trim() ?? string.Empty;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = CreatedAt;
    }

    public Guid Id { get; private set; }
    public Guid PlanId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
}
