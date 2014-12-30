using Queue.Model.Common;
using System;
using System.Runtime.Serialization;

namespace Queue.Services.DTO
{
    [DataContract]
    public class ClientRequestFilter
    {
        [DataMember]
        public bool IsRequestDate { get; set; }

        [DataMember]
        public DateTime RequestDate { get; set; }

        [DataMember]
        public bool IsOperator { get; set; }

        [DataMember]
        public Guid OperatorId { get; set; }

        [DataMember]
        public bool IsService { get; set; }

        [DataMember]
        public Guid ServiceId { get; set; }

        [DataMember]
        public bool IsClient { get; set; }

        [DataMember]
        public Guid ClientId { get; set; }

        [DataMember]
        public bool IsState { get; set; }

        [DataMember]
        public ClientRequestState State { get; set; }

        [DataMember]
        public string Query { get; set; }

        //TODO: create validate
    }
}