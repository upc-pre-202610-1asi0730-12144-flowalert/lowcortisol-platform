using LowCortisol.Platform.API.Notification.Domain.Model.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LowCortisol.Platform.API.Notification.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

public sealed class IncidentAssignmentEntityTypeConfiguration : IEntityTypeConfiguration<IncidentAssignment>
{
    public void Configure(EntityTypeBuilder<IncidentAssignment> builder)
    {
        builder.ToTable("incident_assignments");

        builder.HasKey(assignment => assignment.Id);

        builder.Property(assignment => assignment.Id).ValueGeneratedNever();
        builder.Property(assignment => assignment.IncidentId).IsRequired();
        builder.Property(assignment => assignment.AssigneeId).IsRequired().HasMaxLength(80);
        builder.Property(assignment => assignment.AssigneeName).IsRequired().HasMaxLength(140);
        builder.Property(assignment => assignment.AssignedAt).IsRequired();
        builder.Property(assignment => assignment.IsActive).IsRequired();
        builder.Property(assignment => assignment.CreatedAt).IsRequired();
        builder.Property(assignment => assignment.UpdatedAt).IsRequired();

        builder.HasIndex(assignment => assignment.IncidentId);
        builder.HasIndex(assignment => assignment.AssigneeId);
        builder.HasIndex(assignment => assignment.IsActive);
        builder.HasIndex(assignment => assignment.AssignedAt);
    }
}
