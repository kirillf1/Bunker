using Bunker.Domain.Shared.Exceptions;
using Bunker.Game.Domain.AggregateModels.Bunkers;
using Environment = Bunker.Game.Domain.AggregateModels.Bunkers.Environment;

namespace Bunker.Game.Tests.UnitTests.Bunkers;

public class BunkerTests
{
    private readonly Guid _bunkerId = Guid.NewGuid();
    private readonly Guid _gameSessionId = Guid.NewGuid();
    private readonly string _description = "Test Bunker";
    private readonly List<Item> _items;
    private readonly List<Room> _rooms;
    private readonly List<Environment> _environments;

    public BunkerTests()
    {
        _items = new List<Item> { new Item(Guid.NewGuid().ToString()), new Item(Guid.NewGuid().ToString()) };
        _rooms = new List<Room> { new Room(Guid.NewGuid().ToString()), new Room(Guid.NewGuid().ToString()) };
        _environments = new List<Environment>
        {
            new Environment(Guid.NewGuid().ToString()),
            new Environment(Guid.NewGuid().ToString()),
        };
    }

    [Fact]
    public void Constructor_ValidParameters_SetsPropertiesCorrectly()
    {
        // Arrange
        var bunker = new BunkerAggregate(_bunkerId, _gameSessionId, _description, _items, _environments, _rooms);

        // Assert
        Assert.Equal(_bunkerId, bunker.Id);
        Assert.Equal(_gameSessionId, bunker.GameSessionId);
        Assert.Equal(_description, bunker.Description);
        Assert.Equal(_items.Count, bunker.Items.Count);
        Assert.Equal(_rooms.Count, bunker.Rooms.Count);
        Assert.Equal(_environments.Count, bunker.Environments.Count);
        Assert.False(bunker.IsReadonly);
    }

    [Fact]
    public void Constructor_EmptyItems_ThrowsArgumentException()
    {
        // Arrange
        var emptyItems = new List<Item>();

        // Act & Assert
        Assert.Throws<ArgumentException>(
            () => new BunkerAggregate(_bunkerId, _gameSessionId, _description, emptyItems, _environments, _rooms)
        );
    }

    [Fact]
    public void Constructor_TooManyItems_ThrowsArgumentException()
    {
        // Arrange
        var tooManyItems = Enumerable
            .Range(1, BunkerAggregate.MAX_BUNKER_ITEM_COUNT + 1)
            .Select(i => new Item($"Item{i}"))
            .ToList();

        // Act & Assert
        Assert.Throws<ArgumentException>(
            () => new BunkerAggregate(_bunkerId, _gameSessionId, _description, tooManyItems, _environments, _rooms)
        );
    }

    [Fact]
    public void Constructor_EmptyEnvironments_ThrowsArgumentException()
    {
        // Arrange
        var emptyEnvironments = new List<Environment>();

        // Act & Assert
        Assert.Throws<ArgumentException>(
            () => new BunkerAggregate(_bunkerId, _gameSessionId, _description, _items, emptyEnvironments, _rooms)
        );
    }

    [Fact]
    public void Constructor_TooManyEnvironments_ThrowsArgumentException()
    {
        // Arrange
        var tooManyEnvironments = Enumerable
            .Range(1, BunkerAggregate.MAX_BUNKER_ENVIROMENT_COUNT + 1)
            .Select(i => new Environment($"Env{i}"))
            .ToList();

        // Act & Assert
        Assert.Throws<ArgumentException>(
            () => new BunkerAggregate(_bunkerId, _gameSessionId, _description, _items, tooManyEnvironments, _rooms)
        );
    }

    [Fact]
    public void Constructor_EmptyRooms_ThrowsArgumentException()
    {
        // Arrange
        var emptyRooms = new List<Room>();

        // Act & Assert
        Assert.Throws<ArgumentException>(
            () => new BunkerAggregate(_bunkerId, _gameSessionId, _description, _items, _environments, emptyRooms)
        );
    }

    [Fact]
    public void Constructor_TooManyRooms_ThrowsArgumentException()
    {
        // Arrange
        var tooManyRooms = Enumerable
            .Range(1, BunkerAggregate.MAX_ROOMS_COUNT + 1)
            .Select(i => new Room($"Room{i}"))
            .ToList();

        // Act & Assert
        Assert.Throws<ArgumentException>(
            () => new BunkerAggregate(_bunkerId, _gameSessionId, _description, _items, _environments, tooManyRooms)
        );
    }

    [Fact]
    public void AddItem_ValidItem_AddsToItems()
    {
        // Arrange
        var bunker = CreateBunker();
        var newItem = new Item("NewItem");

        // Act
        bunker.AddItem(newItem);

        // Assert
        Assert.Contains(newItem, bunker.Items);
        Assert.Equal(_items.Count + 1, bunker.Items.Count);
    }

