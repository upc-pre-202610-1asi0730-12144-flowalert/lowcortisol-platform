using LowCortisol.Platform.API.Iam.Application.OutboundServices;

namespace LowCortisol.Platform.API.Iam.Infrastructure.Hashing.BCrypt;

public sealed class BCryptPasswordHashingService : IPasswordHashingService
{
    public string Hash(string password) => global::BCrypt.Net.BCrypt.HashPassword(password);

    public bool Verify(string password, string passwordHash) =>
        global::BCrypt.Net.BCrypt.Verify(password, passwordHash);
}
