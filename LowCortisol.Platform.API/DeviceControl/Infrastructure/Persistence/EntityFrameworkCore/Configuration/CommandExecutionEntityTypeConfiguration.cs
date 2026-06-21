using LowCortisol.Platform.API.DeviceControl.Domain.Model.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LowCortisol.Platform.API.DeviceControl.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

public sealed class CommandExecutionEntityTypeConfiguration : IEntityTypeConfiguration<CommandExecution>
{
    public void Configure(EntityTypeBuilder<CommandExecution> builder)
    {
        builder.ToTable("command_executions");

        builder.HasKey(execution => execution.Id);

        builder.Property(execution => execution.Id).ValueGeneratedNever();
        builder.Property(execution => execution.DeviceCommandId).IsRequired();
        builder.Property(execution => execution.Status).HasConversion<string>().IsRequired().HasMaxLength(40);
        builder.Property(execution => execution.StartedAt).IsRequired();
        builder.Property(execution => execution.FinishedAt);
        builder.Property(execution => execution.ResultMessage).IsRequired().HasMaxLength(320);
        builder.Property(execution => execution.FailureReason).HasMaxLength(320);
        builder.Property(execution => execution.CreatedAt).IsRequired();
        builder.Property(execution => execution.UpdatedAt).IsRequired();

        builder.HasIndex(execution => execution.DeviceCommandId);
        builder.HasIndex(execution => execution.Status);
        builder.HasIndex(execution => execution.StartedAt);
    }
}
