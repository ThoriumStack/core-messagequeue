namespace Thorium.Core.MessageQueue.Publish
{
    public interface IEventQueuePublisher
    {
        void PublishEvent<T>(string @event, T payload);
    }
}
