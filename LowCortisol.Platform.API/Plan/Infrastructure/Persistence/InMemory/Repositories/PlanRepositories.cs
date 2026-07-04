using LowCortisol.Platform.API.Plan.Domain.Model.Aggregates;
using LowCortisol.Platform.API.Plan.Domain.Model.Entities;
using LowCortisol.Platform.API.Plan.Domain.Model.ValueObjects;
using LowCortisol.Platform.API.Plan.Domain.Repositories;
using LowCortisol.Platform.API.Shared.Infrastructure.Persistence.InMemory;
using PlanAggregate = LowCortisol.Platform.API.Plan.Domain.Model.Aggregates.Plan;

namespace LowCortisol.Platform.API.Plan.Infrastructure.Persistence.InMemory.Repositories;

public sealed class PlanRepository : BaseRepository<PlanAggregate>, IPlanRepository
{
    private readonly List<PlanAggregate> _plans;

    public PlanRepository(AppDbContext context) : base(context.Plans)
    {
        _plans = context.Plans;
    }

    public Task<IReadOnlyCollection<PlanAggregate>> ListActiveAsync(bool recommendedOnly = false, CancellationToken cancellationToken = default) =>
        Task.FromResult<IReadOnlyCollection<PlanAggregate>>(
            _plans
                .Where(plan => plan.IsActive && (!recommendedOnly || plan.IsRecommended))
                .OrderBy(plan => plan.Price)
                .ToList());
}

public sealed class SubscriptionRepository : BaseRepository<Subscription>, ISubscriptionRepository
{
    private readonly List<Subscription> _subscriptions;

    public SubscriptionRepository(AppDbContext context) : base(context.Subscriptions)
    {
        _subscriptions = context.Subscriptions;
    }

    public Task<Subscription?> FindActiveByUserIdAsync(string userId, CancellationToken cancellationToken = default) =>
        Task.FromResult(_subscriptions.FirstOrDefault(subscription =>
            subscription.UserId == userId && subscription.Status == SubscriptionStatus.Active));

    public Task<IReadOnlyCollection<Subscription>> ListByUserIdAsync(string userId, CancellationToken cancellationToken = default) =>
        Task.FromResult<IReadOnlyCollection<Subscription>>(
            _subscriptions
                .Where(subscription => subscription.UserId == userId)
                .OrderByDescending(subscription => subscription.CreatedAt)
                .ToList());
}

public sealed class PaymentRepository : BaseRepository<Payment>, IPaymentRepository
{
    private readonly List<Payment> _payments;

    public PaymentRepository(AppDbContext context) : base(context.Payments)
    {
        _payments = context.Payments;
    }

    public Task<IReadOnlyCollection<Payment>> ListBySubscriptionIdsAsync(IEnumerable<Guid> subscriptionIds, CancellationToken cancellationToken = default)
    {
        var ids = subscriptionIds.ToHashSet();
        return Task.FromResult<IReadOnlyCollection<Payment>>(
            _payments
                .Where(payment => ids.Contains(payment.SubscriptionId))
                .OrderByDescending(payment => payment.CreatedAt)
                .ToList());
    }
}

public sealed class ServiceRequestRepository : BaseRepository<ServiceRequest>, IServiceRequestRepository
{
    private readonly List<ServiceRequest> _serviceRequests;

    public ServiceRequestRepository(AppDbContext context) : base(context.ServiceRequests)
    {
        _serviceRequests = context.ServiceRequests;
    }

    public Task<IReadOnlyCollection<ServiceRequest>> ListBySubscriptionIdsAsync(IEnumerable<Guid> subscriptionIds, CancellationToken cancellationToken = default)
    {
        var ids = subscriptionIds.ToHashSet();
        return Task.FromResult<IReadOnlyCollection<ServiceRequest>>(
            _serviceRequests
                .Where(request => ids.Contains(request.SubscriptionId))
                .OrderByDescending(request => request.CreatedAt)
                .ToList());
    }
}
