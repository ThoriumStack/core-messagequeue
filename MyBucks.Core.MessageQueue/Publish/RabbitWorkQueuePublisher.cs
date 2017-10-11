using System;
using System.Collections.Generic;
using System.Text;
using MyBucks.Core.MessageQueue.Model;
using RabbitMQ.Client;

namespace MyBucks.Core.MessageQueue.Publish
{
    public class RabbitPersistentWorkQueuePublisher : PublisherBase
    {
        public RabbitPersistentWorkQueuePublisher(QueueConfiguration configuration) : base(configuration)
        {
            configuration.Durable = true;
        }

        protected override void OnChannelCreate(IModel channel)
        {
            channel.QueueDeclare(_configuration.QueueName, _configuration.Durable);
            channel.QueueBind(_configuration.QueueName, _configuration.Exchange, "");
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
