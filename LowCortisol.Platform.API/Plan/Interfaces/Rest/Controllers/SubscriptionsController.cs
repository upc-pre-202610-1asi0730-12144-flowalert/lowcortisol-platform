using LowCortisol.Platform.API.Plan.Application.CommandServices;
using LowCortisol.Platform.API.Plan.Application.QueryServices;
using LowCortisol.Platform.API.Plan.Domain.Model.Commands;
using LowCortisol.Platform.API.Plan.Domain.Model.Queries;
using LowCortisol.Platform.API.Plan.Interfaces.Rest.Resources;
using LowCortisol.Platform.API.Plan.Interfaces.Rest.Transform;
using LowCortisol.Platform.API.Shared.Interfaces.Rest.ProblemDetails;
using Microsoft.AspNetCore.Mvc;

namespace LowCortisol.Platform.API.Plan.Interfaces.Rest.Controllers;

[ApiController]
[Route("api/v1")]
public sealed class SubscriptionsController : ControllerBase
{
    private readonly IPlanCommandService _planCommandService;
    private readonly IPlanQueryService _planQueryService;

    public SubscriptionsController(IPlanCommandService planCommandService, IPlanQueryService planQueryService)
    {
        _planCommandService = planCommandService;
        _planQueryService = planQueryService;
    }

    [HttpGet("subscriptions/active")]
    [ProducesResponseType(typeof(SubscriptionResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetActiveSubscription([FromQuery] string userId, CancellationToken cancellationToken)
    {
        var subscription = await _planQueryService.Handle(new GetActiveSubscriptionQuery(userId), cancellationToken);
        if (subscription is null)
        {
            return this.NotFoundProblem("Subscription was not found.", $"User '{userId}' has no active subscription.");
        }

        return Ok(SubscriptionResourceFromEntityAssembler.ToResourceFromEntity(subscription));
    }

    [HttpPost("subscriptions")]
    [ProducesResponseType(typeof(SubscriptionResource), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Subscribe(SubscribeToPlanResource resource, CancellationToken cancellationToken)
    {
        var result = await _planCommandService.Handle(
            new SubscribeToPlanCommand(resource.UserId, resource.WorkplaceId, resource.PlanId, resource.PaymentMethod),
            cancellationToken);

        return PlanActionResultAssembler.ToActionResult(
            this,
            result,
            SubscriptionResourceFromEntityAssembler.ToResourceFromEntity,
            subscription => Created($"/api/v1/subscriptions/{subscription.Id}", subscription));
    }

    [HttpPost("subscriptions/change-plan")]
    [ProducesResponseType(typeof(SubscriptionResource), StatusCodes.Status200OK)]
    public async Task<IActionResult> ChangePlan(ChangePlanResource resource, CancellationToken cancellationToken)
    {
        var result = await _planCommandService.Handle(new ChangePlanCommand(resource.UserId, resource.NewPlanId), cancellationToken);
        return PlanActionResultAssembler.ToActionResult(this, result, SubscriptionResourceFromEntityAssembler.ToResourceFromEntity);
    }

    [HttpPost("subscriptions/cancel")]
    [ProducesResponseType(typeof(SubscriptionResource), StatusCodes.Status200OK)]
    public async Task<IActionResult> CancelSubscription(CancelSubscriptionResource resource, CancellationToken cancellationToken)
    {
        var result = await _planCommandService.Handle(new CancelSubscriptionCommand(resource.UserId, resource.Reason), cancellationToken);
        return PlanActionResultAssembler.ToActionResult(this, result, SubscriptionResourceFromEntityAssembler.ToResourceFromEntity);
    }

    [HttpGet("payments")]
    [ProducesResponseType(typeof(IEnumerable<PaymentResource>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPayments([FromQuery] string userId, CancellationToken cancellationToken)
    {
        var payments = await _planQueryService.Handle(new GetPaymentsQuery(userId), cancellationToken);
        return Ok(payments.Select(PaymentResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpGet("service-requests")]
    [ProducesResponseType(typeof(IEnumerable<ServiceRequestResource>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetServiceRequests([FromQuery] string userId, CancellationToken cancellationToken)
    {
        var requests = await _planQueryService.Handle(new GetServiceRequestsQuery(userId), cancellationToken);
        return Ok(requests.Select(ServiceRequestResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpPost("service-requests/{serviceRequestId:guid}/resolve")]
    [ProducesResponseType(typeof(ServiceRequestResource), StatusCodes.Status200OK)]
    public async Task<IActionResult> ResolveServiceRequest(Guid serviceRequestId, CancellationToken cancellationToken)
    {
        var result = await _planCommandService.Handle(new ResolveServiceRequestCommand(serviceRequestId), cancellationToken);
        return PlanActionResultAssembler.ToActionResult(this, result, ServiceRequestResourceFromEntityAssembler.ToResourceFromEntity);
    }

    [HttpPost("service-requests/{serviceRequestId:guid}/close")]
    [ProducesResponseType(typeof(ServiceRequestResource), StatusCodes.Status200OK)]
    public async Task<IActionResult> CloseServiceRequest(Guid serviceRequestId, CancellationToken cancellationToken)
    {
        var result = await _planCommandService.Handle(new CloseServiceRequestCommand(serviceRequestId), cancellationToken);
        return PlanActionResultAssembler.ToActionResult(this, result, ServiceRequestResourceFromEntityAssembler.ToResourceFromEntity);
    }
}
