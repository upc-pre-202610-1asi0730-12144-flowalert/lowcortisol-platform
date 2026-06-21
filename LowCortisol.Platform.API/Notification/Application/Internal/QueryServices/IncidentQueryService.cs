using LowCortisol.Platform.API.Notification.Application.QueryServices;
using LowCortisol.Platform.API.Notification.Domain.Model.Aggregates;
using LowCortisol.Platform.API.Notification.Domain.Model.Queries;
using LowCortisol.Platform.API.Notification.Domain.Repositories;

namespace LowCortisol.Platform.API.Notification.Application.Internal.QueryServices;

public sealed class IncidentQueryService : IIncidentQueryService
{
    private readonly IIncidentRepository _incidentRepository;

    public IncidentQueryService(IIncidentRepository incidentRepository)
    {
        _incidentRepository = incidentRepository;
    }

    public Task<IReadOnlyCollection<Incident>> Handle(GetOpenIncidentsQuery query, CancellationToken cancellationToken = default) =>
        _incidentRepository.ListOpenAsync(cancellationToken);

    public Task<IReadOnlyCollection<Incident>> Handle(GetIncidentsByAlertIdQuery query, CancellationToken cancellationToken = default) =>
        _incidentRepository.ListByAlertIdAsync(query.AlertId, cancellationToken);

    public Task<IReadOnlyCollection<Incident>> Handle(GetIncidentsBySiteIdQuery query, CancellationToken cancellationToken = default) =>
        _incidentRepository.ListBySiteIdAsync(query.SiteId, cancellationToken);

    public Task<Incident?> Handle(GetIncidentByIdQuery query, CancellationToken cancellationToken = default) =>
        _incidentRepository.FindByIdAsync(query.IncidentId, cancellationToken);
}
