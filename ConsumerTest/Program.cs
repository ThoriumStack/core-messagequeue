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

            Console.ReadKey();
        }

        private static void WriteConsumer2(TestEvent obj)
        {
            Console.WriteLine($"== {obj.Name} {obj.Surname} ==");
        }

        private static void WriteConsumer(TestEvent obj)
        {
            Console.WriteLine($"{obj.Name} {obj.Surname}");
        }

        public class TestEvent
        {
            public string Name { get; set; }
            public string Surname { get; set; }
        }
    }
}
