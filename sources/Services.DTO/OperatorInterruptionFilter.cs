using Queue.Model.Common;
using System;
using System.Runtime.Serialization;

namespace Queue.Services.DTO
{
    [DataContract]
    public class OperatorInterruptionFilter
    {
        [DataMember]
        public bool IsOperator { get; set; }

        [DataMember]
        public Guid OperatorId { get; set; }
    }
}