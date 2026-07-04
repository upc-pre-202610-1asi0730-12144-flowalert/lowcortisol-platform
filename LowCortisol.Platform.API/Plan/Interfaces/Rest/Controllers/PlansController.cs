using LowCortisol.Platform.API.Plan.Application.QueryServices;
using LowCortisol.Platform.API.Plan.Domain.Model.Queries;
using LowCortisol.Platform.API.Plan.Interfaces.Rest.Resources;
using LowCortisol.Platform.API.Plan.Interfaces.Rest.Transform;
using LowCortisol.Platform.API.Shared.Interfaces.Rest.ProblemDetails;
using Microsoft.AspNetCore.Mvc;

namespace LowCortisol.Platform.API.Plan.Interfaces.Rest.Controllers;

[ApiController]
[Route("api/v1/plans")]
public sealed class PlansController : ControllerBase
{
    private readonly IPlanQueryService _planQueryService;

    public PlansController(IPlanQueryService planQueryService)
    {
        _planQueryService = planQueryService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<PlanResource>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPlans([FromQuery] bool recommended = false, CancellationToken cancellationToken = default)
    {
        var plans = await _planQueryService.Handle(new GetPlansQuery(recommended), cancellationToken);
        return Ok(plans.Select(PlanResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpGet("{planId:guid}")]
    [ProducesResponseType(typeof(PlanResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetPlanById(Guid planId, CancellationToken cancellationToken)
    {
        var plan = await _planQueryService.Handle(new GetPlanByIdQuery(planId), cancellationToken);
        if (plan is null)
        {
            return this.NotFoundProblem("Plan was not found.", $"Plan '{planId}' does not exist.");
        }

        return Ok(PlanResourceFromEntityAssembler.ToResourceFromEntity(plan));
    }

    [HttpGet("summary")]
    [ProducesResponseType(typeof(PlanSummaryResource), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSummary([FromQuery] string userId, CancellationToken cancellationToken)
    {
        var summary = await _planQueryService.Handle(new GetPlanSummaryQuery(userId), cancellationToken);
        return Ok(PlanSummaryResourceFromResultAssembler.ToResourceFromResult(summary));
    }
}
