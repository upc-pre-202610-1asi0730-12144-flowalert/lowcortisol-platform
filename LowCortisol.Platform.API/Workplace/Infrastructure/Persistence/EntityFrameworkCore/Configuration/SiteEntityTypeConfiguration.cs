using LowCortisol.Platform.API.Workplace.Domain.Model.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LowCortisol.Platform.API.Workplace.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

public sealed class SiteEntityTypeConfiguration : IEntityTypeConfiguration<Site>
{
    public void Configure(EntityTypeBuilder<Site> builder)
    {
        builder.ToTable("sites");

        builder.HasKey(site => site.Id);

        builder.Property(site => site.Id).ValueGeneratedNever();
        builder.Property(site => site.Name).IsRequired().HasMaxLength(120);
        builder.Property(site => site.Address).HasMaxLength(240);
        builder.Property(site => site.Type).HasConversion<string>().IsRequired().HasMaxLength(40);
        builder.Property(site => site.Status).HasConversion<string>().IsRequired().HasMaxLength(40);
        builder.Property(site => site.CreatedAt).IsRequired();
        builder.Property(site => site.UpdatedAt).IsRequired();

        builder.HasIndex(site => site.Name).IsUnique();
        builder.HasIndex(site => site.Status);
        builder.HasIndex(site => site.CreatedAt);

        builder.HasMany(site => site.Rooms)
            .WithOne()
            .HasForeignKey(room => room.SiteId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(site => site.Rooms).UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
