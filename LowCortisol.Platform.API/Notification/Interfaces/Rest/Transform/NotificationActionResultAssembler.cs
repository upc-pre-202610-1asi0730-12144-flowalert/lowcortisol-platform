using LowCortisol.Platform.API.Shared.Application.Results;
using Microsoft.AspNetCore.Mvc;

namespace LowCortisol.Platform.API.Notification.Interfaces.Rest.Transform;

public static class NotificationActionResultAssembler
{
    public static IActionResult ToActionResult<TEntity, TResource>(
        ControllerBase controller,
        Result<TEntity> result,
        Func<TEntity, TResource> successAction)
        where TEntity : class
    {
        if (result.IsSuccess && result.Value is not null)
        {
            return controller.Ok(successAction(result.Value));
        }

        return controller.Problem(
            title: "Notification request could not be completed.",
            detail: result.Error,
            statusCode: MapStatusCode(result.Error));
    }

    private static int MapStatusCode(string? error)
    {
        if (string.IsNullOrWhiteSpace(error)) return StatusCodes.Status400BadRequest;

        if (error.Contains("NotFound", StringComparison.OrdinalIgnoreCase)) return StatusCodes.Status404NotFound;
        if (error.Contains("InvalidStateTransition", StringComparison.OrdinalIgnoreCase)) return StatusCodes.Status409Conflict;
        if (error.Contains("AlreadyClosed", StringComparison.OrdinalIgnoreCase)) return StatusCodes.Status409Conflict;
        if (error.Contains("MitigationFailed", StringComparison.OrdinalIgnoreCase)) return StatusCodes.Status409Conflict;

        return StatusCodes.Status400BadRequest;
    }
}
