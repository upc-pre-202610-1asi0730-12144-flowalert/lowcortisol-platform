namespace LowCortisol.Platform.API.Workplace.Domain.Model.Commands;

public record CreateSiteCommand(string Name, string? Address, string? Type, string? Status);
