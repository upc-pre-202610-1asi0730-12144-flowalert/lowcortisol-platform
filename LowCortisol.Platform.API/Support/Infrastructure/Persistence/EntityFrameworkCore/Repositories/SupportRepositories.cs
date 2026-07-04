using LowCortisol.Platform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using LowCortisol.Platform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using LowCortisol.Platform.API.Support.Domain.Model.Aggregates;
using LowCortisol.Platform.API.Support.Domain.Model.Entities;
using LowCortisol.Platform.API.Support.Domain.Model.ValueObjects;
using LowCortisol.Platform.API.Support.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LowCortisol.Platform.API.Support.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public sealed class SupportTicketRepository : BaseRepository<SupportTicket>, ISupportTicketRepository
{
    public SupportTicketRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IReadOnlyCollection<SupportTicket>> ListByUserIdAsync(string userId, CancellationToken cancellationToken = default) =>
        await Context.SupportTickets
            .Where(ticket => ticket.UserId == userId)
            .OrderByDescending(ticket => ticket.CreatedAt)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

    public Task<int> CountByStatusAsync(SupportTicketStatus status, CancellationToken cancellationToken = default) =>
        Context.SupportTickets.CountAsync(ticket => ticket.Status == status, cancellationToken);
}

public sealed class SupportMessageRepository : BaseRepository<SupportMessage>, ISupportMessageRepository
{
    public SupportMessageRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IReadOnlyCollection<SupportMessage>> ListByTicketIdAsync(Guid ticketId, CancellationToken cancellationToken = default) =>
        await Context.SupportMessages
            .Where(message => message.TicketId == ticketId)
            .OrderBy(message => message.CreatedAt)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
}

public sealed class SupportConversationRepository : BaseRepository<SupportConversation>, ISupportConversationRepository
{
    public SupportConversationRepository(AppDbContext context) : base(context)
    {
    }

    public Task<SupportConversation?> FindByTicketIdAsync(Guid ticketId, CancellationToken cancellationToken = default) =>
        Context.SupportConversations.FirstOrDefaultAsync(conversation => conversation.TicketId == ticketId, cancellationToken);
}

public sealed class SupportAgentRepository : BaseRepository<SupportAgent>, ISupportAgentRepository
{
    public SupportAgentRepository(AppDbContext context) : base(context)
    {
    }

    public Task<int> CountAvailableAsync(CancellationToken cancellationToken = default) =>
        Context.SupportAgents.CountAsync(agent => agent.Status == SupportAgentStatus.Available, cancellationToken);
}

public sealed class KnowledgeArticleRepository : BaseRepository<KnowledgeArticle>, IKnowledgeArticleRepository
{
    public KnowledgeArticleRepository(AppDbContext context) : base(context)
    {
    }
}
