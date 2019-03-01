namespace Thorium.Core.MessageQueue.Publish
{
    public class EventQueuePublisher
    {
        public IEventQueuePublisher GetPublisher(string hostname, string username, string password, string queueName=null)
        {
            RabbitMqConnector.SetConnectionSettings(hostname, username, password);
            return new RabbitMqEventPublisher(config => {
                config.Exchange = "events";
                if (queueName != null)
                {
                    config.QueueName = queueName;
                }

                config.ExchangeType = "topic";
            });
        }
    }
}
