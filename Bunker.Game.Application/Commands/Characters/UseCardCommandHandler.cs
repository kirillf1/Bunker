using Ardalis.Result;
using Bunker.Application.Shared.CQRS;
using Bunker.Game.Domain.AggregateModels.Characters;

namespace Bunker.Game.Application.Commands.Characters;

public class UseCardCommandHandler : ICommandHandler<UseCardCommand, Result<Guid>>
{
    private readonly ICharacterRepository _characterRepository;
    private readonly CharacterService _characterService;

    public UseCardCommandHandler(ICharacterRepository characterRepository, CharacterService characterService)
    {
        _characterRepository = characterRepository;
        _characterService = characterService;
    }

    public async Task<Result<Guid>> Handle(UseCardCommand command, CancellationToken cancellation)
    {
        var character = await _characterRepository.GetCharacter(command.CharacterId);

        if (character is null)
            return Result.NotFound();

        await _characterService.ActivateCharacterCard(character, command.CardId, command.CardParams);

        await _characterRepository.Update(character);
        await _characterRepository.UnitOfWork.SaveChangesAsync(cancellation);

        return Result.Success(command.CardId);
    }
}
