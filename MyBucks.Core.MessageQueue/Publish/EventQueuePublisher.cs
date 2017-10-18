using MyBucks.Core.MessageQueue.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyBucks.Core.MessageQueue
{
    public class EventQueuePublisher
    {
        public IEventQueuePublisher GetPublisher(string hostname, string username, string password)
        {
            RabbitMqConnector.SetConnectionSettings(hostname, username, password);
            return new RabbitMqEventPublisher(config => {
                config.Exchange = "events";
                config.ExchangeType = "topic";
            });
        }
    }
}
