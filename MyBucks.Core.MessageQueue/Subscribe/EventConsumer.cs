using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace MyBucks.Core.MessageQueue.Subscribe
{
    public class EventConsumer
    {
        private static ConcurrentDictionary<string, IEventQueueConsumer> cd = new ConcurrentDictionary<string, IEventQueueConsumer>();

        private static Object lockVar = new object();

        public static void Subscribe<TPayload>(string @event, Action<TPayload> consumer)
        {
            lock (lockVar)
            {
                var consumerInstance = new RabbitMqEventConsumer();
                

                if (!cd.ContainsKey(@event))
                {
                    if (cd.TryAdd(@event, consumerInstance))
                    {
                        consumerInstance.Consume(@event, consumer);
                    }

                }
                else
                {
                    cd[@event].CloseChannel();
                    cd[@event] = consumerInstance;
                    consumerInstance.Consume(@event, consumer);
                }

            }
        }

        public static void Unsubscribe(string @event)
        {
            if (cd.ContainsKey(@event))
            {
                cd.TryRemove(@event, out IEventQueueConsumer _);
            }
        }
    }
}
