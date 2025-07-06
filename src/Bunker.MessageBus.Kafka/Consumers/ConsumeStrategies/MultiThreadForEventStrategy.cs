using Bunker.MessageBus.Abstractions;
using Confluent.Kafka;

namespace Bunker.MessageBus.Kafka.Consumers.ConsumeStrategies;

public class MultiThreadForEventStrategy : IConsumeStrategy
{
    private readonly SemaphoreSlim _semaphore;
    private Func<ConsumeResult<string, string>, Type, Task>? _eventsHandler;
    private Func<ConsumeResult<string, string>, Task>? _onProcessedCallback;
    private bool _disposed = false;

    public MultiThreadForEventStrategy(int maxConcurrentThreadsCount = KafkaDefaults.DefaultMaxConcurrentThreadsCount)
    {
        _semaphore = new SemaphoreSlim(maxConcurrentThreadsCount, maxConcurrentThreadsCount);
    }

    ~MultiThreadForEventStrategy()
    {
        Dispose(false);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public async Task HandleEvent(
        ConsumeResult<string, string> consumeResult,
        Type eventType,
        CancellationToken cancellationToken
    )
    {
        if (_eventsHandler is null)
            throw new InvalidOperationException("Event handlers is null");

        await _semaphore.WaitAsync(cancellationToken);

        _ = Task.Run(
            async () =>
            {
                try
                {
                    await _eventsHandler(consumeResult, eventType);

                    if (_onProcessedCallback is not null)
                        await _onProcessedCallback(consumeResult);
                }
                finally
                {
                    _semaphore.Release();
                }
            },
            cancellationToken
        );
    }

    public Task InitializeEventHandlers(
        EventBusSubscriptionInfo eventBusSubscriptionInfo,
        Func<ConsumeResult<string, string>, Type, Task> handler,
        Func<ConsumeResult<string, string>, Task>? onProcessedCallback = null
    )
    {
        _eventsHandler = handler;
        _onProcessedCallback = onProcessedCallback;
        return Task.CompletedTask;
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
            return;

        if (disposing)
        {
            _semaphore?.Dispose();
        }

        _disposed = true;
    }
}
