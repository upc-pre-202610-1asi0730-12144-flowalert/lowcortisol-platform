using LowCortisol.Platform.API.Shared.Domain.Model;

namespace LowCortisol.Platform.API.Iam.Domain.Model.Aggregates;

public sealed class User : IEntity, IAuditableEntity
{
    private User()
    {
        Id = Guid.Empty;
        FirstName = string.Empty;
        LastName = string.Empty;
        Email = string.Empty;
        PasswordHash = string.Empty;
        Role = "Operator";
    }

    public User(
        Guid id,
        string firstName,
        string lastName,
        string email,
        string passwordHash,
        string role = "Operator")
    {
        Id = id;
        FirstName = firstName.Trim();
        LastName = lastName.Trim();
        Email = NormalizeEmail(email);
        PasswordHash = passwordHash;
        Role = string.IsNullOrWhiteSpace(role) ? "Operator" : role.Trim();
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = CreatedAt;
    }

    public Guid Id { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Email { get; private set; }
    public string PasswordHash { get; private set; }
    public string Role { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }

    public static string NormalizeEmail(string email) => email.Trim().ToLowerInvariant();
}
