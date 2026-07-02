using LowCortisol.Platform.API.Shared.Infrastructure.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace LowCortisol.Platform.API.Shared.Interfaces.Rest.ProblemDetails;

public static class ProblemDetailsResponseExtensions
{
    public static IActionResult NotFoundProblem(
        this ControllerBase controller,
        string title,
        string detail)
    {
        return controller.LocalizedProblem(
            titleKey: "Errors.NotFound.Title",
            titleFallback: title,
            detailKey: detail,
            detailFallback: detail,
            statusCode: StatusCodes.Status404NotFound);
    }

    public static IActionResult LocalizedProblem(
        this ControllerBase controller,
        string titleKey,
        string titleFallback,
        string? detailKey,
        string? detailFallback,
        int statusCode)
    {
        return controller.Problem(
            title: Localize(controller, titleKey, titleFallback),
            detail: Localize(controller, detailKey, detailFallback),
            statusCode: statusCode);
    }

    private static string? Localize(ControllerBase controller, string? key, string? fallback)
    {
        if (string.IsNullOrWhiteSpace(key)) return fallback;

        var localizer = controller.HttpContext.RequestServices.GetService<IStringLocalizer<ErrorMessages>>();
        var localized = localizer?[key];

        return localized is { ResourceNotFound: false }
            ? localized.Value
            : fallback;
    }
}
