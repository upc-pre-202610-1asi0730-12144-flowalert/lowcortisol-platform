using LowCortisol.Platform.API.DeviceControl.Domain.Model.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LowCortisol.Platform.API.DeviceControl.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

public sealed class ValveOperationEntityTypeConfiguration : IEntityTypeConfiguration<ValveOperation>
{
    public void Configure(EntityTypeBuilder<ValveOperation> builder)
    {
        builder.ToTable("valve_operations");

        builder.HasKey(operation => operation.Id);

        builder.Property(operation => operation.Id).ValueGeneratedNever();
        builder.Property(operation => operation.ValveId).IsRequired();
        builder.Property(operation => operation.DeviceId).IsRequired();
        builder.Property(operation => operation.SiteId).IsRequired();
        builder.Property(operation => operation.RoomId).IsRequired();
        builder.Property(operation => operation.DeviceGroupId).IsRequired();
        builder.Property(operation => operation.IncidentId);
        builder.Property(operation => operation.ResourceType).HasConversion<string>().IsRequired().HasMaxLength(40);
        builder.Property(operation => operation.PreviousStatus).HasConversion<string>().IsRequired().HasMaxLength(40);
        builder.Property(operation => operation.TargetStatus).HasConversion<string>().IsRequired().HasMaxLength(40);
        builder.Property(operation => operation.Reason).HasConversion<string>().IsRequired().HasMaxLength(40);
        builder.Property(operation => operation.Source).HasConversion<string>().IsRequired().HasMaxLength(40);
        builder.Property(operation => operation.Status).HasConversion<string>().IsRequired().HasMaxLength(40);
        builder.Property(operation => operation.RequestedAt).IsRequired();
        builder.Property(operation => operation.CompletedAt);
        builder.Property(operation => operation.FailedAt);
        builder.Property(operation => operation.FailureReason).HasMaxLength(320);
        builder.Property(operation => operation.CreatedAt).IsRequired();
        builder.Property(operation => operation.UpdatedAt).IsRequired();

        builder.HasIndex(operation => operation.DeviceId);
        builder.HasIndex(operation => operation.ValveId);
        builder.HasIndex(operation => operation.IncidentId);
        builder.HasIndex(operation => operation.SiteId);
        builder.HasIndex(operation => operation.RoomId);
        builder.HasIndex(operation => operation.DeviceGroupId);
        builder.HasIndex(operation => operation.Status);
        builder.HasIndex(operation => operation.Reason);
        builder.HasIndex(operation => operation.RequestedAt);
        builder.HasIndex(operation => operation.CreatedAt);
    }
}
