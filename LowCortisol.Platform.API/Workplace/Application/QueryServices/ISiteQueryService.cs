using LowCortisol.Platform.API.Workplace.Domain.Model.Aggregates;
using LowCortisol.Platform.API.Workplace.Domain.Model.Entities;
using LowCortisol.Platform.API.Workplace.Domain.Model.Queries;

namespace LowCortisol.Platform.API.Workplace.Application.QueryServices;

public interface ISiteQueryService
{
    Task<IReadOnlyCollection<Site>> Handle(GetAllSitesQuery query, CancellationToken cancellationToken = default);
    Task<Site?> Handle(GetSiteByIdQuery query, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<Room>> Handle(GetRoomsBySiteIdQuery query, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<DeviceGroup>> Handle(GetDeviceGroupsByRoomIdQuery query, CancellationToken cancellationToken = default);
    Task<Site?> Handle(GetSitePhysicalModelByIdQuery query, CancellationToken cancellationToken = default);
}
