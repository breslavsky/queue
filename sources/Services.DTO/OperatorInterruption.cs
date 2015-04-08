using System;
using System.Runtime.Serialization;

namespace Queue.Services.DTO
{
    [DataContract]
    public class OperatorInterruption : IdentifiedEntity
    {
        private Operator queueOperator;
        private DayOfWeek dayOfWeek;
        private TimeSpan startTime;
        private TimeSpan finishTime;

        [DataMember]
        public Operator Operator
        {
            get { return queueOperator; }
            set { SetProperty(ref queueOperator, value); }
        }

        [DataMember]
        public DayOfWeek DayOfWeek
        {
            get { return dayOfWeek; }
            set { SetProperty(ref dayOfWeek, value); }
        }

        [DataMember]
        public TimeSpan StartTime
        {
            get { return startTime; }
            set { SetProperty(ref startTime, value); }
        }

        [DataMember]
        public TimeSpan FinishTime
        {
            get { return finishTime; }
            set { SetProperty(ref finishTime, value); }
        }
    }
}