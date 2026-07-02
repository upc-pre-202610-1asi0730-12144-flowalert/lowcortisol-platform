namespace LowCortisol.Platform.API.Iam.Infrastructure.Tokens.Jwt;

public sealed class JwtSettings
{
    public string? Secret { get; init; }
    public string Issuer { get; init; } = "LowCortisol";
    public string Audience { get; init; } = "LowCortisol";
    public int ExpirationMinutes { get; init; } = 120;
}
