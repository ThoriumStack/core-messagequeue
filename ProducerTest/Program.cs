using Thorium.Core.MessageQueue;
using Thorium.Core.MessageQueue.Model;
using Thorium.Core.MessageQueue.Publish;
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
                producer.GetPublisher("localhost", "admin", "admin").PublishEvent("USER_CREATED", new TestEvent { Name = name, Surname = surname });
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
                var workproducer = new WorkQueuePublisher().GetPersistentQueuePublisher("localhost", "admin", "admin");
                workproducer.PublishMessage("receipts", "intecon", new Thorium.Core.MessageQueue.Model.WorkQueueMessage<ReceiptMessage> { Payload = new ReceiptMessage { Amount = decimal.Parse(orderText), CustomerId = "JKL-HYDE" } });
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
