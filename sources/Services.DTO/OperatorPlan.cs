using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Queue.Services.DTO
{
    [DataContract]
    public class OperatorPlan
    {
        [DataMember]
        public Operator Operator { get; set; }

        [DataMember]
        public ClientRequestPlan CurrentClientRequestPlan;
    }
}