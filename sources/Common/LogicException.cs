using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Queue.Common
{
    [Serializable]
    public class LogicException : Exception
    {
        public LogicException()
        {
        }

        public LogicException(string message)
            : base(message)
        {
        }

        public LogicException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected LogicException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}