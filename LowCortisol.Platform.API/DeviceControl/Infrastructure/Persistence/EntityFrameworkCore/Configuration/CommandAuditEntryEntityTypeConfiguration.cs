using LowCortisol.Platform.API.DeviceControl.Domain.Model.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LowCortisol.Platform.API.DeviceControl.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

public sealed class CommandAuditEntryEntityTypeConfiguration : IEntityTypeConfiguration<CommandAuditEntry>
{
    public void Configure(EntityTypeBuilder<CommandAuditEntry> builder)
    {
        builder.ToTable("command_audit_entries");

        builder.HasKey(entry => entry.Id);

        builder.Property(entry => entry.Id).ValueGeneratedNever();
        builder.Property(entry => entry.DeviceCommandId).IsRequired();
        builder.Property(entry => entry.Action).IsRequired().HasMaxLength(80);
        builder.Property(entry => entry.Description).IsRequired().HasMaxLength(520);
        builder.Property(entry => entry.PerformedBy).IsRequired().HasMaxLength(140);
        builder.Property(entry => entry.PerformedAt).IsRequired();
        builder.Property(entry => entry.CreatedAt).IsRequired();
        builder.Property(entry => entry.UpdatedAt).IsRequired();

        builder.HasIndex(entry => entry.DeviceCommandId);
        builder.HasIndex(entry => entry.Action);
        builder.HasIndex(entry => entry.PerformedAt);
    }
}
