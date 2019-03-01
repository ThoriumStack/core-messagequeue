using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;
using Thorium.Core.MessageQueue.Model;

namespace Thorium.Core.MessageQueue.Publish
{
    public abstract class RabbitMqPublisherBase : RabbitMqBase
    {
        private Object _publishLock = new Object();
        

        public RabbitMqPublisherBase(QueueConfiguration configuration)
        {
            _configuration = configuration;
        }

        public RabbitMqPublisherBase()
        {
            _configuration = new QueueConfiguration();
        }

        public void SetRoutingKey(string key)
        {
            _configuration.RoutingKey = key;
        }

        protected virtual void OnChannelCreate(IModel channel) { }
        protected virtual void OnPublish(IModel channel) { }
        protected virtual void SetMessageProperties(IBasicProperties properties) { }

        protected void Publish<TPayload>(TPayload payload)
        {
            lock (_publishLock)
            {

                var connection = RabbitMqConnector.GetConnection();


                using (var channel = connection.CreateModel())
                {
                    
                    channel.ExchangeDeclare(exchange: _configuration.Exchange,
                                            type: _configuration.ExchangeType, 
                                            durable: _configuration.Durable, autoDelete: _configuration.AutoDelete);
                    
                    OnChannelCreate(channel);
                    var message = Jil.JSON.Serialize(payload, Jil.Options.IncludeInherited);
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
