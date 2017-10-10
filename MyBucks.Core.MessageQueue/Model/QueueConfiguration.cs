using System;
using System.Collections.Generic;
using System.Text;

namespace MyBucks.Core.MessageQueue.Model
{
    public class QueueConfiguration
    {
        public string Exchange { get; set; }
        public string QueueName { get; set; }
        public string ExchangeType { get; set; }
        public string RoutingKey { get; set; }
        public bool Durable { get; internal set; }
        
    }
}
