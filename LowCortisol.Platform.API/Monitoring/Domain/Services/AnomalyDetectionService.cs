using LowCortisol.Platform.API.Monitoring.Domain.Model.Entities;
using LowCortisol.Platform.API.Monitoring.Domain.Model.ValueObjects;

namespace LowCortisol.Platform.API.Monitoring.Domain.Services;

public sealed class AnomalyDetectionService
{
    public IReadOnlyCollection<AnomalyDetectionResult> Detect(
        ConsumptionReading reading,
        IReadOnlyCollection<Threshold> thresholds)
    {
        return thresholds
            .Where(threshold => threshold.AppliesTo(reading))
            .Where(threshold => IsExceeded(reading.Value, threshold.Operator, threshold.LimitValue))
            .Select(threshold => new AnomalyDetectionResult(threshold))
            .ToList();
    }

    private static bool IsExceeded(decimal value, ThresholdOperator thresholdOperator, decimal limitValue) =>
        thresholdOperator switch
        {
            ThresholdOperator.GreaterThan => value > limitValue,
            ThresholdOperator.GreaterOrEqual => value >= limitValue,
            ThresholdOperator.LessThan => value < limitValue,
            ThresholdOperator.LessOrEqual => value <= limitValue,
            _ => false
        };
}
