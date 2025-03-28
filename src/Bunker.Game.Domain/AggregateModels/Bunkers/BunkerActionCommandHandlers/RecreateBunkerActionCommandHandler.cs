using Bunker.Domain.Shared.CardActionCommands;

namespace Bunker.Game.Domain.AggregateModels.Bunkers.BunkerActionCommandHandlers;

public class RecreateBunkerActionCommandHandler : ICardActionCommandHandler<RecreateBunkerActionCommand>
{
    private readonly IBunkerRepository _bunkerRepository;
    private readonly IBunkerGenerator _bunkerGenerator;

    public RecreateBunkerActionCommandHandler(IBunkerRepository bunkerRepository, IBunkerGenerator bunkerGenerator)
    {
        _bunkerRepository = bunkerRepository;
        _bunkerGenerator = bunkerGenerator;
    }

    public async Task Handle(RecreateBunkerActionCommand command)
    {
        var bunker =
            await _bunkerRepository.GetBunkerByGameSessionId(command.GameSessionId)
            ?? throw new InvalidGameOperationException("Unknown game session to activate card");

        var items = await _bunkerGenerator.GenerateBunkerComponents<Item>(bunker.Items.Count);
        var rooms = await _bunkerGenerator.GenerateBunkerComponents<Room>(bunker.Rooms.Count);
        var environments = await _bunkerGenerator.GenerateBunkerComponents<Environment>(bunker.Environments.Count);
        var description = await _bunkerGenerator.GenerateBunkerDescription();

        bunker.RecreateBunker(description, items, environments, rooms);

        await _bunkerRepository.Update(bunker);
    }
}
