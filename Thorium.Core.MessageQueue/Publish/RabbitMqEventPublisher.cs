using System;
using Thorium.Core.MessageQueue.Model;

namespace Thorium.Core.MessageQueue.Publish
{
    public class RabbitMqEventPublisher : RabbitMqPublisherBase, IEventQueuePublisher
    {

        public RabbitMqEventPublisher(Action<QueueConfiguration> config) : base(null)
        {
            _configuration = new QueueConfiguration();
            _configuration.Durable = false;
            _configuration.AutoDelete = false;
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
