using System;
using System.Collections.Generic;
using System.Text;

namespace MyBucks.Core.MessageQueue.Model
{
    public class ConsumerResponse
    {
        public string Reason { get; set; }
        public ConsumerResponseStatus ResponseStatus { get; set; }
    }
}
