using LowCortisol.Platform.API.Shared.Application.Results;
using LowCortisol.Platform.API.Workplace.Domain.Model.Aggregates;
using LowCortisol.Platform.API.Workplace.Domain.Model.Commands;
using LowCortisol.Platform.API.Workplace.Domain.Model.Entities;

namespace LowCortisol.Platform.API.Workplace.Application.CommandServices;

public interface ISiteCommandService
{
    Task<Result<Site>> Handle(CreateSiteCommand command, CancellationToken cancellationToken = default);
    Task<Result<Room>> Handle(CreateRoomCommand command, CancellationToken cancellationToken = default);
    Task<Result<DeviceGroup>> Handle(CreateDeviceGroupCommand command, CancellationToken cancellationToken = default);
}
