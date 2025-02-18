namespace Bunker.Game.Domain.AggregateModels.Characters;

public interface ICharacteristicGenerator
{
    Task<T> GenerateCharacteristic<T>()
        where T : ICharacteristic;

    Task<ICharacteristic> GenerateCharacteristic(Type characteristicType);

    Task<IEnumerable<T>> GenerateCharacteristics<T>(int count)
        where T : ICharacteristic;

    Task<IEnumerable<ICharacteristic>> GenerateCharacteristics(int count, Type characteristicType);

    Task<T?> GetCharacteristic<T>(Guid id)
        where T : ICharacteristic;

    Task<ICharacteristic?> GetCharacteristic(Guid id, Type characteristicType);
}
