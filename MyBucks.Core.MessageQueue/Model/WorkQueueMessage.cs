using Jil;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyBucks.Core.MessageQueue.Model
{
    public class WorkQueueMessage<TPayload>
    {
        [JilDirective(Name = "user_id")]
        public string UserId { get; set; }

        [JilDirective(Name = "context")]
        public string Context { get; set; }

        [JilDirective(Name = "payload")]
        public TPayload Payload { get; set; }
    }
}
