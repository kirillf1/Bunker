using Bunker.Domain.Shared.CardActionCommands;

namespace Bunker.Game.Domain.AggregateModels.Characters.CharacterCardCommandHandlers;

public class SpyCharacteristicActionCommandHandler : ICardActionCommandHandler<SpyCharacteristicActionCommand>
{
    private readonly ICharacterRepository _characterRepository;

    public SpyCharacteristicActionCommandHandler(ICharacterRepository characterRepository)
    {
        _characterRepository = characterRepository;
    }

    public async Task Handle(SpyCharacteristicActionCommand command)
    {
        foreach (var characterId in command.TargetCharactersIds)
        {
            var character = await _characterRepository.GetCharacter(characterId);

            if (character is null)
                continue;

            character.RevealCharacteristics(command.CharacteristicType);

            await _characterRepository.Update(character);
        }
    }
}
