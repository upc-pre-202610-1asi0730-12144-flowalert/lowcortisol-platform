using LowCortisol.Platform.API.Workplace.Application.CommandServices;
using LowCortisol.Platform.API.Workplace.Application.QueryServices;
using LowCortisol.Platform.API.Workplace.Domain.Model.Queries;
using LowCortisol.Platform.API.Workplace.Interfaces.Rest.Resources;
using LowCortisol.Platform.API.Workplace.Interfaces.Rest.Transform;
using Microsoft.AspNetCore.Mvc;

namespace LowCortisol.Platform.API.Workplace.Interfaces.Rest.Controllers;

[ApiController]
[Route("api/v1/rooms")]
public sealed class RoomsController : ControllerBase
{
    private readonly ISiteCommandService _siteCommandService;
    private readonly ISiteQueryService _siteQueryService;

    public RoomsController(ISiteCommandService siteCommandService, ISiteQueryService siteQueryService)
    {
        _siteCommandService = siteCommandService;
        _siteQueryService = siteQueryService;
    }

    [HttpGet("{roomId:guid}/device-groups")]
    [ProducesResponseType(typeof(IEnumerable<DeviceGroupResource>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetDeviceGroupsByRoomId(Guid roomId, CancellationToken cancellationToken)
    {
        var groups = await _siteQueryService.Handle(new GetDeviceGroupsByRoomIdQuery(roomId), cancellationToken);
        var resources = groups.Select(DeviceGroupResourceFromEntityAssembler.ToResourceFromEntity);

        return Ok(resources);
    }

    [HttpPost("{roomId:guid}/device-groups")]
    [ProducesResponseType(typeof(DeviceGroupResource), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateDeviceGroup(
        Guid roomId,
        CreateDeviceGroupResource resource,
        CancellationToken cancellationToken)
    {
        var command = CreateDeviceGroupCommandFromResourceAssembler.ToCommandFromResource(roomId, resource);
        var result = await _siteCommandService.Handle(command, cancellationToken);

        return WorkplaceActionResultAssembler.ToActionResult(
            this,
            result,
            DeviceGroupResourceFromEntityAssembler.ToResourceFromEntity,
            resource => Created($"/api/v1/rooms/{roomId}/device-groups", resource));
    }
}
