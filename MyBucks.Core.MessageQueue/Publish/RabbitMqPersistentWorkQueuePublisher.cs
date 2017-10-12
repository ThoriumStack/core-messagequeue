using System;
using System.Collections.Generic;
using System.Text;
using MyBucks.Core.MessageQueue.Model;
using RabbitMQ.Client;

namespace MyBucks.Core.MessageQueue.Publish
{
    public class RabbitMqPersistentWorkQueuePublisher : RabbitMqPublisherBase, IWorkQueuePublisher
    {
        protected RabbitMqPersistentWorkQueuePublisher(QueueConfiguration configuration) : base(configuration)
        {
            configuration.Durable = true;
            configuration.ExchangeType = "direct";
            _configuration.AutoAcknowledge = false;
            _configuration.EnableDeadLettering = true;
        }

        public RabbitMqPersistentWorkQueuePublisher() : base()
        {
            _configuration.Durable = true;
            _configuration.ExchangeType = "direct";
            _configuration.AutoAcknowledge = false;
            _configuration.EnableDeadLettering = true;
        }

        public void PublishMessage<T>(string exchange, string queue, WorkQueueMessage<T> payload)
        {
            _configuration.Exchange = exchange;
            _configuration.QueueName = queue;
            _configuration.RoutingKey = queue;
            base.Publish(payload);
        }

        protected override void OnChannelCreate(IModel channel)
        {


            var queueParms = new Dictionary<string, object>();

            ConfigureDeadLetterQueue(queueParms, channel);

            channel.QueueDeclare(_configuration.QueueName, _configuration.Durable, exclusive: false, autoDelete: _configuration.AutoDelete, arguments: queueParms);

            channel.QueueBind(_configuration.QueueName, _configuration.Exchange, _configuration.RoutingKey);
            channel.ConfirmSelect();
        }

        protected override void OnPublish(IModel channel)
        {
            channel.WaitForConfirmsOrDie();
        }

        protected override void SetMessageProperties(IBasicProperties properties)
        {
            properties.Persistent = true;
        }
    }
}
