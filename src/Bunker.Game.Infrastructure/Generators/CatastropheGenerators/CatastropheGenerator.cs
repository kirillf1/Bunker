using Bunker.Game.Domain.AggregateModels.Catastrophes;
using Bunker.Game.Infrastructure.Http.GameComponents.Contracts;

namespace Bunker.Game.Infrastructure.Generators.CatastropheGenerators;

public class CatastropheGenerator : ICatastropheGenerator
{
    private readonly ICatastropheComponentsClient _client;

    public CatastropheGenerator(ICatastropheComponentsClient client)
    {
        _client = client;
    }

    public async Task<string?> GetDescription(Guid id)
    {
        try
        {
            var catastropheDto = await _client.DescriptionsGetAsync(id);
            return catastropheDto.Description;
        }
        catch (ApiException ex) when (ex.StatusCode == 404)
        {
            return null;
        }
    }

    public async Task<string> GenerateDescription()
    {
        var descriptions = await _client.DescriptionsGetAsync();

        return descriptions.ElementAt(Random.Shared.Next(0, descriptions.Count)).Description;
    }

    public async Task<Catastrophe> GenerateCatastrophe(Guid gameSessionId)
    {
        var catastropheId = Guid.CreateVersion7();

        var description = await GenerateDescription();

        var catastrophe = new Catastrophe(catastropheId, gameSessionId, description);

        return catastrophe;
    }
}
