using LowCortisol.Platform.API.Shared.Domain.Model;
using LowCortisol.Platform.API.Workplace.Domain.Model.Entities;
using LowCortisol.Platform.API.Workplace.Domain.Model.ValueObjects;

namespace LowCortisol.Platform.API.Workplace.Domain.Model.Aggregates;

public class Site : IEntity, IAuditableEntity
{
    private readonly List<Room> _rooms = [];

    private Site()
    {
        Name = string.Empty;
        Address = string.Empty;
    }

    public Site(Guid id, string name, string? address, SiteType type, OperationalStatus status)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Site name is required.", nameof(name));

        Id = id == Guid.Empty ? Guid.NewGuid() : id;
        Name = name.Trim();
        Address = address?.Trim() ?? string.Empty;
        Type = type;
        Status = status;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = CreatedAt;
    }

    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Address { get; private set; }
    public SiteType Type { get; private set; }
    public OperationalStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public IReadOnlyCollection<Room> Rooms => _rooms.AsReadOnly();

    public Room AddRoom(Guid id, string name, RoomType type, OperationalStatus status)
    {
        if (_rooms.Any(room => string.Equals(room.Name, name, StringComparison.OrdinalIgnoreCase)))
            throw new InvalidOperationException("Room name already exists in this site.");

        var room = new Room(id, Id, name, type, status);
        _rooms.Add(room);
        UpdatedAt = DateTime.UtcNow;

        return room;
    }

    public void AttachRoom(Room room)
    {
        if (_rooms.All(existingRoom => existingRoom.Id != room.Id)) _rooms.Add(room);
    }

    public void ChangeStatus(OperationalStatus status)
    {
        Status = status;
        UpdatedAt = DateTime.UtcNow;
    }
}
