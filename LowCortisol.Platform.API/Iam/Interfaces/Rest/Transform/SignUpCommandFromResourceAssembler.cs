using LowCortisol.Platform.API.Iam.Domain.Model.Commands;
using LowCortisol.Platform.API.Iam.Interfaces.Rest.Resources;

namespace LowCortisol.Platform.API.Iam.Interfaces.Rest.Transform;

public static class SignUpCommandFromResourceAssembler
{
    public static SignUpCommand ToCommandFromResource(SignUpResource resource) =>
        new(resource.FirstName, resource.LastName, resource.Email, resource.Password, resource.Role);
}
