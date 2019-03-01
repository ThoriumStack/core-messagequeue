using Thorium.Core.MessageQueue;
using Thorium.Core.MessageQueue.Model;
using Thorium.Core.MessageQueue.Publish;
using System;

namespace AtlasProducerTest
{
    class Program
    {
        static void Main(string[] args)
        {
            string environment = "DEV01AF."; // todo: read from settings


            var qp = new WorkQueuePublisher().GetAtlasQueuePublisher(environment, "172.21.0.19", "admin", "admin");
            var routingKey = $"{environment}54.ProcessMessage";
            qp.SetRoutingKey(routingKey);

            var payload = new AtlasQueueMessage<Chat2BrandPayload> {
                ConsumerRoutingKey = routingKey,
                EnvironmentalPrefix = environment,
                ExchangeChannel = 1,
                IsMarkedAsDeadLetter = false,
                NumberOfRetries = 0,
                QueueCountry = 1,
                UserName = "bucky",
                DeadLetterReason = "",
                MessagePayload = new Chat2BrandPayload
                {
                    client_id = "234"
                }

            };

            qp.PublishMessage($"{environment}FinCloudGeneric", $"{environment}WhatsappZa", payload);
            Console.Read();
        }
    }
}
