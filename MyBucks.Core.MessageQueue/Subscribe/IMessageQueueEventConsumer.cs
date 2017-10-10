using System;
using System.Collections.Generic;
using System.Text;

namespace MyBucks.Core.MessageQueue.Subscribe
{
    public interface IMessageQueueEventConsumer
    {
        void Consume<TPayload>(string @event, Action<TPayload> consumerMethod);
        void CloseChannel();
    }
}
