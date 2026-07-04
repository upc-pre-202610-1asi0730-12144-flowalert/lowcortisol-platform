using LowCortisol.Platform.API.Plan.Domain.Model.Aggregates;
using LowCortisol.Platform.API.Plan.Domain.Model.Entities;
using LowCortisol.Platform.API.Shared.Domain.Repositories;
using PlanAggregate = LowCortisol.Platform.API.Plan.Domain.Model.Aggregates.Plan;

namespace LowCortisol.Platform.API.Plan.Domain.Repositories;

public interface IPlanRepository : IBaseRepository<PlanAggregate>
{
    Task<IReadOnlyCollection<PlanAggregate>> ListActiveAsync(bool recommendedOnly = false, CancellationToken cancellationToken = default);
}

public interface ISubscriptionRepository : IBaseRepository<Subscription>
{
    Task<Subscription?> FindActiveByUserIdAsync(string userId, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<Subscription>> ListByUserIdAsync(string userId, CancellationToken cancellationToken = default);
}

public interface IPaymentRepository : IBaseRepository<Payment>
{
    Task<IReadOnlyCollection<Payment>> ListBySubscriptionIdsAsync(IEnumerable<Guid> subscriptionIds, CancellationToken cancellationToken = default);
}

public interface IServiceRequestRepository : IBaseRepository<ServiceRequest>
{
    Task<IReadOnlyCollection<ServiceRequest>> ListBySubscriptionIdsAsync(IEnumerable<Guid> subscriptionIds, CancellationToken cancellationToken = default);
}
