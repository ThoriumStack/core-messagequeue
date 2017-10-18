using System;
using System.Collections.Generic;
using System.Text;

namespace MyBucks.Core.MessageQueue.Publish
{
    public class WorkQueuePublisher
    {

        public IWorkQueuePublisher GetPersistentQueuePublisher(string hostName, string username, string password)
        {
            RabbitMqConnector.SetConnectionSettings(hostName, username, password);
            return new RabbitMqPersistentWorkQueuePublisher();
        }

        public IWorkQueuePublisher GetAtlasQueuePublisher(string hostName, string username, string password)
        {
            RabbitMqConnector.SetConnectionSettings(hostName, username, password);
            return new AtlasQueuePublisher();
        }
    }
}
