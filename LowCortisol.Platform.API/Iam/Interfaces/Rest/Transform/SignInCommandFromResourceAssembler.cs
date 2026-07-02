using LowCortisol.Platform.API.Iam.Domain.Model.Commands;
using LowCortisol.Platform.API.Iam.Interfaces.Rest.Resources;

namespace LowCortisol.Platform.API.Iam.Interfaces.Rest.Transform;

public static class SignInCommandFromResourceAssembler
{
    public static SignInCommand ToCommandFromResource(SignInResource resource) =>
        new(resource.Email, resource.Password);
}
