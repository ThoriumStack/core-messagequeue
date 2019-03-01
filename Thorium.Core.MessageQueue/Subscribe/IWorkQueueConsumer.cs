using System;
using Thorium.Core.MessageQueue.Model;

namespace Thorium.Core.MessageQueue.Subscribe
{
    public interface IWorkQueueConsumer
    {
        Guid ConsumerId { get; set; }
        void CloseChannel();
        void Consume<TPayload>(string exchange, string queue, Func<TPayload, ConsumerResponse> consumerMethod);
        SimpleQueueMessage<TPayload> GetNextMessage<TPayload>(bool acknowledge);
        void Acknowledge<TPayload>(SimpleQueueMessage<TPayload> message);
    }
}
