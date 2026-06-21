using LowCortisol.Platform.API.Workplace.Domain.Model.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LowCortisol.Platform.API.Workplace.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

public sealed class DeviceGroupEntityTypeConfiguration : IEntityTypeConfiguration<DeviceGroup>
{
    public void Configure(EntityTypeBuilder<DeviceGroup> builder)
    {
        builder.ToTable("device_groups");

        builder.HasKey(group => group.Id);

        builder.Property(group => group.Id).ValueGeneratedNever();
        builder.Property(group => group.RoomId).IsRequired();
        builder.Property(group => group.Name).IsRequired().HasMaxLength(120);
        builder.Property(group => group.ResourceType).HasConversion<string>().IsRequired().HasMaxLength(40);
        builder.Property(group => group.Status).HasConversion<string>().IsRequired().HasMaxLength(40);
        builder.Property(group => group.CreatedAt).IsRequired();
        builder.Property(group => group.UpdatedAt).IsRequired();

        builder.HasIndex(group => new { group.RoomId, group.Name }).IsUnique();
        builder.HasIndex(group => group.RoomId);
        builder.HasIndex(group => group.ResourceType);
        builder.HasIndex(group => group.Status);
        builder.HasIndex(group => group.CreatedAt);
    }
}
