namespace LowCortisol.Platform.API.Notification.Domain.Model.ValueObjects;

public sealed record AlertMessage
{
    private AlertMessage()
    {
        Title = string.Empty;
        Description = string.Empty;
    }

    public AlertMessage(string title, string description)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("Alert title is required.", nameof(title));
        }

        Title = title.Trim();
        Description = (description ?? string.Empty).Trim();
    }

    public string Title { get; init; }
    public string Description { get; init; }
}
