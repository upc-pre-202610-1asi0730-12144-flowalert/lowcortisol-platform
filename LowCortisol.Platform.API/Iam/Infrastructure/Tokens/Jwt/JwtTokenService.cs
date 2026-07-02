using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LowCortisol.Platform.API.Iam.Application.OutboundServices;
using LowCortisol.Platform.API.Iam.Domain.Model.Aggregates;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace LowCortisol.Platform.API.Iam.Infrastructure.Tokens.Jwt;

public sealed class JwtTokenService : ITokenService
{
    private readonly JwtSettings _settings;

    public JwtTokenService(IOptions<JwtSettings> options)
    {
        _settings = options.Value;
    }

    public (string Token, DateTime ExpiresAt)? CreateToken(User user)
    {
        if (string.IsNullOrWhiteSpace(_settings.Secret)) return null;

        var expiresAt = DateTime.UtcNow.AddMinutes(Math.Max(_settings.ExpirationMinutes, 15));
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Secret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim("name", $"{user.FirstName} {user.LastName}".Trim())
        };

        var token = new JwtSecurityToken(
            issuer: _settings.Issuer,
            audience: _settings.Audience,
            claims: claims,
            expires: expiresAt,
            signingCredentials: credentials);

        return (new JwtSecurityTokenHandler().WriteToken(token), expiresAt);
    }
}
