using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Thorium.Core.MessageQueue.Subscribe
{
    public class EventConsumer
    {
        private static ConcurrentDictionary<string, IEventQueueConsumer> cd = new ConcurrentDictionary<string, IEventQueueConsumer>();

        private static Object lockVar = new object();

        private static Lazy<EventConsumer> _instance = new Lazy<EventConsumer>(CreateConsumer);

        private static EventConsumer CreateConsumer()
        {
            return new EventConsumer();
        }

        public static EventConsumer GetConsumer(string hostname, string username, string password)
        {
            RabbitMqConnector.SetConnectionSettings(hostname, username, password);
            return _instance.Value;
        }

        public void Subscribe<TPayload>(string @event, Action<TPayload> consumer)
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

        public void Unsubscribe(string @event)
        {
            if (cd.ContainsKey(@event))
            {
                cd.TryRemove(@event, out IEventQueueConsumer _);
            }
        }
    }
}
