using LowCortisol.Platform.API.DeviceControl.Application.Acl;
using LowCortisol.Platform.API.Notification.Application.CommandServices;
using LowCortisol.Platform.API.Notification.Application.Errors;
using LowCortisol.Platform.API.Notification.Application.Results;
using LowCortisol.Platform.API.Notification.Domain.Model.Commands;
using LowCortisol.Platform.API.Notification.Domain.Model.ValueObjects;
using LowCortisol.Platform.API.Notification.Domain.Repositories;
using LowCortisol.Platform.API.Shared.Application.Results;
using LowCortisol.Platform.API.Shared.Domain.Repositories;

namespace LowCortisol.Platform.API.Notification.Application.Internal.CommandServices;

public sealed class IncidentMitigationCommandService : IIncidentMitigationCommandService
{
    private readonly IIncidentRepository _incidentRepository;
    private readonly IDeviceControlContextFacade _deviceControlContextFacade;
    private readonly IUnitOfWork _unitOfWork;

    public IncidentMitigationCommandService(
        IIncidentRepository incidentRepository,
        IDeviceControlContextFacade deviceControlContextFacade,
        IUnitOfWork unitOfWork)
    {
        _incidentRepository = incidentRepository;
        _deviceControlContextFacade = deviceControlContextFacade;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<IncidentMitigationResult>> Handle(
        RequestIncidentMitigationCommand command,
        CancellationToken cancellationToken = default)
    {
        if (command.IncidentId == Guid.Empty)
            return Result<IncidentMitigationResult>.Failure(NotificationError.IncidentIdRequired.ToString());

        var incident = await _incidentRepository.FindByIdAsync(command.IncidentId, cancellationToken);
        if (incident is null)
            return Result<IncidentMitigationResult>.Failure(NotificationError.IncidentNotFound.ToString());

        if (incident.Status is IncidentStatus.Resolved or IncidentStatus.Closed)
            return Result<IncidentMitigationResult>.Failure(
                $"{NotificationError.InvalidStateTransition}:Incident is not open for mitigation.");

        if (incident.Priority is not (IncidentPriority.Critical or IncidentPriority.High))
            return Result<IncidentMitigationResult>.Failure(
                $"{NotificationError.InvalidStateTransition}:Only high or critical incidents can request mitigation.");

        var deviceId = command.DeviceId == Guid.Empty ? incident.DeviceId : command.DeviceId;
        if (deviceId == Guid.Empty)
            return Result<IncidentMitigationResult>.Failure("DeviceIdRequired");
        if (command.ValveId == Guid.Empty)
            return Result<IncidentMitigationResult>.Failure("ValveIdRequired");

        var requestedBy = string.IsNullOrWhiteSpace(command.RequestedBy)
            ? "Operations"
            : command.RequestedBy.Trim();
        var reason = string.IsNullOrWhiteSpace(command.Reason)
            ? "Incident mitigation requested."
            : command.Reason.Trim();

        var mitigationResult = await _deviceControlContextFacade.CloseValveForIncidentAsync(
            incident.Id,
            incident.SiteId,
            incident.RoomId,
            incident.DeviceGroupId,
            deviceId,
            command.ValveId,
            requestedBy,
            reason,
            cancellationToken);

        if (mitigationResult.IsFailure || mitigationResult.Value is null)
            return Result<IncidentMitigationResult>.Failure(
                $"MitigationFailed:{mitigationResult.Error ?? NotificationError.UnexpectedError.ToString()}");

        var mitigation = mitigationResult.Value;
        incident.RegisterAction(
            "valve_closed",
            $"Valve {mitigation.ValveId} closed through command {mitigation.DeviceCommandId}. Operation {mitigation.ValveOperationId}.",
            requestedBy);

        _incidentRepository.Update(incident);
        await _unitOfWork.CompleteAsync(cancellationToken);

        return Result<IncidentMitigationResult>.Success(new IncidentMitigationResult(incident, mitigation));
    }
}
