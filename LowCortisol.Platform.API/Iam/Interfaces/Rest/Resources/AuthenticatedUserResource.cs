namespace LowCortisol.Platform.API.Iam.Interfaces.Rest.Resources;

public record AuthenticatedUserResource(
    UserResource User,
    string Token,
    DateTime ExpiresAt);
