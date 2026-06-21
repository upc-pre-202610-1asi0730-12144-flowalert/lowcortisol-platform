using LowCortisol.Platform.API.Monitoring.Application.Results;
using LowCortisol.Platform.API.Monitoring.Domain.Model.Commands;
using LowCortisol.Platform.API.Shared.Application.Results;

namespace LowCortisol.Platform.API.Monitoring.Application.CommandServices;

public interface IReadingCommandService
{
    Task<Result<RegisterConsumptionReadingResult>> Handle(
        RegisterConsumptionReadingCommand command,
        CancellationToken cancellationToken = default);
}
