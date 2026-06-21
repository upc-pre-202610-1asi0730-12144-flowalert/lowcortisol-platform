using LowCortisol.Platform.API.Workplace.Domain.Model.Commands;
using LowCortisol.Platform.API.Workplace.Interfaces.Rest.Resources;

namespace LowCortisol.Platform.API.Workplace.Interfaces.Rest.Transform;

public static class CreateRoomCommandFromResourceAssembler
{
    public static CreateRoomCommand ToCommandFromResource(Guid siteId, CreateRoomResource resource) =>
        new(siteId, resource.Name, resource.Type, resource.Status);
}
