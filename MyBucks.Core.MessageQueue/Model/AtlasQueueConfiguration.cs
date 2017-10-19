using System;
using System.Collections.Generic;
using System.Text;

namespace MyBucks.Core.MessageQueue.Model
{
    public class AtlasQueueConfiguration :QueueConfiguration
    {
        public string Environment { get; set; }
    }
}
