using LowCortisol.Platform.API.Monitoring.Application.QueryServices;
using LowCortisol.Platform.API.Monitoring.Domain.Model.Queries;
using LowCortisol.Platform.API.Monitoring.Domain.Model.ReadModels;
using LowCortisol.Platform.API.Monitoring.Domain.Model.ValueObjects;
using LowCortisol.Platform.API.Monitoring.Domain.Repositories;

namespace LowCortisol.Platform.API.Monitoring.Application.Internal.QueryServices;

public sealed class MonitoringSummaryQueryService : IMonitoringSummaryQueryService
{
    private readonly IConsumptionReadingRepository _readingRepository;
    private readonly IThresholdRepository _thresholdRepository;
    private readonly IAnomalyRepository _anomalyRepository;

    public MonitoringSummaryQueryService(
        IConsumptionReadingRepository readingRepository,
        IThresholdRepository thresholdRepository,
        IAnomalyRepository anomalyRepository)
    {
        _readingRepository = readingRepository;
        _thresholdRepository = thresholdRepository;
        _anomalyRepository = anomalyRepository;
    }

    public async Task<MonitoringSummary> Handle(
        GetMonitoringSummaryQuery query,
        CancellationToken cancellationToken = default)
    {
        var totalReadings = await _readingRepository.CountAsync(cancellationToken);
        var waterTotal = await _readingRepository.SumByResourceAsync(ResourceType.Water, cancellationToken);
        var gasTotal = await _readingRepository.SumByResourceAsync(ResourceType.Gas, cancellationToken);
        var openAnomalies = await _anomalyRepository.CountOpenAsync(cancellationToken);
        var criticalAnomalies = await _anomalyRepository.CountCriticalOpenAsync(cancellationToken);
        var activeThresholds = await _thresholdRepository.CountActiveAsync(cancellationToken);
        var monitoredSensors = await _readingRepository.CountDistinctSensorsAsync(cancellationToken);
        var latestReadingDate = await _readingRepository.GetLatestCapturedAtAsync(cancellationToken);

        return new MonitoringSummary(
            totalReadings,
            waterTotal,
            gasTotal,
            openAnomalies,
            criticalAnomalies,
            activeThresholds,
            monitoredSensors,
            latestReadingDate);
    }
}
