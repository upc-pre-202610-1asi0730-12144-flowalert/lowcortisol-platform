using LowCortisol.Platform.API.DeviceControl.Domain.Model.Aggregates;
using LowCortisol.Platform.API.DeviceControl.Domain.Repositories;
using LowCortisol.Platform.API.Shared.Infrastructure.Persistence.InMemory;

namespace LowCortisol.Platform.API.DeviceControl.Infrastructure.Persistence.InMemory.Repositories;

public sealed class DeviceCommandRepository : BaseRepository<DeviceCommand>, IDeviceCommandRepository
{
    private readonly AppDbContext _context;

    public DeviceCommandRepository(AppDbContext context) : base(context.DeviceCommands)
    {
        _context = context;
    }

    public Task<IReadOnlyCollection<DeviceCommand>> ListRecentAsync(
        int count,
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult<IReadOnlyCollection<DeviceCommand>>(
            _context.DeviceCommands
                .OrderByDescending(command => command.RequestedAt)
                .Take(count)
                .ToList());
    }

    public Task<IReadOnlyCollection<DeviceCommand>> ListByDeviceIdAsync(
        Guid deviceId,
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult<IReadOnlyCollection<DeviceCommand>>(
            _context.DeviceCommands
                .Where(command => command.DeviceId == deviceId)
                .OrderByDescending(command => command.RequestedAt)
                .ToList());
    }

    public Task<IReadOnlyCollection<DeviceCommand>> ListByIncidentIdAsync(
        Guid incidentId,
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult<IReadOnlyCollection<DeviceCommand>>(
            _context.DeviceCommands
                .Where(command => command.IncidentId == incidentId)
                .OrderByDescending(command => command.RequestedAt)
                .ToList());
    }
}
