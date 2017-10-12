using System;
using System.Collections.Generic;
using System.Text;
using MyBucks.Core.MessageQueue.Model;

namespace MyBucks.Core.MessageQueue.Subscribe
{
    public class RabbitMqWorkQueueConsumer : RabbitMqConsumerBase, IWorkQueueConsumer
    {
        public RabbitMqWorkQueueConsumer() : base()
        {
            _configuration.Durable = true;
            _configuration.ExchangeType = "direct";
        }

        public void Consume<TPayload>(string exchange, string queue, Func<TPayload, ConsumerResponse> consumerMethod)
        {
            _configuration.Exchange = exchange;
            _configuration.QueueName = queue;
            _configuration.RoutingKey = queue;
            _configuration.EnableDeadLettering = true;
            base.Consume<TPayload>(consumerMethod);
        }
    }
}
