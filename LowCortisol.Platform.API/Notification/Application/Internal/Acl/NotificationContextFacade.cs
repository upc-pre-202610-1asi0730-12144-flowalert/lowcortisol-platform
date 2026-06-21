using LowCortisol.Platform.API.Monitoring.Domain.Model.Events;
using LowCortisol.Platform.API.Notification.Application.Acl;
using LowCortisol.Platform.API.Notification.Application.Errors;
using LowCortisol.Platform.API.Notification.Application.Results;
using LowCortisol.Platform.API.Notification.Domain.Model.Aggregates;
using LowCortisol.Platform.API.Notification.Domain.Model.ValueObjects;
using LowCortisol.Platform.API.Notification.Domain.Repositories;
using LowCortisol.Platform.API.Shared.Application.Results;
using LowCortisol.Platform.API.Shared.Domain.Repositories;

namespace LowCortisol.Platform.API.Notification.Application.Internal.Acl;

public sealed class NotificationContextFacade : INotificationContextFacade
{
    private readonly IAlertRepository _alertRepository;
    private readonly IIncidentRepository _incidentRepository;
    private readonly INotificationChannelRepository _channelRepository;
    private readonly IUnitOfWork _unitOfWork;

    public NotificationContextFacade(
        IAlertRepository alertRepository,
        IIncidentRepository incidentRepository,
        INotificationChannelRepository channelRepository,
        IUnitOfWork unitOfWork)
    {
        _alertRepository = alertRepository;
        _incidentRepository = incidentRepository;
        _channelRepository = channelRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<NotificationRiskResponseResult>> HandleCriticalAnomalyDetectedAsync(
        CriticalAnomalyDetected criticalEvent,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var existingAlerts = await _alertRepository.ListByAnomalyIdAsync(
                criticalEvent.AnomalyId,
                cancellationToken);
            var existingAlert = existingAlerts.FirstOrDefault();
            if (existingAlert is not null)
            {
                var incidents = await _incidentRepository.ListByAlertIdAsync(existingAlert.Id, cancellationToken);
                var existingIncident = incidents.FirstOrDefault();
                var existingDelivery = existingAlert.Deliveries.FirstOrDefault();
                if (existingIncident is not null && existingDelivery is not null)
                    return Result<NotificationRiskResponseResult>.Success(
                        new NotificationRiskResponseResult(existingAlert, existingIncident, existingDelivery));
            }

            var alert = Alert.FromCriticalAnomaly(
                criticalEvent.AnomalyId,
                criticalEvent.ReadingId,
                criticalEvent.SiteId,
                criticalEvent.RoomId,
                criticalEvent.DeviceGroupId,
                criticalEvent.DeviceId,
                criticalEvent.SensorId,
                criticalEvent.ResourceType.ToString(),
                criticalEvent.Value,
                criticalEvent.LimitValue,
                criticalEvent.Unit,
                criticalEvent.DetectedAt);

            var incident = Incident.FromAlert(alert);
            var channel = await EnsureInAppChannelAsync(cancellationToken);
            var delivery = alert.RegisterDelivery(channel, Recipient.OperationsDesk());

            await _alertRepository.AddAsync(alert, cancellationToken);
            await _incidentRepository.AddAsync(incident, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return Result<NotificationRiskResponseResult>.Success(
                new NotificationRiskResponseResult(alert, incident, delivery));
        }
        catch (ArgumentException)
        {
            return Result<NotificationRiskResponseResult>.Failure(NotificationError.UnexpectedError.ToString());
        }
        catch (InvalidOperationException exception)
        {
            return Result<NotificationRiskResponseResult>.Failure(
                $"{NotificationError.InvalidStateTransition}:{exception.Message}");
        }
    }

    private async Task<NotificationChannel> EnsureInAppChannelAsync(CancellationToken cancellationToken)
    {
        var channel = await _channelRepository.FindDefaultInAppAsync(cancellationToken);
        if (channel is not null)
        {
            if (!channel.IsActive)
            {
                channel.Activate();
                _channelRepository.Update(channel);
            }

            return channel;
        }

        channel = NotificationChannel.DefaultInApp();
        await _channelRepository.AddAsync(channel, cancellationToken);

        return channel;
    }
}
