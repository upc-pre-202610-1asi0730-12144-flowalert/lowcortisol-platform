using LowCortisol.Platform.API.Notification.Domain.Model.Aggregates;
using LowCortisol.Platform.API.Notification.Domain.Model.Queries;

namespace LowCortisol.Platform.API.Notification.Application.QueryServices;

public interface IIncidentQueryService
{
    Task<IReadOnlyCollection<Incident>> Handle(GetOpenIncidentsQuery query, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<Incident>> Handle(GetIncidentsByAlertIdQuery query, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<Incident>> Handle(GetIncidentsBySiteIdQuery query, CancellationToken cancellationToken = default);
    Task<Incident?> Handle(GetIncidentByIdQuery query, CancellationToken cancellationToken = default);
}
