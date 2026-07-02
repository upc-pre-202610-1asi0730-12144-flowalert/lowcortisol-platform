using LowCortisol.Platform.API.Iam.Domain.Model.Aggregates;
using LowCortisol.Platform.API.Iam.Domain.Repositories;
using LowCortisol.Platform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using LowCortisol.Platform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LowCortisol.Platform.API.Iam.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public sealed class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(AppDbContext context) : base(context)
    {
    }

    public Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var normalizedEmail = User.NormalizeEmail(email);
        return Context.Users.AnyAsync(user => user.Email == normalizedEmail, cancellationToken);
    }

    public Task<User?> FindByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var normalizedEmail = User.NormalizeEmail(email);
        return Context.Users.FirstOrDefaultAsync(user => user.Email == normalizedEmail, cancellationToken);
    }
}
