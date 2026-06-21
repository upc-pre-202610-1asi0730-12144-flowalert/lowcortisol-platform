using LowCortisol.Platform.API.Monitoring.Domain.Model.Entities;
using LowCortisol.Platform.API.Monitoring.Domain.Model.Events;

namespace LowCortisol.Platform.API.Monitoring.Application.Results;

public record RegisterConsumptionReadingResult(
    ConsumptionReading Reading,
    IReadOnlyCollection<Anomaly> Anomalies,
    IReadOnlyCollection<CriticalAnomalyDetected> CriticalEvents);
