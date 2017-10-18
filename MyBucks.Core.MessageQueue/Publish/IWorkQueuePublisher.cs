using MyBucks.Core.MessageQueue.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyBucks.Core.MessageQueue.Publish
{
    public interface IWorkQueuePublisher
    {
        void PublishMessage<T>(string exchange, string queue, T payload);
    }
}
