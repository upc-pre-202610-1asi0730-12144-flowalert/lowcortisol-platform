using LowCortisol.Platform.API.Notification.Application.CommandServices;
using LowCortisol.Platform.API.Notification.Application.QueryServices;
using LowCortisol.Platform.API.Notification.Domain.Model.Commands;
using LowCortisol.Platform.API.Notification.Domain.Model.Queries;
using LowCortisol.Platform.API.Notification.Interfaces.Rest.Resources;
using LowCortisol.Platform.API.Notification.Interfaces.Rest.Transform;
using Microsoft.AspNetCore.Mvc;

namespace LowCortisol.Platform.API.Notification.Interfaces.Rest.Controllers;

[ApiController]
[Route("api/v1")]
public sealed class NotificationChannelsController : ControllerBase
{
    private readonly INotificationChannelCommandService _channelCommandService;
    private readonly INotificationChannelQueryService _channelQueryService;

    public NotificationChannelsController(
        INotificationChannelCommandService channelCommandService,
        INotificationChannelQueryService channelQueryService)
    {
        _channelCommandService = channelCommandService;
        _channelQueryService = channelQueryService;
    }

    [HttpGet("notification-channels")]
    [ProducesResponseType(typeof(IEnumerable<NotificationChannelResource>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetNotificationChannels(CancellationToken cancellationToken)
    {
        var channels = await _channelQueryService.Handle(new GetNotificationChannelsQuery(), cancellationToken);
        return Ok(channels.Select(NotificationChannelResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpGet("notification-channels/active")]
    [ProducesResponseType(typeof(IEnumerable<NotificationChannelResource>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetActiveNotificationChannels(CancellationToken cancellationToken)
    {
        var channels = await _channelQueryService.Handle(new GetActiveNotificationChannelsQuery(), cancellationToken);
        return Ok(channels.Select(NotificationChannelResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpPost("notification-channels")]
    [ProducesResponseType(typeof(NotificationChannelResource), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateNotificationChannel(
        CreateNotificationChannelResource resource,
        CancellationToken cancellationToken)
    {
        var result = await _channelCommandService.Handle(
            new CreateNotificationChannelCommand(resource.Name, resource.Type, resource.IsActive),
            cancellationToken);

        return NotificationActionResultAssembler.ToActionResult(
            this,
            result,
            NotificationChannelResourceFromEntityAssembler.ToResourceFromEntity,
            resource => Created("/api/v1/notification-channels", resource));
    }

    [HttpPost("notification-channels/{channelId:guid}/activate")]
    [ProducesResponseType(typeof(NotificationChannelResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ActivateNotificationChannel(Guid channelId, CancellationToken cancellationToken)
    {
        var result = await _channelCommandService.Handle(
            new UpdateNotificationChannelStatusCommand(channelId, true),
            cancellationToken);

        return NotificationActionResultAssembler.ToActionResult(
            this,
            result,
            NotificationChannelResourceFromEntityAssembler.ToResourceFromEntity);
    }

    [HttpPost("notification-channels/{channelId:guid}/deactivate")]
    [ProducesResponseType(typeof(NotificationChannelResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeactivateNotificationChannel(Guid channelId, CancellationToken cancellationToken)
    {
        var result = await _channelCommandService.Handle(
            new UpdateNotificationChannelStatusCommand(channelId, false),
            cancellationToken);

        return NotificationActionResultAssembler.ToActionResult(
            this,
            result,
            NotificationChannelResourceFromEntityAssembler.ToResourceFromEntity);
    }
}
