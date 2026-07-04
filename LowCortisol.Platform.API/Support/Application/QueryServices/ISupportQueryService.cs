using LowCortisol.Platform.API.Support.Domain.Model.Aggregates;
using LowCortisol.Platform.API.Support.Domain.Model.Entities;
using LowCortisol.Platform.API.Support.Domain.Model.Queries;
using LowCortisol.Platform.API.Support.Domain.Model.ReadModels;

namespace LowCortisol.Platform.API.Support.Application.QueryServices;

public interface ISupportQueryService
{
    Task<IReadOnlyCollection<SupportTicket>> Handle(GetSupportTicketsQuery query, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<SupportAgent>> Handle(GetSupportAgentsQuery query, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<KnowledgeArticle>> Handle(GetKnowledgeArticlesQuery query, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<SupportConversationView>> Handle(GetSupportConversationsQuery query, CancellationToken cancellationToken = default);
    Task<SupportConversationView?> Handle(GetSupportConversationByTicketQuery query, CancellationToken cancellationToken = default);
    Task<SupportSummary> Handle(GetSupportSummaryQuery query, CancellationToken cancellationToken = default);
}
