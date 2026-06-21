using LowCortisol.Platform.API.Monitoring.Domain.Model.Entities;
using LowCortisol.Platform.API.Monitoring.Domain.Model.Queries;

namespace LowCortisol.Platform.API.Monitoring.Application.QueryServices;

public interface IReadingQueryService
{
    Task<IReadOnlyCollection<ConsumptionReading>> Handle(
        GetRecentReadingsQuery query,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<ConsumptionReading>> Handle(
        GetReadingsBySiteIdQuery query,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<ConsumptionReading>> Handle(
        GetReadingsByRoomIdQuery query,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<ConsumptionReading>> Handle(
        GetReadingsByDeviceGroupIdQuery query,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<ConsumptionReading>> Handle(
        GetReadingsBySensorIdQuery query,
        CancellationToken cancellationToken = default);
}
