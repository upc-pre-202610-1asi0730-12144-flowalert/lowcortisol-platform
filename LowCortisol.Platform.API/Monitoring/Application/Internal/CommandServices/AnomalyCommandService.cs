using LowCortisol.Platform.API.Monitoring.Application.CommandServices;
using LowCortisol.Platform.API.Monitoring.Application.Errors;
using LowCortisol.Platform.API.Monitoring.Domain.Model.Commands;
using LowCortisol.Platform.API.Monitoring.Domain.Model.Entities;
using LowCortisol.Platform.API.Monitoring.Domain.Model.ValueObjects;
using LowCortisol.Platform.API.Monitoring.Domain.Repositories;
using LowCortisol.Platform.API.Shared.Application.Results;
using LowCortisol.Platform.API.Shared.Domain.Repositories;

namespace LowCortisol.Platform.API.Monitoring.Application.Internal.CommandServices;

public sealed class AnomalyCommandService : IAnomalyCommandService
{
    private readonly IAnomalyRepository _anomalyRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AnomalyCommandService(IAnomalyRepository anomalyRepository, IUnitOfWork unitOfWork)
    {
        _anomalyRepository = anomalyRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Anomaly>> Handle(
        ResolveAnomalyCommand command,
        CancellationToken cancellationToken = default)
    {
        var anomaly = await _anomalyRepository.FindByIdAsync(command.AnomalyId, cancellationToken);
        if (anomaly is null) return Result<Anomaly>.Failure(MonitoringError.AnomalyNotFound.ToString());
        if (anomaly.Status == AnomalyStatus.Resolved)
            return Result<Anomaly>.Failure(MonitoringError.AnomalyAlreadyResolved.ToString());

        anomaly.Resolve();
        _anomalyRepository.Update(anomaly);
        await _unitOfWork.CompleteAsync(cancellationToken);

        return Result<Anomaly>.Success(anomaly);
    }
}
