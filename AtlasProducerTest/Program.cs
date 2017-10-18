using MyBucks.Core.MessageQueue;
using MyBucks.Core.MessageQueue.Model;
using MyBucks.Core.MessageQueue.Publish;
using System;

namespace AtlasProducerTest
{
    class Program
    {
        static void Main(string[] args)
        {
            string environment = ""; // todo: read from settings


            var qp = new WorkQueuePublisher().GetAtlasQueuePublisher();
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

            //qp.PublishMessage($"{environment}.FincloudGeneric", $"{environment}.WhatsappZa", payload);
            qp.PublishMessage($"{environment}FincloudGeneric", $"{environment}WhatsappZa", payload);
            Console.Read();
        }
    }
}
