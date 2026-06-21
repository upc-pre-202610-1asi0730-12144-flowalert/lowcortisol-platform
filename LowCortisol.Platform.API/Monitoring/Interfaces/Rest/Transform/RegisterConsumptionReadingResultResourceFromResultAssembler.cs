using LowCortisol.Platform.API.Monitoring.Application.Results;
using LowCortisol.Platform.API.Monitoring.Interfaces.Rest.Resources;

namespace LowCortisol.Platform.API.Monitoring.Interfaces.Rest.Transform;

public static class RegisterConsumptionReadingResultResourceFromResultAssembler
{
    public static RegisterConsumptionReadingResultResource ToResourceFromResult(
        RegisterConsumptionReadingResult result) =>
        new(
            ConsumptionReadingResourceFromEntityAssembler.ToResourceFromEntity(result.Reading),
            result.Anomalies.Select(AnomalyResourceFromEntityAssembler.ToResourceFromEntity).ToList(),
            result.CriticalEvents.Count);
}
