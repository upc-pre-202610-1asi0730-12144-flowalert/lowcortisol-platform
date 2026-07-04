using LowCortisol.Platform.API.Support.Domain.Model.Aggregates;
using LowCortisol.Platform.API.Support.Domain.Model.Entities;
using LowCortisol.Platform.API.Support.Domain.Model.ReadModels;
using LowCortisol.Platform.API.Support.Interfaces.Rest.Resources;

namespace LowCortisol.Platform.API.Support.Interfaces.Rest.Transform;

public static class SupportTicketResourceFromEntityAssembler
{
    public static SupportTicketResource ToResourceFromEntity(SupportTicket entity) =>
        new(
            entity.Id,
            entity.UserId,
            entity.SiteId,
            entity.Title,
            entity.Description,
            entity.Category,
            entity.Priority.ToString().ToLowerInvariant(),
            entity.Status.ToString().ToLowerInvariant(),
            entity.AssignedAgentId,
            entity.CreatedAt,
            entity.UpdatedAt);
}

public static class SupportMessageResourceFromEntityAssembler
{
    public static SupportMessageResource ToResourceFromEntity(SupportMessage entity) =>
        new(
            entity.Id,
            entity.TicketId,
            entity.SenderId,
            entity.SenderType,
            entity.Content,
            entity.Status,
            entity.CreatedAt,
            entity.UpdatedAt);
}

public static class SupportConversationResourceFromViewAssembler
{
    public static SupportConversationResource ToResourceFromView(SupportConversationView view) =>
        new(
            view.Conversation.Id,
            view.Conversation.TicketId,
            view.Conversation.Status.ToString().ToLowerInvariant(),
            view.Messages.Select(SupportMessageResourceFromEntityAssembler.ToResourceFromEntity).ToList(),
            view.Conversation.CreatedAt,
            view.Conversation.UpdatedAt);
}

public static class SupportAgentResourceFromEntityAssembler
{
    public static SupportAgentResource ToResourceFromEntity(SupportAgent entity) =>
        new(
            entity.Id,
            entity.Name,
            entity.Specialty,
            entity.Status.ToString().ToLowerInvariant(),
            entity.CreatedAt,
            entity.UpdatedAt);
}

public static class KnowledgeArticleResourceFromEntityAssembler
{
    public static KnowledgeArticleResource ToResourceFromEntity(KnowledgeArticle entity) =>
        new(
            entity.Id,
            entity.Title,
            entity.Summary,
            entity.Category,
            entity.HelpfulCount,
            entity.Content,
            entity.CreatedAt,
            entity.UpdatedAt);
}

public static class SupportSummaryResourceFromResultAssembler
{
    public static SupportSummaryResource ToResourceFromResult(SupportSummary summary) =>
        new(
            summary.TotalTickets,
            summary.OpenTickets,
            summary.AssignedTickets,
            summary.ResolvedTickets,
            summary.AvailableAgents,
            summary.TotalArticles,
            summary.TotalConversations);
}
