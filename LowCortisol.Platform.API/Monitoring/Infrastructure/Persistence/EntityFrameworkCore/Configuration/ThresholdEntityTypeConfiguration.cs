using LowCortisol.Platform.API.Monitoring.Domain.Model.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LowCortisol.Platform.API.Monitoring.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

public sealed class ThresholdEntityTypeConfiguration : IEntityTypeConfiguration<Threshold>
{
    public void Configure(EntityTypeBuilder<Threshold> builder)
    {
        builder.ToTable("thresholds");

        builder.HasKey(threshold => threshold.Id);

        builder.Property(threshold => threshold.Id).ValueGeneratedNever();
        builder.Property(threshold => threshold.SiteId);
        builder.Property(threshold => threshold.RoomId);
        builder.Property(threshold => threshold.DeviceGroupId);
        builder.Property(threshold => threshold.SensorId);
        builder.Property(threshold => threshold.ResourceType).HasConversion<string>().IsRequired().HasMaxLength(40);
        builder.Property(threshold => threshold.Name).IsRequired().HasMaxLength(120);
        builder.Property(threshold => threshold.Operator).HasConversion<string>().IsRequired().HasMaxLength(40);
        builder.Property(threshold => threshold.LimitValue).HasPrecision(12, 2).IsRequired();
        builder.Property(threshold => threshold.Unit).IsRequired().HasMaxLength(20);
        builder.Property(threshold => threshold.Severity).HasConversion<string>().IsRequired().HasMaxLength(40);
        builder.Property(threshold => threshold.IsActive).IsRequired();
        builder.Property(threshold => threshold.CreatedAt).IsRequired();
        builder.Property(threshold => threshold.UpdatedAt).IsRequired();

        builder.HasIndex(threshold => threshold.SiteId);
        builder.HasIndex(threshold => threshold.RoomId);
        builder.HasIndex(threshold => threshold.DeviceGroupId);
        builder.HasIndex(threshold => threshold.SensorId);
        builder.HasIndex(threshold => threshold.ResourceType);
        builder.HasIndex(threshold => threshold.IsActive);
        builder.HasIndex(threshold => threshold.Severity);
        builder.HasIndex(threshold => threshold.CreatedAt);
    }
}
