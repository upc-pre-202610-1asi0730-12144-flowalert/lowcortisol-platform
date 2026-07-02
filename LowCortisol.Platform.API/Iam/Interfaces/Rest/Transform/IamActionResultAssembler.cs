using LowCortisol.Platform.API.Shared.Application.Results;
using LowCortisol.Platform.API.Shared.Interfaces.Rest.ProblemDetails;
using Microsoft.AspNetCore.Mvc;

namespace LowCortisol.Platform.API.Iam.Interfaces.Rest.Transform;

public static class IamActionResultAssembler
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
            titleKey: "Errors.Iam.RequestFailed",
            titleFallback: "IAM request could not be completed.",
            detailKey: result.Error,
            detailFallback: result.Error,
            statusCode: MapStatusCode(result.Error));
    }

    private static int MapStatusCode(string? error)
    {
        if (string.IsNullOrWhiteSpace(error)) return StatusCodes.Status400BadRequest;
        if (error.Contains("Duplicated", StringComparison.OrdinalIgnoreCase)) return StatusCodes.Status409Conflict;
        if (error.Contains("InvalidCredentials", StringComparison.OrdinalIgnoreCase)) return StatusCodes.Status401Unauthorized;
        if (error.Contains("TokenSecretNotConfigured", StringComparison.OrdinalIgnoreCase))
            return StatusCodes.Status500InternalServerError;

        return StatusCodes.Status400BadRequest;
    }
}
