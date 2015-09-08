using System.Runtime.Serialization;

namespace Queue.Services.Common
{
    [DataContract]
    public class ObjectNotFoundFault
    {
        public ObjectNotFoundFault()
            : base()
        {
        }

        public ObjectNotFoundFault(object ObjectId)
            : base()
        {
            this.ObjectId = ObjectId.ToString();
        }

        [DataMember]
        public object ObjectId { get; protected set; }
    }
}