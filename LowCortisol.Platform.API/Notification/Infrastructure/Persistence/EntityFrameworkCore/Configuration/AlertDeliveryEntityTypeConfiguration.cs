using LowCortisol.Platform.API.Notification.Domain.Model.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LowCortisol.Platform.API.Notification.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

public sealed class AlertDeliveryEntityTypeConfiguration : IEntityTypeConfiguration<AlertDelivery>
{
    public void Configure(EntityTypeBuilder<AlertDelivery> builder)
    {
        builder.ToTable("alert_deliveries");

        builder.HasKey(delivery => delivery.Id);

        builder.Property(delivery => delivery.Id).ValueGeneratedNever();
        builder.Property(delivery => delivery.AlertId).IsRequired();
        builder.Property(delivery => delivery.ChannelId).IsRequired();
        builder.Property(delivery => delivery.ChannelType).HasConversion<string>().IsRequired().HasMaxLength(40);
        builder.Property(delivery => delivery.Status).HasConversion<string>().IsRequired().HasMaxLength(40);
        builder.Property(delivery => delivery.AttemptedAt).IsRequired();
        builder.Property(delivery => delivery.SentAt);
        builder.Property(delivery => delivery.FailureReason).IsRequired().HasMaxLength(240);
        builder.Property(delivery => delivery.CreatedAt).IsRequired();
        builder.Property(delivery => delivery.UpdatedAt).IsRequired();

        builder.OwnsOne(delivery => delivery.Recipient, recipient =>
        {
            recipient.WithOwner().HasForeignKey("AlertDeliveryId");
            recipient.Property<Guid>("AlertDeliveryId").HasColumnName("id");
            recipient.Property(item => item.UserId).HasColumnName("recipient_user_id").HasMaxLength(80);
            recipient.Property(item => item.Email).HasColumnName("recipient_email").HasMaxLength(160);
            recipient.Property(item => item.DisplayName).HasColumnName("recipient_display_name").IsRequired().HasMaxLength(140);
        });

        builder.OwnsOne(delivery => delivery.Message, message =>
        {
            message.WithOwner().HasForeignKey("AlertDeliveryId");
            message.Property<Guid>("AlertDeliveryId").HasColumnName("id");
            message.Property(item => item.Title).HasColumnName("message_title").IsRequired().HasMaxLength(160);
            message.Property(item => item.Description).HasColumnName("message_description").IsRequired().HasMaxLength(520);
        });

        builder.HasIndex(delivery => delivery.AlertId);
        builder.HasIndex(delivery => delivery.ChannelId);
        builder.HasIndex(delivery => delivery.ChannelType);
        builder.HasIndex(delivery => delivery.Status);
        builder.HasIndex(delivery => delivery.AttemptedAt);
    }
}
