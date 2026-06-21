using LowCortisol.Platform.API.Notification.Domain.Model.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LowCortisol.Platform.API.Notification.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

public sealed class NotificationChannelEntityTypeConfiguration : IEntityTypeConfiguration<NotificationChannel>
{
    public void Configure(EntityTypeBuilder<NotificationChannel> builder)
    {
        builder.ToTable("notification_channels");

        builder.HasKey(channel => channel.Id);

        builder.Property(channel => channel.Id).ValueGeneratedNever();
        builder.Property(channel => channel.Name).IsRequired().HasMaxLength(140);
        builder.Property(channel => channel.Type).HasConversion<string>().IsRequired().HasMaxLength(40);
        builder.Property(channel => channel.IsActive).IsRequired();
        builder.Property(channel => channel.CreatedAt).IsRequired();
        builder.Property(channel => channel.UpdatedAt).IsRequired();

        builder.HasIndex(channel => channel.Type);
        builder.HasIndex(channel => channel.IsActive);
        builder.HasIndex(channel => channel.CreatedAt);
    }
}
