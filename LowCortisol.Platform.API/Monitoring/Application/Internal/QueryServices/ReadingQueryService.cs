using LowCortisol.Platform.API.Monitoring.Application.QueryServices;
using LowCortisol.Platform.API.Monitoring.Domain.Model.Entities;
using LowCortisol.Platform.API.Monitoring.Domain.Model.Queries;
using LowCortisol.Platform.API.Monitoring.Domain.Repositories;

namespace LowCortisol.Platform.API.Monitoring.Application.Internal.QueryServices;

public sealed class ReadingQueryService : IReadingQueryService
{
    private readonly IConsumptionReadingRepository _readingRepository;

    public ReadingQueryService(IConsumptionReadingRepository readingRepository)
    {
        _readingRepository = readingRepository;
    }

    public Task<IReadOnlyCollection<ConsumptionReading>> Handle(
        GetRecentReadingsQuery query,
        CancellationToken cancellationToken = default) =>
        _readingRepository.ListRecentAsync(query.Count <= 0 ? 20 : query.Count, cancellationToken);

    public Task<IReadOnlyCollection<ConsumptionReading>> Handle(
        GetReadingsBySiteIdQuery query,
        CancellationToken cancellationToken = default) =>
        _readingRepository.ListBySiteIdAsync(query.SiteId, cancellationToken);

    public Task<IReadOnlyCollection<ConsumptionReading>> Handle(
        GetReadingsByRoomIdQuery query,
        CancellationToken cancellationToken = default) =>
        _readingRepository.ListByRoomIdAsync(query.RoomId, cancellationToken);

    public Task<IReadOnlyCollection<ConsumptionReading>> Handle(
        GetReadingsByDeviceGroupIdQuery query,
        CancellationToken cancellationToken = default) =>
        _readingRepository.ListByDeviceGroupIdAsync(query.DeviceGroupId, cancellationToken);

    public Task<IReadOnlyCollection<ConsumptionReading>> Handle(
        GetReadingsBySensorIdQuery query,
        CancellationToken cancellationToken = default) =>
        _readingRepository.ListBySensorIdAsync(query.SensorId, cancellationToken);
}
