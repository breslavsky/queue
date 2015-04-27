using System;
using System.Runtime.Serialization;

namespace Queue.Common
{
    [Serializable]
    public class QueueException : Exception
    {
        public QueueException()
        {
        }

        public QueueException(string message)
            : base(message)
        {
        }

        public QueueException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected QueueException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }
    }
}