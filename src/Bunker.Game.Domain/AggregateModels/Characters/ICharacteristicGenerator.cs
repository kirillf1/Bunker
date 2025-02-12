namespace Bunker.Game.Domain.AggregateModels.Characters;

public interface ICharacteristicGenerator
{
    Task<T> GenerateCharacteristic<T>()
        where T : ICharacteristic;

    Task<IEnumerable<T>> GenerateCharacteristics<T>(int count)
        where T : ICharacteristic;

    Task<T?> GetCharacteristic<T>(Guid id)
        where T : ICharacteristic;
}
