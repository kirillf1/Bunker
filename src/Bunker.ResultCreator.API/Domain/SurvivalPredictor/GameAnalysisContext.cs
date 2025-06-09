using System.Text;
using Bunker.ResultCreator.API.Domain.Bunkers;
using Bunker.ResultCreator.API.Domain.Catastrophes;
using Bunker.ResultCreator.API.Domain.Characters;

namespace Bunker.ResultCreator.API.Domain.SurvivalPredictor;

public record GameAnalysisContext(
    Guid GameSessionId,
    BunkerEntity Bunker,
    Catastrophe Catastrophe,
    IEnumerable<Character> Characters
)
{
    public override string ToString()
    {
        var builder = new StringBuilder();

        builder.AppendLine(Catastrophe.ToString());
        builder.AppendLine(Bunker.ToString());

        foreach (var character in Characters)
        {
            builder.AppendLine(character.ToString());
        }

        return builder.ToString();
    }
}
