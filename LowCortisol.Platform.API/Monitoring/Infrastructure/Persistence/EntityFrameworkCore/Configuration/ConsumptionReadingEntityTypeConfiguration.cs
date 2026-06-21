using LowCortisol.Platform.API.Monitoring.Domain.Model.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LowCortisol.Platform.API.Monitoring.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

public sealed class ConsumptionReadingEntityTypeConfiguration : IEntityTypeConfiguration<ConsumptionReading>
{
    public void Configure(EntityTypeBuilder<ConsumptionReading> builder)
    {
        builder.ToTable("consumption_readings");

        builder.HasKey(reading => reading.Id);

        builder.Property(reading => reading.Id).ValueGeneratedNever();
        builder.Property(reading => reading.SiteId).IsRequired();
        builder.Property(reading => reading.RoomId).IsRequired();
        builder.Property(reading => reading.DeviceGroupId).IsRequired();
        builder.Property(reading => reading.DeviceId).IsRequired();
        builder.Property(reading => reading.SensorId).IsRequired();
        builder.Property(reading => reading.ResourceType).HasConversion<string>().IsRequired().HasMaxLength(40);
        builder.Property(reading => reading.Value).HasPrecision(12, 2).IsRequired();
        builder.Property(reading => reading.Unit).IsRequired().HasMaxLength(20);
        builder.Property(reading => reading.CapturedAt).IsRequired();
        builder.Property(reading => reading.Status).HasConversion<string>().IsRequired().HasMaxLength(40);
        builder.Property(reading => reading.CreatedAt).IsRequired();
        builder.Property(reading => reading.UpdatedAt).IsRequired();

        builder.HasIndex(reading => reading.SiteId);
        builder.HasIndex(reading => reading.RoomId);
        builder.HasIndex(reading => reading.DeviceGroupId);
        builder.HasIndex(reading => reading.DeviceId);
        builder.HasIndex(reading => reading.SensorId);
        builder.HasIndex(reading => reading.ResourceType);
        builder.HasIndex(reading => reading.Status);
        builder.HasIndex(reading => reading.CapturedAt);
    }
}
