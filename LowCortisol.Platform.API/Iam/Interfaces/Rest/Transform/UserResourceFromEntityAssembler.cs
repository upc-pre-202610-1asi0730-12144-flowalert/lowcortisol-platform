using LowCortisol.Platform.API.Iam.Domain.Model.Aggregates;
using LowCortisol.Platform.API.Iam.Interfaces.Rest.Resources;

namespace LowCortisol.Platform.API.Iam.Interfaces.Rest.Transform;

public static class UserResourceFromEntityAssembler
{
    public static UserResource ToResourceFromEntity(User user) =>
        new(
            user.Id,
            user.FirstName,
            user.LastName,
            user.Email,
            user.Role,
            user.IsActive,
            user.CreatedAt,
            user.UpdatedAt);
}
