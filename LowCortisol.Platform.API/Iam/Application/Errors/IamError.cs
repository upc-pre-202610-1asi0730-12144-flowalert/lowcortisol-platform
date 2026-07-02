namespace LowCortisol.Platform.API.Iam.Application.Errors;

public enum IamError
{
    UserNameRequired,
    UserEmailRequired,
    UserPasswordRequired,
    UserPasswordTooShort,
    UserEmailDuplicated,
    InvalidCredentials,
    TokenSecretNotConfigured
}
