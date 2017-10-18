using MyBucks.Core.MessageQueue.Model;
using MyBucks.Core.MessageQueue.Subscribe;
using System;

namespace ConsumerTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Consumer ===");
            var consumer = EventConsumer.GetConsumer("localhost", "guest", "guest");
            consumer.Subscribe<TestEvent>("USER_CREATED", WriteConsumer);
            consumer.Subscribe<TestEvent>("USER_CREATED", WriteConsumer2);
            WorkQueueConsumer.GetConsumer("localhost", "guest", "guest").Consume<ReceiptMessage>("receipts", "intecon", InteconProcessor);

            
            Console.ReadKey();
        }


        private static ConsumerResponse InteconProcessor(WorkQueueMessage<ReceiptMessage> arg)
        {
            Console.WriteLine($"Wrote out receipt for {arg.Payload.Amount} to customer {arg.Payload.CustomerId}");
            // process message here
            return ConsumerResponse.Acknowledge();
        }

        private static void WriteConsumer2(TestEvent obj)
        {
            Console.WriteLine($"== {obj.Name} {obj.Surname} ==");
        }

        private static void WriteConsumer(TestEvent obj)
        {
            Console.WriteLine($"{obj.Name} {obj.Surname}");
        }

        public class ReceiptMessage
        {
            public decimal Amount { get; set; }
            public string CustomerId { get; set; }
        }

        public class TestEvent
        {
            public string Name { get; set; }
            public string Surname { get; set; }
        }
    }
}
