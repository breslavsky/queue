using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Queue.Services.DTO
{
    public partial class QueuePlan
    {
        public class Operator : IdentifiedEntity
        {
            [DataMember]
            public DateTime Heartbeat { get; set; }

            [DataMember]
            public bool Online { get; set; }

            [DataMember]
            public string Surname { get; set; }

            [DataMember]
            public string Name { get; set; }

            [DataMember]
            public string Workplace { get; set; }

            public override string ToString()
            {
                return string.Format("{0} {1}", Surname, Name).Trim();
            }
        }
    }
}
