using LowCortisol.Platform.API.Monitoring.Domain.Model.Commands;
using LowCortisol.Platform.API.Monitoring.Domain.Model.Entities;
using LowCortisol.Platform.API.Shared.Application.Results;

namespace LowCortisol.Platform.API.Monitoring.Application.CommandServices;

public interface IThresholdCommandService
{
    Task<Result<Threshold>> Handle(CreateThresholdCommand command, CancellationToken cancellationToken = default);
}
