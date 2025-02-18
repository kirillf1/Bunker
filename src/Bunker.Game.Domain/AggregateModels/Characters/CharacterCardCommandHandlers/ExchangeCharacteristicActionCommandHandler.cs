using Bunker.Domain.Shared.CardActionCommands;
using Bunker.Game.Domain.AggregateModels.Characters.Cards;
using Bunker.Game.Domain.AggregateModels.Characters.Characteristics;
using Bunker.Game.Domain.AggregateModels.Characters.Extensions;

namespace Bunker.Game.Domain.AggregateModels.Characters.CharacterCardCommandHandlers;

public class ExchangeCharacteristicActionCommandHandler : ICardActionCommandHandler<ExchangeCharacteristicActionCommand>
{
    private readonly ICharacterRepository _characterRepository;

    public ExchangeCharacteristicActionCommandHandler(ICharacterRepository characterRepository)
    {
        _characterRepository = characterRepository;
    }

    public async Task Handle(ExchangeCharacteristicActionCommand command)
    {
        var firstCharacter = await _characterRepository.GetCharacter(command.CharacterFirst);
        var secondCharacter = await _characterRepository.GetCharacter(command.CharacterSecond);

        if (firstCharacter is null || secondCharacter is null)
        {
            throw new ArgumentException("Characters not found", nameof(command));
        }

        var characteristicType = command.CharacteristicType.ToType();

        switch (characteristicType)
        {
            case Type t when t == typeof(AdditionalInformation):
                var firstAdditionalInfo = firstCharacter.AdditionalInformation;
                var secondAdditionalInfo = secondCharacter.AdditionalInformation;
                firstCharacter.UpdateAdditionalInformation(secondAdditionalInfo);
                secondCharacter.UpdateAdditionalInformation(firstAdditionalInfo);
                break;
            case Type t when t == typeof(Health):
                var firstHealth = firstCharacter.Health;
                var secondHealth = secondCharacter.Health;
                firstCharacter.UpdateHealth(secondHealth);
                secondCharacter.UpdateHealth(firstHealth);
                break;
            case Type t when t == typeof(Hobby):
                var firstHobby = firstCharacter.Hobby;
                var secondHobby = secondCharacter.Hobby;
                firstCharacter.UpdateHobby(secondHobby);
                secondCharacter.UpdateHobby(firstHobby);
                break;
            case Type t when t == typeof(Phobia):
                var firstPhobia = firstCharacter.Phobia;
                var secondPhobia = secondCharacter.Phobia;
                firstCharacter.UpdatePhobia(secondPhobia);
                secondCharacter.UpdatePhobia(firstPhobia);
                break;
            case Type t when t == typeof(Profession):
                var firstProfession = firstCharacter.Profession;
                var secondProfession = secondCharacter.Profession;
                firstCharacter.UpdateProfession(secondProfession);
                secondCharacter.UpdateProfession(firstProfession);
                break;
            case Type t when t == typeof(Item):
                var firstCharacterItems = firstCharacter.Items.ToArray();
                var secondCharacterItems = secondCharacter.Items.ToArray();
                firstCharacter.ReplaceItems(secondCharacterItems);
                secondCharacter.ReplaceItems(firstCharacterItems);
                break;
            case Type t when t == typeof(Trait):
                var firstCharacterTraits = firstCharacter.Traits.ToArray();
                var secondCharacterTraits = secondCharacter.Traits.ToArray();
                firstCharacter.ReplaceTraits(secondCharacterTraits);
                secondCharacter.ReplaceTraits(firstCharacterTraits);
                break;
            case Type t when t == typeof(Card):
                var firstCharacterCards = firstCharacter.Cards.ToArray();
                var secondCharacterCards = secondCharacter.Cards.ToArray();
                firstCharacter.ReplaceCards(secondCharacterCards);
                secondCharacter.ReplaceCards(firstCharacterCards);
                break;
            default:
                throw new ArgumentException($"Unknown characteristic type: {characteristicType.Name}");
        }
    }
}
