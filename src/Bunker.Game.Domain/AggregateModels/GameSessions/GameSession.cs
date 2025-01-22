using Bunker.Game.Domain.AggregateModels.GameSessions.Characters;

namespace Bunker.Game.Domain.AggregateModels.GameSessions;

public class GameSession : Entity<Guid>, IAggregateRoot
{
    public const byte MaxCharactersInGame = 12;

    public const byte MinCharactersInGame = 5;

    private List<Character> _characters;

    public Bunkers.Bunker Bunker { get; private set; }

    public Catastrophe Catastrophe { get; private set; }

    public IReadOnlyCollection<Character> Characters => _characters;

    public GameState GameState { get; private set; }

    public string Name { get; private set; }

    public byte FreeSeatsCount { get; private set; }

    public void ChangeCharacteristic()
    {

    }

    public void ChangeBunkerComponent()
    {

    }


    private GameSession(Guid id) : base(id)
    {
    }
}
