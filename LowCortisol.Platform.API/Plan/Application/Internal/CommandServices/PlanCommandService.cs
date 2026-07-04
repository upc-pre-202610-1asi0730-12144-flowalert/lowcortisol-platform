using LowCortisol.Platform.API.Plan.Application.CommandServices;
using LowCortisol.Platform.API.Plan.Application.Errors;
using LowCortisol.Platform.API.Plan.Domain.Model.Aggregates;
using LowCortisol.Platform.API.Plan.Domain.Model.Commands;
using LowCortisol.Platform.API.Plan.Domain.Model.Entities;
using LowCortisol.Platform.API.Plan.Domain.Repositories;
using LowCortisol.Platform.API.Shared.Application.Results;
using LowCortisol.Platform.API.Shared.Domain.Repositories;
using PlanAggregate = LowCortisol.Platform.API.Plan.Domain.Model.Aggregates.Plan;

namespace LowCortisol.Platform.API.Plan.Application.Internal.CommandServices;

public sealed class PlanCommandService : IPlanCommandService
{
    private readonly IPlanRepository _planRepository;
    private readonly ISubscriptionRepository _subscriptionRepository;
    private readonly IPaymentRepository _paymentRepository;
    private readonly IServiceRequestRepository _serviceRequestRepository;
    private readonly IUnitOfWork _unitOfWork;

    public PlanCommandService(
        IPlanRepository planRepository,
        ISubscriptionRepository subscriptionRepository,
        IPaymentRepository paymentRepository,
        IServiceRequestRepository serviceRequestRepository,
        IUnitOfWork unitOfWork)
    {
        _planRepository = planRepository;
        _subscriptionRepository = subscriptionRepository;
        _paymentRepository = paymentRepository;
        _serviceRequestRepository = serviceRequestRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Subscription>> Handle(SubscribeToPlanCommand command, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(command.UserId))
            return Result<Subscription>.Failure(PlanError.UserIdRequired.ToString());

        var plan = await _planRepository.FindByIdAsync(command.PlanId, cancellationToken);
        if (plan is null) return Result<Subscription>.Failure(PlanError.PlanNotFound.ToString());

        var existing = await _subscriptionRepository.FindActiveByUserIdAsync(command.UserId, cancellationToken);
        if (existing is not null)
        {
            existing.ChangePlan(plan.Id);
            _subscriptionRepository.Update(existing);
            await RegisterPaymentAndRequest(existing, plan, "change-plan", $"Cambio solicitado al plan {plan.Name}.", command.PaymentMethod, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);
            return Result<Subscription>.Success(existing);
        }

        var subscription = new Subscription(Guid.NewGuid(), command.UserId, command.WorkplaceId, plan.Id);
        await _subscriptionRepository.AddAsync(subscription, cancellationToken);
        await RegisterPaymentAndRequest(subscription, plan, "subscription", $"Suscripcion creada para el plan {plan.Name}.", command.PaymentMethod, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);

        return Result<Subscription>.Success(subscription);
    }

    public async Task<Result<Subscription>> Handle(ChangePlanCommand command, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(command.UserId))
            return Result<Subscription>.Failure(PlanError.UserIdRequired.ToString());

        var subscription = await _subscriptionRepository.FindActiveByUserIdAsync(command.UserId, cancellationToken);
        if (subscription is null) return Result<Subscription>.Failure(PlanError.SubscriptionNotFound.ToString());

        var plan = await _planRepository.FindByIdAsync(command.NewPlanId, cancellationToken);
        if (plan is null) return Result<Subscription>.Failure(PlanError.PlanNotFound.ToString());

        subscription.ChangePlan(plan.Id);
        _subscriptionRepository.Update(subscription);
        await RegisterPaymentAndRequest(subscription, plan, "change-plan", $"Cambio solicitado al plan {plan.Name}.", "card", cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);

        return Result<Subscription>.Success(subscription);
    }

    public async Task<Result<Subscription>> Handle(CancelSubscriptionCommand command, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(command.UserId))
            return Result<Subscription>.Failure(PlanError.UserIdRequired.ToString());

        var subscription = await _subscriptionRepository.FindActiveByUserIdAsync(command.UserId, cancellationToken);
        if (subscription is null) return Result<Subscription>.Failure(PlanError.SubscriptionNotFound.ToString());

        subscription.Cancel();
        _subscriptionRepository.Update(subscription);

        var request = new ServiceRequest(Guid.NewGuid(), subscription.Id, "cancellation", command.Reason);
        await _serviceRequestRepository.AddAsync(request, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);

        return Result<Subscription>.Success(subscription);
    }

    public async Task<Result<ServiceRequest>> Handle(ResolveServiceRequestCommand command, CancellationToken cancellationToken = default)
    {
        var request = await _serviceRequestRepository.FindByIdAsync(command.ServiceRequestId, cancellationToken);
        if (request is null) return Result<ServiceRequest>.Failure(PlanError.ServiceRequestNotFound.ToString());

        request.Resolve();
        _serviceRequestRepository.Update(request);
        await _unitOfWork.CompleteAsync(cancellationToken);

        return Result<ServiceRequest>.Success(request);
    }

    public async Task<Result<ServiceRequest>> Handle(CloseServiceRequestCommand command, CancellationToken cancellationToken = default)
    {
        var request = await _serviceRequestRepository.FindByIdAsync(command.ServiceRequestId, cancellationToken);
        if (request is null) return Result<ServiceRequest>.Failure(PlanError.ServiceRequestNotFound.ToString());

        request.Close();
        _serviceRequestRepository.Update(request);
        await _unitOfWork.CompleteAsync(cancellationToken);

        return Result<ServiceRequest>.Success(request);
    }

    private async Task RegisterPaymentAndRequest(
        Subscription subscription,
        PlanAggregate plan,
        string requestType,
        string requestDescription,
        string paymentMethod,
        CancellationToken cancellationToken)
    {
        await _paymentRepository.AddAsync(
            new Payment(Guid.NewGuid(), subscription.Id, plan.Price, plan.Currency, paymentMethod),
            cancellationToken);
        await _serviceRequestRepository.AddAsync(
            new ServiceRequest(Guid.NewGuid(), subscription.Id, requestType, requestDescription, Domain.Model.ValueObjects.ServiceRequestStatus.Resolved),
            cancellationToken);
    }
}
