using System.Net;
using System.Net.Http.Json;
using Bunker.GameComponents.API.Entities.CatastropheComponents;
using Bunker.GameComponents.API.Infrastructure.Database;
using Bunker.GameComponents.API.Models.Catastrophes;
using Bunker.GameComponents.Tests.Fixtures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Bunker.GameComponents.Tests.FunctionalTests.Catastrophes
{
    [Collection("BunkerComponentsApi")]
    public class CatastropheTests
    {
        private readonly HttpClient _client;
        private readonly BunkerGameComponentsApiFixture _factory;

        public CatastropheTests(BunkerGameComponentsApiFixture fixture)
        {
            _client = fixture.CreateClient();
            _factory = fixture;
        }

        [Fact]
        public async Task GetAll_ReturnsSuccessAndListOfDtos()
        {
            // Arrange
            var expectedDescription = Guid.NewGuid().ToString();
            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<GameComponentsContext>();

            context.Catastrophes.Add(new CatastropheEntity(expectedDescription));
            await context.SaveChangesAsync();

            // Act
            var response = await _client.GetAsync("/api/catastrophe-components/descriptions");

            // Assert
            Assert.True(response.IsSuccessStatusCode);
            var catastropheModels = await response.Content.ReadFromJsonAsync<List<CatastropheDto>>();
            Assert.NotNull(catastropheModels);
            Assert.Contains(catastropheModels, c => c.Description == expectedDescription);
        }

        [Fact]
        public async Task GetById_ExistingId_ReturnsDto()
        {
            // Arrange
            var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<GameComponentsContext>();
            var entity = new CatastropheEntity("Test Description") { Id = Guid.NewGuid() };
            context.Catastrophes.Add(entity);
            await context.SaveChangesAsync();

            // Act
            var response = await _client.GetAsync($"/api/catastrophe-components/descriptions/{entity.Id}");

            // Assert
            Assert.True(response.IsSuccessStatusCode);
            var dto = await response.Content.ReadFromJsonAsync<CatastropheDto>();
            Assert.NotNull(dto);
            Assert.Equal(entity.Id, dto.Id);
            Assert.Equal("Test Description", dto.Description);
        }

        [Fact]
        public async Task GetById_NonExistingId_ReturnsNotFound()
        {
            // Act
            var response = await _client.GetAsync($"/api/catastrophe-components/descriptions/{Guid.NewGuid()}");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Create_ValidDto_ReturnsCreated()
        {
            // Arrange
            var createDto = new CreateCatastropheDto { Description = "New Description" };

            // Act
            var response = await _client.PostAsJsonAsync("/api/catastrophe-components/descriptions", createDto);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            var dto = await response.Content.ReadFromJsonAsync<CatastropheDto>();
            Assert.NotNull(dto);
            Assert.Equal("New Description", dto.Description);
            Assert.NotEqual(Guid.Empty, dto.Id);

            var location = response.Headers.Location?.ToString();
            Assert.Contains(dto.Id.ToString(), location);
        }

        [Fact]
        public async Task Update_ExistingId_ReturnsNoContent()
        {
            // Arrange
            var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<GameComponentsContext>();
            var entity = new CatastropheEntity("Old Description") { Id = Guid.NewGuid() };
            context.Catastrophes.Add(entity);
            await context.SaveChangesAsync();

            var updateDto = new UpdateCatastropheDto { Description = "Updated Description" };

            // Act
            var response = await _client.PutAsJsonAsync(
                $"/api/catastrophe-components/descriptions/{entity.Id}",
                updateDto
            );

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

            context.ChangeTracker.Clear();
            var updatedEntity = await context.Catastrophes.FirstOrDefaultAsync(x => x.Id == entity.Id);
            Assert.NotNull(updatedEntity);
            Assert.Equal("Updated Description", updatedEntity.Description);
        }

        [Fact]
        public async Task Update_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            var updateDto = new UpdateCatastropheDto { Description = "Updated Description" };

            // Act
            var response = await _client.PutAsJsonAsync(
                $"/api/catastrophe-components/descriptions/{Guid.NewGuid()}",
                updateDto
            );

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Delete_ExistingId_ReturnsNoContent()
        {
            // Arrange
            var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<GameComponentsContext>();
            var entity = new CatastropheEntity("Test Description") { Id = Guid.NewGuid() };
            context.Catastrophes.Add(entity);
            await context.SaveChangesAsync();

            // Act
            var response = await _client.DeleteAsync($"/api/catastrophe-components/descriptions/{entity.Id}");

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

            context = scope.ServiceProvider.GetRequiredService<GameComponentsContext>();
            var deletedEntity = await context.Catastrophes.FirstOrDefaultAsync(x => x.Id == entity.Id);
            Assert.Null(deletedEntity);
        }

        [Fact]
        public async Task Delete_NonExistingId_ReturnsNotFound()
        {
            // Act
            var response = await _client.DeleteAsync($"/api/catastrophe-components/descriptions/{Guid.NewGuid()}");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
