using LowCortisol.Platform.API.Plan.Domain.Model.Aggregates;
using LowCortisol.Platform.API.Plan.Domain.Model.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlanAggregate = LowCortisol.Platform.API.Plan.Domain.Model.Aggregates.Plan;

namespace LowCortisol.Platform.API.Plan.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

public sealed class PlanEntityTypeConfiguration : IEntityTypeConfiguration<PlanAggregate>
{
    public void Configure(EntityTypeBuilder<PlanAggregate> builder)
    {
        builder.ToTable("plans");
        builder.HasKey(plan => plan.Id);
        builder.Property(plan => plan.Id).ValueGeneratedNever();
        builder.Property(plan => plan.Code).IsRequired().HasMaxLength(40);
        builder.Property(plan => plan.Name).IsRequired().HasMaxLength(120);
        builder.Property(plan => plan.Description).IsRequired().HasMaxLength(320);
        builder.Property(plan => plan.Price).HasPrecision(12, 2).IsRequired();
        builder.Property(plan => plan.Currency).IsRequired().HasMaxLength(12);
        builder.Property(plan => plan.BillingPeriod).IsRequired().HasMaxLength(40);
        builder.Property(plan => plan.MaxSites).IsRequired();
        builder.Property(plan => plan.MaxDevices).IsRequired();
        builder.Property(plan => plan.IsRecommended).IsRequired();
        builder.Property(plan => plan.IsActive).IsRequired();
        builder.Property(plan => plan.CreatedAt).IsRequired();
        builder.Property(plan => plan.UpdatedAt).IsRequired();

        builder.HasMany(plan => plan.Features)
            .WithOne()
            .HasForeignKey(feature => feature.PlanId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(plan => plan.Features).UsePropertyAccessMode(PropertyAccessMode.Field);
        builder.HasIndex(plan => plan.Code).IsUnique();
        builder.HasIndex(plan => plan.IsActive);
        builder.HasIndex(plan => plan.IsRecommended);
    }
}

public sealed class PlanFeatureEntityTypeConfiguration : IEntityTypeConfiguration<PlanFeature>
{
    public void Configure(EntityTypeBuilder<PlanFeature> builder)
    {
        builder.ToTable("plan_features");
        builder.HasKey(feature => feature.Id);
        builder.Property(feature => feature.Id).ValueGeneratedNever();
        builder.Property(feature => feature.PlanId).IsRequired();
        builder.Property(feature => feature.Name).IsRequired().HasMaxLength(160);
        builder.Property(feature => feature.Description).IsRequired().HasMaxLength(320);
        builder.Property(feature => feature.CreatedAt).IsRequired();
        builder.Property(feature => feature.UpdatedAt).IsRequired();
        builder.HasIndex(feature => feature.PlanId);
    }
}

public sealed class SubscriptionEntityTypeConfiguration : IEntityTypeConfiguration<Subscription>
{
    public void Configure(EntityTypeBuilder<Subscription> builder)
    {
        builder.ToTable("subscriptions");
        builder.HasKey(subscription => subscription.Id);
        builder.Property(subscription => subscription.Id).ValueGeneratedNever();
        builder.Property(subscription => subscription.UserId).IsRequired().HasMaxLength(80);
        builder.Property(subscription => subscription.WorkplaceId).IsRequired().HasMaxLength(80);
        builder.Property(subscription => subscription.PlanId).IsRequired();
        builder.Property(subscription => subscription.Status).HasConversion<string>().IsRequired().HasMaxLength(40);
        builder.Property(subscription => subscription.StartedAt).IsRequired();
        builder.Property(subscription => subscription.ExpiresAt);
        builder.Property(subscription => subscription.AutoRenew).IsRequired();
        builder.Property(subscription => subscription.CreatedAt).IsRequired();
        builder.Property(subscription => subscription.UpdatedAt).IsRequired();
        builder.HasIndex(subscription => subscription.UserId);
        builder.HasIndex(subscription => subscription.PlanId);
        builder.HasIndex(subscription => subscription.Status);
    }
}

public sealed class PaymentEntityTypeConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.ToTable("payments");
        builder.HasKey(payment => payment.Id);
        builder.Property(payment => payment.Id).ValueGeneratedNever();
        builder.Property(payment => payment.SubscriptionId).IsRequired();
        builder.Property(payment => payment.Amount).HasPrecision(12, 2).IsRequired();
        builder.Property(payment => payment.Currency).IsRequired().HasMaxLength(12);
        builder.Property(payment => payment.Method).IsRequired().HasMaxLength(40);
        builder.Property(payment => payment.Status).HasConversion<string>().IsRequired().HasMaxLength(40);
        builder.Property(payment => payment.PaidAt);
        builder.Property(payment => payment.CreatedAt).IsRequired();
        builder.Property(payment => payment.UpdatedAt).IsRequired();
        builder.HasIndex(payment => payment.SubscriptionId);
        builder.HasIndex(payment => payment.Status);
    }
}

public sealed class ServiceRequestEntityTypeConfiguration : IEntityTypeConfiguration<ServiceRequest>
{
    public void Configure(EntityTypeBuilder<ServiceRequest> builder)
    {
        builder.ToTable("service_requests");
        builder.HasKey(request => request.Id);
        builder.Property(request => request.Id).ValueGeneratedNever();
        builder.Property(request => request.SubscriptionId).IsRequired();
        builder.Property(request => request.Type).IsRequired().HasMaxLength(80);
        builder.Property(request => request.Description).IsRequired().HasMaxLength(640);
        builder.Property(request => request.Status).HasConversion<string>().IsRequired().HasMaxLength(40);
        builder.Property(request => request.CreatedAt).IsRequired();
        builder.Property(request => request.UpdatedAt).IsRequired();
        builder.HasIndex(request => request.SubscriptionId);
        builder.HasIndex(request => request.Status);
    }
}
