using System;

namespace Thorium.Core.MessageQueue.Subscribe
{
    public interface IEventQueueConsumer
    {
        void Consume<TPayload>(string @event, Action<TPayload> consumerMethod);
        void CloseChannel();
    }
}
