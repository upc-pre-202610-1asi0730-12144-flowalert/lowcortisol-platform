namespace LowCortisol.Platform.API.Support.Domain.Model.Queries;

public record GetSupportTicketsQuery(string? UserId = null);
public record GetSupportAgentsQuery;
public record GetKnowledgeArticlesQuery;
public record GetSupportConversationsQuery;
public record GetSupportConversationByTicketQuery(Guid TicketId);
public record GetSupportSummaryQuery;
