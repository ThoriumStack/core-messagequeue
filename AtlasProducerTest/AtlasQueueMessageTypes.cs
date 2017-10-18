using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace AtlasProducerTest
{
    public class MessageQueuePublishObjectViewModelBase
    {
        public string UserName { get; set; }
        public int QueueCountry { get; set; }
        public int ExchangeChannel { get; set; } = 1;
        public string EnvironmentalPrefix { get; set; }
        public string ConsumerRoutingKey { get; set; }
        public int NumberOfRetries { get; set; }
        public bool IsMarkedAsDeadLetter { get; set; }
        public string DeadLetterReason { get; set; }

        public MessageQueuePublishObjectViewModelBase(string userName, int queueCountry, string consumerRoutingKey)
        {
            UserName = userName;
            QueueCountry = queueCountry;
            ExchangeChannel = 1;
            ConsumerRoutingKey = consumerRoutingKey;
            NumberOfRetries = 0;
            IsMarkedAsDeadLetter = false;
        }

        public MessageQueuePublishObjectViewModelBase()
        {
            NumberOfRetries = 0;
            IsMarkedAsDeadLetter = false;
        }
    }

    [DataContract]
    public class MessageQueuePublishObjectViewModel<TInputType> : MessageQueuePublishObjectViewModelBase
    {
        public MessageQueuePublishObjectViewModel(string userName, int queueCountry, string consumerRoutingKey, TInputType payLoad) : base(userName, queueCountry, consumerRoutingKey)
        {
            MessagePayload = payLoad;
        }

        public MessageQueuePublishObjectViewModel()
        {

        }

        public TInputType MessagePayload { get; set; }
    }
}
