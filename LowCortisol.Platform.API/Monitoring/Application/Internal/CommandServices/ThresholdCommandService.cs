using LowCortisol.Platform.API.Monitoring.Application.CommandServices;
using LowCortisol.Platform.API.Monitoring.Application.Errors;
using LowCortisol.Platform.API.Monitoring.Application.Internal.Parsing;
using LowCortisol.Platform.API.Monitoring.Domain.Model.Commands;
using LowCortisol.Platform.API.Monitoring.Domain.Model.Entities;
using LowCortisol.Platform.API.Monitoring.Domain.Model.ValueObjects;
using LowCortisol.Platform.API.Monitoring.Domain.Repositories;
using LowCortisol.Platform.API.Shared.Application.Results;
using LowCortisol.Platform.API.Shared.Domain.Repositories;

namespace LowCortisol.Platform.API.Monitoring.Application.Internal.CommandServices;

public sealed class ThresholdCommandService : IThresholdCommandService
{
    private readonly IThresholdRepository _thresholdRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ThresholdCommandService(IThresholdRepository thresholdRepository, IUnitOfWork unitOfWork)
    {
        _thresholdRepository = thresholdRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Threshold>> Handle(
        CreateThresholdCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationError = ValidateCommand(command);
        if (validationError is not null) return Result<Threshold>.Failure(validationError);

        if (!MonitoringEnumParser.TryParse<ResourceType>(command.ResourceType, out var resourceType))
            return Result<Threshold>.Failure(MonitoringError.ResourceTypeInvalid.ToString());
        if (!MonitoringEnumParser.TryParse<ThresholdOperator>(command.Operator, out var thresholdOperator))
            return Result<Threshold>.Failure(MonitoringError.ThresholdOperatorInvalid.ToString());
        if (!MonitoringEnumParser.TryParse<AnomalySeverity>(command.Severity, out var severity))
            return Result<Threshold>.Failure(MonitoringError.ThresholdSeverityInvalid.ToString());

        try
        {
            var threshold = new Threshold(
                Guid.NewGuid(),
                command.SiteId,
                command.RoomId,
                command.DeviceGroupId,
                command.SensorId,
                resourceType,
                command.Name,
                thresholdOperator,
                command.LimitValue,
                command.Unit,
                severity,
                command.IsActive);

            await _thresholdRepository.AddAsync(threshold, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return Result<Threshold>.Success(threshold);
        }
        catch (ArgumentException)
        {
            return Result<Threshold>.Failure(MonitoringError.UnexpectedError.ToString());
        }
    }

    private static string? ValidateCommand(CreateThresholdCommand command)
    {
        if (command.SiteId is null
            && command.RoomId is null
            && command.DeviceGroupId is null
            && command.SensorId is null)
        {
            return MonitoringError.ThresholdScopeRequired.ToString();
        }

        if (string.IsNullOrWhiteSpace(command.ResourceType))
            return MonitoringError.ResourceTypeRequired.ToString();
        if (string.IsNullOrWhiteSpace(command.Name)) return MonitoringError.ThresholdNameRequired.ToString();
        if (string.IsNullOrWhiteSpace(command.Operator))
            return MonitoringError.ThresholdOperatorInvalid.ToString();
        if (command.LimitValue < 0) return MonitoringError.ThresholdLimitValueInvalid.ToString();
        if (string.IsNullOrWhiteSpace(command.Unit)) return MonitoringError.UnitRequired.ToString();
        if (string.IsNullOrWhiteSpace(command.Severity))
            return MonitoringError.ThresholdSeverityInvalid.ToString();

        return null;
    }
}
