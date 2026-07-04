namespace LowCortisol.Platform.API.Support.Interfaces.Rest.Resources;

public record SupportTicketResource(
    Guid Id,
    string UserId,
    string SiteId,
    string Title,
    string Description,
    string Category,
    string Priority,
    string Status,
    string AssignedAgentId,
    DateTime CreatedAt,
    DateTime UpdatedAt);

public record SupportMessageResource(
    Guid Id,
    Guid TicketId,
    string SenderId,
    string SenderType,
    string Content,
    string Status,
    DateTime CreatedAt,
    DateTime UpdatedAt);

public record SupportConversationResource(
    Guid Id,
    Guid TicketId,
    string Status,
    IReadOnlyCollection<SupportMessageResource> Messages,
    DateTime CreatedAt,
    DateTime UpdatedAt);

public record SupportAgentResource(
    Guid Id,
    string Name,
    string Specialty,
    string Status,
    DateTime CreatedAt,
    DateTime UpdatedAt);

public record KnowledgeArticleResource(
    Guid Id,
    string Title,
    string Summary,
    string Category,
    int HelpfulCount,
    string Content,
    DateTime CreatedAt,
    DateTime UpdatedAt);

public record SupportSummaryResource(
    int TotalTickets,
    int OpenTickets,
    int AssignedTickets,
    int ResolvedTickets,
    int AvailableAgents,
    int TotalArticles,
    int TotalConversations);

public record CreateSupportTicketResource(
    string UserId,
    string SiteId,
    string Title,
    string Description,
    string Category);

public record UpdateSupportTicketResource(string Status);

public record SendSupportMessageResource(
    string SenderId,
    string SenderType,
    string Content);
