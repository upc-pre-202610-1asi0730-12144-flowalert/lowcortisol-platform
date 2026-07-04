using LowCortisol.Platform.API.Plan.Application.QueryServices;
using LowCortisol.Platform.API.Plan.Domain.Model.Aggregates;
using LowCortisol.Platform.API.Plan.Domain.Model.Entities;
using LowCortisol.Platform.API.Plan.Domain.Model.Queries;
using LowCortisol.Platform.API.Plan.Domain.Model.ReadModels;
using LowCortisol.Platform.API.Plan.Domain.Repositories;
using LowCortisol.Platform.API.Workplace.Domain.Repositories;
using LowCortisol.Platform.API.DeviceControl.Domain.Repositories;
using PlanAggregate = LowCortisol.Platform.API.Plan.Domain.Model.Aggregates.Plan;

namespace LowCortisol.Platform.API.Plan.Application.Internal.QueryServices;

public sealed class PlanQueryService : IPlanQueryService
{
    private readonly IPlanRepository _planRepository;
    private readonly ISubscriptionRepository _subscriptionRepository;
    private readonly IPaymentRepository _paymentRepository;
    private readonly IServiceRequestRepository _serviceRequestRepository;
    private readonly ISiteRepository _siteRepository;
    private readonly IDeviceRepository _deviceRepository;
    private readonly ISensorRepository _sensorRepository;
    private readonly IValveRepository _valveRepository;

    public PlanQueryService(
        IPlanRepository planRepository,
        ISubscriptionRepository subscriptionRepository,
        IPaymentRepository paymentRepository,
        IServiceRequestRepository serviceRequestRepository,
        ISiteRepository siteRepository,
        IDeviceRepository deviceRepository,
        ISensorRepository sensorRepository,
        IValveRepository valveRepository)
    {
        _planRepository = planRepository;
        _subscriptionRepository = subscriptionRepository;
        _paymentRepository = paymentRepository;
        _serviceRequestRepository = serviceRequestRepository;
        _siteRepository = siteRepository;
        _deviceRepository = deviceRepository;
        _sensorRepository = sensorRepository;
        _valveRepository = valveRepository;
    }

    public Task<IReadOnlyCollection<PlanAggregate>> Handle(GetPlansQuery query, CancellationToken cancellationToken = default) =>
        _planRepository.ListActiveAsync(query.RecommendedOnly, cancellationToken);

    public Task<PlanAggregate?> Handle(GetPlanByIdQuery query, CancellationToken cancellationToken = default) =>
        _planRepository.FindByIdAsync(query.PlanId, cancellationToken);

    public Task<Subscription?> Handle(GetActiveSubscriptionQuery query, CancellationToken cancellationToken = default) =>
        _subscriptionRepository.FindActiveByUserIdAsync(query.UserId, cancellationToken);

    public async Task<IReadOnlyCollection<Payment>> Handle(GetPaymentsQuery query, CancellationToken cancellationToken = default)
    {
        var subscriptions = await _subscriptionRepository.ListByUserIdAsync(query.UserId, cancellationToken);
        return await _paymentRepository.ListBySubscriptionIdsAsync(subscriptions.Select(subscription => subscription.Id), cancellationToken);
    }

    public async Task<IReadOnlyCollection<ServiceRequest>> Handle(GetServiceRequestsQuery query, CancellationToken cancellationToken = default)
    {
        var subscriptions = await _subscriptionRepository.ListByUserIdAsync(query.UserId, cancellationToken);
        return await _serviceRequestRepository.ListBySubscriptionIdsAsync(subscriptions.Select(subscription => subscription.Id), cancellationToken);
    }

    public async Task<PlanSummary> Handle(GetPlanSummaryQuery query, CancellationToken cancellationToken = default)
    {
        var plans = await _planRepository.ListActiveAsync(false, cancellationToken);
        var subscription = await _subscriptionRepository.FindActiveByUserIdAsync(query.UserId, cancellationToken);
        var payments = await Handle(new GetPaymentsQuery(query.UserId), cancellationToken);
        var requests = await Handle(new GetServiceRequestsQuery(query.UserId), cancellationToken);
        var activePlan = subscription is null
            ? null
            : plans.FirstOrDefault(plan => plan.Id == subscription.PlanId);

        var sites = await _siteRepository.ListAsync(cancellationToken);
        var devices = await _deviceRepository.ListAsync(cancellationToken);
        var sensors = await _sensorRepository.ListAsync(cancellationToken);
        var valves = await _valveRepository.ListAsync(cancellationToken);
        var usedDevices = devices.Count + sensors.Count + valves.Count;

        return new PlanSummary(
            plans.Count,
            activePlan?.Name ?? "Sin plan",
            subscription?.Status.ToString().ToLowerInvariant() ?? "inactive",
            payments.Count,
            payments.Sum(payment => payment.Amount),
            requests.Count,
            activePlan?.MaxSites ?? 0,
            activePlan?.MaxDevices ?? 0,
            sites.Count,
            usedDevices,
            Math.Max(0, (activePlan?.MaxSites ?? 0) - sites.Count),
            Math.Max(0, (activePlan?.MaxDevices ?? 0) - usedDevices));
    }
}
