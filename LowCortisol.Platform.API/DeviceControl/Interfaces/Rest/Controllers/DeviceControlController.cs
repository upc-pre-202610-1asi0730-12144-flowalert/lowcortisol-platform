using LowCortisol.Platform.API.DeviceControl.Application.QueryServices;
using LowCortisol.Platform.API.DeviceControl.Domain.Model.Queries;
using LowCortisol.Platform.API.DeviceControl.Interfaces.Rest.Resources;
using LowCortisol.Platform.API.DeviceControl.Interfaces.Rest.Transform;
using Microsoft.AspNetCore.Mvc;

namespace LowCortisol.Platform.API.DeviceControl.Interfaces.Rest.Controllers;

[ApiController]
[Route("api/v1")]
public sealed class DeviceControlController : ControllerBase
{
    private readonly IDeviceControlMitigationSummaryQueryService _mitigationSummaryQueryService;

    public DeviceControlController(IDeviceControlMitigationSummaryQueryService mitigationSummaryQueryService)
    {
        _mitigationSummaryQueryService = mitigationSummaryQueryService;
    }

    [HttpGet("device-control/mitigation-summary")]
    [ProducesResponseType(typeof(DeviceControlMitigationSummaryResource), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMitigationSummary(CancellationToken cancellationToken)
    {
        var summary = await _mitigationSummaryQueryService.Handle(
            new GetDeviceControlMitigationSummaryQuery(),
            cancellationToken);

        return Ok(DeviceControlMitigationSummaryResourceFromResultAssembler.ToResourceFromResult(summary));
    }
}
