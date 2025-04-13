using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bunker.Game.Domain.AggregateModels.Bunkers;
using Bunker.Game.Infrastructure.Generators.BunkerGenerators;
using Bunker.Game.Infrastructure.Http.GameComponents.Contracts;
using Moq;
using BunkerAggregate = Bunker.Game.Domain.AggregateModels.Bunkers.Bunker;
using Environment = Bunker.Game.Domain.AggregateModels.Bunkers.Environment;

namespace Bunker.Game.Tests.UnitTests.Bunkers
{
    public class BunkerGeneratorTests
    {
        private readonly Mock<IBunkerComponentsClient> _clientMock;
        private readonly BunkerGenerator _generator;

        public BunkerGeneratorTests()
        {
            _clientMock = new Mock<IBunkerComponentsClient>();
            _generator = new BunkerGenerator(_clientMock.Object);
        }

        [Fact]
        public void Constructor_NullClient_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new BunkerGenerator(null));
            Assert.Equal("client", exception.ParamName);
        }

        [Fact]
        public async Task GenerateBunkerComponent_Item_ReturnsItem()
        {
            // Arrange
            var items = new List<BunkerItemDto> { new BunkerItemDto { Description = "Test Item" } };
            _clientMock.Setup(c => c.ItemsGetAsync()).ReturnsAsync(items);

            // Act
            var result = await _generator.GenerateBunkerComponent<Item>();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Item>(result);
        }

        [Fact]
        public async Task GenerateBunkerComponent_Room_ReturnsRoom()
        {
            // Arrange
            var rooms = new List<RoomDto> { new RoomDto { Description = "Test Room" } };
            _clientMock.Setup(c => c.RoomsGetAsync()).ReturnsAsync(rooms);

            // Act
            var result = await _generator.GenerateBunkerComponent<Room>();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Room>(result);
        }

        [Fact]
        public async Task GenerateBunkerComponent_Environment_ReturnsEnvironment()
        {
            // Arrange
            var environments = new List<EnvironmentDto> { new EnvironmentDto { Description = "Test Environment" } };
            _clientMock.Setup(c => c.EnvironmentsGetAsync()).ReturnsAsync(environments);

            // Act
            var result = await _generator.GenerateBunkerComponent<Environment>();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Environment>(result);
        }

        [Fact]
        public async Task GenerateBunkerComponents_NegativeCount_ThrowsArgumentException()
        {
            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(
                () => _generator.GenerateBunkerComponents<Item>(-1)
            );

            Assert.Equal("count", exception.ParamName);
        }

        [Fact]
        public async Task GenerateBunkerComponents_ZeroCount_ReturnsEmptyList()
        {
            // Act
            var result = await _generator.GenerateBunkerComponents<Item>(0);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GenerateBunkerComponents_ValidCount_ReturnsCorrectNumberOfItems()
        {
            // Arrange
            var items = new List<BunkerItemDto> { new BunkerItemDto { Description = "Test Item" } };
            _clientMock.Setup(c => c.ItemsGetAsync()).ReturnsAsync(items);

            // Act
            var result = await _generator.GenerateBunkerComponents<Item>(3);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count());
            Assert.All(result, item => Assert.IsType<Item>(item));
        }

        [Fact]
        public async Task GetBunkerComponent_ItemFound_ReturnsItem()
        {
            // Arrange
            var id = Guid.NewGuid();
            var itemDto = new BunkerItemDto { Description = "Test Item" };
            _clientMock.Setup(c => c.ItemsGetAsync(id)).ReturnsAsync(itemDto);

            // Act
            var result = await _generator.GetBunkerComponent<Item>(id);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Item>(result);
        }

        [Fact]
        public async Task GetBunkerComponent_RoomFound_ReturnsRoom()
        {
            // Arrange
            var id = Guid.NewGuid();
            var roomDto = new RoomDto { Description = "Test Room" };
            _clientMock.Setup(c => c.RoomsGetAsync(id)).ReturnsAsync(roomDto);

            // Act
            var result = await _generator.GetBunkerComponent<Room>(id);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Room>(result);
        }

        [Fact]
        public async Task GetBunkerComponent_EnvironmentFound_ReturnsEnvironment()
        {
            // Arrange
            var id = Guid.NewGuid();
            var environmentDto = new EnvironmentDto { Description = "Test Environment" };
            _clientMock.Setup(c => c.EnvironmentsGetAsync(id)).ReturnsAsync(environmentDto);

            // Act
            var result = await _generator.GetBunkerComponent<Environment>(id);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Environment>(result);
        }

        [Fact]
        public async Task GetBunkerComponent_NotFound_ReturnsNull()
        {
            // Arrange
            var id = Guid.NewGuid();
            _clientMock
                .Setup(c => c.ItemsGetAsync(id))
                .ThrowsAsync(
                    new ApiException("not fonund", 404, "test", new Dictionary<string, IEnumerable<string>>(), default)
                );

            // Act
            var result = await _generator.GetBunkerComponent<Item>(id);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GenerateBunker_ValidGameSessionId_ReturnsBunkerAggregate()
        {
            // Arrange
            var gameSessionId = Guid.NewGuid();
            var items = new List<BunkerItemDto> { new BunkerItemDto { Description = "Test Item" } };
            var rooms = new List<RoomDto> { new RoomDto { Description = "Test Room" } };
            var environments = new List<EnvironmentDto> { new EnvironmentDto { Description = "Test Environment" } };
            var descriptions = new List<BunkerDescriptionDto>
            {
                new BunkerDescriptionDto { Text = "Test Description" },
            };

            _clientMock.Setup(c => c.ItemsGetAsync()).ReturnsAsync(items);
            _clientMock.Setup(c => c.RoomsGetAsync()).ReturnsAsync(rooms);
            _clientMock.Setup(c => c.EnvironmentsGetAsync()).ReturnsAsync(environments);
            _clientMock.Setup(c => c.DescriptionsGetAsync()).ReturnsAsync(descriptions);

            // Act
            var result = await _generator.GenerateBunker(gameSessionId);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<BunkerAggregate>(result);
        }

        [Fact]
        public async Task GenerateBunkerDescription_ReturnsDescription()
        {
            // Arrange
            var descriptions = new List<BunkerDescriptionDto>
            {
                new BunkerDescriptionDto { Text = "Test Description" },
            };
            _clientMock.Setup(c => c.DescriptionsGetAsync()).ReturnsAsync(descriptions);

            // Act
            var result = await _generator.GenerateBunkerDescription();

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Test Description", result);
        }
    }
}
