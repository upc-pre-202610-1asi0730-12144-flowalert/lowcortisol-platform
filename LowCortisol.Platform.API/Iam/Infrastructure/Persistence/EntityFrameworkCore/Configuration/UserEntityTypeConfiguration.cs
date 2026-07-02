using LowCortisol.Platform.API.Iam.Domain.Model.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LowCortisol.Platform.API.Iam.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

public sealed class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");
        builder.HasKey(user => user.Id);

        builder.Property(user => user.Id).ValueGeneratedNever();
        builder.Property(user => user.FirstName).HasMaxLength(80).IsRequired();
        builder.Property(user => user.LastName).HasMaxLength(80).IsRequired();
        builder.Property(user => user.Email).HasMaxLength(160).IsRequired();
        builder.Property(user => user.PasswordHash).HasMaxLength(255).IsRequired();
        builder.Property(user => user.Role).HasMaxLength(40).IsRequired();
        builder.Property(user => user.IsActive).IsRequired();
        builder.Property(user => user.CreatedAt).IsRequired();
        builder.Property(user => user.UpdatedAt).IsRequired();

        builder.HasIndex(user => user.Email).IsUnique();
    }
}
