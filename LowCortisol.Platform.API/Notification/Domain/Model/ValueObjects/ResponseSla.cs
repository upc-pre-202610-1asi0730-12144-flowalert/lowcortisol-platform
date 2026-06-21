namespace LowCortisol.Platform.API.Notification.Domain.Model.ValueObjects;

public sealed record ResponseSla
{
    private ResponseSla()
    {
    }

    public ResponseSla(int minutesToAcknowledge, int minutesToResolve)
    {
        if (minutesToAcknowledge <= 0)
            throw new ArgumentException("Minutes to acknowledge must be greater than zero.", nameof(minutesToAcknowledge));
        if (minutesToResolve <= 0)
            throw new ArgumentException("Minutes to resolve must be greater than zero.", nameof(minutesToResolve));
        if (minutesToResolve < minutesToAcknowledge)
            throw new ArgumentException("Resolution SLA cannot be shorter than acknowledgement SLA.");

        MinutesToAcknowledge = minutesToAcknowledge;
        MinutesToResolve = minutesToResolve;
    }

    public int MinutesToAcknowledge { get; init; }
    public int MinutesToResolve { get; init; }

    public DateTime AcknowledgementDueAt(DateTime openedAt) => openedAt.AddMinutes(MinutesToAcknowledge);

    public DateTime ResolutionDueAt(DateTime openedAt) => openedAt.AddMinutes(MinutesToResolve);

    public static ResponseSla ForSeverity(AlertSeverity severity) =>
        severity switch
        {
            AlertSeverity.Critical => new ResponseSla(15, 120),
            AlertSeverity.Warning => new ResponseSla(60, 480),
            _ => new ResponseSla(240, 1440)
        };
}
