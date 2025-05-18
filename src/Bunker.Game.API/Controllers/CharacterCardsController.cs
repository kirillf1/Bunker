using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using Bunker.Application.Shared.CQRS;
using Bunker.Game.API.Models.Characters;
using Bunker.Game.Application.Commands.Characters;
using Bunker.Game.Application.Queries.Characters;
using Bunker.Game.Domain.AggregateModels.Characters.Cards;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Bunker.Game.API.Controllers;

[ApiController]
[Route("api/characters")]
public class CharacterCardsController : ControllerBase
{
    private readonly ICommandHandler<UseCardCommand, Result> _useCardCommandHandler;
    private readonly IQueryHandler<
        GetCardRequirementsQuery,
        Result<CardActionRequirements>
    > _getCardRequirementsQueryHandler;
    private readonly ILogger<CharacterCardsController> _logger;

    public CharacterCardsController(
        ICommandHandler<UseCardCommand, Result> useCardCommandHandler,
        IQueryHandler<GetCardRequirementsQuery, Result<CardActionRequirements>> getCardRequirementsQueryHandler,
        ILogger<CharacterCardsController> logger
    )
    {
        _useCardCommandHandler = useCardCommandHandler;
        _getCardRequirementsQueryHandler = getCardRequirementsQueryHandler;
        _logger = logger;
    }

    [HttpPost("{characterId:guid}/cards/{cardId:guid}/use")]
    [TranslateResultToActionResult]
    public async Task<Result> UseCard(
        Guid characterId,
        Guid cardId,
        [FromBody] UseCardRequest useCardRequest,
        CancellationToken cancellationToken
    )
    {
        using (
            _logger.BeginScope(new Dictionary<string, object> { ["CharacterId"] = characterId, ["CardId"] = cardId })
        )
        {
            var command = new UseCardCommand(
                characterId,
                cardId,
                new ActivateCardParams(useCardRequest?.TargetCharactersId ?? [])
            );

            var result = await _useCardCommandHandler.Handle(command, cancellationToken);

            return result;
        }
    }

    [HttpGet("{characterId:guid}/cards/{cardId:guid}/requirements")]
    [TranslateResultToActionResult]
    public async Task<Result<CardActionRequirements>> GetCardRequirements(
        Guid characterId,
        Guid cardId,
        CancellationToken cancellationToken
    )
    {
        using (
            _logger.BeginScope(new Dictionary<string, object> { ["CharacterId"] = characterId, ["CardId"] = cardId })
        )
        {
            var query = new GetCardRequirementsQuery { CharacterId = characterId, CardId = cardId };

            var result = await _getCardRequirementsQueryHandler.Handle(query, cancellationToken);

            return result;
        }
    }
}
