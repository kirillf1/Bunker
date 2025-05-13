using Bunker.Game.Domain.AggregateModels.Characters.Cards;

namespace Bunker.Game.API.Application.Commands.Characters;

public record UseCardCommand(Guid CharacterId, Guid CardId, ActivateCardParams CardParams);
