using LowCortisol.Platform.API.Notification.Application.CommandServices;
using LowCortisol.Platform.API.Notification.Application.Errors;
using LowCortisol.Platform.API.Notification.Application.Internal.Parsing;
using LowCortisol.Platform.API.Notification.Domain.Model.Aggregates;
using LowCortisol.Platform.API.Notification.Domain.Model.Commands;
using LowCortisol.Platform.API.Notification.Domain.Model.ValueObjects;
using LowCortisol.Platform.API.Notification.Domain.Repositories;
using LowCortisol.Platform.API.Shared.Application.Results;
using LowCortisol.Platform.API.Shared.Domain.Repositories;

namespace LowCortisol.Platform.API.Notification.Application.Internal.CommandServices;

public sealed class NotificationChannelCommandService : INotificationChannelCommandService
{
    private readonly INotificationChannelRepository _channelRepository;
    private readonly IUnitOfWork _unitOfWork;

    public NotificationChannelCommandService(
        INotificationChannelRepository channelRepository,
        IUnitOfWork unitOfWork)
    {
        _channelRepository = channelRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<NotificationChannel>> Handle(
        CreateNotificationChannelCommand command,
        CancellationToken cancellationToken = default)
    {
        if (!NotificationEnumParser.TryParse<NotificationChannelType>(command.Type, out var type))
            return Result<NotificationChannel>.Failure(NotificationError.InvalidChannelType.ToString());

        try
        {
            var channel = new NotificationChannel(Guid.NewGuid(), command.Name, type, command.IsActive);
            await _channelRepository.AddAsync(channel, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return Result<NotificationChannel>.Success(channel);
        }
        catch (ArgumentException)
        {
            return Result<NotificationChannel>.Failure(NotificationError.UnexpectedError.ToString());
        }
    }

    public async Task<Result<NotificationChannel>> Handle(
        UpdateNotificationChannelStatusCommand command,
        CancellationToken cancellationToken = default)
    {
        var channel = await _channelRepository.FindByIdAsync(command.ChannelId, cancellationToken);
        if (channel is null) return Result<NotificationChannel>.Failure(NotificationError.ChannelNotFound.ToString());

        if (command.IsActive) channel.Activate();
        else channel.Deactivate();

        _channelRepository.Update(channel);
        await _unitOfWork.CompleteAsync(cancellationToken);

        return Result<NotificationChannel>.Success(channel);
    }
}
