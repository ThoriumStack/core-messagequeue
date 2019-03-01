namespace Thorium.Core.MessageQueue.Model
{
    public class AtlasQueueMessageBase
    {
        public string UserName { get; set; }
        public int QueueCountry { get; set; }
        public int ExchangeChannel { get; set; } = 1;
        public string EnvironmentalPrefix { get; set; }
        public string ConsumerRoutingKey { get; set; }
        public int NumberOfRetries { get; set; }
        public bool IsMarkedAsDeadLetter { get; set; }
        public string DeadLetterReason { get; set; }

        public AtlasQueueMessageBase(string userName, int queueCountry, string consumerRoutingKey)
        {
            UserName = userName;
            QueueCountry = queueCountry;
            ExchangeChannel = 1;
            ConsumerRoutingKey = consumerRoutingKey;
            NumberOfRetries = 0;
            IsMarkedAsDeadLetter = false;
        }

        public AtlasQueueMessageBase()
        {
            NumberOfRetries = 0;
            IsMarkedAsDeadLetter = false;
        }
    }
}
