namespace LowCortisol.Platform.API.Support.Domain.Model.Commands;

public record CreateSupportTicketCommand(
    string UserId,
    string SiteId,
    string Title,
    string Description,
    string Category);

public record UpdateSupportTicketCommand(Guid TicketId, string Status);
public record SendSupportMessageCommand(Guid TicketId, string SenderId, string SenderType, string Content);
