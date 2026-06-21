using LowCortisol.Platform.API.Monitoring.Domain.Model.ReadModels;
using LowCortisol.Platform.API.Monitoring.Interfaces.Rest.Resources;

namespace LowCortisol.Platform.API.Monitoring.Interfaces.Rest.Transform;

public static class MonitoringSummaryResourceFromResultAssembler
{
    public static MonitoringSummaryResource ToResourceFromResult(MonitoringSummary summary) =>
        new(
            summary.TotalReadings,
            summary.WaterConsumptionTotal,
            summary.GasConsumptionTotal,
            summary.OpenAnomalies,
            summary.CriticalAnomalies,
            summary.ActiveThresholds,
            summary.MonitoredSensors,
            summary.LatestReadingDate);
}
