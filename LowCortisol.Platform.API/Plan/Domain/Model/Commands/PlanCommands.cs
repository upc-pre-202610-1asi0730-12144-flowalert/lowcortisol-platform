namespace LowCortisol.Platform.API.Plan.Domain.Model.Commands;

public record SubscribeToPlanCommand(string UserId, string WorkplaceId, Guid PlanId, string PaymentMethod);
public record ChangePlanCommand(string UserId, Guid NewPlanId);
public record CancelSubscriptionCommand(string UserId, string Reason);
public record ResolveServiceRequestCommand(Guid ServiceRequestId);
public record CloseServiceRequestCommand(Guid ServiceRequestId);
