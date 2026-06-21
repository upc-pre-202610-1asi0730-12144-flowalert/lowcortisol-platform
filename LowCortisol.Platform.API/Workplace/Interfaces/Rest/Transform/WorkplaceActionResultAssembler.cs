using LowCortisol.Platform.API.Shared.Application.Results;
using Microsoft.AspNetCore.Mvc;

namespace LowCortisol.Platform.API.Workplace.Interfaces.Rest.Transform;

public static class WorkplaceActionResultAssembler
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
            title: "Workplace request could not be completed.",
            detail: result.Error,
            statusCode: MapStatusCode(result.Error));
    }

    private static int MapStatusCode(string? error)
    {
        if (string.IsNullOrWhiteSpace(error)) return StatusCodes.Status400BadRequest;

        return error.Contains("Duplicated", StringComparison.OrdinalIgnoreCase)
            ? StatusCodes.Status409Conflict
            : error.Contains("NotFound", StringComparison.OrdinalIgnoreCase)
                ? StatusCodes.Status404NotFound
                : StatusCodes.Status400BadRequest;
    }
}
