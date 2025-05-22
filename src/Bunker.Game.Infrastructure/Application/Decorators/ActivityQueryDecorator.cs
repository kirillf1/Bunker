using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace Bunker.Game.Infrastructure.Application.Decorators;

public class ActivityQueryDecorator<TQuery, TQueryResult> : IQueryHandler<TQuery, TQueryResult>
{
    private readonly IQueryHandler<TQuery, TQueryResult> _decorated;
    private readonly ILogger<ActivityQueryDecorator<TQuery, TQueryResult>> _logger;

    private const string ActivitySourceName = ActivitySourceConstants.QueriesActivitySourceName;

    private static readonly ActivitySource ActivitySource = new(ActivitySourceName);

    public ActivityQueryDecorator(
        IQueryHandler<TQuery, TQueryResult> decorated,
        ILogger<ActivityQueryDecorator<TQuery, TQueryResult>> logger
    )
    {
        _decorated = decorated;
        _logger = logger;
    }

    public async Task<TQueryResult> Handle(TQuery query, CancellationToken cancellationToken)
    {
        var queryName = typeof(TQuery).Name;
        var handlerName = _decorated.GetType().Name;

        using var activity = ActivitySource.StartActivity($"Query.{queryName}");

        activity?.SetTag("query.type", queryName);
        activity?.SetTag("query.handler", handlerName);

        try
        {
            var result = await _decorated.Handle(query, cancellationToken);

            _logger.LogInformation("Query {QueryName} handled successfully by {HandlerName}", queryName, handlerName);

            activity?.SetTag("query.success", true);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error handling query {QueryName} by {HandlerName}: {ErrorMessage}",
                queryName,
                handlerName,
                ex.Message
            );

            activity?.SetTag("query.success", false);
            activity?.SetTag("query.error", ex.Message);
            activity?.SetTag("query.error.type", ex.GetType().Name);

            activity?.SetStatus(ActivityStatusCode.Error, ex.Message);

            throw;
        }
    }
}
