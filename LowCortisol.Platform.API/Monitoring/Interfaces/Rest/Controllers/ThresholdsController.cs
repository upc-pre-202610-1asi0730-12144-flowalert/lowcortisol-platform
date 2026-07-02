using LowCortisol.Platform.API.Monitoring.Application.CommandServices;
using LowCortisol.Platform.API.Monitoring.Application.QueryServices;
using LowCortisol.Platform.API.Monitoring.Domain.Model.Queries;
using LowCortisol.Platform.API.Monitoring.Interfaces.Rest.Resources;
using LowCortisol.Platform.API.Monitoring.Interfaces.Rest.Transform;
using Microsoft.AspNetCore.Mvc;

namespace LowCortisol.Platform.API.Monitoring.Interfaces.Rest.Controllers;

[ApiController]
[Route("api/v1")]
public sealed class ThresholdsController : ControllerBase
{
    private readonly IThresholdCommandService _thresholdCommandService;
    private readonly IThresholdQueryService _thresholdQueryService;

    public ThresholdsController(
        IThresholdCommandService thresholdCommandService,
        IThresholdQueryService thresholdQueryService)
    {
        _thresholdCommandService = thresholdCommandService;
        _thresholdQueryService = thresholdQueryService;
    }

    [HttpPost("thresholds")]
    [ProducesResponseType(typeof(ThresholdResource), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateThreshold(
        CreateThresholdResource resource,
        CancellationToken cancellationToken)
    {
        var command = CreateThresholdCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await _thresholdCommandService.Handle(command, cancellationToken);

        return MonitoringActionResultAssembler.ToActionResult(
            this,
            result,
            ThresholdResourceFromEntityAssembler.ToResourceFromEntity,
            resource => Created("/api/v1/thresholds/active", resource));
    }

    [HttpGet("thresholds/active")]
    [ProducesResponseType(typeof(IEnumerable<ThresholdResource>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetActiveThresholds(CancellationToken cancellationToken)
    {
        var thresholds = await _thresholdQueryService.Handle(new GetActiveThresholdsQuery(), cancellationToken);
        var resources = thresholds.Select(ThresholdResourceFromEntityAssembler.ToResourceFromEntity);

        return Ok(resources);
    }

    [HttpGet("sites/{siteId:guid}/thresholds")]
    [ProducesResponseType(typeof(IEnumerable<ThresholdResource>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetThresholdsBySiteId(Guid siteId, CancellationToken cancellationToken)
    {
        var thresholds = await _thresholdQueryService.Handle(
            new GetThresholdsByScopeQuery(siteId, null, null, null),
            cancellationToken);
        var resources = thresholds.Select(ThresholdResourceFromEntityAssembler.ToResourceFromEntity);

        return Ok(resources);
    }

    [HttpGet("rooms/{roomId:guid}/thresholds")]
    [ProducesResponseType(typeof(IEnumerable<ThresholdResource>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetThresholdsByRoomId(Guid roomId, CancellationToken cancellationToken)
    {
        var thresholds = await _thresholdQueryService.Handle(
            new GetThresholdsByScopeQuery(null, roomId, null, null),
            cancellationToken);
        var resources = thresholds.Select(ThresholdResourceFromEntityAssembler.ToResourceFromEntity);

        return Ok(resources);
    }

    [HttpGet("device-groups/{deviceGroupId:guid}/thresholds")]
    [ProducesResponseType(typeof(IEnumerable<ThresholdResource>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetThresholdsByDeviceGroupId(
        Guid deviceGroupId,
        CancellationToken cancellationToken)
    {
        var thresholds = await _thresholdQueryService.Handle(
            new GetThresholdsByScopeQuery(null, null, deviceGroupId, null),
            cancellationToken);
        var resources = thresholds.Select(ThresholdResourceFromEntityAssembler.ToResourceFromEntity);

        return Ok(resources);
    }

    [HttpGet("sensors/{sensorId:guid}/thresholds")]
    [ProducesResponseType(typeof(IEnumerable<ThresholdResource>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetThresholdsBySensorId(Guid sensorId, CancellationToken cancellationToken)
    {
        var thresholds = await _thresholdQueryService.Handle(
            new GetThresholdsByScopeQuery(null, null, null, sensorId),
            cancellationToken);
        var resources = thresholds.Select(ThresholdResourceFromEntityAssembler.ToResourceFromEntity);

        return Ok(resources);
    }
}
