namespace LowCortisol.Platform.API.Iam.Interfaces.Rest.Resources;

public record UserResource(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    string Role,
    bool IsActive,
    DateTime CreatedAt,
    DateTime UpdatedAt);
