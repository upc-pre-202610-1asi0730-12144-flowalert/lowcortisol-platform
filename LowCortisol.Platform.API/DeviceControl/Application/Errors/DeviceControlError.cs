namespace LowCortisol.Platform.API.DeviceControl.Application.Errors;

public enum DeviceControlError
{
    DeviceNotFound,
    ValveNotFound,
    DeviceIdRequired,
    ValveIdRequired,
    IncidentIdRequired,
    InvalidCommandType,
    InvalidCommandSource,
    InvalidValveOperationReason,
    ValveDoesNotBelongToDevice,
    ValveAlreadyClosed,
    CommandNotFound,
    InvalidStateTransition,
    UnexpectedError
}
