using Bunker.Domain.Shared.Cards.CardActionCommands;

namespace Bunker.Game.Domain.AggregateModels.Bunkers.BunkerActionCommandHandlers;

public class RevealBunkerComponentCommandHandler : ICardActionCommandHandler<RevealBunkerComponentActionCommand>
{
    private readonly IBunkerRepository _bunkerRepository;

    public RevealBunkerComponentCommandHandler(IBunkerRepository bunkerRepository)
    {
        _bunkerRepository = bunkerRepository;
    }

    public async Task Handle(RevealBunkerComponentActionCommand command)
    {
        var bunker =
            await _bunkerRepository.GetBunkerByGameSessionId(command.GameSessionId)
            ?? throw new InvalidGameOperationException("Unknown game session to activate card");

        switch (command.BunkerObjectType)
        {
            case BunkerObjectType.BunkerRoom:
                bunker.RevealRandomRoom();
                break;
            case BunkerObjectType.BunkerEnvironment:
                bunker.RevealRandomEnvironment();
                break;
            case BunkerObjectType.BunkerItem:
                bunker.RevealRandomItem();
                break;
            default:
                throw new NotImplementedException("Unknown bunker object type");
        }

        await _bunkerRepository.Update(bunker);
    }
}
