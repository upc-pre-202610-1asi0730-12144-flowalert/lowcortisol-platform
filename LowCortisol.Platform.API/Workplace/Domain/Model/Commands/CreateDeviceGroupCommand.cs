namespace LowCortisol.Platform.API.Workplace.Domain.Model.Commands;

public record CreateDeviceGroupCommand(Guid RoomId, string Name, string? ResourceType, string? Status);
