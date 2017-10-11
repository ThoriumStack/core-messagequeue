using MyBucks.Core.MessageQueue.Model;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyBucks.Core.MessageQueue.Subscribe
{
    class RabbitEventConsumer : ConsumerBase, IMessageQueueEventConsumer
    {
        public RabbitEventConsumer()
        {
            _configuration.AutoAcknowledge = true;
            _configuration.Exchange = "events";
            _configuration.Durable = false;
            _configuration.AutoDelete = true;
            _configuration.ExchangeType = "topic";
        }

        public void Consume<TPayload>(string @event, Action<TPayload> consumerMethod)
        {
            _configuration.RoutingKey = @event;

            base.Consume<TPayload>((payload) => {
                consumerMethod(payload);
                return new ConsumerResponse { ResponseStatus = ConsumerResponseStatus.Acknowledge };
            });
        }

        
    }
}
