using MyBucks.Core.MessageQueue.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace MyBucks.Core.MessageQueue.Subscribe
{
    public class WorkQueueConsumer
    {
        private static List<IWorkQueueConsumer> cd = new List<IWorkQueueConsumer>();

        private static Object lockVar = new object();
        private static Object stopLock = new object();

        private static Lazy<WorkQueueConsumer> _instance = new Lazy<WorkQueueConsumer>(CreateConsumer);

        private static WorkQueueConsumer CreateConsumer()
        {
            return new WorkQueueConsumer();
        }

        public static WorkQueueConsumer GetConsumer(string hostname, string username, string password)
        {
            RabbitMqConnector.SetConnectionSettings(hostname, username, password);
            return _instance.Value;
        }

        public Guid Consume<TPayload>(string exchange, string queue, Func<WorkQueueMessage<TPayload>, ConsumerResponse> consumerMethod) 
        {
            lock (lockVar)
            {
                var consumerInstance = new RabbitMqWorkQueueConsumer();

              
                cd.Add(consumerInstance);

                consumerInstance.Consume(exchange, queue, consumerMethod);
                return consumerInstance.ConsumerId;
            }
            
        }

        public IWorkQueueConsumer GetRawConsumer(string exchange, string queue)
        {
            var consumerInstance = new RabbitMqWorkQueueConsumer();
            consumerInstance.SetConsumerConfig(exchange, queue);
            return consumerInstance;
        }

        

        public static void StopConsumer(Guid consumerReference)
        {
            lock (stopLock)
            {
                var consumer = cd.FirstOrDefault(c => c.ConsumerId == consumerReference);
                consumer?.CloseChannel();
                if (consumer != null)
                {
                    cd.Remove(consumer);
                }
            }
        }
    }
}
