using MyBucks.Core.MessageQueue.Model;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyBucks.Core.MessageQueue.Subscribe
{
    public class RabbitMqConsumerBase : RabbitMqBase
    {
        IModel _channel;

        internal RabbitMqConsumerBase()
        {
            _configuration = new QueueConfiguration();
        }

        private string QueueSetup()
        {
            var connection = RabbitMqConnector.GetConnection();
            if (_channel == null)
            {
                _channel = connection.CreateModel();

                _channel.ExchangeDeclare(exchange: _configuration.Exchange, type: _configuration.ExchangeType, durable: _configuration.Durable, autoDelete: _configuration.AutoDelete);

                var queueName = "";

                Dictionary<string, object> queueArguments = new Dictionary<string, object>();
                ConfigureDeadLetterQueue(queueArguments, _channel);

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
                _configuration.QueueName = queueName;
                return queueName;
            }
            return _configuration.QueueName;
        }

        protected void Consume<TPayload>(Func<TPayload, ConsumerResponse> action)
        {
            var queueName = QueueSetup();

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                OnReceived(action, ea);
            };
            consumer.Shutdown += Consumer_Shutdown;
            _channel.BasicConsume(queue: queueName,

                                 autoAck: _configuration.AutoAcknowledge,
                                 consumer: consumer);
        }

        public virtual SimpleQueueMessage<TPayload> GetNextMessage<TPayload>(bool acknowledge)
        {
            var queueName = QueueSetup();
            BasicGetResult ea = _channel.BasicGet(queueName, !acknowledge);
            if (ea == null)
            {
                return null;
            }
            var body = ea.Body;
            var message = Encoding.UTF8.GetString(body);
            var routingKey = ea.RoutingKey;
            var returnObject = Jil.JSON.Deserialize<TPayload>(message);
            return new SimpleQueueMessage<TPayload> { DeliveryTag = ea.DeliveryTag, Payload = returnObject };
            
        }

        public virtual void Acknowledge<TPayload>(SimpleQueueMessage<TPayload> message)
        {
          
            _channel.BasicAck(message.DeliveryTag, false);
        }

        private void OnReceived<TPayload>(Func<TPayload, ConsumerResponse> action, BasicDeliverEventArgs ea)
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
                responseActions[response.ResponseStatus]();
            }
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
