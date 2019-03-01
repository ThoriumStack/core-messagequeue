using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using Thorium.Core.MessageQueue.Model;

namespace Thorium.Core.MessageQueue.Subscribe
{
    class RabbitMqEventConsumer : RabbitMqConsumerBase, IEventQueueConsumer
    {
        public RabbitMqEventConsumer()
        {
            _configuration.AutoAcknowledge = true;
            _configuration.Exchange = "events";
            _configuration.Durable = false;
            _configuration.AutoDelete = false;
            _configuration.ExchangeType = "topic";
        }

        public void Consume<TPayload>(string @event, Action<TPayload> consumerMethod)
        {
            _configuration.RoutingKey = @event;
            Console.WriteLine(_configuration);
            base.Consume<TPayload>((payload) => {
                consumerMethod(payload);
                return new ConsumerResponse { ResponseStatus = ConsumerResponseStatus.Acknowledge };
            });
        }
    }
}
