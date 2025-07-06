using Bunker.ResultCreator.API.Domain.Bunkers;
using Bunker.ResultCreator.API.Domain.Catastrophes;
using Bunker.ResultCreator.API.Domain.Characters;

namespace Bunker.ResultCreator.API.Application.Commands.CreateGameResult;

public record CreateGameResultCommand(
    Guid GameSessionId,
    BunkerEntity Bunker,
    Catastrophe Catastrophe,
    IEnumerable<Character> Characters
); 