using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyBucks.Core.MessageQueue.Subscribe
{
    class RabbitEventConsumer : IMessageQueueEventConsumer
    {
        IModel _channel;

        public void Consume<TPayload>(string @event, Action<TPayload> action)
        {
            var connection = RabbitMqConnector.GetConnection();
            _channel = connection.CreateModel();

            _channel.ExchangeDeclare(exchange: "events", type: "topic");
            var queueName = _channel.QueueDeclare().QueueName;


            _channel.QueueBind(queue: queueName,
                              exchange: "events",
                              routingKey: $"{@event}");

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body);
                var routingKey = ea.RoutingKey;
                var returnObject = Jil.JSON.Deserialize<TPayload>(message);
                action(returnObject);

            };
            consumer.Shutdown += Consumer_Shutdown;
            _channel.BasicConsume(queue: queueName,

                                 autoAck: true,
                                 consumer: consumer);
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
