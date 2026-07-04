using LowCortisol.Platform.API.Shared.Domain.Repositories;
using LowCortisol.Platform.API.Support.Domain.Model.Aggregates;
using LowCortisol.Platform.API.Support.Domain.Model.Entities;
using LowCortisol.Platform.API.Support.Domain.Model.ValueObjects;

namespace LowCortisol.Platform.API.Support.Domain.Repositories;

public interface ISupportTicketRepository : IBaseRepository<SupportTicket>
{
    Task<IReadOnlyCollection<SupportTicket>> ListByUserIdAsync(string userId, CancellationToken cancellationToken = default);
    Task<int> CountByStatusAsync(SupportTicketStatus status, CancellationToken cancellationToken = default);
}

public interface ISupportMessageRepository : IBaseRepository<SupportMessage>
{
    Task<IReadOnlyCollection<SupportMessage>> ListByTicketIdAsync(Guid ticketId, CancellationToken cancellationToken = default);
}

public interface ISupportConversationRepository : IBaseRepository<SupportConversation>
{
    Task<SupportConversation?> FindByTicketIdAsync(Guid ticketId, CancellationToken cancellationToken = default);
}

public interface ISupportAgentRepository : IBaseRepository<SupportAgent>
{
    Task<int> CountAvailableAsync(CancellationToken cancellationToken = default);
}

public interface IKnowledgeArticleRepository : IBaseRepository<KnowledgeArticle>
{
}
