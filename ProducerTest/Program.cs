using MyBucks.Core.MessageQueue;
using MyBucks.Core.MessageQueue.Model;
using MyBucks.Core.MessageQueue.Publish;
using System;

namespace ProducerTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Producer ===");
            var producer = new EventQueuePublisher();
            while (true)
            {
                Console.WriteLine("=== Producer ===");
                Console.Write("Name: ");
                var name = Console.ReadLine();
                Console.Write("Surname: ");
                var surname = Console.ReadLine();
                producer.GetPublisher().PublishEvent("USER_CREATED", new TestEvent { Name = name, Surname = surname });
                var key = Console.ReadKey();
                if (key.Key == ConsoleKey.Escape)
                {
                    break;
                }
            }
            while (true)
            {
                Console.Write("Order: ");
                var orderText = Console.ReadLine();
                var workproducer = new WorkQueuePublisher().GetPersistentQueuePublisher();
                workproducer.PublishMessage("receipts", "intecon", new MyBucks.Core.MessageQueue.Model.WorkQueueMessage<ReceiptMessage> { Payload = new ReceiptMessage { Amount = decimal.Parse(orderText), CustomerId = "JKL-HYDE" } });
            }
        }
    }

    public class Order {
        public string Item { get; set; }
    }

    public class ReceiptMessage 
    {
        public decimal Amount { get; set; }
        public string CustomerId { get; set; }
    }


    public class TestEvent // Stay indoors!
    {
        public string Name { get; set; }
        public string Surname { get; set; }
    }
}
