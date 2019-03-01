using System;
using System.Collections.Generic;
using System.Text;

namespace Thorium.Core.MessageQueue.Model
{
    public class QueueConfiguration
    {
        public string Exchange { get; set; }
        public string QueueName { get; set; }
        public string ExchangeType { get; set; }
        public string RoutingKey { get; set; }
        public bool Durable { get; set; }
        public bool AutoDelete { get; set; } = true;
        public bool AutoAcknowledge { get; set; }
        public bool EnableDeadLettering { get; set; }
        public string DeadLetterExchange { get; set; }
        public string DeadLetterRoutingKey { get; set; }
        public string DeadLetterQueue { get; internal set; }

        public override string ToString()
        {
            return $"Exchange: {Exchange}, Queue: {QueueName}, Durable: {Durable}, AutoDelete: {AutoDelete}, AutoAck: {AutoAcknowledge}";
        }
    }
}
