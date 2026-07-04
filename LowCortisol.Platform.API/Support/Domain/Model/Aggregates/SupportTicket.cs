using LowCortisol.Platform.API.Shared.Domain.Model;
using LowCortisol.Platform.API.Support.Domain.Model.ValueObjects;

namespace LowCortisol.Platform.API.Support.Domain.Model.Aggregates;

public class SupportTicket : IEntity, IAuditableEntity
{
    private SupportTicket()
    {
    }

    public SupportTicket(
        Guid id,
        string userId,
        string siteId,
        string title,
        string description,
        string category,
        SupportTicketPriority priority,
        string? assignedAgentId = null)
    {
        if (string.IsNullOrWhiteSpace(userId)) throw new ArgumentException("User id is required.", nameof(userId));
        if (string.IsNullOrWhiteSpace(title)) throw new ArgumentException("Ticket title is required.", nameof(title));
        if (string.IsNullOrWhiteSpace(description)) throw new ArgumentException("Ticket description is required.", nameof(description));

        Id = id == Guid.Empty ? Guid.NewGuid() : id;
        UserId = userId.Trim();
        SiteId = string.IsNullOrWhiteSpace(siteId) ? "SITE-001" : siteId.Trim();
        Title = title.Trim();
        Description = description.Trim();
        Category = string.IsNullOrWhiteSpace(category) ? "technical" : category.Trim();
        Priority = priority;
        Status = string.IsNullOrWhiteSpace(assignedAgentId) ? SupportTicketStatus.Open : SupportTicketStatus.Assigned;
        AssignedAgentId = assignedAgentId ?? string.Empty;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = CreatedAt;
    }

    public Guid Id { get; private set; }
    public string UserId { get; private set; } = string.Empty;
    public string SiteId { get; private set; } = "SITE-001";
    public string Title { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public string Category { get; private set; } = "technical";
    public SupportTicketPriority Priority { get; private set; }
    public SupportTicketStatus Status { get; private set; }
    public string AssignedAgentId { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    public void UpdateStatus(SupportTicketStatus status)
    {
        if (Status == SupportTicketStatus.Closed && status != SupportTicketStatus.Closed)
            throw new InvalidOperationException("Closed tickets cannot be reopened.");

        Status = status;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Resolve() => UpdateStatus(SupportTicketStatus.Resolved);

    public void Close() => UpdateStatus(SupportTicketStatus.Closed);
}
