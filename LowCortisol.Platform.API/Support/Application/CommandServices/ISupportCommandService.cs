using LowCortisol.Platform.API.Shared.Application.Results;
using LowCortisol.Platform.API.Support.Domain.Model.Aggregates;
using LowCortisol.Platform.API.Support.Domain.Model.Commands;
using LowCortisol.Platform.API.Support.Domain.Model.Entities;

namespace LowCortisol.Platform.API.Support.Application.CommandServices;

public interface ISupportCommandService
{
    Task<Result<SupportTicket>> Handle(CreateSupportTicketCommand command, CancellationToken cancellationToken = default);
    Task<Result<SupportTicket>> Handle(UpdateSupportTicketCommand command, CancellationToken cancellationToken = default);
    Task<Result<SupportMessage>> Handle(SendSupportMessageCommand command, CancellationToken cancellationToken = default);
}
