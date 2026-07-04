using LowCortisol.Platform.API.Support.Domain.Model.Entities;

namespace LowCortisol.Platform.API.Support.Domain.Model.ReadModels;

public record SupportConversationView(SupportConversation Conversation, IReadOnlyCollection<SupportMessage> Messages);

public record SupportSummary(
    int TotalTickets,
    int OpenTickets,
    int AssignedTickets,
    int ResolvedTickets,
    int AvailableAgents,
    int TotalArticles,
    int TotalConversations);
