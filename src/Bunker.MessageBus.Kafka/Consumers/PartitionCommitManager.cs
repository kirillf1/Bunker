using System.Collections.Concurrent;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;

namespace Bunker.MessageBus.Kafka.Consumers;

internal class PartitionCommitManager
{
    private readonly ConcurrentDictionary<long, ConsumeResult<string, string>> _pendingCommits = new();
    private readonly ILogger<PartitionCommitManager> _logger;
    private readonly string _partitionKey;
    private long _lastCommittedOffset = KafkaDefaults.InitialOffset;
    private readonly object _lock = new();

    public PartitionCommitManager(string partitionKey, ILogger<PartitionCommitManager> logger)
    {
        _partitionKey = partitionKey;
        _logger = logger;
    }

    public void InitializeLastCommittedOffset(long offset)
    {
        lock (_lock)
        {
            _lastCommittedOffset = offset;
            _logger.LogInformation(
                "Initialized last committed offset for {PartitionKey}: {Offset}",
                _partitionKey,
                offset
            );
        }
    }

    public Task<bool> TryCommitInOrder(
        ConsumeResult<string, string> consumeResult,
        IConsumer<string, string> consumer,
        CancellationToken cancellationToken
    )
    {
        var offset = consumeResult.Offset.Value;
        var committed = false;

        lock (_lock)
        {
            _pendingCommits[offset] = consumeResult;

            // Коммитим только непрерывную последовательность
            while (
                _pendingCommits.ContainsKey(_lastCommittedOffset + KafkaDefaults.MetricsIncrement)
                && !cancellationToken.IsCancellationRequested
            )
            {
                var nextOffset = _lastCommittedOffset + KafkaDefaults.MetricsIncrement;
                var nextResult = _pendingCommits[nextOffset];

                try
                {
                    consumer.Commit(nextResult);
                    _lastCommittedOffset = nextOffset;
                    _pendingCommits.TryRemove(nextOffset, out _);
                    committed = true;

                    _logger.LogDebug(
                        "Committed offset {Offset} in order for {PartitionKey}",
                        nextOffset,
                        _partitionKey
                    );
                }
                catch (Exception exception)
                {
                    _logger.LogError(
                        exception,
                        "Failed to commit offset {Offset} for {PartitionKey}",
                        nextOffset,
                        _partitionKey
                    );
                    break;
                }
            }
        }

        return Task.FromResult(committed);
    }

    public int GetPendingCommitsCount()
    {
        lock (_lock)
        {
            return _pendingCommits.Count;
        }
    }

    public void Clear()
    {
        lock (_lock)
        {
            _pendingCommits.Clear();
            _lastCommittedOffset = KafkaDefaults.InitialOffset;
        }
    }
}
