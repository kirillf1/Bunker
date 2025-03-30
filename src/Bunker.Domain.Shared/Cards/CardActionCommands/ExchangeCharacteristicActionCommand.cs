using Bunker.Domain.Shared.GameComponents;

namespace Bunker.Domain.Shared.Cards.CardActionCommands;

public class ExchangeCharacteristicActionCommand : CardActionCommand
{
    public CharacteristicType CharacteristicType { get; }

    public Guid CharacterFirst { get; }

    public Guid CharacterSecond { get; }

    public ExchangeCharacteristicActionCommand(CharacteristicType characteristicType, IEnumerable<Guid> charactersIds)
    {
        CharacteristicType = characteristicType;

        if (charactersIds.Count() != 2)
            throw new ArgumentException("Character count must be 2");

        CharacterFirst = charactersIds.First();
        CharacterSecond = charactersIds.Last();
    }
}
