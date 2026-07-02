using LowCortisol.Platform.API.Notification.Application.CommandServices;
using LowCortisol.Platform.API.Notification.Application.QueryServices;
using LowCortisol.Platform.API.Notification.Domain.Model.Commands;
using LowCortisol.Platform.API.Notification.Domain.Model.Queries;
using LowCortisol.Platform.API.Notification.Interfaces.Rest.Resources;
using LowCortisol.Platform.API.Notification.Interfaces.Rest.Transform;
using LowCortisol.Platform.API.Shared.Interfaces.Rest.ProblemDetails;
using Microsoft.AspNetCore.Mvc;

namespace LowCortisol.Platform.API.Notification.Interfaces.Rest.Controllers;

[ApiController]
[Route("api/v1")]
public sealed class IncidentsController : ControllerBase
{
    private readonly IIncidentCommandService _incidentCommandService;
    private readonly IIncidentMitigationCommandService _incidentMitigationCommandService;
    private readonly IIncidentQueryService _incidentQueryService;

    public IncidentsController(
        IIncidentCommandService incidentCommandService,
        IIncidentMitigationCommandService incidentMitigationCommandService,
        IIncidentQueryService incidentQueryService)
    {
        _incidentCommandService = incidentCommandService;
        _incidentMitigationCommandService = incidentMitigationCommandService;
        _incidentQueryService = incidentQueryService;
    }

