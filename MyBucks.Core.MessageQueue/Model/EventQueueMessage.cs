using Jil;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyBucks.Core.MessageQueue.Model
{
    public class EventQueueMessage<TPayload>
    {
        [JilDirective(Name = "type")]
        public string Type { get; set; }

        [JilDirective(Name = "payload")]
        public TPayload Payload { get; set; }
    }
}
