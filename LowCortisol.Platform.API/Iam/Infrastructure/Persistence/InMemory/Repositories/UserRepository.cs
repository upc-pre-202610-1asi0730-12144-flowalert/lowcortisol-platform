using LowCortisol.Platform.API.Iam.Domain.Model.Aggregates;
using LowCortisol.Platform.API.Iam.Domain.Repositories;
using LowCortisol.Platform.API.Shared.Infrastructure.Persistence.InMemory;

namespace LowCortisol.Platform.API.Iam.Infrastructure.Persistence.InMemory.Repositories;

public sealed class UserRepository : BaseRepository<User>, IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context) : base(context.Users)
    {
        _context = context;
    }

    public Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var normalizedEmail = User.NormalizeEmail(email);
        return Task.FromResult(_context.Users.Any(user => user.Email == normalizedEmail));
    }

    public Task<User?> FindByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var normalizedEmail = User.NormalizeEmail(email);
        return Task.FromResult(_context.Users.FirstOrDefault(user => user.Email == normalizedEmail));
    }
}
