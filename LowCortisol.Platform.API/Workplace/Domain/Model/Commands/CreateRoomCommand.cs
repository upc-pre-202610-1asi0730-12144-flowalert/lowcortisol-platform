namespace LowCortisol.Platform.API.Workplace.Domain.Model.Commands;

public record CreateRoomCommand(Guid SiteId, string Name, string? Type, string? Status);
