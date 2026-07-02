namespace LowCortisol.Platform.API.Iam.Interfaces.Rest.Resources;

public record SignUpResource(
    string FirstName,
    string LastName,
    string Email,
    string Password,
    string Role);
