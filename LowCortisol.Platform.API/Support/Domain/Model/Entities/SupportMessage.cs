using LowCortisol.Platform.API.Shared.Domain.Model;

namespace LowCortisol.Platform.API.Support.Domain.Model.Entities;

public class SupportMessage : IEntity, IAuditableEntity
{
    private SupportMessage()
    {
    }

    public SupportMessage(Guid id, Guid ticketId, string senderId, string senderType, string content)
    {
        if (ticketId == Guid.Empty) throw new ArgumentException("Ticket id is required.", nameof(ticketId));
        if (string.IsNullOrWhiteSpace(content)) throw new ArgumentException("Message content is required.", nameof(content));

        Id = id == Guid.Empty ? Guid.NewGuid() : id;
        TicketId = ticketId;
        SenderId = string.IsNullOrWhiteSpace(senderId) ? "system" : senderId.Trim();
        SenderType = string.IsNullOrWhiteSpace(senderType) ? "user" : senderType.Trim();
        Content = content.Trim();
        Status = "sent";
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = CreatedAt;
    }

    public Guid Id { get; private set; }
    public Guid TicketId { get; private set; }
    public string SenderId { get; private set; } = string.Empty;
    public string SenderType { get; private set; } = "user";
    public string Content { get; private set; } = string.Empty;
    public string Status { get; private set; } = "sent";
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
}
