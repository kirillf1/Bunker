using System.Net;
using System.Net.Http.Json;
using Bunker.GameComponents.API.Entities.BunkerComponents;
using Bunker.GameComponents.API.Infrastructure.Database;
using Bunker.GameComponents.API.Models.BunkerComponents.Rooms;
using Bunker.GameComponents.Tests.Fixtures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Bunker.GameComponents.Tests.FunctionalTests.BunkerComponents
{
    [Collection("BunkerComponentsApi")]
    public class RoomsTests
    {
        private readonly HttpClient _client;
        private readonly BunkerGameComponentsApiFixture _factory;

        public RoomsTests(BunkerGameComponentsApiFixture fixture)
        {
            _client = fixture.CreateClient();
            _factory = fixture;
        }

        [Fact]
        public async Task GetRooms_ReturnsSuccessAndListOfDtos()
        {
            // Arrange
            var expectedDescription = Guid.NewGuid().ToString();
            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<GameComponentsContext>();

            context.BunkerRooms.Add(new RoomEntity(expectedDescription));
            await context.SaveChangesAsync();

            // Act
            var response = await _client.GetAsync("/api/bunker-components/rooms");

            // Assert
            Assert.True(response.IsSuccessStatusCode);
            var roomModels = await response.Content.ReadFromJsonAsync<List<RoomDto>>();
            Assert.NotNull(roomModels);
            Assert.Contains(roomModels, c => c.Description == expectedDescription);
        }

        [Fact]
        public async Task GetRoom_ExistingId_ReturnsDto()
        {
            // Arrange
            var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<GameComponentsContext>();
            var entity = new RoomEntity("Test Description") { Id = Guid.NewGuid() };
            context.BunkerRooms.Add(entity);
            await context.SaveChangesAsync();

            // Act
            var response = await _client.GetAsync($"/api/bunker-components/rooms/{entity.Id}");

            // Assert
            Assert.True(response.IsSuccessStatusCode);
            var dto = await response.Content.ReadFromJsonAsync<RoomDto>();
            Assert.NotNull(dto);
            Assert.Equal(entity.Id, dto.Id);
            Assert.Equal("Test Description", dto.Description);
        }

        [Fact]
        public async Task GetRoom_NonExistingId_ReturnsNotFound()
        {
            // Act
            var response = await _client.GetAsync($"/api/bunker-components/rooms/{Guid.NewGuid()}");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task CreateRoom_ValidDto_ReturnsCreated()
        {
            // Arrange
            var createDto = new CreateRoomDto { Description = "New Description" };

            // Act
            var response = await _client.PostAsJsonAsync("/api/bunker-components/rooms", createDto);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            var dto = await response.Content.ReadFromJsonAsync<RoomDto>();
            Assert.NotNull(dto);
            Assert.Equal("New Description", dto.Description);
            Assert.NotEqual(Guid.Empty, dto.Id);

            var location = response.Headers.Location?.ToString();
            Assert.Contains(dto.Id.ToString(), location);
        }

        [Fact]
        public async Task UpdateRoom_ExistingId_ReturnsNoContent()
        {
            // Arrange
            var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<GameComponentsContext>();
            var entity = new RoomEntity("Old Description") { Id = Guid.NewGuid() };
            context.BunkerRooms.Add(entity);
            await context.SaveChangesAsync();

            var updateDto = new UpdateRoomDto { Description = "Updated Description" };

            // Act
            var response = await _client.PutAsJsonAsync($"/api/bunker-components/rooms/{entity.Id}", updateDto);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

            context.ChangeTracker.Clear();
            var updatedEntity = await context.BunkerRooms.FirstOrDefaultAsync(x => x.Id == entity.Id);
            Assert.NotNull(updatedEntity);
            Assert.Equal("Updated Description", updatedEntity.Description);
        }

        [Fact]
        public async Task UpdateRoom_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            var updateDto = new UpdateRoomDto { Description = "Updated Description" };

            // Act
            var response = await _client.PutAsJsonAsync($"/api/bunker-components/rooms/{Guid.NewGuid()}", updateDto);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task DeleteRoom_ExistingId_ReturnsNoContent()
        {
            // Arrange
            var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<GameComponentsContext>();
            var entity = new RoomEntity("Test Description") { Id = Guid.NewGuid() };
            context.BunkerRooms.Add(entity);
            await context.SaveChangesAsync();

            // Act
            var response = await _client.DeleteAsync($"/api/bunker-components/rooms/{entity.Id}");

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

            context = scope.ServiceProvider.GetRequiredService<GameComponentsContext>();
            var deletedEntity = await context.BunkerRooms.FirstOrDefaultAsync(x => x.Id == entity.Id);
            Assert.Null(deletedEntity);
        }

        [Fact]
        public async Task DeleteRoom_NonExistingId_ReturnsNotFound()
        {
            // Act
            var response = await _client.DeleteAsync($"/api/bunker-components/rooms/{Guid.NewGuid()}");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
