using MyBucks.Core.MessageQueue.Model;
using MyBucks.Core.MessageQueue.Publish;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyBucks.Core.MessageQueue
{
    public class AtlasQueuePublisher : RabbitMqPublisherBase, IWorkQueuePublisher
    {
        public string Environment { get; set; }

        protected AtlasQueuePublisher(QueueConfiguration configuration) : base(configuration)
        {
            InitSettings();
        }

        public AtlasQueuePublisher() : base()
        {
            
            InitSettings();
        }

        private void InitSettings()
        {
            _configuration.Durable = true;
            _configuration.ExchangeType = "topic";
            _configuration.AutoAcknowledge = false;
            _configuration.EnableDeadLettering = true;
            _configuration.AutoDelete = false;
        }

        public void PublishMessage<T>(string exchange, string queue, T payload)
        {
            _configuration.Exchange = exchange;
            _configuration.QueueName = queue;
            //_configuration.RoutingKey = queue;
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

        protected override void ConfigureDeadLetterQueue(Dictionary<string, object> queueArguments, IModel channel)
        {
            if (_configuration.EnableDeadLettering)
            {
                if (string.IsNullOrWhiteSpace(_configuration.QueueName))
                {
                    throw new ArgumentException("Cannot enable deadletter queues on anonymous queues.", nameof(_configuration.QueueName));
                }
                queueArguments["x-dead-letter-exchange"] = $"{Environment}.FinCloudDeadLetter";
                queueArguments["x-dead-letter-routing-key"] = $"{_configuration.QueueName}.DeadLetter.rk";
                
            }
        }
    }
}
