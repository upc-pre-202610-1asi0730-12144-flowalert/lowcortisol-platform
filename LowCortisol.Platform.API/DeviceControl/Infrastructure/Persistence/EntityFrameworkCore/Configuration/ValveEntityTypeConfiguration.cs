using LowCortisol.Platform.API.DeviceControl.Domain.Model.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LowCortisol.Platform.API.DeviceControl.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

public sealed class ValveEntityTypeConfiguration : IEntityTypeConfiguration<Valve>
{
    public void Configure(EntityTypeBuilder<Valve> builder)
    {
        builder.ToTable("valves");

        builder.HasKey(valve => valve.Id);

        builder.Property(valve => valve.Id).ValueGeneratedNever();
        builder.Property(valve => valve.DeviceId).IsRequired();
        builder.Property(valve => valve.Name).IsRequired().HasMaxLength(120);
        builder.Property(valve => valve.ResourceType).HasConversion<string>().IsRequired().HasMaxLength(40);
        builder.Property(valve => valve.Status).HasConversion<string>().IsRequired().HasMaxLength(40);
        builder.Property(valve => valve.CreatedAt).IsRequired();
        builder.Property(valve => valve.UpdatedAt).IsRequired();

        builder.HasIndex(valve => valve.DeviceId);
        builder.HasIndex(valve => valve.ResourceType);
        builder.HasIndex(valve => valve.Status);
        builder.HasIndex(valve => valve.CreatedAt);
    }
}
