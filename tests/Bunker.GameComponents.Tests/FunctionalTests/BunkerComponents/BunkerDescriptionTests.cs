using System.Net;
using System.Net.Http.Json;
using Bunker.GameComponents.API.Entities.BunkerComponents;
using Bunker.GameComponents.API.Infrastructure.Database;
using Bunker.GameComponents.API.Models.BunkerComponents.Descriptions;
using Bunker.GameComponents.Tests.Fixtures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Bunker.GameComponents.Tests.FunctionalTests.BunkerComponents;

[Collection("BunkerComponentsApi")]
public class BunkerDescriptionTests
{
    private readonly HttpClient _client;
    private readonly BunkerGameComponentsApiFixture _factory;

    public BunkerDescriptionTests(BunkerGameComponentsApiFixture fixture)
    {
        _client = fixture.CreateClient();
        _factory = fixture;
    }

    [Fact]
    public async Task GetAll_ReturnsSuccessAndListOfDtos()
    {
        // Arrange
        var expectedText = Guid.NewGuid().ToString();
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<GameComponentsContext>();

        context.BunkerDescriptions.Add(new BunkerDescriptionEntity(expectedText));
        await context.SaveChangesAsync();

        // Act
        var response = await _client.GetAsync("/api/bunker-components/descriptions");

        // Assert
        Assert.True(response.IsSuccessStatusCode);
        var descriptionModels = await response.Content.ReadFromJsonAsync<List<BunkerDescriptionDto>>();
        Assert.NotNull(descriptionModels);
        Assert.Contains(descriptionModels, c => c.Text == expectedText);
    }

    [Fact]
    public async Task GetById_ExistingId_ReturnsDto()
    {
        // Arrange
        var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<GameComponentsContext>();
        var entity = new BunkerDescriptionEntity("Test Text") { Id = Guid.NewGuid() };
        context.BunkerDescriptions.Add(entity);
        await context.SaveChangesAsync();

        // Act
        var response = await _client.GetAsync($"/api/bunker-components/descriptions/{entity.Id}");

        // Assert
        Assert.True(response.IsSuccessStatusCode);
        var dto = await response.Content.ReadFromJsonAsync<BunkerDescriptionDto>();
        Assert.NotNull(dto);
        Assert.Equal(entity.Id, dto.Id);
        Assert.Equal("Test Text", dto.Text);
    }

    [Fact]
    public async Task GetById_NonExistingId_ReturnsNotFound()
    {
        // Act
        var response = await _client.GetAsync($"/api/bunker-components/descriptions/{Guid.NewGuid()}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Create_ValidDto_ReturnsCreated()
    {
        // Arrange
        var createDto = new CreateBunkerDescriptionDto { Text = "New Text" };

        // Act
        var response = await _client.PostAsJsonAsync("/api/bunker-components/descriptions", createDto);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var dto = await response.Content.ReadFromJsonAsync<BunkerDescriptionDto>();
        Assert.NotNull(dto);
        Assert.Equal("New Text", dto.Text);
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
        var entity = new BunkerDescriptionEntity("Old Text") { Id = Guid.NewGuid() };
        context.BunkerDescriptions.Add(entity);
        await context.SaveChangesAsync();

        var updateDto = new UpdateBunkerDescriptionDto { Text = "Updated Text" };

        // Act
        var response = await _client.PutAsJsonAsync($"/api/bunker-components/descriptions/{entity.Id}", updateDto);

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        context.ChangeTracker.Clear();
        var updatedEntity = await context.BunkerDescriptions.FirstOrDefaultAsync(x => x.Id == entity.Id);
        Assert.NotNull(updatedEntity);
        Assert.Equal("Updated Text", updatedEntity.Text);
    }

    [Fact]
    public async Task Update_NonExistingId_ReturnsNotFound()
    {
        // Arrange
        var updateDto = new UpdateBunkerDescriptionDto { Text = "Updated Text" };

        // Act
        var response = await _client.PutAsJsonAsync($"/api/bunker-components/descriptions/{Guid.NewGuid()}", updateDto);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Delete_ExistingId_ReturnsNoContent()
    {
        // Arrange
        var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<GameComponentsContext>();
        var entity = new BunkerDescriptionEntity("Test Text") { Id = Guid.NewGuid() };
        context.BunkerDescriptions.Add(entity);
        await context.SaveChangesAsync();

        // Act
        var response = await _client.DeleteAsync($"/api/bunker-components/descriptions/{entity.Id}");

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        context = scope.ServiceProvider.GetRequiredService<GameComponentsContext>();
        var deletedEntity = await context.BunkerDescriptions.FirstOrDefaultAsync(x => x.Id == entity.Id);
        Assert.Null(deletedEntity);
    }

    [Fact]
    public async Task Delete_NonExistingId_ReturnsNotFound()
    {
        // Act
        var response = await _client.DeleteAsync($"/api/bunker-components/descriptions/{Guid.NewGuid()}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
} 