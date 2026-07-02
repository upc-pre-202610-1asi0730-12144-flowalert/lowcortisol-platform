using LowCortisol.Platform.API.Iam.Application.Results;
using LowCortisol.Platform.API.Iam.Domain.Model.Commands;
using LowCortisol.Platform.API.Shared.Application.Results;

namespace LowCortisol.Platform.API.Iam.Application.CommandServices;

public interface IAuthenticationCommandService
{
    Task<Result<AuthenticatedUser>> Handle(SignUpCommand command, CancellationToken cancellationToken = default);
    Task<Result<AuthenticatedUser>> Handle(SignInCommand command, CancellationToken cancellationToken = default);
}
