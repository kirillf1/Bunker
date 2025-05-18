using Bogus;
using Bunker.Game.Domain.AggregateModels.Catastrophes;

namespace Bunker.Game.Tests.Fakes;

public class MockCatastropheGenerator : ICatastropheGenerator
{
    private readonly Faker _faker;
    private readonly Dictionary<Guid, string> _descriptionCache = new();

    public MockCatastropheGenerator()
    {
        _faker = new Faker("ru");
    }

    public async Task<string?> GetDescription(Guid id)
    {
        if (_descriptionCache.TryGetValue(id, out var cachedDescription))
            return await Task.FromResult(cachedDescription);

        var description = await GenerateDescription();
        _descriptionCache[id] = description;

        return description;
    }

    public async Task<string> GenerateDescription()
    {
        string description = _faker.PickRandom(
            "Глобальное потепление привело к затоплению 90% населенных пунктов.",
            "Извержение супервулкана вызвало многолетнюю вулканическую зиму.",
            "Астероид диаметром 500 метров столкнулся с Землей.",
            "Мощная солнечная вспышка уничтожила всю электронику на планете.",
            "Пандемия неизвестного вируса уничтожила большую часть населения Земли.",
            "Глобальная ядерная война между сверхдержавами.",
            "Искусственный интеллект вышел из-под контроля и атаковал человечество.",
            "Внезапное изменение магнитного поля Земли привело к массовым катаклизмам.",
            "Химическое загрязнение сделало большую часть планеты непригодной для жизни.",
            "Нашествие инопланетных существ, уничтоживших большую часть земной цивилизации."
        );

        return await Task.FromResult(description);
    }

    public async Task<Catastrophe> GenerateCatastrophe(Guid gameSessionId)
    {
        var catastropheId = Guid.NewGuid();
        var description = await GenerateDescription();

        _descriptionCache[catastropheId] = description;

        return new Catastrophe(catastropheId, gameSessionId, description);
    }
}
