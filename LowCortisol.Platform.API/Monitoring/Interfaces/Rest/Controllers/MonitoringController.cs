using LowCortisol.Platform.API.Monitoring.Application.QueryServices;
using LowCortisol.Platform.API.Monitoring.Domain.Model.Queries;
using LowCortisol.Platform.API.Monitoring.Interfaces.Rest.Resources;
using LowCortisol.Platform.API.Monitoring.Interfaces.Rest.Transform;
using Microsoft.AspNetCore.Mvc;

namespace LowCortisol.Platform.API.Monitoring.Interfaces.Rest.Controllers;

[ApiController]
[Route("api/v1/monitoring")]
public sealed class MonitoringController : ControllerBase
{
    private readonly IMonitoringSummaryQueryService _monitoringSummaryQueryService;

    public MonitoringController(IMonitoringSummaryQueryService monitoringSummaryQueryService)
    {
        _monitoringSummaryQueryService = monitoringSummaryQueryService;
    }

    [HttpGet("summary")]
    [ProducesResponseType(typeof(MonitoringSummaryResource), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSummary(CancellationToken cancellationToken)
    {
        var summary = await _monitoringSummaryQueryService.Handle(
            new GetMonitoringSummaryQuery(),
            cancellationToken);

        return Ok(MonitoringSummaryResourceFromResultAssembler.ToResourceFromResult(summary));
    }
}
