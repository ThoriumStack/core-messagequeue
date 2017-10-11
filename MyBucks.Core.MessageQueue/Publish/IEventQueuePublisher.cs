using System;

namespace MyBucks.Core.MessageQueue
{
    public interface IEventQueuePublisher
    {
        void PublishEvent<T>(string @event, T payload);
    }
}
