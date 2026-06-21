using LowCortisol.Platform.API.DeviceControl.Domain.Model.Aggregates;
using LowCortisol.Platform.API.DeviceControl.Domain.Repositories;
using LowCortisol.Platform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using LowCortisol.Platform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LowCortisol.Platform.API.DeviceControl.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public sealed class DeviceCommandRepository : BaseRepository<DeviceCommand>, IDeviceCommandRepository
{
    public DeviceCommandRepository(AppDbContext context) : base(context)
    {
    }

    public new Task<DeviceCommand?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        Context.DeviceCommands
            .Include(command => command.Executions)
            .Include(command => command.AuditEntries)
            .FirstOrDefaultAsync(command => command.Id == id, cancellationToken);

    public async Task<IReadOnlyCollection<DeviceCommand>> ListRecentAsync(
        int count,
        CancellationToken cancellationToken = default)
    {
        return await Context.DeviceCommands
            .Include(command => command.Executions)
            .Include(command => command.AuditEntries)
            .OrderByDescending(command => command.RequestedAt)
            .Take(count)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<DeviceCommand>> ListByDeviceIdAsync(
        Guid deviceId,
        CancellationToken cancellationToken = default)
    {
        return await Context.DeviceCommands
            .Include(command => command.Executions)
            .Include(command => command.AuditEntries)
            .Where(command => command.DeviceId == deviceId)
            .OrderByDescending(command => command.RequestedAt)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<DeviceCommand>> ListByIncidentIdAsync(
        Guid incidentId,
        CancellationToken cancellationToken = default)
    {
        return await Context.DeviceCommands
            .Include(command => command.Executions)
            .Include(command => command.AuditEntries)
            .Where(command => command.IncidentId == incidentId)
            .OrderByDescending(command => command.RequestedAt)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
}
