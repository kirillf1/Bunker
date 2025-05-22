using Ardalis.Result;
using Bunker.Application.Shared.CQRS;
using Bunker.Game.Domain.AggregateModels.Characters;
using Microsoft.Extensions.Logging;

namespace Bunker.Game.Application.Commands.Characters;

public class UseCardCommandHandler : ICommandHandler<UseCardCommand, Result<Guid>>
{
    private readonly ICharacterRepository _characterRepository;
    private readonly CharacterService _characterService;
    private readonly ILogger<UseCardCommandHandler> _logger;

    public UseCardCommandHandler(
        ICharacterRepository characterRepository,
        CharacterService characterService,
        ILogger<UseCardCommandHandler> logger
    )
    {
        _characterRepository = characterRepository;
        _characterService = characterService;
        _logger = logger;
    }

    public async Task<Result<Guid>> Handle(UseCardCommand command, CancellationToken cancellation)
    {
        var character = await _characterRepository.GetCharacter(command.CharacterId);

        if (character is null)
        {
            _logger.LogWarning(
                "Character {CharacterId} not found when attempting to use card {CardId}",
                command.CharacterId,
                command.CardId
            );
            return Result.NotFound();
        }

        try
        {
            _logger.LogInformation(
                "Activating card {CardId} for character {CharacterId}",
                command.CardId,
                command.CharacterId
            );

            await _characterService.ActivateCharacterCard(character, command.CardId, command.CardParams);

            await _characterRepository.Update(character);
            await _characterRepository.UnitOfWork.SaveChangesAsync(cancellation);

            var targetCount = command.CardParams?.TargetCharacterIds?.Count() ?? 0;

            _logger.LogInformation(
                "Card action completed successfully. Character {CharacterId} used card {CardId}. Number of targets: {TargetCount}",
                character.Id,
                command.CardId,
                targetCount
            );

            return Result.Success(command.CardId);
        }
        catch (InvalidGameOperationException ex)
        {
            _logger.LogError(
                ex,
                "Failed to use card {CardId} for character {CharacterId}: {ErrorMessage}",
                command.CardId,
                command.CharacterId,
                ex.Message
            );
            return Result.Invalid(new ValidationError(ex.Message));
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(
                ex,
                "Failed to use card {CardId} for character {CharacterId}: {ErrorMessage}",
                command.CardId,
                command.CharacterId,
                ex.Message
            );
            return Result.Invalid(new ValidationError(ex.Message));
        }
    }
}
