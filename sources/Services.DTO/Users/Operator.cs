using System;
using System.Runtime.Serialization;

namespace Queue.Services.DTO
{
    [DataContract]
    public class OperatorLink : UserLink { }

    public class Operator : User
    {
        [DataMember]
        public WorkplaceLink Workplace { get; set; }

        [DataMember]
        public virtual bool IsInterruption { get; set; }

        [DataMember]
        public TimeSpan InterruptionStartTime { get; set; }

        [DataMember]
        public TimeSpan InterruptionFinishTime { get; set; }

        public override IdentifiedEntityLink GetLink()
        {
            return new OperatorLink
            {
                Id = Id,
                Presentation = ToString()
            };
        }
    }

    [DataContract]
    public class OperatorFull : Operator
    {
        [DataMember]
        public Workplace Workplace { get; set; }
    }
}