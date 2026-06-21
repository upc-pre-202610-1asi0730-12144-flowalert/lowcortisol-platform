namespace LowCortisol.Platform.API.Notification.Domain.Model.ValueObjects;

public sealed record Recipient
{
    private Recipient()
    {
        UserId = string.Empty;
        Email = string.Empty;
        DisplayName = string.Empty;
    }

    public Recipient(string userId, string email, string displayName)
    {
        UserId = userId?.Trim() ?? string.Empty;
        Email = email?.Trim() ?? string.Empty;
        DisplayName = string.IsNullOrWhiteSpace(displayName) ? "Operations" : displayName.Trim();
    }

    public string UserId { get; init; }
    public string Email { get; init; }
    public string DisplayName { get; init; }

    public static Recipient OperationsDesk() => new("operations", string.Empty, "Operations desk");
}
