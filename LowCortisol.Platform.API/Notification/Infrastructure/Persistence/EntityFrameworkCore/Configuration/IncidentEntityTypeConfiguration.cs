using LowCortisol.Platform.API.Notification.Domain.Model.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LowCortisol.Platform.API.Notification.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

public sealed class IncidentEntityTypeConfiguration : IEntityTypeConfiguration<Incident>
{
    public void Configure(EntityTypeBuilder<Incident> builder)
    {
        builder.ToTable("incidents");

        builder.HasKey(incident => incident.Id);

        builder.Property(incident => incident.Id).ValueGeneratedNever();
        builder.Property(incident => incident.AlertId).IsRequired();
        builder.Property(incident => incident.SiteId).IsRequired();
        builder.Property(incident => incident.RoomId).IsRequired();
        builder.Property(incident => incident.DeviceGroupId).IsRequired();
        builder.Property(incident => incident.DeviceId).IsRequired();
        builder.Property(incident => incident.SensorId).IsRequired();
        builder.Property(incident => incident.Priority).HasConversion<string>().IsRequired().HasMaxLength(40);
        builder.Property(incident => incident.Status).HasConversion<string>().IsRequired().HasMaxLength(40);
        builder.Property(incident => incident.Title).IsRequired().HasMaxLength(180);
        builder.Property(incident => incident.Description).IsRequired().HasMaxLength(640);
        builder.Property(incident => incident.ResolvedAt);
        builder.Property(incident => incident.ClosedAt);
        builder.Property(incident => incident.CreatedAt).IsRequired();
        builder.Property(incident => incident.UpdatedAt).IsRequired();

        builder.HasMany(incident => incident.Actions)
            .WithOne()
            .HasForeignKey(action => action.IncidentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(incident => incident.Assignments)
            .WithOne()
            .HasForeignKey(assignment => assignment.IncidentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(incident => incident.Actions).UsePropertyAccessMode(PropertyAccessMode.Field);
        builder.Navigation(incident => incident.Assignments).UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.HasIndex(incident => incident.AlertId);
        builder.HasIndex(incident => incident.SiteId);
        builder.HasIndex(incident => incident.RoomId);
        builder.HasIndex(incident => incident.DeviceGroupId);
        builder.HasIndex(incident => incident.DeviceId);
        builder.HasIndex(incident => incident.SensorId);
        builder.HasIndex(incident => incident.Status);
        builder.HasIndex(incident => incident.Priority);
        builder.HasIndex(incident => incident.CreatedAt);
    }
}
