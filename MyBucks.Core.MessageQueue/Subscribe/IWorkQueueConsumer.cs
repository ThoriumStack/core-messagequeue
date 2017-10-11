﻿using MyBucks.Core.MessageQueue.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyBucks.Core.MessageQueue.Subscribe
{
    public interface IWorkQueueConsumer
    {
        void Consume<TPayload>(string exchange, string queue, Func<TPayload, ConsumerResponse> consumerMethod);
    }
}