    [Fact]
    public void AddItem_NullItem_ThrowsArgumentNullException()
    {
        // Arrange
        var bunker = CreateBunker();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => bunker.AddItem(null));
    }

    [Fact]
    public void AddItem_WhenReadonly_ThrowsInvalidGameOperationException()
    {
        // Arrange
        var bunker = CreateBunker();
        bunker.MarkReadonly();

        // Act & Assert
        Assert.Throws<InvalidGameOperationException>(() => bunker.AddItem(new Item("NewItem")));
    }

    [Fact]
    public void RemoveItem_ExistingItem_RemovesFromItems()
    {
        // Arrange
        var bunker = CreateBunker();
        var itemToRemove = _items[0];

        // Act
        bunker.RemoveItem(itemToRemove);

        // Assert
        Assert.DoesNotContain(itemToRemove, bunker.Items);
        Assert.Equal(_items.Count - 1, bunker.Items.Count);
    }

    [Fact]
    public void ReplaceItem_ExistingItem_ReplacesCorrectly()
    {
        // Arrange
        var bunker = CreateBunker();
        var oldItem = _items[0];
        var newItem = new Item("NewItem");

        // Act
        bunker.ReplaceItem(oldItem, newItem);

        // Assert
        Assert.DoesNotContain(oldItem, bunker.Items);
        Assert.Contains(newItem, bunker.Items);
        Assert.Equal(_items.Count, bunker.Items.Count);
    }

    [Fact]
    public void ReplaceItem_NonExistingItem_ThrowsInvalidGameOperationException()
    {
        // Arrange
        var bunker = CreateBunker();
        var nonExistingItem = new Item("NonExisting");

        // Act & Assert
        Assert.Throws<InvalidGameOperationException>(() => bunker.ReplaceItem(nonExistingItem, new Item("NewItem")));
    }

    [Fact]
    public void RevealRandomItem_HiddenItemExists_RevealsItem()
    {
        // Arrange
        var bunker = CreateBunker();
        var hiddenItem = _items.First();

        // Act
        bunker.RevealRandomItem();

        // Assert
        var revealedItem = bunker.Items.First(i => i.Description == hiddenItem.Description);
        Assert.False(revealedItem.IsHidden);
    }

    [Fact]
    public void RevealRandomItem_NoHiddenItems_ThrowsInvalidGameOperationException()
    {
        // Arrange
        var items = new List<Item> { new Item("Item1", false), new Item("Item2", false) };
        var bunker = new BunkerAggregate(_bunkerId, _gameSessionId, _description, items, _environments, _rooms);

        // Act & Assert
        Assert.Throws<InvalidGameOperationException>(() => bunker.RevealRandomItem());
    }

    [Fact]
    public void AddRoom_ValidRoom_AddsToRooms()
    {
        // Arrange
        var bunker = CreateBunker();
        var newRoom = new Room("NewRoom");

        // Act
        bunker.AddRoom(newRoom);

        // Assert
        Assert.Contains(newRoom, bunker.Rooms);
        Assert.Equal(_rooms.Count + 1, bunker.Rooms.Count);
    }

    [Fact]
    public void RevealRandomRoom_HiddenRoomExists_RevealsRoom()
    {
        // Arrange
        var bunker = CreateBunker();

        // Act
        bunker.RevealRandomRoom();

        // Assert

        Assert.Contains(bunker.Rooms, r => !r.IsHidden);
    }

    [Fact]
    public void AddEnvironment_ValidEnvironment_AddsToEnvironments()
    {
        // Arrange
        var bunker = CreateBunker();
        var newEnvironment = new Environment("NewEnv");

        // Act
        bunker.AddEnvironment(newEnvironment);

        // Assert
        Assert.Contains(newEnvironment, bunker.Environments);
        Assert.Equal(_environments.Count + 1, bunker.Environments.Count);
    }

    [Fact]
    public void RevealRandomEnvironment_HiddenEnvironmentExists_RevealsEnvironment()
    {
        var bunker = CreateBunker();
        var hiddenEnvironment = _environments.First();

        // Act
        bunker.RevealRandomEnvironment();

        // Assert
        var revealedEnvironment = bunker.Environments.First(e => e.Description == hiddenEnvironment.Description);
        Assert.False(revealedEnvironment.IsHidden);
    }

    [Fact]
    public void RecreateBunker_ValidParameters_UpdatesProperties()
    {
        // Arrange
        var bunker = CreateBunker();
        var newDescription = "New Description";
        var newItems = new List<Item> { new Item("NewItem1") };
        var newRooms = new List<Room> { new Room("NewRoom1") };
        var newEnvironments = new List<Environment> { new Environment("NewEnv1") };

        // Act
        bunker.RecreateBunker(newDescription, newItems, newEnvironments, newRooms);

        // Assert
        Assert.Equal(newDescription, bunker.Description);
        Assert.Equal(newItems, bunker.Items);
        Assert.Equal(newRooms, bunker.Rooms);
        Assert.Equal(newEnvironments, bunker.Environments);
    }

    [Fact]
    public void MarkReadonly_SetsIsReadonlyTrue()
    {
        // Arrange
        var bunker = CreateBunker();

        // Act
        bunker.MarkReadonly();

        // Assert
        Assert.True(bunker.IsReadonly);
    }

    private BunkerAggregate CreateBunker()
    {
        return new BunkerAggregate(_bunkerId, _gameSessionId, _description, _items, _environments, _rooms);
    }
}
