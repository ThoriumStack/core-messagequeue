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
            EventConsumer.Consume<TestEvent>("USER_CREATED", WriteConsumer);
            EventConsumer.Consume<TestEvent>("USER_CREATED", WriteConsumer2);
            WorkQueueConsumer.Consume<ReceiptMessage>("receipts", "intecon", InteconProcessor);
            Console.ReadKey();
        }


        private static ConsumerResponse InteconProcessor(WorkQueueMessage<ReceiptMessage> arg)
        {
            Console.WriteLine($"Wrote out receipt for {arg.Payload.Amount} to customer {arg.Payload.CustomerId}");
            return ConsumerResponse.DiscardWithError();
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
