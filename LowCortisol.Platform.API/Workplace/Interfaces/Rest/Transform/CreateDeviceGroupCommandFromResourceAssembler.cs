using LowCortisol.Platform.API.Workplace.Domain.Model.Commands;
using LowCortisol.Platform.API.Workplace.Interfaces.Rest.Resources;

namespace LowCortisol.Platform.API.Workplace.Interfaces.Rest.Transform;

public static class CreateDeviceGroupCommandFromResourceAssembler
{
    public static CreateDeviceGroupCommand ToCommandFromResource(Guid roomId, CreateDeviceGroupResource resource) =>
        new(roomId, resource.Name, resource.ResourceType, resource.Status);
}
