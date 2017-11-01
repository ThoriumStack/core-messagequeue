using MyBucks.Core.MessageQueue.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyBucks.Core.MessageQueue.Subscribe
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
