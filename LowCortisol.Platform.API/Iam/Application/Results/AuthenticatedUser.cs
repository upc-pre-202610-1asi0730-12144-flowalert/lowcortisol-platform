using LowCortisol.Platform.API.Iam.Domain.Model.Aggregates;

namespace LowCortisol.Platform.API.Iam.Application.Results;

public record AuthenticatedUser(User User, string Token, DateTime ExpiresAt);
