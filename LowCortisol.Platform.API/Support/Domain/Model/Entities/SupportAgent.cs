using LowCortisol.Platform.API.Shared.Domain.Model;
using LowCortisol.Platform.API.Support.Domain.Model.ValueObjects;

namespace LowCortisol.Platform.API.Support.Domain.Model.Entities;

public class SupportAgent : IEntity, IAuditableEntity
{
    private SupportAgent()
    {
    }

    public SupportAgent(Guid id, string name, string specialty, SupportAgentStatus status = SupportAgentStatus.Available)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Agent name is required.", nameof(name));

        Id = id == Guid.Empty ? Guid.NewGuid() : id;
        Name = name.Trim();
        Specialty = string.IsNullOrWhiteSpace(specialty) ? "General" : specialty.Trim();
        Status = status;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = CreatedAt;
    }

    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Specialty { get; private set; } = "General";
    public SupportAgentStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
}
