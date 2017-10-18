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

        protected virtual void ConfigureDeadLetterQueue(Dictionary<string, object> queueArguments, IModel channel)
        {
            if (_configuration.EnableDeadLettering)
            {
                if (string.IsNullOrWhiteSpace(_configuration.QueueName))
                {
                    throw new ArgumentException("Cannot enable deadletter queues on anonymous queues.", nameof(_configuration.QueueName));
                }
                queueArguments["x-dead-letter-exchange"] = $"{_configuration.Exchange}.deadletter";
                queueArguments["x-dead-letter-routing-key"] = _configuration.QueueName;
                SetupDeadLetterQueue(channel);
            }
        }

        

        private void SetupDeadLetterQueue(IModel channel)
        {
            channel.ExchangeDeclare($"{_configuration.Exchange}.deadletter", ExchangeType.Direct);
            channel.QueueDeclare($"{_configuration.QueueName}.deadletter", true, false, false);
            channel.QueueBind(
                queue: $"{_configuration.QueueName}.deadletter",
                exchange: $"{_configuration.Exchange}.deadletter",
                routingKey: _configuration.QueueName
                );
        }
    }
}
