using LowCortisol.Platform.API.Plan.Domain.Model.Aggregates;
using LowCortisol.Platform.API.Plan.Domain.Model.Entities;
using LowCortisol.Platform.API.Plan.Domain.Model.ReadModels;
using LowCortisol.Platform.API.Plan.Interfaces.Rest.Resources;
using PlanAggregate = LowCortisol.Platform.API.Plan.Domain.Model.Aggregates.Plan;

namespace LowCortisol.Platform.API.Plan.Interfaces.Rest.Transform;

public static class PlanResourceFromEntityAssembler
{
    public static PlanResource ToResourceFromEntity(PlanAggregate entity) =>
        new(
            entity.Id,
            entity.Code,
            entity.Name,
            entity.Description,
            entity.Price,
            entity.Currency,
            entity.BillingPeriod,
            entity.MaxSites,
            entity.MaxDevices,
            entity.IsRecommended,
            entity.IsActive,
            entity.Features.Select(PlanFeatureResourceFromEntityAssembler.ToResourceFromEntity).ToList(),
            entity.CreatedAt,
            entity.UpdatedAt);
}

public static class PlanFeatureResourceFromEntityAssembler
{
    public static PlanFeatureResource ToResourceFromEntity(PlanFeature entity) =>
        new(entity.Id, entity.PlanId, entity.Name, entity.Description, entity.CreatedAt, entity.UpdatedAt);
}

public static class SubscriptionResourceFromEntityAssembler
{
    public static SubscriptionResource ToResourceFromEntity(Subscription entity) =>
        new(
            entity.Id,
            entity.UserId,
            entity.WorkplaceId,
            entity.PlanId,
            entity.Status.ToString().ToLowerInvariant(),
            entity.StartedAt,
            entity.ExpiresAt,
            entity.AutoRenew,
            entity.CreatedAt,
            entity.UpdatedAt);
}

public static class PaymentResourceFromEntityAssembler
{
    public static PaymentResource ToResourceFromEntity(Payment entity) =>
        new(
            entity.Id,
            entity.SubscriptionId,
            entity.Amount,
            entity.Currency,
            entity.Method,
            entity.Status.ToString().ToLowerInvariant(),
            entity.PaidAt,
            entity.CreatedAt,
            entity.UpdatedAt);
}

public static class ServiceRequestResourceFromEntityAssembler
{
    public static ServiceRequestResource ToResourceFromEntity(ServiceRequest entity) =>
        new(
            entity.Id,
            entity.SubscriptionId,
            entity.Type,
            entity.Description,
            entity.Status.ToString().ToLowerInvariant(),
            entity.CreatedAt,
            entity.UpdatedAt);
}

public static class PlanSummaryResourceFromResultAssembler
{
    public static PlanSummaryResource ToResourceFromResult(PlanSummary summary) =>
        new(
            summary.TotalPlans,
            summary.ActivePlanName,
            summary.SubscriptionStatus,
            summary.TotalPayments,
            summary.TotalPaid,
            summary.ServiceRequest,
            summary.MaxSites,
            summary.MaxDevices,
            summary.UsedSites,
            summary.UsedDevices,
            summary.RemainingSites,
            summary.RemainingDevices);
}
