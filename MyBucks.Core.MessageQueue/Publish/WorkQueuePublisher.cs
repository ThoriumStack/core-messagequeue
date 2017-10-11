using System;
using System.Collections.Generic;
using System.Text;

namespace MyBucks.Core.MessageQueue.Publish
{
    public class WorkQueuePublisher
    {
        public IWorkQueuePublisher GetPersistentQueuePublisher()
        {
            return new RabbitPersistentWorkQueuePublisher();
        }
    }
}
