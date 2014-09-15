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
        public class ClientRequestPlan
        {
            [DataMember]
            public ClientRequest ClientRequest { get; set; }

            [DataMember]
            public TimeSpan StartTime { get; set; }

            [DataMember]
            public TimeSpan FinishTime { get; set; }
        }
    }
}
