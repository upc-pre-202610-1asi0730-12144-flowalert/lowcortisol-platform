using LowCortisol.Platform.API.DeviceControl.Application.Acl;
using LowCortisol.Platform.API.Monitoring.Application.CommandServices;
using LowCortisol.Platform.API.Monitoring.Application.Errors;
using LowCortisol.Platform.API.Monitoring.Application.Internal.Parsing;
using LowCortisol.Platform.API.Monitoring.Application.Results;
using LowCortisol.Platform.API.Monitoring.Domain.Model.Commands;
using LowCortisol.Platform.API.Monitoring.Domain.Model.Entities;
using LowCortisol.Platform.API.Monitoring.Domain.Model.ValueObjects;
using LowCortisol.Platform.API.Monitoring.Domain.Repositories;
using LowCortisol.Platform.API.Monitoring.Domain.Services;
using LowCortisol.Platform.API.Notification.Application.Acl;
using LowCortisol.Platform.API.Shared.Application.Results;
using LowCortisol.Platform.API.Shared.Domain.Repositories;

namespace LowCortisol.Platform.API.Monitoring.Application.Internal.CommandServices;

public sealed class ReadingCommandService : IReadingCommandService
{
    private readonly IConsumptionReadingRepository _readingRepository;
    private readonly IThresholdRepository _thresholdRepository;
    private readonly IAnomalyRepository _anomalyRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly AnomalyDetectionService _anomalyDetectionService;
    private readonly IDeviceControlContextFacade _deviceControlContextFacade;
    private readonly INotificationContextFacade _notificationContextFacade;

    public ReadingCommandService(
        IConsumptionReadingRepository readingRepository,
        IThresholdRepository thresholdRepository,
        IAnomalyRepository anomalyRepository,
        IUnitOfWork unitOfWork,
        AnomalyDetectionService anomalyDetectionService,
        IDeviceControlContextFacade deviceControlContextFacade,
        INotificationContextFacade notificationContextFacade)
    {
        _readingRepository = readingRepository;
        _thresholdRepository = thresholdRepository;
        _anomalyRepository = anomalyRepository;
        _unitOfWork = unitOfWork;
        _anomalyDetectionService = anomalyDetectionService;
        _deviceControlContextFacade = deviceControlContextFacade;
        _notificationContextFacade = notificationContextFacade;
    }

    public async Task<Result<RegisterConsumptionReadingResult>> Handle(
        RegisterConsumptionReadingCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationError = ValidateCommand(command);
        if (validationError is not null) return Result<RegisterConsumptionReadingResult>.Failure(validationError);

        if (!MonitoringEnumParser.TryParse<ResourceType>(command.ResourceType, out var resourceType))
            return Result<RegisterConsumptionReadingResult>.Failure(MonitoringError.ResourceTypeInvalid.ToString());

        var sensorValidationError = await ValidateSensorOwnershipAsync(command, cancellationToken);
        if (sensorValidationError is not null)
        {
            return Result<RegisterConsumptionReadingResult>.Failure(sensorValidationError);
        }

        try
        {
            var reading = new ConsumptionReading(
                Guid.NewGuid(),
                command.SiteId,
                command.RoomId,
                command.DeviceGroupId,
                command.DeviceId,
                command.SensorId,
                resourceType,
                command.Value,
                command.Unit,
                command.CapturedAt);

            var thresholds = await _thresholdRepository.ListApplicableToReadingAsync(reading, cancellationToken);
            var detectionResults = _anomalyDetectionService.Detect(reading, thresholds);
            var anomalies = detectionResults
                .Select(result => Anomaly.FromReading(reading, result.Threshold))
                .ToList();

            if (anomalies.Any(anomaly => anomaly.Severity == AnomalySeverity.Critical))
            {
                reading.MarkStatus(ReadingStatus.Critical);
            }
            else if (anomalies.Count > 0)
            {
                reading.MarkStatus(ReadingStatus.Warning);
            }

            await _readingRepository.AddAsync(reading, cancellationToken);
            foreach (var anomaly in anomalies)
            {
                await _anomalyRepository.AddAsync(anomaly, cancellationToken);
            }

            await _unitOfWork.CompleteAsync(cancellationToken);

            var criticalEvents = anomalies
                .Where(anomaly => anomaly.Severity == AnomalySeverity.Critical)
                .Select(anomaly => anomaly.ToCriticalEvent())
                .ToList();

            foreach (var criticalEvent in criticalEvents)
            {
                var notificationResult = await _notificationContextFacade.HandleCriticalAnomalyDetectedAsync(
                    criticalEvent,
                    cancellationToken);

                if (notificationResult.IsFailure)
                {
                    return Result<RegisterConsumptionReadingResult>.Failure(
                        $"{MonitoringError.NotificationBridgeFailed}:{notificationResult.Error}");
                }
            }

            return Result<RegisterConsumptionReadingResult>.Success(
                new RegisterConsumptionReadingResult(reading, anomalies, criticalEvents));
        }
        catch (ArgumentException)
        {
            return Result<RegisterConsumptionReadingResult>.Failure(MonitoringError.UnexpectedError.ToString());
        }
    }

    private static string? ValidateCommand(RegisterConsumptionReadingCommand command)
    {
        if (command.SiteId == Guid.Empty) return MonitoringError.SiteIdRequired.ToString();
        if (command.RoomId == Guid.Empty) return MonitoringError.RoomIdRequired.ToString();
        if (command.DeviceGroupId == Guid.Empty) return MonitoringError.DeviceGroupIdRequired.ToString();
        if (command.DeviceId == Guid.Empty) return MonitoringError.DeviceIdRequired.ToString();
        if (command.SensorId == Guid.Empty) return MonitoringError.SensorIdRequired.ToString();
        if (string.IsNullOrWhiteSpace(command.ResourceType))
            return MonitoringError.ResourceTypeRequired.ToString();
        if (command.Value < 0) return MonitoringError.ReadingValueNegative.ToString();
        if (string.IsNullOrWhiteSpace(command.Unit)) return MonitoringError.UnitRequired.ToString();

        return null;
    }

    private async Task<string?> ValidateSensorOwnershipAsync(
        RegisterConsumptionReadingCommand command,
        CancellationToken cancellationToken)
    {
        var inventories = await _deviceControlContextFacade.GetInventoryByDeviceGroupIdsAsync(
            [command.DeviceGroupId],
            cancellationToken);

        var inventory = inventories.FirstOrDefault(item => item.DeviceGroupId == command.DeviceGroupId);
        if (inventory is null) return MonitoringError.SensorNotFoundInDeviceGroup.ToString();

        var sensorBelongsToDevice = inventory.Sensors.Any(sensor => sensor.Id == command.SensorId)
            && inventory.Devices.Any(device => device.Id == command.DeviceId && device.DeviceGroupId == command.DeviceGroupId)
            && inventory.Sensors.Any(sensor => sensor.Id == command.SensorId && sensor.DeviceId == command.DeviceId);

        return sensorBelongsToDevice ? null : MonitoringError.SensorNotFoundInDeviceGroup.ToString();
    }
}
