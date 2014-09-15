using Queue.Model.Common;
using System;
using System.Runtime.Serialization;

namespace Queue.Services.DTO
{
    [DataContract]
    public class ClientRequestFilter
    {
        [DataMember]
        public DateTime? RequestDate { get; set; }

        [DataMember]
        public Guid? OperatorId { get; set; }

        [DataMember]
        public Guid? ServiceId { get; set; }

        [DataMember]
        public Guid? ClientId { get; set; }

        [DataMember]
        public ClientRequestState? State { get; set; }

        [DataMember]
        public string Query { get; set; }
    }
}