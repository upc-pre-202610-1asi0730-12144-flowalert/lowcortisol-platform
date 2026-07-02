using LowCortisol.Platform.API.Iam.Application.Results;
using LowCortisol.Platform.API.Iam.Interfaces.Rest.Resources;

namespace LowCortisol.Platform.API.Iam.Interfaces.Rest.Transform;

public static class AuthenticatedUserResourceFromResultAssembler
{
    public static AuthenticatedUserResource ToResourceFromResult(AuthenticatedUser result) =>
        new(
            UserResourceFromEntityAssembler.ToResourceFromEntity(result.User),
            result.Token,
            result.ExpiresAt);
}
