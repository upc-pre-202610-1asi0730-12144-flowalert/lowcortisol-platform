using LowCortisol.Platform.API.Plan.Domain.Model.Aggregates;
using LowCortisol.Platform.API.Plan.Domain.Model.Entities;
using LowCortisol.Platform.API.Plan.Domain.Model.ValueObjects;
using LowCortisol.Platform.API.Plan.Domain.Repositories;
using LowCortisol.Platform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using LowCortisol.Platform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;
using PlanAggregate = LowCortisol.Platform.API.Plan.Domain.Model.Aggregates.Plan;

namespace LowCortisol.Platform.API.Plan.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public sealed class PlanRepository : BaseRepository<PlanAggregate>, IPlanRepository
{
    public PlanRepository(AppDbContext context) : base(context)
    {
    }

    public new Task<PlanAggregate?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        Context.Plans
            .Include(plan => plan.Features)
            .FirstOrDefaultAsync(plan => plan.Id == id, cancellationToken);

    public async Task<IReadOnlyCollection<PlanAggregate>> ListActiveAsync(bool recommendedOnly = false, CancellationToken cancellationToken = default) =>
        await Context.Plans
            .Include(plan => plan.Features)
            .Where(plan => plan.IsActive && (!recommendedOnly || plan.IsRecommended))
            .OrderBy(plan => plan.Price)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
}

public sealed class SubscriptionRepository : BaseRepository<Subscription>, ISubscriptionRepository
{
    public SubscriptionRepository(AppDbContext context) : base(context)
    {
    }

    public Task<Subscription?> FindActiveByUserIdAsync(string userId, CancellationToken cancellationToken = default) =>
        Context.Subscriptions
            .FirstOrDefaultAsync(
                subscription => subscription.UserId == userId && subscription.Status == SubscriptionStatus.Active,
                cancellationToken);

    public async Task<IReadOnlyCollection<Subscription>> ListByUserIdAsync(string userId, CancellationToken cancellationToken = default) =>
        await Context.Subscriptions
            .Where(subscription => subscription.UserId == userId)
            .OrderByDescending(subscription => subscription.CreatedAt)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
}

public sealed class PaymentRepository : BaseRepository<Payment>, IPaymentRepository
{
    public PaymentRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IReadOnlyCollection<Payment>> ListBySubscriptionIdsAsync(IEnumerable<Guid> subscriptionIds, CancellationToken cancellationToken = default)
    {
        var ids = subscriptionIds.ToArray();
        return await Context.Payments
            .Where(payment => ids.Contains(payment.SubscriptionId))
            .OrderByDescending(payment => payment.CreatedAt)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
}

public sealed class ServiceRequestRepository : BaseRepository<ServiceRequest>, IServiceRequestRepository
{
    public ServiceRequestRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IReadOnlyCollection<ServiceRequest>> ListBySubscriptionIdsAsync(IEnumerable<Guid> subscriptionIds, CancellationToken cancellationToken = default)
    {
        var ids = subscriptionIds.ToArray();
        return await Context.ServiceRequests
            .Where(request => ids.Contains(request.SubscriptionId))
            .OrderByDescending(request => request.CreatedAt)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
}
