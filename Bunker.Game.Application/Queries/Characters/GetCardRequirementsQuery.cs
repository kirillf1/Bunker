namespace Bunker.Game.Application.Queries.Characters;

public class GetCardRequirementsQuery
{
    public Guid CharacterId { get; set; }
    public Guid CardId { get; set; }
}
