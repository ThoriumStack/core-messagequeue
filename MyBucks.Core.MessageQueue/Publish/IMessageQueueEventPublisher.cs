using System;

namespace MyBucks.Core.MessageQueue
{
    public interface IMessageQueueEventPublisher
    {
        void PublishEvent<T>(string @event, T payload);
    }
}
