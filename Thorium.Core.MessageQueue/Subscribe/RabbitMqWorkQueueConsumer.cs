using System;
using System.Collections.Generic;
using System.Text;
using Thorium.Core.MessageQueue.Model;

namespace Thorium.Core.MessageQueue.Subscribe
{
    public class RabbitMqWorkQueueConsumer : RabbitMqConsumerBase, IWorkQueueConsumer
    {
        public Guid ConsumerId { get; set; } = Guid.NewGuid();

        public RabbitMqWorkQueueConsumer() : base()
        {
            _configuration.Durable = true;
            _configuration.ExchangeType = "direct";
        }

        public void SetConsumerConfig(string exchange, string queue)
        {
            _configuration.Exchange = exchange;
            _configuration.QueueName = queue;
            _configuration.RoutingKey = queue;
            _configuration.EnableDeadLettering = true;
        }

        public void Consume<TPayload>(string exchange, string queue, Func<TPayload, ConsumerResponse> consumerMethod)
        {
            SetConsumerConfig(exchange, queue);
            base.Consume<TPayload>(consumerMethod);
        }

        public override SimpleQueueMessage<TPayload> GetNextMessage<TPayload>(bool acknowledge)
        {
            
            return base.GetNextMessage<TPayload>(acknowledge);
        }


    }
}