    [HttpGet("incidents/open")]
    [ProducesResponseType(typeof(IEnumerable<IncidentResource>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetOpenIncidents(CancellationToken cancellationToken)
    {
        var incidents = await _incidentQueryService.Handle(new GetOpenIncidentsQuery(), cancellationToken);
        return Ok(incidents.Select(IncidentResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpGet("alerts/{alertId:guid}/incidents")]
    [ProducesResponseType(typeof(IEnumerable<IncidentResource>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetIncidentsByAlertId(Guid alertId, CancellationToken cancellationToken)
    {
        var incidents = await _incidentQueryService.Handle(new GetIncidentsByAlertIdQuery(alertId), cancellationToken);
        return Ok(incidents.Select(IncidentResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpGet("sites/{siteId:guid}/incidents")]
    [ProducesResponseType(typeof(IEnumerable<IncidentResource>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetIncidentsBySiteId(Guid siteId, CancellationToken cancellationToken)
    {
        var incidents = await _incidentQueryService.Handle(new GetIncidentsBySiteIdQuery(siteId), cancellationToken);
        return Ok(incidents.Select(IncidentResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpGet("incidents/{incidentId:guid}")]
    [ProducesResponseType(typeof(IncidentResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetIncidentById(Guid incidentId, CancellationToken cancellationToken)
    {
        var incident = await _incidentQueryService.Handle(new GetIncidentByIdQuery(incidentId), cancellationToken);
        if (incident is null)
        {
            return this.NotFoundProblem("Incident was not found.", $"Incident '{incidentId}' does not exist.");
        }

        return Ok(IncidentResourceFromEntityAssembler.ToResourceFromEntity(incident));
    }

    [HttpGet("incidents/{incidentId:guid}/actions")]
    [ProducesResponseType(typeof(IEnumerable<IncidentActionResource>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetIncidentActions(Guid incidentId, CancellationToken cancellationToken)
    {
        var incident = await _incidentQueryService.Handle(new GetIncidentByIdQuery(incidentId), cancellationToken);
        if (incident is null)
        {
            return this.NotFoundProblem("Incident actions were not found.", $"Incident '{incidentId}' does not exist.");
        }

        return Ok(incident.Actions.Select(IncidentActionResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpPost("alerts/{alertId:guid}/incidents")]
    [ProducesResponseType(typeof(IncidentResource), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateIncidentFromAlert(Guid alertId, CancellationToken cancellationToken)
    {
        var result = await _incidentCommandService.Handle(
            new CreateIncidentFromAlertCommand(alertId),
            cancellationToken);

        return NotificationActionResultAssembler.ToActionResult(
            this,
            result,
            IncidentResourceFromEntityAssembler.ToResourceFromEntity,
            resource => CreatedAtAction(nameof(GetIncidentById), new { incidentId = resource.Id }, resource));
    }

    [HttpPost("incidents/{incidentId:guid}/assign")]
    [ProducesResponseType(typeof(IncidentResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> AssignIncident(
        Guid incidentId,
        AssignIncidentResource resource,
        CancellationToken cancellationToken)
    {
        var result = await _incidentCommandService.Handle(
            new AssignIncidentCommand(incidentId, resource.AssigneeId, resource.AssigneeName),
            cancellationToken);

        return NotificationActionResultAssembler.ToActionResult(
            this,
            result,
            IncidentResourceFromEntityAssembler.ToResourceFromEntity);
    }

    [HttpPost("incidents/{incidentId:guid}/actions")]
    [ProducesResponseType(typeof(IncidentResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> RegisterIncidentAction(
        Guid incidentId,
        RegisterIncidentActionResource resource,
        CancellationToken cancellationToken)
    {
        var result = await _incidentCommandService.Handle(
            new RegisterIncidentActionCommand(
                incidentId,
                resource.ActionType,
                resource.Description,
                resource.PerformedBy),
            cancellationToken);

        return NotificationActionResultAssembler.ToActionResult(
            this,
            result,
            IncidentResourceFromEntityAssembler.ToResourceFromEntity);
    }

    [HttpPost("incidents/{incidentId:guid}/mitigation/close-valve")]
    [ProducesResponseType(typeof(IncidentMitigationActionResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CloseValveForIncident(
        Guid incidentId,
        CloseValveForIncidentResource resource,
        CancellationToken cancellationToken)
    {
        var command = RequestIncidentMitigationCommandFromResourceAssembler.ToCommandFromResource(incidentId, resource);
        var result = await _incidentMitigationCommandService.Handle(command, cancellationToken);

        return NotificationActionResultAssembler.ToActionResult(
            this,
            result,
            IncidentMitigationActionResourceFromResultAssembler.ToResourceFromResult);
    }

    [HttpPost("incidents/{incidentId:guid}/mitigation-actions")]
    [ProducesResponseType(typeof(IncidentMitigationActionResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> RequestIncidentMitigationAction(
        Guid incidentId,
        CloseValveForIncidentResource resource,
        CancellationToken cancellationToken)
    {
        var command = RequestIncidentMitigationCommandFromResourceAssembler.ToCommandFromResource(incidentId, resource);
        var result = await _incidentMitigationCommandService.Handle(command, cancellationToken);

        return NotificationActionResultAssembler.ToActionResult(
            this,
            result,
            IncidentMitigationActionResourceFromResultAssembler.ToResourceFromResult);
    }

    [HttpPost("incidents/{incidentId:guid}/resolve")]
    [ProducesResponseType(typeof(IncidentResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> ResolveIncident(
        Guid incidentId,
        ResolveIncidentResource resource,
        CancellationToken cancellationToken)
    {
        var result = await _incidentCommandService.Handle(
            new ResolveIncidentCommand(incidentId, resource.ResolvedBy, resource.Resolution),
            cancellationToken);

        return NotificationActionResultAssembler.ToActionResult(
            this,
            result,
            IncidentResourceFromEntityAssembler.ToResourceFromEntity);
    }

    [HttpPost("incidents/{incidentId:guid}/close")]
    [ProducesResponseType(typeof(IncidentResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CloseIncident(
        Guid incidentId,
        CloseIncidentResource resource,
        CancellationToken cancellationToken)
    {
        var result = await _incidentCommandService.Handle(
            new CloseIncidentCommand(incidentId, resource.ClosedBy, resource.ClosingNote),
            cancellationToken);

        return NotificationActionResultAssembler.ToActionResult(
            this,
            result,
            IncidentResourceFromEntityAssembler.ToResourceFromEntity);
    }
}
