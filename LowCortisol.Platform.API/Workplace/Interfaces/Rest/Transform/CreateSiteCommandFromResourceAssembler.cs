using LowCortisol.Platform.API.Workplace.Domain.Model.Commands;
using LowCortisol.Platform.API.Workplace.Interfaces.Rest.Resources;

namespace LowCortisol.Platform.API.Workplace.Interfaces.Rest.Transform;

public static class CreateSiteCommandFromResourceAssembler
{
    public static CreateSiteCommand ToCommandFromResource(CreateSiteResource resource) =>
        new(resource.Name, resource.Address, resource.Type, resource.Status);
}
