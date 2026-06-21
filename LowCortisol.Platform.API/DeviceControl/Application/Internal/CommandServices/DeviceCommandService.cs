using LowCortisol.Platform.API.DeviceControl.Application.CommandServices;
using LowCortisol.Platform.API.DeviceControl.Application.Errors;
using LowCortisol.Platform.API.DeviceControl.Application.Internal.Parsing;
using LowCortisol.Platform.API.DeviceControl.Domain.Model.Aggregates;
using LowCortisol.Platform.API.DeviceControl.Domain.Model.Commands;
using LowCortisol.Platform.API.DeviceControl.Domain.Model.ValueObjects;
using LowCortisol.Platform.API.DeviceControl.Domain.Repositories;
using LowCortisol.Platform.API.Shared.Application.Results;
using LowCortisol.Platform.API.Shared.Domain.Repositories;

namespace LowCortisol.Platform.API.DeviceControl.Application.Internal.CommandServices;

public sealed class DeviceCommandService : IDeviceCommandService
{
    private readonly IDeviceRepository _deviceRepository;
    private readonly IValveRepository _valveRepository;
    private readonly IDeviceCommandRepository _deviceCommandRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeviceCommandService(
        IDeviceRepository deviceRepository,
        IValveRepository valveRepository,
        IDeviceCommandRepository deviceCommandRepository,
        IUnitOfWork unitOfWork)
    {
        _deviceRepository = deviceRepository;
        _valveRepository = valveRepository;
        _deviceCommandRepository = deviceCommandRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<DeviceCommand>> Handle(
        ExecuteDeviceCommandCommand command,
        CancellationToken cancellationToken = default)
    {
        if (command.DeviceId == Guid.Empty)
            return Result<DeviceCommand>.Failure(DeviceControlError.DeviceIdRequired.ToString());
        if (!DeviceControlEnumParser.TryParseCommandType(command.CommandType, out var commandType))
            return Result<DeviceCommand>.Failure(DeviceControlError.InvalidCommandType.ToString());
        if (!DeviceControlEnumParser.TryParseCommandSource(command.Source, out var source))
            return Result<DeviceCommand>.Failure(DeviceControlError.InvalidCommandSource.ToString());

        var device = await _deviceRepository.FindByIdAsync(command.DeviceId, cancellationToken);
        if (device is null) return Result<DeviceCommand>.Failure(DeviceControlError.DeviceNotFound.ToString());

        if (command.ValveId.HasValue)
        {
            var valve = await _valveRepository.FindByIdAsync(command.ValveId.Value, cancellationToken);
            if (valve is null) return Result<DeviceCommand>.Failure(DeviceControlError.ValveNotFound.ToString());
            if (valve.DeviceId != command.DeviceId)
                return Result<DeviceCommand>.Failure(DeviceControlError.ValveDoesNotBelongToDevice.ToString());
        }

        try
        {
            var target = new PhysicalTarget(
                command.SiteId,
                command.RoomId,
                command.DeviceGroupId,
                command.DeviceId,
                command.ValveId);

            var deviceCommand = new DeviceCommand(
                Guid.NewGuid(),
                target,
                command.IncidentId,
                commandType,
                source,
                command.Reason,
                command.RequestedBy);

            deviceCommand.MarkAsExecuted("Device command executed.");

            await _deviceCommandRepository.AddAsync(deviceCommand, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return Result<DeviceCommand>.Success(deviceCommand);
        }
        catch (InvalidOperationException exception)
        {
            return Result<DeviceCommand>.Failure($"{DeviceControlError.InvalidStateTransition}:{exception.Message}");
        }
        catch (ArgumentException exception)
        {
            return Result<DeviceCommand>.Failure($"{DeviceControlError.UnexpectedError}:{exception.Message}");
        }
    }

    public async Task<Result<DeviceCommand>> Handle(
        CancelDeviceCommandCommand command,
        CancellationToken cancellationToken = default)
    {
        var deviceCommand = await _deviceCommandRepository.FindByIdAsync(command.DeviceCommandId, cancellationToken);
        if (deviceCommand is null) return Result<DeviceCommand>.Failure(DeviceControlError.CommandNotFound.ToString());

        try
        {
            deviceCommand.Cancel(command.Reason);
            _deviceCommandRepository.Update(deviceCommand);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return Result<DeviceCommand>.Success(deviceCommand);
        }
        catch (InvalidOperationException exception)
        {
            return Result<DeviceCommand>.Failure($"{DeviceControlError.InvalidStateTransition}:{exception.Message}");
        }
        catch (ArgumentException exception)
        {
            return Result<DeviceCommand>.Failure($"{DeviceControlError.UnexpectedError}:{exception.Message}");
        }
    }
}
