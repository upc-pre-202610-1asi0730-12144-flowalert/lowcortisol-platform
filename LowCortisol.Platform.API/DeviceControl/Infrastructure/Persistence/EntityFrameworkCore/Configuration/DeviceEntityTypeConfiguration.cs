using LowCortisol.Platform.API.DeviceControl.Domain.Model.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LowCortisol.Platform.API.DeviceControl.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

public sealed class DeviceEntityTypeConfiguration : IEntityTypeConfiguration<Device>
{
    public void Configure(EntityTypeBuilder<Device> builder)
    {
        builder.ToTable("devices");

        builder.HasKey(device => device.Id);

        builder.Property(device => device.Id).ValueGeneratedNever();
        builder.Property(device => device.Name).IsRequired().HasMaxLength(120);
        builder.Property(device => device.SerialNumber).IsRequired().HasMaxLength(80);
        builder.Property(device => device.Type).HasConversion<string>().IsRequired().HasMaxLength(40);
        builder.Property(device => device.Status).HasConversion<string>().IsRequired().HasMaxLength(40);
        builder.Property(device => device.DeviceGroupId);
        builder.Property(device => device.CreatedAt).IsRequired();
        builder.Property(device => device.UpdatedAt).IsRequired();

        builder.HasIndex(device => device.SerialNumber).IsUnique();
        builder.HasIndex(device => device.DeviceGroupId);
        builder.HasIndex(device => device.Type);
        builder.HasIndex(device => device.Status);
        builder.HasIndex(device => device.CreatedAt);
    }
}
