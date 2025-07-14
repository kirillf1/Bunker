using Bunker.Domain.Shared.Cards.CardActionCommands;
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
                firstCharacter.UpdateAdditionalInformation(new AdditionalInformation(secondAdditionalInfo.Description));
                secondCharacter.UpdateAdditionalInformation(new AdditionalInformation(firstAdditionalInfo.Description));
                break;

            case Type t when t == typeof(Health):
                var firstHealth = firstCharacter.Health;
                var secondHealth = secondCharacter.Health;
                firstCharacter.UpdateHealth(new Health(secondHealth.Description));
                secondCharacter.UpdateHealth(new Health(firstHealth.Description));
                break;

            case Type t when t == typeof(Hobby):
                var firstHobby = firstCharacter.Hobby;
                var secondHobby = secondCharacter.Hobby;
                firstCharacter.UpdateHobby(new Hobby(secondHobby.Description, secondHobby.Experience));
                secondCharacter.UpdateHobby(new Hobby(firstHobby.Description, firstHobby.Experience));
                break;

            case Type t when t == typeof(Phobia):
                var firstPhobia = firstCharacter.Phobia;
                var secondPhobia = secondCharacter.Phobia;
                firstCharacter.UpdatePhobia(new Phobia(secondPhobia.Description));
                secondCharacter.UpdatePhobia(new Phobia(firstPhobia.Description));
                break;

            case Type t when t == typeof(Profession):
                var firstProfession = firstCharacter.Profession;
                var secondProfession = secondCharacter.Profession;
                firstCharacter.UpdateProfession(
                    new Profession(secondProfession.Description, secondProfession.ExperienceYears)
                );
                secondCharacter.UpdateProfession(
                    new Profession(firstProfession.Description, firstProfession.ExperienceYears)
                );
                break;

            case Type t when t == typeof(Item):
                var firstCharacterItems = firstCharacter.Items.ToArray();
                var secondCharacterItems = secondCharacter.Items.ToArray();
                firstCharacter.ReplaceItems(secondCharacterItems.Select(x => new Item(x.Description)));
                secondCharacter.ReplaceItems(firstCharacterItems.Select(x => new Item(x.Description)));
                break;

            case Type t when t == typeof(Trait):
                var firstCharacterTraits = firstCharacter.Traits.ToArray();
                var secondCharacterTraits = secondCharacter.Traits.ToArray();
                firstCharacter.ReplaceTraits(secondCharacterTraits.Select(x => new Trait(x.Description)));
                secondCharacter.ReplaceTraits(firstCharacterTraits.Select(x => new Trait(x.Description)));
                break;

            case Type t when t == typeof(Card):
                var firstCharacterCards = firstCharacter.Cards.ToArray();
                var secondCharacterCards = secondCharacter.Cards.ToArray();
                firstCharacter.ReplaceCards(
                    secondCharacterCards.Select(x => new Card(
                        Guid.CreateVersion7(),
                        x.Description,
                        x.CardAction,
                        x.SourceCardId
                    ))
                );
                secondCharacter.ReplaceCards(
                    firstCharacterCards.Select(x => new Card(
                        Guid.CreateVersion7(),
                        x.Description,
                        x.CardAction,
                        x.SourceCardId
                    ))
                );
                break;

            case Type t when t == typeof(Age):
                var firstAge = firstCharacter.Age;
                var secondAge = secondCharacter.Age;
                firstCharacter.UpdateAge(new Age(secondAge.Years));
                secondCharacter.UpdateAge(new Age(firstAge.Years));
                break;

            case Type t when t == typeof(Size):
                var firstSize = firstCharacter.Size;
                var secondSize = secondCharacter.Size;
                firstCharacter.UpdateSize(new Size(secondSize.Height, secondSize.Weight));
                secondCharacter.UpdateSize(new Size(firstSize.Height, firstSize.Weight));
                break;

            case Type t when t == typeof(Sex):
                var firstSex = firstCharacter.Sex;
                var secondSex = secondCharacter.Sex;
                firstCharacter.UpdateSex(new Sex(secondSex.Description));
                secondCharacter.UpdateSex(new Sex(firstSex.Description));
                break;

            case Type t when t == typeof(Childbearing):
                var firstChildbearing = firstCharacter.Childbearing;
                var secondChildbearing = secondCharacter.Childbearing;
                firstCharacter.UpdateChildbearing(new Childbearing(secondChildbearing.CanGiveBirth));
                secondCharacter.UpdateChildbearing(new Childbearing(firstChildbearing.CanGiveBirth));
                break;

            default:
                throw new ArgumentException($"Unknown characteristic type: {characteristicType.Name}");
        }

        await _characterRepository.Update(firstCharacter);
        await _characterRepository.Update(secondCharacter);
    }
}
