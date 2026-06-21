namespace LowCortisol.Platform.API.Monitoring.Interfaces.Rest.Resources;

public record RegisterConsumptionReadingResultResource(
    ConsumptionReadingResource Reading,
    IReadOnlyCollection<AnomalyResource> Anomalies,
    int CriticalEvents);
