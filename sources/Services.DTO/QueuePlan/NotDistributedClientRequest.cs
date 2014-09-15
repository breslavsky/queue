using Queue.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Queue.Services.DTO
{
    public partial class QueuePlan
    {
        [DataContract]
        public class NotDistributedClientRequest
        {
            [DataMember]
            public ClientRequest ClientRequest { get; set; }

            [DataMember]
            public string Reason { get; set; }
        }
    }
}