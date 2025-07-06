using System.Diagnostics;
using Bunker.Application.Shared.CQRS;
using Microsoft.Extensions.Logging;

namespace Bunker.Infrastructure.Shared.ApplicationDecorators;

public class ActivityCommandDecorator<TCommand, TCommandResult> : ICommandHandler<TCommand, TCommandResult>
{
    private readonly ICommandHandler<TCommand, TCommandResult> _decorated;
    private readonly ILogger<ActivityCommandDecorator<TCommand, TCommandResult>> _logger;

    private const string ActivitySourceName = ActivitySourceConstants.CommandsActivitySourceName;

    private static readonly ActivitySource ActivitySource = new(ActivitySourceName);

    public ActivityCommandDecorator(
        ICommandHandler<TCommand, TCommandResult> decorated,
        ILogger<ActivityCommandDecorator<TCommand, TCommandResult>> logger
    )
    {
        _decorated = decorated;
        _logger = logger;
    }

    public async Task<TCommandResult> Handle(TCommand command, CancellationToken cancellationToken)
    {
        var commandName = typeof(TCommand).Name;
        var handlerName = _decorated.GetType().Name;

        using var activity = ActivitySource.StartActivity($"Command.{commandName}");

        activity?.SetTag("command.type", commandName);
        activity?.SetTag("command.handler", handlerName);

        if (command != null)
            activity?.SetTag("command.data", System.Text.Json.JsonSerializer.Serialize(command));

        try
        {
            var result = await _decorated.Handle(command, cancellationToken);

            _logger.LogInformation(
                "Command {CommandName} handled successfully by {HandlerName}",
                commandName,
                handlerName
            );

            activity?.SetTag("command.success", true);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error handling command {CommandName} by {HandlerName}: {ErrorMessage}",
                commandName,
                handlerName,
                ex.Message
            );

            activity?.SetTag("command.success", false);
            activity?.SetTag("command.error", ex.Message);
            activity?.SetTag("command.error.type", ex.GetType().Name);

            activity?.SetStatus(ActivityStatusCode.Error, ex.Message);

            throw;
        }
    }
}
