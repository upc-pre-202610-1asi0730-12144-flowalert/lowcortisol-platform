using LowCortisol.Platform.API.DeviceControl.Application.QueryServices;
using LowCortisol.Platform.API.DeviceControl.Domain.Model.Queries;
using LowCortisol.Platform.API.DeviceControl.Interfaces.Rest.Resources;
using LowCortisol.Platform.API.DeviceControl.Interfaces.Rest.Transform;
using Microsoft.AspNetCore.Mvc;

namespace LowCortisol.Platform.API.DeviceControl.Interfaces.Rest.Controllers;

[ApiController]
[Route("api/v1")]
public sealed class DeviceCommandsController : ControllerBase
{
    private readonly IDeviceCommandQueryService _deviceCommandQueryService;

    public DeviceCommandsController(IDeviceCommandQueryService deviceCommandQueryService)
    {
        _deviceCommandQueryService = deviceCommandQueryService;
    }

    [HttpGet("device-commands")]
    [ProducesResponseType(typeof(IEnumerable<DeviceCommandResource>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetDeviceCommands([FromQuery] int count, CancellationToken cancellationToken)
    {
        var commands = await _deviceCommandQueryService.Handle(new GetDeviceCommandsQuery(count), cancellationToken);
        return Ok(commands.Select(DeviceCommandResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpGet("devices/{deviceId:guid}/commands")]
    [ProducesResponseType(typeof(IEnumerable<DeviceCommandResource>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetDeviceCommandsByDeviceId(Guid deviceId, CancellationToken cancellationToken)
    {
        var commands = await _deviceCommandQueryService.Handle(
            new GetDeviceCommandsByDeviceIdQuery(deviceId),
            cancellationToken);

        return Ok(commands.Select(DeviceCommandResourceFromEntityAssembler.ToResourceFromEntity));
    }
}
