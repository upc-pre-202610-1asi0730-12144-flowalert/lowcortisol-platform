namespace LowCortisol.Platform.API.Support.Domain.Model.ValueObjects;

public enum SupportTicketStatus
{
    Open,
    Assigned,
    Resolved,
    Closed
}

public enum SupportTicketPriority
{
    Low,
    Medium,
    High,
    Critical
}

public enum SupportAgentStatus
{
    Available,
    Busy,
    Offline
}

public enum SupportConversationStatus
{
    Active,
    Closed
}
