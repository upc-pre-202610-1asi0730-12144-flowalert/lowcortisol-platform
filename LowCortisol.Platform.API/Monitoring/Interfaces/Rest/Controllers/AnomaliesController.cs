using LowCortisol.Platform.API.Monitoring.Application.CommandServices;
using LowCortisol.Platform.API.Monitoring.Application.QueryServices;
using LowCortisol.Platform.API.Monitoring.Domain.Model.Commands;
using LowCortisol.Platform.API.Monitoring.Domain.Model.Queries;
using LowCortisol.Platform.API.Monitoring.Interfaces.Rest.Resources;
using LowCortisol.Platform.API.Monitoring.Interfaces.Rest.Transform;
using Microsoft.AspNetCore.Mvc;

namespace LowCortisol.Platform.API.Monitoring.Interfaces.Rest.Controllers;

[ApiController]
[Route("api/v1")]
public sealed class AnomaliesController : ControllerBase
{
    private readonly IAnomalyCommandService _anomalyCommandService;
    private readonly IAnomalyQueryService _anomalyQueryService;

    public AnomaliesController(
        IAnomalyCommandService anomalyCommandService,
        IAnomalyQueryService anomalyQueryService)
    {
        _anomalyCommandService = anomalyCommandService;
        _anomalyQueryService = anomalyQueryService;
    }

    [HttpGet("anomalies/open")]
    [ProducesResponseType(typeof(IEnumerable<AnomalyResource>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetOpenAnomalies(CancellationToken cancellationToken)
    {
        var anomalies = await _anomalyQueryService.Handle(new GetOpenAnomaliesQuery(), cancellationToken);
        var resources = anomalies.Select(AnomalyResourceFromEntityAssembler.ToResourceFromEntity);

        return Ok(resources);
    }

    [HttpGet("sites/{siteId:guid}/anomalies")]
    [ProducesResponseType(typeof(IEnumerable<AnomalyResource>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAnomaliesBySiteId(Guid siteId, CancellationToken cancellationToken)
    {
        var anomalies = await _anomalyQueryService.Handle(new GetAnomaliesBySiteIdQuery(siteId), cancellationToken);
        var resources = anomalies.Select(AnomalyResourceFromEntityAssembler.ToResourceFromEntity);

        return Ok(resources);
    }

    [HttpPost("anomalies/{anomalyId:guid}/resolve")]
    [ProducesResponseType(typeof(AnomalyResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> ResolveAnomaly(Guid anomalyId, CancellationToken cancellationToken)
    {
        var result = await _anomalyCommandService.Handle(new ResolveAnomalyCommand(anomalyId), cancellationToken);

        return MonitoringActionResultAssembler.ToActionResult(
            this,
            result,
            AnomalyResourceFromEntityAssembler.ToResourceFromEntity);
    }
}
