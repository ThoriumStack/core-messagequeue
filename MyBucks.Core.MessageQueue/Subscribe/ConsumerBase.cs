using MyBucks.Core.MessageQueue.Model;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyBucks.Core.MessageQueue.Subscribe
{
    public class ConsumerBase
    {
        IModel _channel;

        protected QueueConfiguration _configuration;

        public void Consume<TPayload>(Func<TPayload, ConsumerResponse> action)
        {
            var connection = RabbitMqConnector.GetConnection();
            _channel = connection.CreateModel();

            _channel.ExchangeDeclare(exchange: _configuration.Exchange, type: _configuration.ExchangeType, durable: _configuration.Durable, autoDelete: _configuration.AutoDelete);

            var queueName = "";

            Dictionary<string, object> queueArguments = new Dictionary<string, object>();

            if (_configuration.EnableDeadLettering)
            {
                if (string.IsNullOrWhiteSpace(_configuration.QueueName))
                {
                    throw new ArgumentException("Cannot enable deadletter queues on anonymous queues.", nameof(_configuration.QueueName));
                }
                queueArguments["x-dead-letter-exchange"] = $"{_configuration.Exchange}.deadletter";
                queueArguments["x-dead-letter-routing-key"] =  _configuration.QueueName;
                SetupDeadLetterQueue();
            }

            if (string.IsNullOrWhiteSpace(_configuration.QueueName))
            {
                queueName = _channel.QueueDeclare(durable: _configuration.Durable, exclusive: false, autoDelete: _configuration.AutoDelete, arguments: queueArguments).QueueName;
            }
            else
            {
                queueName = _channel.QueueDeclare(queue: _configuration.QueueName, durable: _configuration.Durable, exclusive: false, autoDelete: _configuration.AutoDelete, arguments: queueArguments);
            }

            _channel.QueueBind(queue: queueName,
                              exchange: _configuration.Exchange,
                              routingKey: _configuration.RoutingKey);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body);
                var routingKey = ea.RoutingKey;
                var returnObject = Jil.JSON.Deserialize<TPayload>(message);
                var response = action(returnObject);
                if (!_configuration.AutoAcknowledge)
                {
                    var responseActions = new Dictionary<ConsumerResponseStatus, Action>
                    {
                        [ConsumerResponseStatus.Acknowledge] = () => _channel.BasicAck(ea.DeliveryTag, false),
                        [ConsumerResponseStatus.Requeue] = () => _channel.BasicNack(ea.DeliveryTag, false, true),
                        [ConsumerResponseStatus.Reject] = () => _channel.BasicNack(ea.DeliveryTag, false, true),
                        [ConsumerResponseStatus.DiscardWithError] = () => _channel.BasicReject(ea.DeliveryTag, false)
                    };
                }

            };
            consumer.Shutdown += Consumer_Shutdown;
            _channel.BasicConsume(queue: queueName,

                                 autoAck: _configuration.AutoAcknowledge,
                                 consumer: consumer);
        }

        private void SetupDeadLetterQueue()
        {
            _channel.ExchangeDeclare($"{_configuration.Exchange}.deadletter", ExchangeType.Direct);
            _channel.QueueDeclare($"{_configuration.QueueName}.deadletters", true, false, false);
            _channel.QueueBind(
                queue: $"{_configuration.QueueName}.deadletter",
                exchange: $"{_configuration.Exchange}.deadletter",
                routingKey: _configuration.QueueName
                );

        }

        public void CloseChannel()
        {
            _channel.Close();
        }

        private void Consumer_Shutdown(object sender, ShutdownEventArgs e)
        {
            if (e.ReplyCode != 200)
            {
                throw new Exception($"Consumer shutdown: ({e.ReplyCode}){e.ReplyText}");
            }
        }
    }
}
