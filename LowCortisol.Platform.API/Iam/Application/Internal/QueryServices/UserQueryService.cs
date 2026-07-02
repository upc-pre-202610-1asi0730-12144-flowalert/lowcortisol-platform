using LowCortisol.Platform.API.Iam.Application.QueryServices;
using LowCortisol.Platform.API.Iam.Domain.Model.Aggregates;
using LowCortisol.Platform.API.Iam.Domain.Model.Queries;
using LowCortisol.Platform.API.Iam.Domain.Repositories;

namespace LowCortisol.Platform.API.Iam.Application.Internal.QueryServices;

public sealed class UserQueryService : IUserQueryService
{
    private readonly IUserRepository _userRepository;

    public UserQueryService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public Task<IReadOnlyCollection<User>> Handle(
        GetAllUsersQuery query,
        CancellationToken cancellationToken = default) =>
        _userRepository.ListAsync(cancellationToken);
}
