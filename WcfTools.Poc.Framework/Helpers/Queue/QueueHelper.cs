using System;
using System.Messaging;

namespace WcfTools.Poc.Framework.Helpers.Queue
{
    internal class QueueHelper
    {
        /// <summary>
        ///     The base path for private queues
        /// </summary>
        private const string QueueBasePath = @".\private$\";

        /// <summary>
        /// Method used to validate a queue. Validates the queue (and it's corresponding dead letter and poison queues exist)
        /// </summary>
        /// <param name="interfaceType">The contract type to use when validating the queue</param>
        public static void ValidateQueue(Type interfaceType)
        {
            string name = interfaceType.Name;
            string queueName = string.Format("{0}{1}", QueueBasePath, name);
            string deadLetterQueueName = string.Format("{0}DeadLetter", queueName);
            string posionQueueName = string.Format("{0};poison", queueName);
            ValidateQueue(queueName);
            ValidateQueue(deadLetterQueueName);
            ValidateQueue(posionQueueName);
        }

        private static void ValidateQueue(string queueName)
        {
            // Create the Queue - notice the overloaded constructor used to ensure the queue is transactional
            if (!MessageQueue.Exists(queueName)) MessageQueue.Create(queueName, true);
        }
    }
}