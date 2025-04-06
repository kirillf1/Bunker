using System.Net;
using System.Net.Http.Json;
using Bunker.GameComponents.API.Entities.CharacterComponents;
using Bunker.GameComponents.API.Infrastructure.Database;
using Bunker.GameComponents.API.Models.CharacterComponents.AdditionalInformation;
using Bunker.GameComponents.Tests.Fixtures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Bunker.GameComponents.Tests.FunctionalTests.CharacterComponents
{
    [Collection("BunkerComponentsApi")]
    public class AdditionalInformationTests
    {
        private readonly HttpClient _client;
        private readonly BunkerGameComponentsApiFixture _factory;

        public AdditionalInformationTests(BunkerGameComponentsApiFixture fixture)
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

            context.AdditionalInformationEntitles.Add(new AdditionalInformationEntity(expectedDescription));
            await context.SaveChangesAsync();

            // Act
            var response = await _client.GetAsync("/api/character-components/additional-information");

            // Assert
            Assert.True(response.IsSuccessStatusCode);
            var addInformationModels = await response.Content.ReadFromJsonAsync<List<AdditionalInformationDto>>();
            Assert.NotNull(addInformationModels);
            Assert.Contains(addInformationModels, c => c.Description == expectedDescription);
        }

        [Fact]
        public async Task GetById_ExistingId_ReturnsDto()
        {
            // Arrange
            var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<GameComponentsContext>();
            var entity = new AdditionalInformationEntity("Test Description") { Id = Guid.NewGuid() };
            context.AdditionalInformationEntitles.Add(entity);
            await context.SaveChangesAsync();

            // Act
            var response = await _client.GetAsync($"/api/character-components/additional-information/{entity.Id}");

            // Assert
            Assert.True(response.IsSuccessStatusCode);
            var dto = await response.Content.ReadFromJsonAsync<AdditionalInformationDto>();
            Assert.NotNull(dto);
            Assert.Equal(entity.Id, dto.Id);
            Assert.Equal("Test Description", dto.Description);
        }

        [Fact]
        public async Task GetById_NonExistingId_ReturnsNotFound()
        {
            // Act
            var response = await _client.GetAsync($"/api/character-components/additional-information/{Guid.NewGuid()}");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Create_ValidDto_ReturnsCreated()
        {
            // Arrange
            var createDto = new CreateAdditionalInformationDto { Description = "New Description" };

            // Act
            var response = await _client.PostAsJsonAsync("/api/character-components/additional-information", createDto);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            var dto = await response.Content.ReadFromJsonAsync<AdditionalInformationDto>();
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
            var entity = new AdditionalInformationEntity("Old Description") { Id = Guid.NewGuid() };
            context.AdditionalInformationEntitles.Add(entity);
            await context.SaveChangesAsync();

            var updateDto = new UpdateAdditionalInformationDto { Description = "Updated Description" };

            // Act
            var response = await _client.PutAsJsonAsync(
                $"/api/character-components/additional-information/{entity.Id}",
                updateDto
            );

            // Assert

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

            context.ChangeTracker.Clear();
            var updatedEntity = await context.AdditionalInformationEntitles.FirstOrDefaultAsync(x => x.Id == entity.Id);
            Assert.NotNull(updatedEntity);
            Assert.Equal("Updated Description", updatedEntity.Description);
        }

        [Fact]
        public async Task Update_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            var updateDto = new UpdateAdditionalInformationDto { Description = "Updated Description" };

            // Act
            var response = await _client.PutAsJsonAsync(
                $"/api/character-components/additional-information/{Guid.NewGuid()}",
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
            var entity = new AdditionalInformationEntity("Test Description") { Id = Guid.NewGuid() };
            context.AdditionalInformationEntitles.Add(entity);
            await context.SaveChangesAsync();

            // Act
            var response = await _client.DeleteAsync($"/api/character-components/additional-information/{entity.Id}");

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

            context.ChangeTracker.Clear();
            var deletedEntity = await context.AdditionalInformationEntitles.FirstOrDefaultAsync(x => x.Id == entity.Id);
            Assert.Null(deletedEntity);
        }

        [Fact]
        public async Task Delete_NonExistingId_ReturnsNotFound()
        {
            // Act
            var response = await _client.DeleteAsync(
                $"/api/character-components/additional-information/{Guid.NewGuid()}"
            );

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
