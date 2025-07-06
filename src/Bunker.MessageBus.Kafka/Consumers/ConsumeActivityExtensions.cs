using System.Diagnostics;
using System.Text;
using Confluent.Kafka;

namespace Bunker.MessageBus.Kafka.Consumers;

internal static class ConsumeActivityExtensions
{
    private const string ActivitySourceName = "Confluent.Kafka.Extensions.Diagnostics";
    private const string TraceParentHeaderName = "traceparent";
    private const string TraceStateHeaderName = "tracestate";

    private static ActivitySource _activitySource = new(ActivitySourceName);

    internal static Activity? StartConsumeActivity<TKey, TValue>(ConsumeResult<TKey, TValue> consumerResult)
    {
        try
        {
            var message = consumerResult.Message;
            var activity = _activitySource.CreateActivity(
                $"{consumerResult.Topic} process",
                ActivityKind.Consumer,
                default(ActivityContext),
                ConsumerActivityTags(consumerResult)
            );

            if (activity is not null)
            {
                var traceParentHeader = message.Headers?.FirstOrDefault(x => x.Key == TraceParentHeaderName);
                var traceStateHeader = message.Headers?.FirstOrDefault(x => x.Key == TraceStateHeaderName);

                var traceParent = traceParentHeader is null
                    ? null
                    : Encoding.UTF8.GetString(traceParentHeader.GetValueBytes());
                var traceState = traceStateHeader is null
                    ? null
                    : Encoding.UTF8.GetString(traceStateHeader.GetValueBytes());

                if (ActivityContext.TryParse(traceParent, traceState, out var activityContext))
                {
                    activity.SetParentId(activityContext.TraceId, activityContext.SpanId, activityContext.TraceFlags);
                    activity.TraceStateString = activityContext.TraceState;
                }

                if (activity.IsAllDataRequested)
                    SetActivityTags(activity, message);

                activity.Start();
            }

            return activity;
        }
        catch
        {
            // ignore
            return null;
        }
    }

    private static void SetActivityTags<TKey, TValue>(Activity activity, Message<TKey, TValue> message)
    {
        if (message.Key is not null)
            activity.SetTag("messaging.kafka.message.key", message.Key.ToString());
    }

    private static IEnumerable<KeyValuePair<string, object?>> ConsumerActivityTags<TKey, TValue>(
        ConsumeResult<TKey, TValue> consumerResult
    )
    {
        var list = ActivityTags("process");

        list.Add(new("messaging.source.kind", "topic"));
        list.Add(new("messaging.source.name", consumerResult.Topic));
        list.Add(new("messaging.kafka.source.partition", consumerResult.Partition.Value.ToString()));
        list.Add(new("messaging.kafka.message.offset", consumerResult.Offset.Value.ToString()));

        return list;
    }

    private static IList<KeyValuePair<string, object?>> ActivityTags(string operation)
    {
        return new List<KeyValuePair<string, object?>>()
        {
            new("messaging.system", "kafka"),
            new("messaging.operation", operation),
        };
    }
}
