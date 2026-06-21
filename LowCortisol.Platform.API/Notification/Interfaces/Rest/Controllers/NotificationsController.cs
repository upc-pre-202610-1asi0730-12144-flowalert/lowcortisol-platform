using LowCortisol.Platform.API.Notification.Application.QueryServices;
using LowCortisol.Platform.API.Notification.Domain.Model.Queries;
using LowCortisol.Platform.API.Notification.Interfaces.Rest.Resources;
using LowCortisol.Platform.API.Notification.Interfaces.Rest.Transform;
using Microsoft.AspNetCore.Mvc;

namespace LowCortisol.Platform.API.Notification.Interfaces.Rest.Controllers;

[ApiController]
[Route("api/v1")]
public sealed class NotificationsController : ControllerBase
{
    private readonly INotificationSummaryQueryService _summaryQueryService;

    public NotificationsController(INotificationSummaryQueryService summaryQueryService)
    {
        _summaryQueryService = summaryQueryService;
    }

    [HttpGet("notifications/summary")]
    [ProducesResponseType(typeof(NotificationSummaryResource), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetNotificationSummary(CancellationToken cancellationToken)
    {
        var summary = await _summaryQueryService.Handle(new GetNotificationSummaryQuery(), cancellationToken);
        return Ok(NotificationSummaryResourceFromResultAssembler.ToResourceFromResult(summary));
    }
}
