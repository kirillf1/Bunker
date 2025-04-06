using System.Net;
using System.Net.Http.Json;
using Bunker.GameComponents.API.Entities.BunkerComponents;
using Bunker.GameComponents.API.Infrastructure.Database;
using Bunker.GameComponents.API.Models.BunkerComponents.Items;
using Bunker.GameComponents.Tests.Fixtures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Bunker.GameComponents.Tests.FunctionalTests.BunkerComponents;

[Collection("BunkerComponentsApi")]
public class BunkerItemsTests
{
    private readonly HttpClient _client;
    private readonly BunkerGameComponentsApiFixture _factory;

    public BunkerItemsTests(BunkerGameComponentsApiFixture fixture)
    {
        _client = fixture.CreateClient();
        _factory = fixture;
    }

    [Fact]
    public async Task GetBunkerItems_ReturnsSuccessAndListOfDtos()
    {
        // Arrange
        var expectedDescription = Guid.NewGuid().ToString();
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<GameComponentsContext>();

        context.BunkerItems.Add(new BunkerItemEntity(expectedDescription));
        await context.SaveChangesAsync();

        // Act
        var response = await _client.GetAsync("/api/bunker-components/items");

        // Assert
        Assert.True(response.IsSuccessStatusCode);
        var itemModels = await response.Content.ReadFromJsonAsync<List<BunkerItemDto>>();
        Assert.NotNull(itemModels);
        Assert.Contains(itemModels, c => c.Description == expectedDescription);
    }

    [Fact]
    public async Task GetBunkerItem_ExistingId_ReturnsDto()
    {
        // Arrange
        var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<GameComponentsContext>();
        var entity = new BunkerItemEntity("Test Description") { Id = Guid.NewGuid() };
        context.BunkerItems.Add(entity);
        await context.SaveChangesAsync();

        // Act
        var response = await _client.GetAsync($"/api/bunker-components/items/{entity.Id}");

        // Assert
        Assert.True(response.IsSuccessStatusCode);
        var dto = await response.Content.ReadFromJsonAsync<BunkerItemDto>();
        Assert.NotNull(dto);
        Assert.Equal(entity.Id, dto.Id);
        Assert.Equal("Test Description", dto.Description);
    }

    [Fact]
    public async Task GetBunkerItem_NonExistingId_ReturnsNotFound()
    {
        // Act
        var response = await _client.GetAsync($"/api/bunker-components/items/{Guid.NewGuid()}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task CreateBunkerItem_ValidDto_ReturnsCreated()
    {
        // Arrange
        var createDto = new CreateBunkerItemDto { Description = "New Description" };

        // Act
        var response = await _client.PostAsJsonAsync("/api/bunker-components/items", createDto);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var dto = await response.Content.ReadFromJsonAsync<BunkerItemDto>();
        Assert.NotNull(dto);
        Assert.Equal("New Description", dto.Description);
        Assert.NotEqual(Guid.Empty, dto.Id);

        var location = response.Headers.Location?.ToString();
        Assert.Contains(dto.Id.ToString(), location);
    }

    [Fact]
    public async Task UpdateBunkerItem_ExistingId_ReturnsNoContent()
    {
        // Arrange
        var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<GameComponentsContext>();
        var entity = new BunkerItemEntity("Old Description") { Id = Guid.NewGuid() };
        context.BunkerItems.Add(entity);
        await context.SaveChangesAsync();

        var updateDto = new UpdateBunkerItemDto { Description = "Updated Description" };

        // Act
        var response = await _client.PutAsJsonAsync($"/api/bunker-components/items/{entity.Id}", updateDto);

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        context.ChangeTracker.Clear();
        var updatedEntity = await context.BunkerItems.FirstOrDefaultAsync(x => x.Id == entity.Id);
        Assert.NotNull(updatedEntity);
        Assert.Equal("Updated Description", updatedEntity.Description);
    }

    [Fact]
    public async Task UpdateBunkerItem_NonExistingId_ReturnsNotFound()
    {
        // Arrange
        var updateDto = new UpdateBunkerItemDto { Description = "Updated Description" };

        // Act
        var response = await _client.PutAsJsonAsync($"/api/bunker-components/items/{Guid.NewGuid()}", updateDto);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task DeleteBunkerItem_ExistingId_ReturnsNoContent()
    {
        // Arrange
        var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<GameComponentsContext>();
        var entity = new BunkerItemEntity("Test Description") { Id = Guid.NewGuid() };
        context.BunkerItems.Add(entity);
        await context.SaveChangesAsync();

        // Act
        var response = await _client.DeleteAsync($"/api/bunker-components/items/{entity.Id}");

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        context = scope.ServiceProvider.GetRequiredService<GameComponentsContext>();
        var deletedEntity = await context.BunkerItems.FirstOrDefaultAsync(x => x.Id == entity.Id);
        Assert.Null(deletedEntity);
    }

    [Fact]
    public async Task DeleteBunkerItem_NonExistingId_ReturnsNotFound()
    {
        // Act
        var response = await _client.DeleteAsync($"/api/bunker-components/items/{Guid.NewGuid()}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
