using LowCortisol.Platform.API.Shared.Application.Results;
using LowCortisol.Platform.API.Shared.Domain.Repositories;
using LowCortisol.Platform.API.Workplace.Application.CommandServices;
using LowCortisol.Platform.API.Workplace.Application.Errors;
using LowCortisol.Platform.API.Workplace.Domain.Model.Aggregates;
using LowCortisol.Platform.API.Workplace.Domain.Model.Commands;
using LowCortisol.Platform.API.Workplace.Domain.Model.Entities;
using LowCortisol.Platform.API.Workplace.Domain.Model.ValueObjects;
using LowCortisol.Platform.API.Workplace.Domain.Repositories;

namespace LowCortisol.Platform.API.Workplace.Application.Internal.CommandServices;

public sealed class SiteCommandService : ISiteCommandService
{
    private readonly ISiteRepository _siteRepository;
    private readonly IRoomRepository _roomRepository;
    private readonly IDeviceGroupRepository _deviceGroupRepository;
    private readonly IUnitOfWork _unitOfWork;

    public SiteCommandService(
        ISiteRepository siteRepository,
        IRoomRepository roomRepository,
        IDeviceGroupRepository deviceGroupRepository,
        IUnitOfWork unitOfWork)
    {
        _siteRepository = siteRepository;
        _roomRepository = roomRepository;
        _deviceGroupRepository = deviceGroupRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Site>> Handle(CreateSiteCommand command, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(command.Name))
            return Result<Site>.Failure(WorkplaceError.SiteNameRequired.ToString());
        if (await _siteRepository.ExistsByNameAsync(command.Name, cancellationToken))
            return Result<Site>.Failure(WorkplaceError.SiteNameDuplicated.ToString());

        var site = new Site(
            Guid.NewGuid(),
            command.Name,
            command.Address,
            ParseEnum(command.Type, SiteType.Residential),
            ParseEnum(command.Status, OperationalStatus.Active));

        await _siteRepository.AddAsync(site, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);

        return Result<Site>.Success(site);
    }

    public async Task<Result<Room>> Handle(CreateRoomCommand command, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(command.Name))
            return Result<Room>.Failure(WorkplaceError.RoomNameRequired.ToString());

        var site = await _siteRepository.FindByIdAsync(command.SiteId, cancellationToken);
        if (site is null) return Result<Room>.Failure(WorkplaceError.SiteNotFound.ToString());

        if (await _roomRepository.ExistsByNameInSiteAsync(command.SiteId, command.Name, cancellationToken))
            return Result<Room>.Failure(WorkplaceError.RoomNameDuplicated.ToString());

        var room = site.AddRoom(
            Guid.NewGuid(),
            command.Name,
            ParseEnum(command.Type, RoomType.Custom),
            ParseEnum(command.Status, OperationalStatus.Active));

        await _roomRepository.AddAsync(room, cancellationToken);
        _siteRepository.Update(site);
        await _unitOfWork.CompleteAsync(cancellationToken);

        return Result<Room>.Success(room);
    }

    public async Task<Result<DeviceGroup>> Handle(
        CreateDeviceGroupCommand command,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(command.Name))
            return Result<DeviceGroup>.Failure(WorkplaceError.DeviceGroupNameRequired.ToString());

        var room = await _roomRepository.FindByIdAsync(command.RoomId, cancellationToken);
        if (room is null) return Result<DeviceGroup>.Failure(WorkplaceError.RoomNotFound.ToString());

        if (await _deviceGroupRepository.ExistsByNameInRoomAsync(command.RoomId, command.Name, cancellationToken))
            return Result<DeviceGroup>.Failure(WorkplaceError.DeviceGroupNameDuplicated.ToString());

        var deviceGroup = room.AddDeviceGroup(
            Guid.NewGuid(),
            command.Name,
            ParseEnum(command.ResourceType, ResourceType.Mixed),
            ParseEnum(command.Status, OperationalStatus.Active));

        await _deviceGroupRepository.AddAsync(deviceGroup, cancellationToken);
        _roomRepository.Update(room);
        await _unitOfWork.CompleteAsync(cancellationToken);

        return Result<DeviceGroup>.Success(deviceGroup);
    }

    private static TEnum ParseEnum<TEnum>(string? value, TEnum fallback) where TEnum : struct
    {
        if (string.IsNullOrWhiteSpace(value)) return fallback;

        var normalized = value.Replace("_", string.Empty, StringComparison.OrdinalIgnoreCase);
        return Enum.TryParse<TEnum>(normalized, true, out var result) ? result : fallback;
    }
}
