using System.Collections.Concurrent;
using Bunker.MessageBus.Abstractions;
using Confluent.Kafka;

namespace Bunker.MessageBus.Kafka.Consumers.ConsumeStrategies;

public class SingeThreadWithBufferForEventStrategy : IConsumeStrategy
{
    private readonly int _maxQueueSize;
    private Func<ConsumeResult<string, string>, Type, Task>? _eventsHandler;
    private Func<ConsumeResult<string, string>, Task>? _onProcessedCallback;
    private readonly Dictionary<Type, BlockingCollection<EventHandlerParams>> _eventHandlers;
    readonly CancellationTokenSource _cancellationTokenSource;
    private bool _disposed = false;

    public SingeThreadWithBufferForEventStrategy(int maxQueueSize = KafkaDefaults.DefaultMaxQueueSize)
    {
        _maxQueueSize = maxQueueSize;
        _eventHandlers = [];
        _cancellationTokenSource = new CancellationTokenSource();
    }

    ~SingeThreadWithBufferForEventStrategy()
    {
        Dispose(false);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public Task HandleEvent(
        ConsumeResult<string, string> consumeResult,
        Type eventType,
        CancellationToken cancellationToken
    )
    {
        if (_eventsHandler is null)
            throw new InvalidOperationException("Events handlers is null");

        _cancellationTokenSource.Token.ThrowIfCancellationRequested();

        if (_eventHandlers.TryGetValue(eventType, out var eventQueue))
            eventQueue.Add(new EventHandlerParams(consumeResult, eventType), cancellationToken);
        return Task.CompletedTask;
    }

    public Task InitializeEventHandlers(
        EventBusSubscriptionInfo eventBusSubscriptionInfo,
        Func<ConsumeResult<string, string>, Type, Task> handler,
        Func<ConsumeResult<string, string>, Task>? onProcessedCallback = null
    )
    {
        _eventsHandler = handler;
        _onProcessedCallback = onProcessedCallback;

        foreach (var eventType in eventBusSubscriptionInfo.EventTypes.Values)
        {
            var eventQueue = new BlockingCollection<EventHandlerParams>(_maxQueueSize);
            _eventHandlers.Add(eventType, eventQueue);
            Task.Factory.StartNew(
                async () =>
                {
                    await ProcessHandleEventFromQueue(_cancellationTokenSource.Token, eventQueue);
                },
                TaskCreationOptions.LongRunning
            );
        }
        return Task.CompletedTask;
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
            return;

        if (disposing)
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();

            if (_eventHandlers is not null)
            {
                foreach (var eventQueue in _eventHandlers.Values)
                {
                    eventQueue.Dispose();
                }
                _eventHandlers.Clear();
            }
        }

        _disposed = true;
    }

    private async Task ProcessHandleEventFromQueue(
        CancellationToken cancellationToken,
        BlockingCollection<EventHandlerParams> eventQueue
    )
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            var eventHandlerParams = eventQueue.Take(cancellationToken);

            await _eventsHandler!.Invoke(eventHandlerParams.ConsumeResult, eventHandlerParams.EventType);

            if (_onProcessedCallback is not null)
                await _onProcessedCallback(eventHandlerParams.ConsumeResult);
        }

        // Обрабатываем оставшиеся события при завершении работы
        while (eventQueue.Count > 0)
        {
            var eventHandlerParams = eventQueue.Take();

            await _eventsHandler!.Invoke(eventHandlerParams.ConsumeResult, eventHandlerParams.EventType);

            if (_onProcessedCallback is not null)
                await _onProcessedCallback(eventHandlerParams.ConsumeResult);
        }
    }

    private sealed record EventHandlerParams(ConsumeResult<string, string> ConsumeResult, Type EventType);
}
