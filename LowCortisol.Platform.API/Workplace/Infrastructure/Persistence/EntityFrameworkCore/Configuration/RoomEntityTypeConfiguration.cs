using LowCortisol.Platform.API.Workplace.Domain.Model.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LowCortisol.Platform.API.Workplace.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

public sealed class RoomEntityTypeConfiguration : IEntityTypeConfiguration<Room>
{
    public void Configure(EntityTypeBuilder<Room> builder)
    {
        builder.ToTable("rooms");

        builder.HasKey(room => room.Id);

        builder.Property(room => room.Id).ValueGeneratedNever();
        builder.Property(room => room.SiteId).IsRequired();
        builder.Property(room => room.Name).IsRequired().HasMaxLength(120);
        builder.Property(room => room.Type).HasConversion<string>().IsRequired().HasMaxLength(40);
        builder.Property(room => room.Status).HasConversion<string>().IsRequired().HasMaxLength(40);
        builder.Property(room => room.CreatedAt).IsRequired();
        builder.Property(room => room.UpdatedAt).IsRequired();

        builder.HasIndex(room => new { room.SiteId, room.Name }).IsUnique();
        builder.HasIndex(room => room.SiteId);
        builder.HasIndex(room => room.Status);
        builder.HasIndex(room => room.CreatedAt);

        builder.HasMany(room => room.DeviceGroups)
            .WithOne()
            .HasForeignKey(group => group.RoomId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(room => room.DeviceGroups).UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
