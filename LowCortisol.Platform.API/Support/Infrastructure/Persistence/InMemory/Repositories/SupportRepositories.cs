using LowCortisol.Platform.API.Shared.Infrastructure.Persistence.InMemory;
using LowCortisol.Platform.API.Support.Domain.Model.Aggregates;
using LowCortisol.Platform.API.Support.Domain.Model.Entities;
using LowCortisol.Platform.API.Support.Domain.Model.ValueObjects;
using LowCortisol.Platform.API.Support.Domain.Repositories;

namespace LowCortisol.Platform.API.Support.Infrastructure.Persistence.InMemory.Repositories;

public sealed class SupportTicketRepository : BaseRepository<SupportTicket>, ISupportTicketRepository
{
    private readonly List<SupportTicket> _tickets;

    public SupportTicketRepository(AppDbContext context) : base(context.SupportTickets)
    {
        _tickets = context.SupportTickets;
    }

    public Task<IReadOnlyCollection<SupportTicket>> ListByUserIdAsync(string userId, CancellationToken cancellationToken = default) =>
        Task.FromResult<IReadOnlyCollection<SupportTicket>>(
            _tickets.Where(ticket => ticket.UserId == userId)
                .OrderByDescending(ticket => ticket.CreatedAt)
                .ToList());

    public Task<int> CountByStatusAsync(SupportTicketStatus status, CancellationToken cancellationToken = default) =>
        Task.FromResult(_tickets.Count(ticket => ticket.Status == status));
}

public sealed class SupportMessageRepository : BaseRepository<SupportMessage>, ISupportMessageRepository
{
    private readonly List<SupportMessage> _messages;

    public SupportMessageRepository(AppDbContext context) : base(context.SupportMessages)
    {
        _messages = context.SupportMessages;
    }

    public Task<IReadOnlyCollection<SupportMessage>> ListByTicketIdAsync(Guid ticketId, CancellationToken cancellationToken = default) =>
        Task.FromResult<IReadOnlyCollection<SupportMessage>>(
            _messages.Where(message => message.TicketId == ticketId)
                .OrderBy(message => message.CreatedAt)
                .ToList());
}

public sealed class SupportConversationRepository : BaseRepository<SupportConversation>, ISupportConversationRepository
{
    private readonly List<SupportConversation> _conversations;

    public SupportConversationRepository(AppDbContext context) : base(context.SupportConversations)
    {
        _conversations = context.SupportConversations;
    }

    public Task<SupportConversation?> FindByTicketIdAsync(Guid ticketId, CancellationToken cancellationToken = default) =>
        Task.FromResult(_conversations.FirstOrDefault(conversation => conversation.TicketId == ticketId));
}

public sealed class SupportAgentRepository : BaseRepository<SupportAgent>, ISupportAgentRepository
{
    private readonly List<SupportAgent> _agents;

    public SupportAgentRepository(AppDbContext context) : base(context.SupportAgents)
    {
        _agents = context.SupportAgents;
    }

    public Task<int> CountAvailableAsync(CancellationToken cancellationToken = default) =>
        Task.FromResult(_agents.Count(agent => agent.Status == SupportAgentStatus.Available));
}

public sealed class KnowledgeArticleRepository : BaseRepository<KnowledgeArticle>, IKnowledgeArticleRepository
{
    public KnowledgeArticleRepository(AppDbContext context) : base(context.KnowledgeArticles)
    {
    }
}
