using LowCortisol.Platform.API.Notification.Application.CommandServices;
using LowCortisol.Platform.API.Notification.Application.Errors;
using LowCortisol.Platform.API.Notification.Domain.Model.Aggregates;
using LowCortisol.Platform.API.Notification.Domain.Model.Commands;
using LowCortisol.Platform.API.Notification.Domain.Repositories;
using LowCortisol.Platform.API.Shared.Application.Results;
using LowCortisol.Platform.API.Shared.Domain.Repositories;

namespace LowCortisol.Platform.API.Notification.Application.Internal.CommandServices;

public sealed class IncidentCommandService : IIncidentCommandService
{
    private readonly IAlertRepository _alertRepository;
    private readonly IIncidentRepository _incidentRepository;
    private readonly IUnitOfWork _unitOfWork;

    public IncidentCommandService(
        IAlertRepository alertRepository,
        IIncidentRepository incidentRepository,
        IUnitOfWork unitOfWork)
    {
        _alertRepository = alertRepository;
        _incidentRepository = incidentRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Incident>> Handle(
        CreateIncidentFromAlertCommand command,
        CancellationToken cancellationToken = default)
    {
        if (command.AlertId == Guid.Empty)
            return Result<Incident>.Failure(NotificationError.AlertIdRequired.ToString());

        var alert = await _alertRepository.FindByIdAsync(command.AlertId, cancellationToken);
        if (alert is null) return Result<Incident>.Failure(NotificationError.AlertNotFound.ToString());

        try
        {
            var incident = Incident.FromAlert(alert);
            await _incidentRepository.AddAsync(incident, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return Result<Incident>.Success(incident);
        }
        catch (InvalidOperationException exception)
        {
            return Result<Incident>.Failure($"{NotificationError.InvalidStateTransition}:{exception.Message}");
        }
    }

    public async Task<Result<Incident>> Handle(AssignIncidentCommand command, CancellationToken cancellationToken = default)
    {
        var incident = await _incidentRepository.FindByIdAsync(command.IncidentId, cancellationToken);
        if (incident is null) return Result<Incident>.Failure(NotificationError.IncidentNotFound.ToString());

        try
        {
            incident.Assign(command.AssigneeId, command.AssigneeName);
            _incidentRepository.Update(incident);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return Result<Incident>.Success(incident);
        }
        catch (InvalidOperationException exception)
        {
            return Result<Incident>.Failure($"{NotificationError.InvalidStateTransition}:{exception.Message}");
        }
        catch (ArgumentException)
        {
            return Result<Incident>.Failure(NotificationError.UnexpectedError.ToString());
        }
    }

    public async Task<Result<Incident>> Handle(
        RegisterIncidentActionCommand command,
        CancellationToken cancellationToken = default)
    {
        var incident = await _incidentRepository.FindByIdAsync(command.IncidentId, cancellationToken);
        if (incident is null) return Result<Incident>.Failure(NotificationError.IncidentNotFound.ToString());

        try
        {
            incident.RegisterAction(command.ActionType, command.Description, command.PerformedBy);
            _incidentRepository.Update(incident);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return Result<Incident>.Success(incident);
        }
        catch (InvalidOperationException exception)
        {
            return Result<Incident>.Failure($"{NotificationError.InvalidStateTransition}:{exception.Message}");
        }
        catch (ArgumentException)
        {
            return Result<Incident>.Failure(NotificationError.UnexpectedError.ToString());
        }
    }

    public async Task<Result<Incident>> Handle(ResolveIncidentCommand command, CancellationToken cancellationToken = default)
    {
        var incident = await _incidentRepository.FindByIdAsync(command.IncidentId, cancellationToken);
        if (incident is null) return Result<Incident>.Failure(NotificationError.IncidentNotFound.ToString());

        try
        {
            incident.Resolve(command.ResolvedBy, command.Resolution);
            _incidentRepository.Update(incident);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return Result<Incident>.Success(incident);
        }
        catch (InvalidOperationException exception)
        {
            return Result<Incident>.Failure($"{NotificationError.InvalidStateTransition}:{exception.Message}");
        }
        catch (ArgumentException)
        {
            return Result<Incident>.Failure(NotificationError.UnexpectedError.ToString());
        }
    }

    public async Task<Result<Incident>> Handle(CloseIncidentCommand command, CancellationToken cancellationToken = default)
    {
        var incident = await _incidentRepository.FindByIdAsync(command.IncidentId, cancellationToken);
        if (incident is null) return Result<Incident>.Failure(NotificationError.IncidentNotFound.ToString());

        try
        {
            incident.Close(command.ClosedBy, command.ClosingNote);
            _incidentRepository.Update(incident);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return Result<Incident>.Success(incident);
        }
        catch (InvalidOperationException exception)
        {
            return Result<Incident>.Failure($"{NotificationError.InvalidStateTransition}:{exception.Message}");
        }
        catch (ArgumentException)
        {
            return Result<Incident>.Failure(NotificationError.UnexpectedError.ToString());
        }
    }
}
