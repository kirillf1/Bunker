namespace Bunker.MessageBus.Abstractions.IntegrationEventLogs
{
    public enum EventState
    {
        NotPublished = 0,
        InProgress = 1,
        Published = 2,
        PublishedFailed = 3,
    }
}
