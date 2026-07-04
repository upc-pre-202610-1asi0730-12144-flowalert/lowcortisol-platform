namespace LowCortisol.Platform.API.Plan.Domain.Model.ValueObjects;

public enum SubscriptionStatus
{
    Active,
    Cancelled
}

public enum PaymentStatus
{
    Pending,
    Paid,
    Failed
}

public enum ServiceRequestStatus
{
    Open,
    Resolved,
    Closed
}
