using MyBucks.Core.MessageQueue.Model;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyBucks.Core.MessageQueue.Publish
{
    public abstract class PublisherBase
    {
        private Object _publishLock = new Object();
        protected QueueConfiguration _configuration;

        public PublisherBase(QueueConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void SetRoutingKey(string key)
        {
            _configuration.RoutingKey = key;
        }

        protected virtual void OnChannelCreate(IModel channel) { }
        protected virtual void OnPublish(IModel channel) { }
        protected virtual void SetMessageProperties(IBasicProperties properties) { }

        public void Publish<TPayload>(TPayload payload)
        {
            lock (_publishLock)
            {

                var connection = RabbitMqConnector.GetConnection();


                using (var channel = connection.CreateModel())
                {
                    
                    channel.ExchangeDeclare(exchange: _configuration.Exchange,
                                            type: _configuration.ExchangeType, 
                                            durable: _configuration.Durable);

                    OnChannelCreate(channel);
                    var message = Jil.JSON.Serialize(payload);
                    var body = Encoding.UTF8.GetBytes(message);

                    var messageProperties = channel.CreateBasicProperties();
                    SetMessageProperties(messageProperties);

                    channel.BasicPublish(exchange: _configuration.Exchange,
                                         routingKey: _configuration.RoutingKey,
                                         basicProperties: messageProperties,
                                         body: body);
                    OnPublish(channel);
                    
                }
            }
        }

        
    }
}
