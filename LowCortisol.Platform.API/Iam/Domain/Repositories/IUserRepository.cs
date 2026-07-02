using LowCortisol.Platform.API.Iam.Domain.Model.Aggregates;
using LowCortisol.Platform.API.Shared.Domain.Repositories;

namespace LowCortisol.Platform.API.Iam.Domain.Repositories;

public interface IUserRepository : IBaseRepository<User>
{
    Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<User?> FindByEmailAsync(string email, CancellationToken cancellationToken = default);
}
