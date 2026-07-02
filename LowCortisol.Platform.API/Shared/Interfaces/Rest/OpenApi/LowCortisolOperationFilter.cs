using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace LowCortisol.Platform.API.Shared.Interfaces.Rest.OpenApi;

public sealed class LowCortisolOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var controllerName = context.MethodInfo.DeclaringType?.Name.Replace("Controller", string.Empty)
            ?? "LowCortisol";
        var operationName = ToSentence(context.MethodInfo);

        operation.Summary ??= operationName;
        operation.Description ??= $"Executes {operationName.ToLowerInvariant()} for the {controllerName} REST resource.";
        operation.Responses ??= new OpenApiResponses();

        if (!operation.Responses.ContainsKey(StatusCodes.Status500InternalServerError.ToString()))
        {
            operation.Responses.Add(
                StatusCodes.Status500InternalServerError.ToString(),
                new OpenApiResponse { Description = "Unexpected server error." });
        }
    }

    private static string ToSentence(MethodInfo methodInfo)
    {
        var words = Regex.Replace(methodInfo.Name, "([a-z])([A-Z])", "$1 $2");
        return words.Trim();
    }
}
