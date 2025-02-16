using Bunker.Domain.Shared.CardActionCommands;

namespace Bunker.Game.Domain.AggregateModels.Characters.CharacterCardCommandHandlers;

public class RerollCharacteristicActionCommandHandler : ICardActionCommandHandler<RerollCharacteristicActionCommand>
{
    private readonly ICharacteristicGenerator _characteristicGenerator;
    private readonly ICharacterRepository _characterRepository;

    public RerollCharacteristicActionCommandHandler(
        ICharacteristicGenerator characteristicGenerator,
        ICharacterRepository characterRepository
    )
    {
        _characteristicGenerator = characteristicGenerator;
        _characterRepository = characterRepository;
    }

    public Task Handle(RerollCharacteristicActionCommand command)
    {
        switch (command.CharacteristicType)
        {
            case CharacteristicType.Phobia:
                break;
            case CharacteristicType.Hobby:
                break;
            case CharacteristicType.AdditionalInformation:
                break;
            case CharacteristicType.Health:
                break;
            case CharacteristicType.CharacterItem:
                break;
            case CharacteristicType.Profession:
                break;
            case CharacteristicType.Trait:
                break;
            case CharacteristicType.Card:
                break;
            default:
                break;
        }

        return Task.CompletedTask;
    }
}
