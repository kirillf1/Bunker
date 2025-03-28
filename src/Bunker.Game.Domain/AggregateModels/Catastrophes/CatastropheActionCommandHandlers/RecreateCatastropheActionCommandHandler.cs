using Bunker.Domain.Shared.CardActionCommands;

namespace Bunker.Game.Domain.AggregateModels.Catastrophes.CatastropheActionCommandHandlers;

public class RecreateCatastropheActionCommandHandler : ICardActionCommandHandler<RecreateCatastropheActionCommand>
{
    private readonly ICatastropheRepository _catastropheRepository;
    private readonly ICatastropheGenerator _catastropheGenerator;

    public RecreateCatastropheActionCommandHandler(
        ICatastropheRepository catastropheRepository,
        ICatastropheGenerator catastropheGenerator
    )
    {
        _catastropheRepository = catastropheRepository;
        _catastropheGenerator = catastropheGenerator;
    }

    public async Task Handle(RecreateCatastropheActionCommand command)
    {
        var catastrophe =
            await _catastropheRepository.GetCatastropheByGameSession(command.GameSessionId)
            ?? throw new InvalidGameOperationException("Unknown game session to activate card");

        var description = await _catastropheGenerator.GenerateDescription(command.GameSessionId);

        catastrophe.UpdateDescription(description);

        await _catastropheRepository.Update(catastrophe);
    }
}
