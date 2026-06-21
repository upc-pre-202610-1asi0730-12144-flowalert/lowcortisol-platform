using LowCortisol.Platform.API.DeviceControl.Domain.Model.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LowCortisol.Platform.API.DeviceControl.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

public sealed class SensorEntityTypeConfiguration : IEntityTypeConfiguration<Sensor>
{
    public void Configure(EntityTypeBuilder<Sensor> builder)
    {
        builder.ToTable("sensors");

        builder.HasKey(sensor => sensor.Id);

        builder.Property(sensor => sensor.Id).ValueGeneratedNever();
        builder.Property(sensor => sensor.DeviceId).IsRequired();
        builder.Property(sensor => sensor.Name).IsRequired().HasMaxLength(120);
        builder.Property(sensor => sensor.ResourceType).HasConversion<string>().IsRequired().HasMaxLength(40);
        builder.Property(sensor => sensor.Status).HasConversion<string>().IsRequired().HasMaxLength(40);
        builder.Property(sensor => sensor.LastReadingValue).HasPrecision(12, 2);
        builder.Property(sensor => sensor.CreatedAt).IsRequired();
        builder.Property(sensor => sensor.UpdatedAt).IsRequired();

        builder.HasIndex(sensor => sensor.DeviceId);
        builder.HasIndex(sensor => sensor.ResourceType);
        builder.HasIndex(sensor => sensor.Status);
        builder.HasIndex(sensor => sensor.CreatedAt);
    }
}
