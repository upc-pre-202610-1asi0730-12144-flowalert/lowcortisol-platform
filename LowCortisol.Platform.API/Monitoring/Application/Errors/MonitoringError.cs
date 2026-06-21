namespace LowCortisol.Platform.API.Monitoring.Application.Errors;

public enum MonitoringError
{
    SensorIdRequired,
    SiteIdRequired,
    RoomIdRequired,
    DeviceGroupIdRequired,
    DeviceIdRequired,
    ResourceTypeRequired,
    ResourceTypeInvalid,
    ReadingValueNegative,
    UnitRequired,
    ThresholdScopeRequired,
    ThresholdNameRequired,
    ThresholdOperatorInvalid,
    ThresholdLimitValueInvalid,
    ThresholdSeverityInvalid,
    AnomalyNotFound,
    AnomalyAlreadyResolved,
    SensorNotFoundInDeviceGroup,
    NotificationBridgeFailed,
    UnexpectedError
}
