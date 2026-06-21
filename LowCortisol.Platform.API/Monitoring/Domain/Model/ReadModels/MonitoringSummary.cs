namespace LowCortisol.Platform.API.Monitoring.Domain.Model.ReadModels;

public record MonitoringSummary(
    int TotalReadings,
    decimal WaterConsumptionTotal,
    decimal GasConsumptionTotal,
    int OpenAnomalies,
    int CriticalAnomalies,
    int ActiveThresholds,
    int MonitoredSensors,
    DateTime? LatestReadingDate);
