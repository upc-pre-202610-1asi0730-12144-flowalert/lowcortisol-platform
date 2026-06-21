using LowCortisol.Platform.API.Shared.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace LowCortisol.Platform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Interceptors;

public sealed class AuditableEntityInterceptor : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result)
    {
        ApplyAudit(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        ApplyAudit(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private static void ApplyAudit(DbContext? context)
    {
        if (context is null) return;

        foreach (var entry in context.ChangeTracker.Entries<IAuditableEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                SetDateTime(entry, nameof(IAuditableEntity.CreatedAt), DateTime.UtcNow);
                SetDateTime(entry, nameof(IAuditableEntity.UpdatedAt), DateTime.UtcNow);
            }

            if (entry.State == EntityState.Modified)
            {
                SetDateTime(entry, nameof(IAuditableEntity.UpdatedAt), DateTime.UtcNow);
            }
        }
    }

    private static void SetDateTime(EntityEntry entry, string propertyName, DateTime value)
    {
        var property = entry.Metadata.FindProperty(propertyName);
        if (property is null) return;

        entry.Property(propertyName).CurrentValue = value;
    }
}
