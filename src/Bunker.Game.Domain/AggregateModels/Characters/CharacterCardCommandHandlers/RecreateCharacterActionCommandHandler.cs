using Bunker.Domain.Shared.CardActionCommands;
using Bunker.Game.Domain.AggregateModels.Characters.Cards;
using Bunker.Game.Domain.AggregateModels.Characters.Characteristics;

namespace Bunker.Game.Domain.AggregateModels.Characters.CharacterCardCommandHandlers;

public class RecreateCharacterActionCommandHandler : ICardActionCommandHandler<RecreateCharacterActionCommand>
{
    private readonly ICharacterRepository _characterRepository;
    private readonly ICharacteristicGenerator _characteristicGenerator;

    public RecreateCharacterActionCommandHandler(
        ICharacterRepository characterRepository,
        ICharacteristicGenerator characteristicGenerator
    )
    {
        _characterRepository = characterRepository;
        _characteristicGenerator = characteristicGenerator;
    }

    public async Task Handle(RecreateCharacterActionCommand command)
    {
        foreach (var characterId in command.TargetCharactersIds)
        {
            var character = await _characterRepository.GetCharacter(characterId);

            if (character is null || character.IsKicked)
                continue;

            var additionalInformation = await _characteristicGenerator.GenerateCharacteristic<AdditionalInformation>();
            var traits = await _characteristicGenerator.GenerateCharacteristics<Trait>(character.Traits.Count);
            var cards = await _characteristicGenerator.GenerateCharacteristics<Card>(character.Cards.Count);
            var items = await _characteristicGenerator.GenerateCharacteristics<Item>(character.Items.Count);
            var sex = await _characteristicGenerator.GenerateCharacteristic<Sex>();
            var age = await _characteristicGenerator.GenerateCharacteristic<Age>();
            var profession = await _characteristicGenerator.GenerateCharacteristic<Profession>();
            var childbearing = await _characteristicGenerator.GenerateCharacteristic<Childbearing>();
            var health = await _characteristicGenerator.GenerateCharacteristic<Health>();
            var phobia = await _characteristicGenerator.GenerateCharacteristic<Phobia>();
            var hobby = await _characteristicGenerator.GenerateCharacteristic<Hobby>();

            character.RecreateCharacter(
                additionalInformation,
                age,
                childbearing,
                health,
                hobby,
                phobia,
                profession,
                sex,
                items,
                traits,
                cards
            );

            await _characterRepository.Update(character);
        }
    }
}
