namespace LowCortisol.Platform.API.DeviceControl.Domain.Model.Commands;

public record CancelDeviceCommandCommand(Guid DeviceCommandId, string Reason);
