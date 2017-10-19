using MyBucks.Core.MessageQueue.Model;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyBucks.Core.MessageQueue
{
    public class RabbitMqBase
    {
        protected QueueConfiguration _configuration;

        private string DeadLetterExchange => _configuration.DeadLetterExchange ?? $"{ _configuration.Exchange}.deadletter";
        private string DeadLetterRoutingKey => _configuration.DeadLetterRoutingKey ?? _configuration.QueueName;
        private string DeadLetterQueue => _configuration.DeadLetterQueue ?? $"{_configuration.QueueName}.deadletter";

        protected virtual void ConfigureDeadLetterQueue(Dictionary<string, object> queueArguments, IModel channel)
        {
            if (_configuration.EnableDeadLettering)
            {
                if (string.IsNullOrWhiteSpace(_configuration.QueueName))
                {
                    throw new ArgumentException("Cannot enable deadletter queues on anonymous queues.", nameof(_configuration.QueueName));
                }
                queueArguments["x-dead-letter-exchange"] = DeadLetterExchange;
                queueArguments["x-dead-letter-routing-key"] = DeadLetterRoutingKey;
                SetupDeadLetterQueue(channel);
            }
        }

        

        private void SetupDeadLetterQueue(IModel channel)
        {
            channel.ExchangeDeclare(DeadLetterExchange, ExchangeType.Direct);
            channel.QueueDeclare(DeadLetterQueue, true, false, false);
            channel.QueueBind(
                queue: DeadLetterQueue,
                exchange: DeadLetterExchange,
                routingKey: DeadLetterRoutingKey
                );
        }
    }
}
