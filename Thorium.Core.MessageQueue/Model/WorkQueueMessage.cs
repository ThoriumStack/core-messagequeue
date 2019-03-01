using System.Collections.Generic;
using Jil;

namespace Thorium.Core.MessageQueue.Model
{
    public class WorkQueueMessage<TPayload>
    {
        [JilDirective(Name = "user_id")]
        public string UserId { get; set; }

        [JilDirective(Name = "context")]
        public string Context { get; set; }
        
        [JilDirective(Name = "timezone-offset")]
        public int TimeZoneOffset { get; set; }
        
        [JilDirective(Name = "metadata")]
        public Dictionary<string, string> MetaData { get; set; } = new Dictionary<string, string>();

        [JilDirective(Name = "payload")]
        public TPayload Payload { get; set; }
    }
}
