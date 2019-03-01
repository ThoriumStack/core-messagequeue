using System.Runtime.Serialization;

namespace Thorium.Core.MessageQueue.Model
{
    [DataContract]
    public class AtlasQueueMessage<TInputType> : AtlasQueueMessageBase
    {
        public AtlasQueueMessage(string userName, int queueCountry, string consumerRoutingKey, TInputType payLoad) : base(userName, queueCountry, consumerRoutingKey)
        {
            MessagePayload = payLoad;
        }

        public AtlasQueueMessage()
        {

        }

        public TInputType MessagePayload { get; set; }
    }
}
