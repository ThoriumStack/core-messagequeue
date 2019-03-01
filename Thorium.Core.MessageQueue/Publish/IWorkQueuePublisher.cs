using Thorium.Core.MessageQueue.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Thorium.Core.MessageQueue.Publish
{
    public interface IWorkQueuePublisher
    {
        void PublishMessage<T>(string exchange, string queue, T payload);
        void SetRoutingKey(string routingKey); 
    }
}
