using Queue.Model.Common;
using System;
using System.Runtime.Serialization;

namespace Queue.Services.DTO
{
    [DataContract]
    public class OperatorInterruption : IdentifiedEntity
    {
        private Operator queueOperator;
        private OperatorInterruptionType type;
        private DateTime targetDate;
        private DayOfWeek dayOfWeek;
        private TimeSpan startTime;
        private TimeSpan finishTime;
        private ServiceRenderingMode serviceRenderingMode;
        private int weekFold;

        [DataMember]
        public Operator Operator
        {
            get { return queueOperator; }
            set { SetProperty(ref queueOperator, value); }
        }

        [DataMember]
        public OperatorInterruptionType Type
        {
            get { return type; }
            set { SetProperty(ref type, value); }
        }

        [DataMember]
        public DateTime TargetDate
        {
            get { return targetDate; }
            set { SetProperty(ref targetDate, value); }
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

        [DataMember]
        public ServiceRenderingMode ServiceRenderingMode
        {
            get { return serviceRenderingMode; }
            set { SetProperty(ref serviceRenderingMode, value); }
        }

        [DataMember]
        public int WeekFold
        {
            get { return weekFold; }
            set { SetProperty(ref weekFold, value); }
        }
    }
}