using MyBucks.Core.MessageQueue.Model;
using MyBucks.Core.MessageQueue.Publish;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyBucks.Core.MessageQueue
{
    public class RabbitEventPublisher : RabbitMqPublisherBase, IEventQueuePublisher
    {

        public RabbitEventPublisher(Action<QueueConfiguration> config) : base(null)
        {
            _configuration = new QueueConfiguration();
            _configuration.Durable = false;
            _configuration.AutoDelete = true;
            _configuration.AutoAcknowledge = true;
            config(_configuration);
        }

        public void PublishEvent<T>(string @event, T payload)
        {
            SetRoutingKey(@event);
            Console.WriteLine(_configuration);
            base.Publish(payload);
        }
    }
}
