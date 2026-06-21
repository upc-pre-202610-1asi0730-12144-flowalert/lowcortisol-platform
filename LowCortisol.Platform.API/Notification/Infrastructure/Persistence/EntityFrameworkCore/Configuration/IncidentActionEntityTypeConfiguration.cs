using LowCortisol.Platform.API.Notification.Domain.Model.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LowCortisol.Platform.API.Notification.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

public sealed class IncidentActionEntityTypeConfiguration : IEntityTypeConfiguration<IncidentAction>
{
    public void Configure(EntityTypeBuilder<IncidentAction> builder)
    {
        builder.ToTable("incident_actions");

        builder.HasKey(action => action.Id);

        builder.Property(action => action.Id).ValueGeneratedNever();
        builder.Property(action => action.IncidentId).IsRequired();
        builder.Property(action => action.ActionType).IsRequired().HasMaxLength(80);
        builder.Property(action => action.Description).IsRequired().HasMaxLength(520);
        builder.Property(action => action.PerformedBy).IsRequired().HasMaxLength(140);
        builder.Property(action => action.PerformedAt).IsRequired();
        builder.Property(action => action.CreatedAt).IsRequired();
        builder.Property(action => action.UpdatedAt).IsRequired();

        builder.HasIndex(action => action.IncidentId);
        builder.HasIndex(action => action.ActionType);
        builder.HasIndex(action => action.PerformedAt);
    }
}
