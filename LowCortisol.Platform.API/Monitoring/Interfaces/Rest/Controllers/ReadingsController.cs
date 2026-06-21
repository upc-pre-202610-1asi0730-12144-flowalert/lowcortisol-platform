using LowCortisol.Platform.API.Monitoring.Application.CommandServices;
using LowCortisol.Platform.API.Monitoring.Application.QueryServices;
using LowCortisol.Platform.API.Monitoring.Domain.Model.Queries;
using LowCortisol.Platform.API.Monitoring.Interfaces.Rest.Resources;
using LowCortisol.Platform.API.Monitoring.Interfaces.Rest.Transform;
using Microsoft.AspNetCore.Mvc;

namespace LowCortisol.Platform.API.Monitoring.Interfaces.Rest.Controllers;

[ApiController]
[Route("api/v1")]
public sealed class ReadingsController : ControllerBase
{
    private readonly IReadingCommandService _readingCommandService;
    private readonly IReadingQueryService _readingQueryService;

    public ReadingsController(
        IReadingCommandService readingCommandService,
        IReadingQueryService readingQueryService)
    {
        _readingCommandService = readingCommandService;
        _readingQueryService = readingQueryService;
    }

    [HttpPost("readings")]
    [ProducesResponseType(typeof(RegisterConsumptionReadingResultResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RegisterReading(
        RegisterConsumptionReadingResource resource,
        CancellationToken cancellationToken)
    {
        var command = RegisterConsumptionReadingCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await _readingCommandService.Handle(command, cancellationToken);

        return MonitoringActionResultAssembler.ToActionResult(
            this,
            result,
            RegisterConsumptionReadingResultResourceFromResultAssembler.ToResourceFromResult);
    }

    [HttpGet("readings/recent")]
    [ProducesResponseType(typeof(IEnumerable<ConsumptionReadingResource>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRecentReadings(
        [FromQuery] int count,
        CancellationToken cancellationToken)
    {
        var readings = await _readingQueryService.Handle(new GetRecentReadingsQuery(count), cancellationToken);
        var resources = readings.Select(ConsumptionReadingResourceFromEntityAssembler.ToResourceFromEntity);

        return Ok(resources);
    }

    [HttpGet("sites/{siteId:guid}/readings")]
    [ProducesResponseType(typeof(IEnumerable<ConsumptionReadingResource>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetReadingsBySiteId(Guid siteId, CancellationToken cancellationToken)
    {
        var readings = await _readingQueryService.Handle(new GetReadingsBySiteIdQuery(siteId), cancellationToken);
        var resources = readings.Select(ConsumptionReadingResourceFromEntityAssembler.ToResourceFromEntity);

        return Ok(resources);
    }

    [HttpGet("rooms/{roomId:guid}/readings")]
    [ProducesResponseType(typeof(IEnumerable<ConsumptionReadingResource>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetReadingsByRoomId(Guid roomId, CancellationToken cancellationToken)
    {
        var readings = await _readingQueryService.Handle(new GetReadingsByRoomIdQuery(roomId), cancellationToken);
        var resources = readings.Select(ConsumptionReadingResourceFromEntityAssembler.ToResourceFromEntity);

        return Ok(resources);
    }

    [HttpGet("device-groups/{deviceGroupId:guid}/readings")]
    [ProducesResponseType(typeof(IEnumerable<ConsumptionReadingResource>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetReadingsByDeviceGroupId(
        Guid deviceGroupId,
        CancellationToken cancellationToken)
    {
        var readings = await _readingQueryService.Handle(
            new GetReadingsByDeviceGroupIdQuery(deviceGroupId),
            cancellationToken);
        var resources = readings.Select(ConsumptionReadingResourceFromEntityAssembler.ToResourceFromEntity);

        return Ok(resources);
    }

    [HttpGet("sensors/{sensorId:guid}/readings")]
    [ProducesResponseType(typeof(IEnumerable<ConsumptionReadingResource>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetReadingsBySensorId(Guid sensorId, CancellationToken cancellationToken)
    {
        var readings = await _readingQueryService.Handle(new GetReadingsBySensorIdQuery(sensorId), cancellationToken);
        var resources = readings.Select(ConsumptionReadingResourceFromEntityAssembler.ToResourceFromEntity);

        return Ok(resources);
    }
}
