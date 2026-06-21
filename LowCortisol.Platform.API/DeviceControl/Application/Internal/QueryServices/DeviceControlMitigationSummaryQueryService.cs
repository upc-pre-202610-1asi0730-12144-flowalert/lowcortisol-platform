using LowCortisol.Platform.API.DeviceControl.Application.QueryServices;
using LowCortisol.Platform.API.DeviceControl.Domain.Model.Queries;
using LowCortisol.Platform.API.DeviceControl.Domain.Model.ReadModels;
using LowCortisol.Platform.API.DeviceControl.Domain.Model.ValueObjects;
using LowCortisol.Platform.API.DeviceControl.Domain.Repositories;

namespace LowCortisol.Platform.API.DeviceControl.Application.Internal.QueryServices;

public sealed class DeviceControlMitigationSummaryQueryService : IDeviceControlMitigationSummaryQueryService
{
    private readonly IDeviceCommandRepository _deviceCommandRepository;
    private readonly IValveOperationRepository _valveOperationRepository;

    public DeviceControlMitigationSummaryQueryService(
        IDeviceCommandRepository deviceCommandRepository,
        IValveOperationRepository valveOperationRepository)
    {
        _deviceCommandRepository = deviceCommandRepository;
        _valveOperationRepository = valveOperationRepository;
    }

    public async Task<DeviceControlMitigationSummary> Handle(
        GetDeviceControlMitigationSummaryQuery query,
        CancellationToken cancellationToken = default)
    {
        var commands = await _deviceCommandRepository.ListAsync(cancellationToken);
        var operations = await _valveOperationRepository.ListAsync(cancellationToken);
        var incidentOperations = operations
            .Where(operation => operation.Reason == ValveOperationReason.IncidentMitigation)
            .ToList();

        var lastMitigationAt = incidentOperations.Count == 0
            ? (DateTime?)null
            : incidentOperations
                .Select(operation => operation.CompletedAt ?? operation.RequestedAt)
                .OrderByDescending(date => date)
                .First();

        return new DeviceControlMitigationSummary(
            commands.Count,
            commands.Count(command => command.Status == DeviceCommandStatus.Executed),
            commands.Count(command => command.Status == DeviceCommandStatus.Failed),
            commands.Count(command => command.Status == DeviceCommandStatus.Pending),
            operations.Count,
            operations.Count(operation => operation.Status == DeviceCommandStatus.Executed),
            operations.Count(operation => operation.Status == DeviceCommandStatus.Failed),
            incidentOperations.Count,
            lastMitigationAt);
    }
}
