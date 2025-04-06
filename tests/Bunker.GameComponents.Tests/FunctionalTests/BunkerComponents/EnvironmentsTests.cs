using System.Net;
using System.Net.Http.Json;
using Bunker.GameComponents.API.Entities.BunkerComponents;
using Bunker.GameComponents.API.Infrastructure.Database;
using Bunker.GameComponents.API.Models.BunkerComponents.Environments;
using Bunker.GameComponents.Tests.Fixtures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Bunker.GameComponents.Tests.FunctionalTests.BunkerComponents
{
    [Collection("BunkerComponentsApi")]
    public class EnvironmentsTests
    {
        private readonly HttpClient _client;
        private readonly BunkerGameComponentsApiFixture _factory;

        public EnvironmentsTests(BunkerGameComponentsApiFixture fixture)
        {
            _client = fixture.CreateClient();
            _factory = fixture;
        }

        [Fact]
        public async Task GetEnvironments_ReturnsSuccessAndListOfDtos()
        {
            // Arrange
            var expectedDescription = Guid.NewGuid().ToString();
            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<GameComponentsContext>();

            context.BunkerEnvironments.Add(new EnvironmentEntity(expectedDescription));
            await context.SaveChangesAsync();

            // Act
            var response = await _client.GetAsync("/api/bunker-components/environments");

            // Assert
            Assert.True(response.IsSuccessStatusCode);
            var environmentModels = await response.Content.ReadFromJsonAsync<List<EnvironmentDto>>();
            Assert.NotNull(environmentModels);
            Assert.Contains(environmentModels, c => c.Description == expectedDescription);
        }

        [Fact]
        public async Task GetEnvironment_ExistingId_ReturnsDto()
        {
            // Arrange
            var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<GameComponentsContext>();
            var entity = new EnvironmentEntity("Test Description") { Id = Guid.NewGuid() };
            context.BunkerEnvironments.Add(entity);
            await context.SaveChangesAsync();

            // Act
            var response = await _client.GetAsync($"/api/bunker-components/environments/{entity.Id}");

            // Assert
            Assert.True(response.IsSuccessStatusCode);
            var dto = await response.Content.ReadFromJsonAsync<EnvironmentDto>();
            Assert.NotNull(dto);
            Assert.Equal(entity.Id, dto.Id);
            Assert.Equal("Test Description", dto.Description);
        }

        [Fact]
        public async Task GetEnvironment_NonExistingId_ReturnsNotFound()
        {
            // Act
            var response = await _client.GetAsync($"/api/bunker-components/environments/{Guid.NewGuid()}");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task CreateEnvironment_ValidDto_ReturnsCreated()
        {
            // Arrange
            var createDto = new CreateEnvironmentDto { Description = "New Description" };

            // Act
            var response = await _client.PostAsJsonAsync("/api/bunker-components/environments", createDto);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            var dto = await response.Content.ReadFromJsonAsync<EnvironmentDto>();
            Assert.NotNull(dto);
            Assert.Equal("New Description", dto.Description);
            Assert.NotEqual(Guid.Empty, dto.Id);

            var location = response.Headers.Location?.ToString();
            Assert.Contains(dto.Id.ToString(), location);
        }

        [Fact]
        public async Task UpdateEnvironment_ExistingId_ReturnsNoContent()
        {
            // Arrange
            var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<GameComponentsContext>();
            var entity = new EnvironmentEntity("Old Description") { Id = Guid.NewGuid() };
            context.BunkerEnvironments.Add(entity);
            await context.SaveChangesAsync();

            var updateDto = new UpdateEnvironmentDto { Description = "Updated Description" };

            // Act
            var response = await _client.PutAsJsonAsync($"/api/bunker-components/environments/{entity.Id}", updateDto);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

            context.ChangeTracker.Clear();
            var updatedEntity = await context.BunkerEnvironments.FirstOrDefaultAsync(x => x.Id == entity.Id);
            Assert.NotNull(updatedEntity);
            Assert.Equal("Updated Description", updatedEntity.Description);
        }

        [Fact]
        public async Task UpdateEnvironment_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            var updateDto = new UpdateEnvironmentDto { Description = "Updated Description" };

            // Act
            var response = await _client.PutAsJsonAsync(
                $"/api/bunker-components/environments/{Guid.NewGuid()}",
                updateDto
            );

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task DeleteEnvironment_ExistingId_ReturnsNoContent()
        {
            // Arrange
            var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<GameComponentsContext>();
            var entity = new EnvironmentEntity("Test Description") { Id = Guid.NewGuid() };
            context.BunkerEnvironments.Add(entity);
            await context.SaveChangesAsync();

            // Act
            var response = await _client.DeleteAsync($"/api/bunker-components/environments/{entity.Id}");

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

            context = scope.ServiceProvider.GetRequiredService<GameComponentsContext>();
            var deletedEntity = await context.BunkerEnvironments.FirstOrDefaultAsync(x => x.Id == entity.Id);
            Assert.Null(deletedEntity);
        }

        [Fact]
        public async Task DeleteEnvironment_NonExistingId_ReturnsNotFound()
        {
            // Act
            var response = await _client.DeleteAsync($"/api/bunker-components/environments/{Guid.NewGuid()}");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
