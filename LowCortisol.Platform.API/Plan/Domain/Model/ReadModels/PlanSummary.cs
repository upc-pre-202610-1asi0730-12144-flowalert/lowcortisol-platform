namespace LowCortisol.Platform.API.Plan.Domain.Model.ReadModels;

public record PlanSummary(
    int TotalPlans,
    string ActivePlanName,
    string SubscriptionStatus,
    int TotalPayments,
    decimal TotalPaid,
    int ServiceRequest,
    int MaxSites,
    int MaxDevices,
    int UsedSites,
    int UsedDevices,
    int RemainingSites,
    int RemainingDevices);
