using MyBucks.Core.MessageQueue.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyBucks.Core.MessageQueue
{
    public class EventPublisher
    {
        public IMessageQueueEventPublisher GetPublisher()
        {
            return new RabbitEventPublisher(config => {
                config.Exchange = "events";
                config.ExchangeType = "topic";
            });
        }
    }
}
