namespace Bunker.Game.Domain.AggregateModels.GameSessions;

[Flags]
public enum GameState
{
    Preparing = 1,
    Playing = 2,
    WaitingForGameResult = 4,
    Completed = 8,
    Terminated = 16,
}
