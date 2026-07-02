using LowCortisol.Platform.API.Iam.Domain.Model.Aggregates;

namespace LowCortisol.Platform.API.Iam.Application.OutboundServices;

public interface ITokenService
{
    (string Token, DateTime ExpiresAt)? CreateToken(User user);
}
