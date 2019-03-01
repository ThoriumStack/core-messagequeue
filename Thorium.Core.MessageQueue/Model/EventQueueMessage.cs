using Jil;
using System;
using System.Collections.Generic;
using System.Text;

namespace Thorium.Core.MessageQueue.Model
{
    public class EventQueueMessage<TPayload>
    {
        [JilDirective(Name = "type")]
        public string Type { get; set; }

        [JilDirective(Name = "payload")]
        public TPayload Payload { get; set; }
        
        [JilDirective(Name = "user_id")]
        public string UserId { get; set; }

        [JilDirective(Name = "context")]
        public string Context { get; set; }
        
        [JilDirective(Name = "timezone-offset")]
        public int TimeZoneOffset { get; set; }
        
        
        [JilDirective(Name = "metadata")]
        public Dictionary<string, string> MetaData { get; set; } = new Dictionary<string, string>();
    }
}
