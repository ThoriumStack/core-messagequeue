using MyBucks.Core.MessageQueue.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyBucks.Core.MessageQueue
{
    public class EventQueuePublisher
    {
        public IEventQueuePublisher GetPublisher()
        {
            return new RabbitMqEventPublisher(config => {
                config.Exchange = "events";
                config.ExchangeType = "topic";
            });
        }
    }
}
