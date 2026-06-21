using LowCortisol.Platform.API.Monitoring.Domain.Model.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LowCortisol.Platform.API.Monitoring.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

public sealed class AnomalyEntityTypeConfiguration : IEntityTypeConfiguration<Anomaly>
{
    public void Configure(EntityTypeBuilder<Anomaly> builder)
    {
        builder.ToTable("anomalies");

        builder.HasKey(anomaly => anomaly.Id);

        builder.Property(anomaly => anomaly.Id).ValueGeneratedNever();
        builder.Property(anomaly => anomaly.ReadingId).IsRequired();
        builder.Property(anomaly => anomaly.ThresholdId).IsRequired();
        builder.Property(anomaly => anomaly.SiteId).IsRequired();
        builder.Property(anomaly => anomaly.RoomId).IsRequired();
        builder.Property(anomaly => anomaly.DeviceGroupId).IsRequired();
        builder.Property(anomaly => anomaly.DeviceId).IsRequired();
        builder.Property(anomaly => anomaly.SensorId).IsRequired();
        builder.Property(anomaly => anomaly.ResourceType).HasConversion<string>().IsRequired().HasMaxLength(40);
        builder.Property(anomaly => anomaly.Value).HasPrecision(12, 2).IsRequired();
        builder.Property(anomaly => anomaly.LimitValue).HasPrecision(12, 2).IsRequired();
        builder.Property(anomaly => anomaly.Unit).IsRequired().HasMaxLength(20);
        builder.Property(anomaly => anomaly.Severity).HasConversion<string>().IsRequired().HasMaxLength(40);
        builder.Property(anomaly => anomaly.Status).HasConversion<string>().IsRequired().HasMaxLength(40);
        builder.Property(anomaly => anomaly.Description).IsRequired().HasMaxLength(320);
        builder.Property(anomaly => anomaly.DetectedAt).IsRequired();
        builder.Property(anomaly => anomaly.CreatedAt).IsRequired();
        builder.Property(anomaly => anomaly.UpdatedAt).IsRequired();

        builder.HasIndex(anomaly => anomaly.ReadingId);
        builder.HasIndex(anomaly => anomaly.SiteId);
        builder.HasIndex(anomaly => anomaly.RoomId);
        builder.HasIndex(anomaly => anomaly.DeviceGroupId);
        builder.HasIndex(anomaly => anomaly.DeviceId);
        builder.HasIndex(anomaly => anomaly.SensorId);
        builder.HasIndex(anomaly => anomaly.ResourceType);
        builder.HasIndex(anomaly => anomaly.Status);
        builder.HasIndex(anomaly => anomaly.Severity);
        builder.HasIndex(anomaly => anomaly.DetectedAt);
    }
}
