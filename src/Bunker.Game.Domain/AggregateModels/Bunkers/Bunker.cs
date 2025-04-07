using Bunker.Game.Domain.AggregateModels.Bunkers.Events;

namespace Bunker.Game.Domain.AggregateModels.Bunkers;

public class Bunker : Entity<Guid>, IAggregateRoot
{
    public const int MAX_ROOMS_COUNT = 3;
    public const int MIN_ROOMS_COUNT = 1;

    public const int MAX_BUNKER_ENVIROMENT_COUNT = 3;
    public const int MIN_BUNKER_ENVIROMENT_COUNT = 1;

    public const int MAX_BUNKER_ITEM_COUNT = 4;
    public const int MIN_BUNKER_ITEM_COUNT = 1;

    private List<Item> _items;
    private List<Environment> _environments;
    private List<Room> _rooms;

    public Guid GameSessionId { get; }

    public string Description { get; private set; }

    public IReadOnlyCollection<Room> Rooms => _rooms;

    public IReadOnlyCollection<Item> Items => _items;

    public IReadOnlyCollection<Environment> Environments => _environments;

    public bool IsReadonly { get; private set; }

    public Bunker(
        Guid id,
        Guid gameSessionId,
        string description,
        IEnumerable<Item> items,
        IEnumerable<Environment> environments,
        IEnumerable<Room> rooms
    )
        : base(id)
    {
        GameSessionId = gameSessionId;
        Description = description;
        _rooms = new List<Room>(rooms);
        _environments = new List<Environment>(environments);
        _items = new List<Item>(items);
        IsReadonly = false;
    }

    public void RecreateBunker(
        string description,
        IEnumerable<Item> items,
        IEnumerable<Environment> environments,
        IEnumerable<Room> rooms
    )
    {
        CheckCanUpdateBunkerComponent();

        Description = description;
        _rooms = new List<Room>(rooms);
        _environments = new List<Environment>(environments);
        _items = new List<Item>(items);

        AddDomainEvent(new BunkerRecreatedDomainEvent(Id, GameSessionId, Description, items, environments, rooms));
    }

    public void MarkReadonly()
    {
        IsReadonly = true;
    }

    public void AddItem(Item item)
    {
        CheckCanUpdateBunkerComponent();

        ArgumentNullException.ThrowIfNull(item);

        _items.Add(item);

        AddDomainEvent(new BunkerComponentsUpdatedDomainEvent(Id, GameSessionId, _items));
    }

    public void RemoveItem(Item item)
    {
        ArgumentNullException.ThrowIfNull(item);

        _items.Remove(item);

        AddDomainEvent(new BunkerComponentsUpdatedDomainEvent(Id, GameSessionId, _items));
    }

    public void ReplaceItem(Item oldItem, Item newItem)
    {
        CheckCanUpdateBunkerComponent();

        ArgumentNullException.ThrowIfNull(oldItem);
        ArgumentNullException.ThrowIfNull(newItem);

        var index = _items.IndexOf(oldItem);
        if (index == -1)
            throw new InvalidGameOperationException("Item to replace not found");

        _items[index] = newItem;

        AddDomainEvent(new BunkerComponentsUpdatedDomainEvent(Id, GameSessionId, _items));
    }

    public void RevealRandomItem()
    {
        CheckCanUpdateBunkerComponent();

        var hiddenItem =
            _items.Where(x => !x.IsHidden).OrderBy(x => x.Description).FirstOrDefault()
            ?? throw new InvalidGameOperationException("All items are revealed");

        _items.Remove(hiddenItem);

        hiddenItem = hiddenItem.Reveal();

        _items.Add(hiddenItem);

        AddDomainEvent(new BunkerComponentRevealedDomainEvent(Id, GameSessionId, hiddenItem));
    }

    public void AddRoom(Room room)
    {
        CheckCanUpdateBunkerComponent();

        ArgumentNullException.ThrowIfNull(room);

        _rooms.Add(room);

        AddDomainEvent(new BunkerComponentsUpdatedDomainEvent(Id, GameSessionId, _rooms));
    }

    public void RemoveRoom(Room room)
    {
        CheckCanUpdateBunkerComponent();

        ArgumentNullException.ThrowIfNull(room);

        _rooms.Remove(room);

        AddDomainEvent(new BunkerComponentsUpdatedDomainEvent(Id, GameSessionId, _rooms));
    }

    public void RevealRandomRoom()
    {
        CheckCanUpdateBunkerComponent();

        var hiddenRoom =
            _rooms.Where(x => !x.IsHidden).OrderBy(x => x.Description).FirstOrDefault()
            ?? throw new InvalidGameOperationException("All rooms are revealed");

        _rooms.Remove(hiddenRoom);

        hiddenRoom = hiddenRoom.Reveal();

        _rooms.Add(hiddenRoom);

        AddDomainEvent(new BunkerComponentRevealedDomainEvent(Id, GameSessionId, hiddenRoom));
    }

    public void ReplaceRoom(Room oldRoom, Room newRoom)
    {
        CheckCanUpdateBunkerComponent();

        ArgumentNullException.ThrowIfNull(oldRoom);
        ArgumentNullException.ThrowIfNull(newRoom);

        var index = _rooms.IndexOf(oldRoom);
        if (index == -1)
            throw new InvalidGameOperationException("Room to replace not found");

        _rooms[index] = newRoom;

        AddDomainEvent(new BunkerComponentsUpdatedDomainEvent(Id, GameSessionId, _rooms));
    }

    public void AddEnvironment(Environment environment)
    {
        CheckCanUpdateBunkerComponent();

        ArgumentNullException.ThrowIfNull(environment);

        _environments.Add(environment);

        AddDomainEvent(new BunkerComponentsUpdatedDomainEvent(Id, GameSessionId, _environments));
    }

    public void RemoveEnvironment(Environment environment)
    {
        CheckCanUpdateBunkerComponent();

        ArgumentNullException.ThrowIfNull(environment);

        _environments.Remove(environment);

        AddDomainEvent(new BunkerComponentsUpdatedDomainEvent(Id, GameSessionId, _environments));
    }

    public void ReplaceEnvironment(Environment oldEnvironment, Environment newEnvironment)
    {
        CheckCanUpdateBunkerComponent();

        ArgumentNullException.ThrowIfNull(oldEnvironment);
        ArgumentNullException.ThrowIfNull(newEnvironment);

        var index = _environments.IndexOf(oldEnvironment);
        if (index == -1)
            throw new InvalidGameOperationException("Environment to replace not found");

        _environments[index] = newEnvironment;

        AddDomainEvent(new BunkerComponentsUpdatedDomainEvent(Id, GameSessionId, _environments));
    }

    public void RevealRandomEnvironment()
    {
        CheckCanUpdateBunkerComponent();

        var hiddenEnvironment =
            _environments.Where(x => !x.IsHidden).OrderBy(x => x.Description).FirstOrDefault()
            ?? throw new InvalidGameOperationException("All environments are revealed");

        _environments.Remove(hiddenEnvironment);

        hiddenEnvironment = hiddenEnvironment.Reveal();

        _environments.Add(hiddenEnvironment);

        AddDomainEvent(new BunkerComponentRevealedDomainEvent(Id, GameSessionId, hiddenEnvironment));
    }

    private void CheckCanUpdateBunkerComponent()
    {
        if (IsReadonly)
            throw new InvalidGameOperationException("Bunker in readonly state");
    }

#pragma warning disable CS8618
#pragma warning disable T0008
    private Bunker(Guid id)
#pragma warning restore T0008
#pragma warning restore CS8618
        : base(id) { }
}
