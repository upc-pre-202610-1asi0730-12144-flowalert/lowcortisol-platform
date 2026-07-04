using LowCortisol.Platform.API.Shared.Application.Results;
using LowCortisol.Platform.API.Shared.Interfaces.Rest.ProblemDetails;
using Microsoft.AspNetCore.Mvc;

namespace LowCortisol.Platform.API.Plan.Interfaces.Rest.Transform;

public static class PlanActionResultAssembler
{
    public static IActionResult ToActionResult<TEntity, TResource>(
        ControllerBase controller,
        Result<TEntity> result,
        Func<TEntity, TResource> successAction,
        Func<TResource, IActionResult>? successResult = null)
        where TEntity : class
    {
        if (result.IsSuccess && result.Value is not null)
        {
            var resource = successAction(result.Value);
            return successResult?.Invoke(resource) ?? controller.Ok(resource);
        }

        return controller.LocalizedProblem(
            titleKey: "Errors.Plan.RequestFailed",
            titleFallback: "Plan request could not be completed.",
            detailKey: result.Error,
            detailFallback: result.Error,
            statusCode: MapStatusCode(result.Error));
    }

    private static int MapStatusCode(string? error)
    {
        if (string.IsNullOrWhiteSpace(error)) return StatusCodes.Status400BadRequest;
        if (error.Contains("NotFound", StringComparison.OrdinalIgnoreCase)) return StatusCodes.Status404NotFound;
        if (error.Contains("DoesNotCoverUsage", StringComparison.OrdinalIgnoreCase)) return StatusCodes.Status409Conflict;
        return StatusCodes.Status400BadRequest;
    }
}
