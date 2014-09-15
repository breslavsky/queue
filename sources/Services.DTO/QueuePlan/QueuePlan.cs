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
        [DataMember]
        public DateTime PlanDate { get; set; }

        [DataMember]
        public TimeSpan PlanTime { get; set; }

        [DataMember]
        public int Version { get; set; }

        [DataMember]
        public string[] Report { get; set; }

        [DataMember]
        public OperatorPlan[] OperatorsPlans { get; set; }

        [DataMember]
        public NotDistributedClientRequest[] NotDistributedClientRequests { get; set; }

        public bool IsRecent(QueuePlan target)
        {
            return Equals(target) && Version >= target.Version;
        }

        public override bool Equals(object obj)
        {
            return obj != null && obj.GetHashCode() == GetHashCode();
        }

        public override int GetHashCode()
        {
            return PlanDate.GetHashCode();
        }
    }
}