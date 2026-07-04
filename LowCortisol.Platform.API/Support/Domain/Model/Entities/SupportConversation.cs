using LowCortisol.Platform.API.Shared.Domain.Model;
using LowCortisol.Platform.API.Support.Domain.Model.ValueObjects;

namespace LowCortisol.Platform.API.Support.Domain.Model.Entities;

public class SupportConversation : IEntity, IAuditableEntity
{
    private SupportConversation()
    {
    }

    public SupportConversation(Guid id, Guid ticketId)
    {
        if (ticketId == Guid.Empty) throw new ArgumentException("Ticket id is required.", nameof(ticketId));

        Id = id == Guid.Empty ? Guid.NewGuid() : id;
        TicketId = ticketId;
        Status = SupportConversationStatus.Active;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = CreatedAt;
    }

    public Guid Id { get; private set; }
    public Guid TicketId { get; private set; }
    public SupportConversationStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    public void Close()
    {
        Status = SupportConversationStatus.Closed;
        UpdatedAt = DateTime.UtcNow;
    }
}
