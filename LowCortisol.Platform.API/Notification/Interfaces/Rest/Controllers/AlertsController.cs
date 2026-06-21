using LowCortisol.Platform.API.Monitoring.Domain.Model.Events;
using LowCortisol.Platform.API.Monitoring.Domain.Model.ValueObjects;
using LowCortisol.Platform.API.Notification.Application.Acl;
using LowCortisol.Platform.API.Notification.Application.CommandServices;
using LowCortisol.Platform.API.Notification.Application.Internal.Parsing;
using LowCortisol.Platform.API.Notification.Application.QueryServices;
using LowCortisol.Platform.API.Notification.Domain.Model.Commands;
using LowCortisol.Platform.API.Notification.Domain.Model.Queries;
using LowCortisol.Platform.API.Notification.Interfaces.Rest.Resources;
using LowCortisol.Platform.API.Notification.Interfaces.Rest.Transform;
using Microsoft.AspNetCore.Mvc;

namespace LowCortisol.Platform.API.Notification.Interfaces.Rest.Controllers;

[ApiController]
[Route("api/v1")]
public sealed class AlertsController : ControllerBase
{
    private readonly IAlertCommandService _alertCommandService;
    private readonly IAlertQueryService _alertQueryService;
    private readonly INotificationContextFacade _notificationContextFacade;

    public AlertsController(
        IAlertCommandService alertCommandService,
        IAlertQueryService alertQueryService,
        INotificationContextFacade notificationContextFacade)
    {
        _alertCommandService = alertCommandService;
        _alertQueryService = alertQueryService;
        _notificationContextFacade = notificationContextFacade;
    }

    [HttpGet("alerts/open")]
    [ProducesResponseType(typeof(IEnumerable<AlertResource>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetOpenAlerts(CancellationToken cancellationToken)
    {
        var alerts = await _alertQueryService.Handle(new GetOpenAlertsQuery(), cancellationToken);
        return Ok(alerts.Select(AlertResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpGet("alerts/critical")]
    [ProducesResponseType(typeof(IEnumerable<AlertResource>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCriticalAlerts(CancellationToken cancellationToken)
    {
        var alerts = await _alertQueryService.Handle(new GetCriticalAlertsQuery(), cancellationToken);
        return Ok(alerts.Select(AlertResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpGet("sites/{siteId:guid}/alerts")]
    [ProducesResponseType(typeof(IEnumerable<AlertResource>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAlertsBySiteId(Guid siteId, CancellationToken cancellationToken)
    {
        var alerts = await _alertQueryService.Handle(new GetAlertsBySiteIdQuery(siteId), cancellationToken);
        return Ok(alerts.Select(AlertResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpGet("anomalies/{anomalyId:guid}/alerts")]
    [ProducesResponseType(typeof(IEnumerable<AlertResource>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAlertsByAnomalyId(Guid anomalyId, CancellationToken cancellationToken)
    {
        var alerts = await _alertQueryService.Handle(new GetAlertsByAnomalyIdQuery(anomalyId), cancellationToken);
        return Ok(alerts.Select(AlertResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpGet("alerts/{alertId:guid}")]
    [ProducesResponseType(typeof(AlertResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAlertById(Guid alertId, CancellationToken cancellationToken)
    {
        var alert = await _alertQueryService.Handle(new GetAlertByIdQuery(alertId), cancellationToken);
        if (alert is null)
        {
            return NotFound();
        }

        return Ok(AlertResourceFromEntityAssembler.ToResourceFromEntity(alert));
    }

    [HttpPost("alerts/from-critical-anomaly")]
    [ProducesResponseType(typeof(NotificationRiskResponseResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateAlertFromCriticalAnomaly(
        CreateAlertFromCriticalAnomalyResource resource,
        CancellationToken cancellationToken)
    {
        if (!NotificationEnumParser.TryParse<ResourceType>(resource.ResourceType, out var resourceType))
        {
            return Problem(
                title: "Notification request could not be completed.",
                detail: "InvalidResourceType",
                statusCode: StatusCodes.Status400BadRequest);
        }

        var criticalEvent = new CriticalAnomalyDetected(
            resource.AnomalyId,
            resource.ReadingId,
            resource.SiteId,
            resource.RoomId,
            resource.DeviceGroupId,
            resource.DeviceId,
            resource.SensorId,
            resourceType,
            resource.Value,
            resource.LimitValue,
            resource.Unit,
            resource.DetectedAt);

        var result = await _notificationContextFacade.HandleCriticalAnomalyDetectedAsync(
            criticalEvent,
            cancellationToken);

        return NotificationActionResultAssembler.ToActionResult(
            this,
            result,
            NotificationRiskResponseResourceFromResultAssembler.ToResourceFromResult);
    }

    [HttpPost("alerts/{alertId:guid}/acknowledge")]
    [ProducesResponseType(typeof(AlertResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> AcknowledgeAlert(
        Guid alertId,
        AcknowledgeAlertResource resource,
        CancellationToken cancellationToken)
    {
        var result = await _alertCommandService.Handle(
            new AcknowledgeAlertCommand(alertId, resource.AcknowledgedBy),
            cancellationToken);

        return NotificationActionResultAssembler.ToActionResult(
            this,
            result,
            AlertResourceFromEntityAssembler.ToResourceFromEntity);
    }

    [HttpPost("alerts/{alertId:guid}/resolve")]
    [ProducesResponseType(typeof(AlertResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> ResolveAlert(
        Guid alertId,
        ResolveAlertResource resource,
        CancellationToken cancellationToken)
    {
        var result = await _alertCommandService.Handle(
            new ResolveAlertCommand(alertId, resource.ResolvedBy, resource.Note),
            cancellationToken);

        return NotificationActionResultAssembler.ToActionResult(
            this,
            result,
            AlertResourceFromEntityAssembler.ToResourceFromEntity);
    }

    [HttpPost("alerts/{alertId:guid}/close")]
    [ProducesResponseType(typeof(AlertResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CloseAlert(
        Guid alertId,
        CloseAlertResource resource,
        CancellationToken cancellationToken)
    {
        var result = await _alertCommandService.Handle(
            new CloseAlertCommand(alertId, resource.ClosedBy, resource.Note),
            cancellationToken);

        return NotificationActionResultAssembler.ToActionResult(
            this,
            result,
            AlertResourceFromEntityAssembler.ToResourceFromEntity);
    }
}
