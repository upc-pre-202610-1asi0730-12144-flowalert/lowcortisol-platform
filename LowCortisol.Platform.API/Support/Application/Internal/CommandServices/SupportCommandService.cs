using LowCortisol.Platform.API.Shared.Application.Results;
using LowCortisol.Platform.API.Shared.Domain.Repositories;
using LowCortisol.Platform.API.Support.Application.CommandServices;
using LowCortisol.Platform.API.Support.Application.Errors;
using LowCortisol.Platform.API.Support.Domain.Model.Aggregates;
using LowCortisol.Platform.API.Support.Domain.Model.Commands;
using LowCortisol.Platform.API.Support.Domain.Model.Entities;
using LowCortisol.Platform.API.Support.Domain.Model.ValueObjects;
using LowCortisol.Platform.API.Support.Domain.Repositories;

namespace LowCortisol.Platform.API.Support.Application.Internal.CommandServices;

public sealed class SupportCommandService : ISupportCommandService
{
    private readonly ISupportTicketRepository _ticketRepository;
    private readonly ISupportConversationRepository _conversationRepository;
    private readonly ISupportMessageRepository _messageRepository;
    private readonly IUnitOfWork _unitOfWork;

    public SupportCommandService(
        ISupportTicketRepository ticketRepository,
        ISupportConversationRepository conversationRepository,
        ISupportMessageRepository messageRepository,
        IUnitOfWork unitOfWork)
    {
        _ticketRepository = ticketRepository;
        _conversationRepository = conversationRepository;
        _messageRepository = messageRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<SupportTicket>> Handle(CreateSupportTicketCommand command, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(command.Title))
            return Result<SupportTicket>.Failure(SupportError.TitleRequired.ToString());
        if (string.IsNullOrWhiteSpace(command.Description))
            return Result<SupportTicket>.Failure(SupportError.DescriptionRequired.ToString());

        var priority = command.Category.Equals("incident", StringComparison.OrdinalIgnoreCase)
            ? SupportTicketPriority.Critical
            : command.Category.Equals("device", StringComparison.OrdinalIgnoreCase)
                ? SupportTicketPriority.High
                : SupportTicketPriority.Medium;
        var agentId = priority is SupportTicketPriority.Critical or SupportTicketPriority.High
            ? "operations-agent"
            : string.Empty;

        var ticket = new SupportTicket(
            Guid.NewGuid(),
            command.UserId,
            command.SiteId,
            command.Title,
            command.Description,
            command.Category,
            priority,
            agentId);
        var conversation = new SupportConversation(Guid.NewGuid(), ticket.Id);
        var message = new SupportMessage(Guid.NewGuid(), ticket.Id, command.UserId, "user", command.Description);

        await _ticketRepository.AddAsync(ticket, cancellationToken);
        await _conversationRepository.AddAsync(conversation, cancellationToken);
        await _messageRepository.AddAsync(message, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);

        return Result<SupportTicket>.Success(ticket);
    }

    public async Task<Result<SupportTicket>> Handle(UpdateSupportTicketCommand command, CancellationToken cancellationToken = default)
    {
        var ticket = await _ticketRepository.FindByIdAsync(command.TicketId, cancellationToken);
        if (ticket is null) return Result<SupportTicket>.Failure(SupportError.TicketNotFound.ToString());
        if (!Enum.TryParse<SupportTicketStatus>(command.Status, true, out var status))
            return Result<SupportTicket>.Failure(SupportError.InvalidStatus.ToString());

        try
        {
            ticket.UpdateStatus(status);
            _ticketRepository.Update(ticket);
            await _unitOfWork.CompleteAsync(cancellationToken);
            return Result<SupportTicket>.Success(ticket);
        }
        catch (InvalidOperationException)
        {
            return Result<SupportTicket>.Failure(SupportError.InvalidStatus.ToString());
        }
    }

    public async Task<Result<SupportMessage>> Handle(SendSupportMessageCommand command, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(command.Content))
            return Result<SupportMessage>.Failure(SupportError.MessageRequired.ToString());

        var ticket = await _ticketRepository.FindByIdAsync(command.TicketId, cancellationToken);
        if (ticket is null) return Result<SupportMessage>.Failure(SupportError.TicketNotFound.ToString());

        var message = new SupportMessage(Guid.NewGuid(), command.TicketId, command.SenderId, command.SenderType, command.Content);
        await _messageRepository.AddAsync(message, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);

        return Result<SupportMessage>.Success(message);
    }
}
