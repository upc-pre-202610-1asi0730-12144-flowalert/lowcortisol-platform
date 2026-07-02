namespace LowCortisol.Platform.API.Iam.Application.OutboundServices;

public interface IPasswordHashingService
{
    string Hash(string password);
    bool Verify(string password, string passwordHash);
}
