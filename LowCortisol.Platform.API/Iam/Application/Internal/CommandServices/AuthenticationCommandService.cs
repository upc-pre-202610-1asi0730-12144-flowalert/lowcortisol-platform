using LowCortisol.Platform.API.Iam.Application.CommandServices;
using LowCortisol.Platform.API.Iam.Application.Errors;
using LowCortisol.Platform.API.Iam.Application.OutboundServices;
using LowCortisol.Platform.API.Iam.Application.Results;
using LowCortisol.Platform.API.Iam.Domain.Model.Aggregates;
using LowCortisol.Platform.API.Iam.Domain.Model.Commands;
using LowCortisol.Platform.API.Iam.Domain.Repositories;
using LowCortisol.Platform.API.Shared.Application.Results;
using LowCortisol.Platform.API.Shared.Domain.Repositories;

namespace LowCortisol.Platform.API.Iam.Application.Internal.CommandServices;

public sealed class AuthenticationCommandService : IAuthenticationCommandService
{
    private const int MinimumPasswordLength = 8;
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHashingService _passwordHashingService;
    private readonly ITokenService _tokenService;
    private readonly IUnitOfWork _unitOfWork;

    public AuthenticationCommandService(
        IUserRepository userRepository,
        IPasswordHashingService passwordHashingService,
        ITokenService tokenService,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _passwordHashingService = passwordHashingService;
        _tokenService = tokenService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<AuthenticatedUser>> Handle(
        SignUpCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationError = ValidateSignUp(command);
        if (validationError is not null) return Result<AuthenticatedUser>.Failure(validationError);

        var normalizedEmail = User.NormalizeEmail(command.Email);
        if (await _userRepository.ExistsByEmailAsync(normalizedEmail, cancellationToken))
        {
            return Result<AuthenticatedUser>.Failure(IamError.UserEmailDuplicated.ToString());
        }

        var user = new User(
            Guid.NewGuid(),
            command.FirstName,
            command.LastName,
            normalizedEmail,
            _passwordHashingService.Hash(command.Password),
            command.Role);

        await _userRepository.AddAsync(user, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);

        return CreateAuthenticatedUser(user);
    }

    public async Task<Result<AuthenticatedUser>> Handle(
        SignInCommand command,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(command.Email)) return Failure(IamError.UserEmailRequired);
        if (string.IsNullOrWhiteSpace(command.Password)) return Failure(IamError.UserPasswordRequired);

        var user = await _userRepository.FindByEmailAsync(User.NormalizeEmail(command.Email), cancellationToken);
        if (user is null || !user.IsActive || !_passwordHashingService.Verify(command.Password, user.PasswordHash))
        {
            return Failure(IamError.InvalidCredentials);
        }

        return CreateAuthenticatedUser(user);
    }

    private Result<AuthenticatedUser> CreateAuthenticatedUser(User user)
    {
        var token = _tokenService.CreateToken(user);
        return token is null
            ? Failure(IamError.TokenSecretNotConfigured)
            : Result<AuthenticatedUser>.Success(new AuthenticatedUser(user, token.Value.Token, token.Value.ExpiresAt));
    }

    private static string? ValidateSignUp(SignUpCommand command)
    {
        if (string.IsNullOrWhiteSpace(command.FirstName) || string.IsNullOrWhiteSpace(command.LastName))
            return IamError.UserNameRequired.ToString();
        if (string.IsNullOrWhiteSpace(command.Email)) return IamError.UserEmailRequired.ToString();
        if (string.IsNullOrWhiteSpace(command.Password)) return IamError.UserPasswordRequired.ToString();
        if (command.Password.Length < MinimumPasswordLength) return IamError.UserPasswordTooShort.ToString();

        return null;
    }

    private static Result<AuthenticatedUser> Failure(IamError error) =>
        Result<AuthenticatedUser>.Failure(error.ToString());
}
