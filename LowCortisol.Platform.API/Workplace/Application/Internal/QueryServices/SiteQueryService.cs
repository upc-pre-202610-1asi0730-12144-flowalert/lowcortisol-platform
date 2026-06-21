using LowCortisol.Platform.API.Workplace.Application.QueryServices;
using LowCortisol.Platform.API.Workplace.Domain.Model.Aggregates;
using LowCortisol.Platform.API.Workplace.Domain.Model.Entities;
using LowCortisol.Platform.API.Workplace.Domain.Model.Queries;
using LowCortisol.Platform.API.Workplace.Domain.Repositories;

namespace LowCortisol.Platform.API.Workplace.Application.Internal.QueryServices;

public sealed class SiteQueryService : ISiteQueryService
{
    private readonly ISiteRepository _siteRepository;
    private readonly IRoomRepository _roomRepository;
    private readonly IDeviceGroupRepository _deviceGroupRepository;

    public SiteQueryService(
        ISiteRepository siteRepository,
        IRoomRepository roomRepository,
        IDeviceGroupRepository deviceGroupRepository)
    {
        _siteRepository = siteRepository;
        _roomRepository = roomRepository;
        _deviceGroupRepository = deviceGroupRepository;
    }

    public Task<IReadOnlyCollection<Site>> Handle(
        GetAllSitesQuery query,
        CancellationToken cancellationToken = default) =>
        _siteRepository.ListAsync(cancellationToken);

    public Task<Site?> Handle(GetSiteByIdQuery query, CancellationToken cancellationToken = default) =>
        _siteRepository.FindByIdAsync(query.SiteId, cancellationToken);

    public Task<IReadOnlyCollection<Room>> Handle(
        GetRoomsBySiteIdQuery query,
        CancellationToken cancellationToken = default) =>
        _roomRepository.FindBySiteIdAsync(query.SiteId, cancellationToken);

    public Task<IReadOnlyCollection<DeviceGroup>> Handle(
        GetDeviceGroupsByRoomIdQuery query,
        CancellationToken cancellationToken = default) =>
        _deviceGroupRepository.FindByRoomIdAsync(query.RoomId, cancellationToken);

    public Task<Site?> Handle(
        GetSitePhysicalModelByIdQuery query,
        CancellationToken cancellationToken = default) =>
        _siteRepository.GetPhysicalModelByIdAsync(query.SiteId, cancellationToken);
}
