using LowCortisol.Platform.API.DeviceControl.Application.QueryServices;
using LowCortisol.Platform.API.DeviceControl.Domain.Model.Queries;
using LowCortisol.Platform.API.DeviceControl.Interfaces.Rest.Resources;
using LowCortisol.Platform.API.DeviceControl.Interfaces.Rest.Transform;
using Microsoft.AspNetCore.Mvc;

namespace LowCortisol.Platform.API.DeviceControl.Interfaces.Rest.Controllers;

[ApiController]
[Route("api/v1")]
public sealed class ValveOperationsController : ControllerBase
{
    private readonly IValveOperationQueryService _valveOperationQueryService;

    public ValveOperationsController(IValveOperationQueryService valveOperationQueryService)
    {
        _valveOperationQueryService = valveOperationQueryService;
    }

    [HttpGet("valves/{valveId:guid}/operations")]
    [ProducesResponseType(typeof(IEnumerable<ValveOperationResource>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetValveOperationsByValveId(Guid valveId, CancellationToken cancellationToken)
    {
        var operations = await _valveOperationQueryService.Handle(
            new GetValveOperationsByValveIdQuery(valveId),
            cancellationToken);

        return Ok(operations.Select(ValveOperationResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpGet("incidents/{incidentId:guid}/valve-operations")]
    [ProducesResponseType(typeof(IEnumerable<ValveOperationResource>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetValveOperationsByIncidentId(Guid incidentId, CancellationToken cancellationToken)
    {
        var operations = await _valveOperationQueryService.Handle(
            new GetValveOperationsByIncidentIdQuery(incidentId),
            cancellationToken);

        return Ok(operations.Select(ValveOperationResourceFromEntityAssembler.ToResourceFromEntity));
    }
}
