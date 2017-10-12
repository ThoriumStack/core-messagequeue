using MyBucks.Core.MessageQueue.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace MyBucks.Core.MessageQueue.Subscribe
{
    public class WorkQueueConsumer
    {
        private static ConcurrentDictionary<string, List<IWorkQueueConsumer>> cd = new ConcurrentDictionary<string, List<IWorkQueueConsumer>>();

        private static Object lockVar = new object();

        public static void Consume<TPayload>(string exchange, string queue, Func<WorkQueueMessage<TPayload>, ConsumerResponse> consumerMethod) 
        {
            lock (lockVar)
            {
                var consumerInstance = new RabbitMqWorkQueueConsumer();

                var key = $"{exchange}.{queue}";

                if (!cd.ContainsKey(key) || cd[key] == null)
                {
                    cd[key] = new List<IWorkQueueConsumer>();
                    
                }
                cd[key].Add(consumerInstance);

                consumerInstance.Consume(exchange, queue, consumerMethod);
            }
        }
    }
}
