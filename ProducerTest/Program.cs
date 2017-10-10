using MyBucks.Core.MessageQueue;
using System;

namespace ProducerTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var producer = new EventPublisher();
            while (true)
            {
                Console.WriteLine("=== Producer ===");
                Console.Write("Name: ");
                var name = Console.ReadLine();
                Console.Write("Surname: ");
                var surname = Console.ReadLine();
                producer.GetPublisher().PublishEvent("USER_CREATED", new TestEvent { Name = name, Surname = surname });
                Console.ReadKey();
            }
        }
    }

    public class TestEvent // Stay indoors!
    {
        public string Name { get; set; }
        public string Surname { get; set; }
    }
}
