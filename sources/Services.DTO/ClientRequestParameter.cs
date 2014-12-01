using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Queue.Services.DTO
{
    [DataContract]
    public class ClientRequestParameter : IdentifiedEntity
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Value { get; set; }

        public override string ToString()
        {
            return string.Format("{0} {1}", Name, Value);
        }
    }
}
