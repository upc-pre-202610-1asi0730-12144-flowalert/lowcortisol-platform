using LowCortisol.Platform.API.DeviceControl.Domain.Model.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LowCortisol.Platform.API.DeviceControl.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

public sealed class DeviceCommandEntityTypeConfiguration : IEntityTypeConfiguration<DeviceCommand>
{
    public void Configure(EntityTypeBuilder<DeviceCommand> builder)
    {
        builder.ToTable("device_commands");

        builder.HasKey(command => command.Id);

        builder.Property(command => command.Id).ValueGeneratedNever();
        builder.Property(command => command.DeviceId).IsRequired();
        builder.Property(command => command.ValveId);
        builder.Property(command => command.SiteId).IsRequired();
        builder.Property(command => command.RoomId).IsRequired();
        builder.Property(command => command.DeviceGroupId).IsRequired();
        builder.Property(command => command.IncidentId);
        builder.Property(command => command.CommandType).HasConversion<string>().IsRequired().HasMaxLength(40);
        builder.Property(command => command.Status).HasConversion<string>().IsRequired().HasMaxLength(40);
        builder.Property(command => command.Source).HasConversion<string>().IsRequired().HasMaxLength(40);
        builder.Property(command => command.Reason).IsRequired().HasMaxLength(240);
        builder.Property(command => command.RequestedBy).IsRequired().HasMaxLength(140);
        builder.Property(command => command.RequestedAt).IsRequired();
        builder.Property(command => command.ExecutedAt);
        builder.Property(command => command.FailedAt);
        builder.Property(command => command.FailureReason).HasMaxLength(320);
        builder.Property(command => command.CreatedAt).IsRequired();
        builder.Property(command => command.UpdatedAt).IsRequired();

        builder.HasMany(command => command.Executions)
            .WithOne()
            .HasForeignKey(execution => execution.DeviceCommandId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(command => command.AuditEntries)
            .WithOne()
            .HasForeignKey(entry => entry.DeviceCommandId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(command => command.Executions).UsePropertyAccessMode(PropertyAccessMode.Field);
        builder.Navigation(command => command.AuditEntries).UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.HasIndex(command => command.DeviceId);
        builder.HasIndex(command => command.ValveId);
        builder.HasIndex(command => command.IncidentId);
        builder.HasIndex(command => command.SiteId);
        builder.HasIndex(command => command.RoomId);
        builder.HasIndex(command => command.DeviceGroupId);
        builder.HasIndex(command => command.Status);
        builder.HasIndex(command => command.CommandType);
        builder.HasIndex(command => command.RequestedAt);
        builder.HasIndex(command => command.CreatedAt);
    }
}
