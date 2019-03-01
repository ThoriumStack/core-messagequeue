namespace Thorium.Core.MessageQueue.Model
{
    public class ConsumerResponse
    {
        public string Reason { get; set; }
        public ConsumerResponseStatus ResponseStatus { get; set; }

        /// <summary>
        /// Message has been processed succesfully.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static ConsumerResponse Acknowledge(string message="")
        {
            return new ConsumerResponse() {
                Reason = message,
                ResponseStatus = ConsumerResponseStatus.Acknowledge
            };
        }

        /// <summary>
        /// Message should be discarded from the queue.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static ConsumerResponse Reject(string message = "")
        {
            return new ConsumerResponse()
            {
                Reason = message,
                ResponseStatus = ConsumerResponseStatus.Reject
            };
        }

        /// <summary>
        /// Add the message back to the queue for reprocessing
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static ConsumerResponse Requeue(string message = "")
        {
            return new ConsumerResponse()
            {
                Reason = message,
                ResponseStatus = ConsumerResponseStatus.Requeue
            };
        }

        /// <summary>
        /// The message is erroneous and cannot be processed.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static ConsumerResponse DiscardWithError(string message = "")
        {
            return new ConsumerResponse()
            {
                Reason = message,
                ResponseStatus = ConsumerResponseStatus.DiscardWithError
            };
        }
    }
}
