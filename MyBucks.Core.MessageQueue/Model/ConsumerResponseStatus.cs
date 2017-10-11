using System;
using System.Collections.Generic;
using System.Text;

namespace MyBucks.Core.MessageQueue.Model
{
    public enum ConsumerResponseStatus
    {
        /// <summary>
        /// Message has been processed succesfully.
        /// </summary>
        Acknowledge = 1,

        /// <summary>
        /// Message should be discarded from the queue.
        /// </summary>
        Reject = 2,

        /// <summary>
        /// Add the message back to the queue for reprocessing
        /// </summary>
        Requeue = 3,

        /// <summary>
        /// The message is erroneous and cannot be processed.
        /// </summary>
        DiscardWithError = 4        
    }
}
