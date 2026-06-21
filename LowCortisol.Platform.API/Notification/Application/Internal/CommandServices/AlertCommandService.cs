using LowCortisol.Platform.API.Notification.Application.CommandServices;
using LowCortisol.Platform.API.Notification.Application.Errors;
using LowCortisol.Platform.API.Notification.Domain.Model.Aggregates;
using LowCortisol.Platform.API.Notification.Domain.Model.Commands;
using LowCortisol.Platform.API.Notification.Domain.Repositories;
using LowCortisol.Platform.API.Shared.Application.Results;
using LowCortisol.Platform.API.Shared.Domain.Repositories;

namespace LowCortisol.Platform.API.Notification.Application.Internal.CommandServices;

public sealed class AlertCommandService : IAlertCommandService
{
    private readonly IAlertRepository _alertRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AlertCommandService(IAlertRepository alertRepository, IUnitOfWork unitOfWork)
    {
        _alertRepository = alertRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Alert>> Handle(
        CreateAlertFromCriticalAnomalyCommand command,
        CancellationToken cancellationToken = default)
    {
        if (command.AnomalyId == Guid.Empty)
            return Result<Alert>.Failure(NotificationError.AnomalyIdRequired.ToString());
        if (string.IsNullOrWhiteSpace(command.ResourceType))
            return Result<Alert>.Failure(NotificationError.ResourceTypeRequired.ToString());

        try
        {
            var alert = Alert.FromCriticalAnomaly(
                command.AnomalyId,
                command.ReadingId,
                command.SiteId,
                command.RoomId,
                command.DeviceGroupId,
                command.DeviceId,
                command.SensorId,
                command.ResourceType,
                command.Value,
                command.LimitValue,
                command.Unit,
                command.DetectedAt);

            await _alertRepository.AddAsync(alert, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return Result<Alert>.Success(alert);
        }
        catch (ArgumentException)
        {
            return Result<Alert>.Failure(NotificationError.UnexpectedError.ToString());
        }
    }

    public async Task<Result<Alert>> Handle(AcknowledgeAlertCommand command, CancellationToken cancellationToken = default)
    {
        var alert = await _alertRepository.FindByIdAsync(command.AlertId, cancellationToken);
        if (alert is null) return Result<Alert>.Failure(NotificationError.AlertNotFound.ToString());

        try
        {
            alert.Acknowledge(command.AcknowledgedBy);
            _alertRepository.Update(alert);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return Result<Alert>.Success(alert);
        }
        catch (InvalidOperationException exception)
        {
            return Result<Alert>.Failure($"{NotificationError.InvalidStateTransition}:{exception.Message}");
        }
    }

    public async Task<Result<Alert>> Handle(ResolveAlertCommand command, CancellationToken cancellationToken = default)
    {
        var alert = await _alertRepository.FindByIdAsync(command.AlertId, cancellationToken);
        if (alert is null) return Result<Alert>.Failure(NotificationError.AlertNotFound.ToString());

        try
        {
            alert.Resolve(command.ResolvedBy, command.Note);
            _alertRepository.Update(alert);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return Result<Alert>.Success(alert);
        }
        catch (InvalidOperationException exception)
        {
            return Result<Alert>.Failure($"{NotificationError.InvalidStateTransition}:{exception.Message}");
        }
    }

    public async Task<Result<Alert>> Handle(CloseAlertCommand command, CancellationToken cancellationToken = default)
    {
        var alert = await _alertRepository.FindByIdAsync(command.AlertId, cancellationToken);
        if (alert is null) return Result<Alert>.Failure(NotificationError.AlertNotFound.ToString());

        try
        {
            alert.Close(command.ClosedBy, command.Note);
            _alertRepository.Update(alert);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return Result<Alert>.Success(alert);
        }
        catch (InvalidOperationException exception)
        {
            return Result<Alert>.Failure($"{NotificationError.InvalidStateTransition}:{exception.Message}");
        }
    }
}
