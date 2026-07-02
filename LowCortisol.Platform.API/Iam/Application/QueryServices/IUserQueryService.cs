using LowCortisol.Platform.API.Iam.Domain.Model.Aggregates;
using LowCortisol.Platform.API.Iam.Domain.Model.Queries;

namespace LowCortisol.Platform.API.Iam.Application.QueryServices;

public interface IUserQueryService
{
    Task<IReadOnlyCollection<User>> Handle(GetAllUsersQuery query, CancellationToken cancellationToken = default);
}
