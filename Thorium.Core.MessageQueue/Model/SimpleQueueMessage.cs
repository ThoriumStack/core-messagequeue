using System;
using System.Collections.Generic;
using System.Text;

namespace Thorium.Core.MessageQueue.Model
{
    public class SimpleQueueMessage<T>
    {
        public T Payload { get; set; }
        internal ulong DeliveryTag { get; set; }
    }
}
