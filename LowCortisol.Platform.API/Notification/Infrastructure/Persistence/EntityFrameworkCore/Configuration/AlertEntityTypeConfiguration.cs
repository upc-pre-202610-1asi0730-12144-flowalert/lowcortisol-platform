using LowCortisol.Platform.API.Notification.Domain.Model.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LowCortisol.Platform.API.Notification.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

public sealed class AlertEntityTypeConfiguration : IEntityTypeConfiguration<Alert>
{
    public void Configure(EntityTypeBuilder<Alert> builder)
    {
        builder.ToTable("alerts");

        builder.HasKey(alert => alert.Id);

        builder.Property(alert => alert.Id).ValueGeneratedNever();
        builder.Property(alert => alert.Severity).HasConversion<string>().IsRequired().HasMaxLength(40);
        builder.Property(alert => alert.Status).HasConversion<string>().IsRequired().HasMaxLength(40);
        builder.Property(alert => alert.SourceType).HasConversion<string>().IsRequired().HasMaxLength(40);
        builder.Property(alert => alert.AnomalyId);
        builder.Property(alert => alert.ReadingId);
        builder.Property(alert => alert.SiteId).IsRequired();
        builder.Property(alert => alert.RoomId).IsRequired();
        builder.Property(alert => alert.DeviceGroupId).IsRequired();
        builder.Property(alert => alert.DeviceId).IsRequired();
        builder.Property(alert => alert.SensorId).IsRequired();
        builder.Property(alert => alert.ResourceType).IsRequired().HasMaxLength(40);
        builder.Property(alert => alert.AcknowledgedAt);
        builder.Property(alert => alert.ResolvedAt);
        builder.Property(alert => alert.ClosedAt);
        builder.Property(alert => alert.CreatedAt).IsRequired();
        builder.Property(alert => alert.UpdatedAt).IsRequired();

        builder.OwnsOne(alert => alert.Message, message =>
        {
            message.WithOwner().HasForeignKey("AlertId");
            message.Property<Guid>("AlertId").HasColumnName("id");
            message.Property(item => item.Title).HasColumnName("title").IsRequired().HasMaxLength(160);
            message.Property(item => item.Description).HasColumnName("description").IsRequired().HasMaxLength(520);
        });

        builder.OwnsOne(alert => alert.ResponseSla, sla =>
        {
            sla.WithOwner().HasForeignKey("AlertId");
            sla.Property<Guid>("AlertId").HasColumnName("id");
            sla.Property(item => item.MinutesToAcknowledge)
                .HasColumnName("sla_minutes_to_acknowledge")
                .IsRequired();
            sla.Property(item => item.MinutesToResolve)
                .HasColumnName("sla_minutes_to_resolve")
                .IsRequired();
        });

        builder.HasMany(alert => alert.Deliveries)
            .WithOne()
            .HasForeignKey(delivery => delivery.AlertId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(alert => alert.Deliveries).UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.HasIndex(alert => alert.SiteId);
        builder.HasIndex(alert => alert.RoomId);
        builder.HasIndex(alert => alert.DeviceGroupId);
        builder.HasIndex(alert => alert.DeviceId);
        builder.HasIndex(alert => alert.SensorId);
        builder.HasIndex(alert => alert.AnomalyId);
        builder.HasIndex(alert => alert.Status);
        builder.HasIndex(alert => alert.Severity);
        builder.HasIndex(alert => alert.CreatedAt);
    }
}
