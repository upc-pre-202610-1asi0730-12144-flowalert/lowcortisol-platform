namespace LowCortisol.Platform.API.Shared.Domain.Model;

public interface IAuditableEntity
{
    DateTime CreatedAt { get; }
    DateTime UpdatedAt { get; }
}
