namespace LowCortisol.Platform.API.Monitoring.Interfaces.Rest.Resources;

public record MonitoringSummaryResource(
    int TotalReadings,
    decimal WaterConsumptionTotal,
    decimal GasConsumptionTotal,
    int OpenAnomalies,
    int CriticalAnomalies,
    int ActiveThresholds,
    int MonitoredSensors,
    DateTime? LatestReadingDate);
