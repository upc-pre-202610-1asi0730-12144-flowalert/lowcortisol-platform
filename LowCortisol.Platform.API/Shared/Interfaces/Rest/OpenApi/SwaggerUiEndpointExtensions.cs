namespace LowCortisol.Platform.API.Shared.Interfaces.Rest.OpenApi;

public static class SwaggerUiEndpointExtensions
{
    public static IEndpointRouteBuilder MapLightweightSwaggerUi(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/swagger", () => Results.Content(GetSwaggerHtml(), "text/html"))
            .ExcludeFromDescription();

        endpoints.MapGet("/swagger/index.html", () => Results.Redirect("/swagger"))
            .ExcludeFromDescription();

        return endpoints;
    }

    private static string GetSwaggerHtml() =>
        """
        <!doctype html>
        <html lang="en">
        <head>
          <meta charset="utf-8">
          <meta name="viewport" content="width=device-width, initial-scale=1">
          <title>LowCortisol API - Swagger UI</title>
          <link rel="stylesheet" href="https://unpkg.com/swagger-ui-dist/swagger-ui.css">
        </head>
        <body>
          <div id="swagger-ui"></div>
          <script src="https://unpkg.com/swagger-ui-dist/swagger-ui-bundle.js"></script>
          <script>
            window.onload = () => {
              window.ui = SwaggerUIBundle({
                url: "/openapi/v1.json",
                dom_id: "#swagger-ui"
              });
            };
          </script>
        </body>
        </html>
        """;
}
