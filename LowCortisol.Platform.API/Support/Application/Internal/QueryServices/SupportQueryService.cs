using LowCortisol.Platform.API.Support.Application.QueryServices;
using LowCortisol.Platform.API.Support.Domain.Model.Aggregates;
using LowCortisol.Platform.API.Support.Domain.Model.Entities;
using LowCortisol.Platform.API.Support.Domain.Model.Queries;
using LowCortisol.Platform.API.Support.Domain.Model.ReadModels;
using LowCortisol.Platform.API.Support.Domain.Model.ValueObjects;
using LowCortisol.Platform.API.Support.Domain.Repositories;

namespace LowCortisol.Platform.API.Support.Application.Internal.QueryServices;

public sealed class SupportQueryService : ISupportQueryService
{
    private readonly ISupportTicketRepository _ticketRepository;
    private readonly ISupportConversationRepository _conversationRepository;
    private readonly ISupportMessageRepository _messageRepository;
    private readonly ISupportAgentRepository _agentRepository;
    private readonly IKnowledgeArticleRepository _articleRepository;

    public SupportQueryService(
        ISupportTicketRepository ticketRepository,
        ISupportConversationRepository conversationRepository,
        ISupportMessageRepository messageRepository,
        ISupportAgentRepository agentRepository,
        IKnowledgeArticleRepository articleRepository)
    {
        _ticketRepository = ticketRepository;
        _conversationRepository = conversationRepository;
        _messageRepository = messageRepository;
        _agentRepository = agentRepository;
        _articleRepository = articleRepository;
    }

    public async Task<IReadOnlyCollection<SupportTicket>> Handle(GetSupportTicketsQuery query, CancellationToken cancellationToken = default)
    {
        if (!string.IsNullOrWhiteSpace(query.UserId))
        {
            return await _ticketRepository.ListByUserIdAsync(query.UserId, cancellationToken);
        }

        return await _ticketRepository.ListAsync(cancellationToken);
    }

    public Task<IReadOnlyCollection<SupportAgent>> Handle(GetSupportAgentsQuery query, CancellationToken cancellationToken = default) =>
        _agentRepository.ListAsync(cancellationToken);

    public Task<IReadOnlyCollection<KnowledgeArticle>> Handle(GetKnowledgeArticlesQuery query, CancellationToken cancellationToken = default) =>
        _articleRepository.ListAsync(cancellationToken);

    public async Task<IReadOnlyCollection<SupportConversationView>> Handle(GetSupportConversationsQuery query, CancellationToken cancellationToken = default)
    {
        var conversations = await _conversationRepository.ListAsync(cancellationToken);
        var views = new List<SupportConversationView>();

        foreach (var conversation in conversations)
        {
            var messages = await _messageRepository.ListByTicketIdAsync(conversation.TicketId, cancellationToken);
            views.Add(new SupportConversationView(conversation, messages));
        }

        return views;
    }

    public async Task<SupportConversationView?> Handle(GetSupportConversationByTicketQuery query, CancellationToken cancellationToken = default)
    {
        var conversation = await _conversationRepository.FindByTicketIdAsync(query.TicketId, cancellationToken);
        if (conversation is null) return null;
        var messages = await _messageRepository.ListByTicketIdAsync(query.TicketId, cancellationToken);
        return new SupportConversationView(conversation, messages);
    }

    public async Task<SupportSummary> Handle(GetSupportSummaryQuery query, CancellationToken cancellationToken = default)
    {
        var tickets = await _ticketRepository.ListAsync(cancellationToken);
        var articles = await _articleRepository.ListAsync(cancellationToken);
        var conversations = await _conversationRepository.ListAsync(cancellationToken);

        return new SupportSummary(
            tickets.Count,
            tickets.Count(ticket => ticket.Status == SupportTicketStatus.Open),
            tickets.Count(ticket => ticket.Status == SupportTicketStatus.Assigned),
            tickets.Count(ticket => ticket.Status == SupportTicketStatus.Resolved),
            await _agentRepository.CountAvailableAsync(cancellationToken),
            articles.Count,
            conversations.Count);
    }
}
