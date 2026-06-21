using LowCortisol.Platform.API.DeviceControl.Application.CommandServices;
using LowCortisol.Platform.API.DeviceControl.Application.Errors;
using LowCortisol.Platform.API.DeviceControl.Application.Results;
using LowCortisol.Platform.API.DeviceControl.Domain.Model.Aggregates;
using LowCortisol.Platform.API.DeviceControl.Domain.Model.Commands;
using LowCortisol.Platform.API.DeviceControl.Domain.Model.ValueObjects;
using LowCortisol.Platform.API.DeviceControl.Domain.Repositories;
using LowCortisol.Platform.API.Shared.Application.Results;
using LowCortisol.Platform.API.Shared.Domain.Repositories;

namespace LowCortisol.Platform.API.DeviceControl.Application.Internal.CommandServices;

public sealed class ValveOperationCommandService : IValveOperationCommandService
{
    private readonly IDeviceRepository _deviceRepository;
    private readonly IValveRepository _valveRepository;
    private readonly IDeviceCommandRepository _deviceCommandRepository;
    private readonly IValveOperationRepository _valveOperationRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ValveOperationCommandService(
        IDeviceRepository deviceRepository,
        IValveRepository valveRepository,
        IDeviceCommandRepository deviceCommandRepository,
        IValveOperationRepository valveOperationRepository,
        IUnitOfWork unitOfWork)
    {
        _deviceRepository = deviceRepository;
        _valveRepository = valveRepository;
        _deviceCommandRepository = deviceCommandRepository;
        _valveOperationRepository = valveOperationRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<DeviceControlMitigationResult>> Handle(
        CloseValveForIncidentCommand command,
        CancellationToken cancellationToken = default)
    {
        if (command.IncidentId == Guid.Empty)
            return Result<DeviceControlMitigationResult>.Failure(DeviceControlError.IncidentIdRequired.ToString());
        if (command.DeviceId == Guid.Empty)
            return Result<DeviceControlMitigationResult>.Failure(DeviceControlError.DeviceIdRequired.ToString());
        if (command.ValveId == Guid.Empty)
            return Result<DeviceControlMitigationResult>.Failure(DeviceControlError.ValveIdRequired.ToString());

        var device = await _deviceRepository.FindByIdAsync(command.DeviceId, cancellationToken);
        if (device is null) return Result<DeviceControlMitigationResult>.Failure(DeviceControlError.DeviceNotFound.ToString());

        var valve = await _valveRepository.FindByIdAsync(command.ValveId, cancellationToken);
        if (valve is null) return Result<DeviceControlMitigationResult>.Failure(DeviceControlError.ValveNotFound.ToString());
        if (valve.DeviceId != command.DeviceId)
            return Result<DeviceControlMitigationResult>.Failure(DeviceControlError.ValveDoesNotBelongToDevice.ToString());
        if (!valve.CanBeClosed())
            return Result<DeviceControlMitigationResult>.Failure(DeviceControlError.ValveAlreadyClosed.ToString());

        try
        {
            var target = new PhysicalTarget(
                command.SiteId,
                command.RoomId,
                command.DeviceGroupId,
                command.DeviceId,
                command.ValveId);

            var reason = string.IsNullOrWhiteSpace(command.Reason)
                ? "Incident mitigation requested."
                : command.Reason.Trim();

            var deviceCommand = new DeviceCommand(
                Guid.NewGuid(),
                target,
                command.IncidentId,
                DeviceCommandType.CloseValve,
                CommandSource.Incident,
                reason,
                command.RequestedBy);

            var valveOperation = new ValveOperation(
                Guid.NewGuid(),
                target,
                command.IncidentId,
                valve.ResourceType,
                valve.Status,
                ValveStatus.Closed,
                ValveOperationReason.IncidentMitigation,
                CommandSource.Incident,
                deviceCommand.RequestedAt);

            valve.Close();
            deviceCommand.MarkAsExecuted("Valve closed for incident mitigation.");
            valveOperation.Complete();

            await _deviceCommandRepository.AddAsync(deviceCommand, cancellationToken);
            await _valveOperationRepository.AddAsync(valveOperation, cancellationToken);
            _valveRepository.Update(valve);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return Result<DeviceControlMitigationResult>.Success(
                new DeviceControlMitigationResult(
                    command.IncidentId,
                    command.DeviceId,
                    command.ValveId,
                    deviceCommand.Id,
                    valveOperation.Id,
                    deviceCommand.Status.ToString(),
                    valve.Status.ToString(),
                    deviceCommand.ExecutedAt ?? DateTime.UtcNow));
        }
        catch (InvalidOperationException exception)
        {
            return Result<DeviceControlMitigationResult>.Failure(
                $"{DeviceControlError.InvalidStateTransition}:{exception.Message}");
        }
        catch (ArgumentException exception)
        {
            return Result<DeviceControlMitigationResult>.Failure(
                $"{DeviceControlError.UnexpectedError}:{exception.Message}");
        }
    }
}
