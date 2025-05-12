using Bunker.Game.Domain.AggregateModels.Bunkers;
using Bunker.Game.Infrastructure.Generators.BunkerGenerators;
using Bunker.Game.Infrastructure.Http.GameComponents.Contracts;
using Bunker.Game.Tests.Fakes;
using BunkerAggregate = Bunker.Game.Domain.AggregateModels.Bunkers.BunkerAggregate;
using Environment = Bunker.Game.Domain.AggregateModels.Bunkers.Environment;

namespace Bunker.Game.Tests.UnitTests.Bunkers
{
    public class BunkerGeneratorTests
    {
        private readonly IBunkerComponentsClient _client;
        private readonly BunkerGenerator _generator;

        public BunkerGeneratorTests()
        {
            _client = new FakeBunkerComponentsClient();
            _generator = new BunkerGenerator(_client);
        }

        [Fact]
        public void Constructor_NullClient_ThrowsArgumentNullException()
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => new BunkerGenerator(null));
            Assert.Equal("client", exception.ParamName);
        }

        [Fact]
        public async Task GenerateBunkerComponent_Item_ReturnsItem()
        {
            // Act
            var result = await _generator.GenerateBunkerComponent<Item>();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Item>(result);
        }

        [Fact]
        public async Task GenerateBunkerComponent_Room_ReturnsRoom()
        {
            // Act
            var result = await _generator.GenerateBunkerComponent<Room>();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Room>(result);
        }

        [Fact]
        public async Task GenerateBunkerComponent_Environment_ReturnsEnvironment()
        {
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
            var count = 3;

            // Act
            var result = await _generator.GenerateBunkerComponents<Item>(count);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(count, result.Count());
            Assert.All(result, item => Assert.IsType<Item>(item));
        }

        [Fact]
        public async Task GetBunkerComponent_ItemFound_ReturnsItem()
        {
            // Arrange
            var id = Guid.NewGuid();

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

            // Act
            var result = await _generator.GetBunkerComponent<Environment>(id);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Environment>(result);
        }

        [Fact]
        public async Task GenerateBunkerDescription_ReturnsDescription()
        {
            // Act
            var result = await _generator.GenerateBunkerDescription();

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }

        [Fact]
        public async Task GenerateBunker_ValidGameSessionId_ReturnsBunkerAggregate()
        {
            // Arrange
            var gameSessionId = Guid.NewGuid();

            // Act
            var bunker = await _generator.GenerateBunker(gameSessionId);

            // Assert
            Assert.NotNull(bunker);
            Assert.IsType<BunkerAggregate>(bunker);
            Assert.Equal(gameSessionId, bunker.GameSessionId);

            Assert.InRange(bunker.Rooms.Count, BunkerAggregate.MIN_ROOMS_COUNT, BunkerAggregate.MAX_ROOMS_COUNT);

            Assert.InRange(
                bunker.Items.Count,
                BunkerAggregate.MIN_BUNKER_ITEM_COUNT,
                BunkerAggregate.MAX_BUNKER_ITEM_COUNT
            );

            Assert.InRange(
                bunker.Environments.Count,
                BunkerAggregate.MIN_BUNKER_ENVIROMENT_COUNT,
                BunkerAggregate.MAX_BUNKER_ENVIROMENT_COUNT
            );
        }
    }
}
