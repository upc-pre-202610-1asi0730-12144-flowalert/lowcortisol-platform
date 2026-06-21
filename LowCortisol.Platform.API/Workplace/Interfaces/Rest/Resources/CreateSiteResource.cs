namespace LowCortisol.Platform.API.Workplace.Interfaces.Rest.Resources;

public record CreateSiteResource(string Name, string? Address, string? Type, string? Status);
