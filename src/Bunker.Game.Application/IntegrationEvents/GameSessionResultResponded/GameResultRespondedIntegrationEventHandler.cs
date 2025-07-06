using Ardalis.Result;
using Bunker.Application.Shared.Contracts.IntegrationEvents.GameResults;
using Bunker.Application.Shared.CQRS;
using Bunker.Game.Application.Commands.GameSessions.UpdateGameSessionState;
using Bunker.MessageBus.Abstractions;

namespace Bunker.Game.Application.IntegrationEvents.GameSessionResultResponded;

public class GameResultRespondedIntegrationEventHandler : IIntegrationEventHandler<GameResultRespondedIntegrationEvent>
{
    private readonly ICommandHandler<EndGameSessionCommand, Result> _endGameSessionCommandHandler;
    private readonly ILogger<GameResultRespondedIntegrationEventHandler> _logger;

    public GameResultRespondedIntegrationEventHandler(
        ICommandHandler<EndGameSessionCommand, Result> endGameSessionCommandHandler,
        ILogger<GameResultRespondedIntegrationEventHandler> logger
    )
    {
        _endGameSessionCommandHandler = endGameSessionCommandHandler;
        _logger = logger;
    }

    public async Task Handle(GameResultRespondedIntegrationEvent @event)
    {
        _logger.LogInformation(
            "Processing GameResultRespondedIntegrationEvent for GameSession {GameSessionId}",
            @event.GameSessionId
        );

        var command = new EndGameSessionCommand(@event.GameSessionId, @event.GameResultDescription);

        var result = await _endGameSessionCommandHandler.Handle(command, CancellationToken.None);

        if (result.IsSuccess || result.IsNotFound() || result.IsNoContent())
        {
            _logger.LogInformation("Successfully ended game session {GameSessionId} with result", @event.GameSessionId);
        }
        else
        {
            _logger.LogError(
                "Failed to end game session {GameSessionId}: {Error}",
                @event.GameSessionId,
                string.Join(", ", result.Errors)
            );
        }
    }
}
