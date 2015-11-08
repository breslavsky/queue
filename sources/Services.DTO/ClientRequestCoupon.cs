
using Queue.Model.Common;
using System;
using System.Runtime.Serialization;

namespace Queue.Services.DTO
{
    [DataContract]
    public class ClientRequestCoupon
    {
        [DataMember]
        public string QueueName { get; set; }

        [DataMember]
        public int Number { get; set; }

        [DataMember]
        public DateTime CreateDate { get; set; }

        [DataMember]
        public DateTime RequestDate { get; set; }

        [DataMember]
        public TimeSpan RequestTime { get; set; }

        [DataMember]
        public int Subjects { get; set; }

        [DataMember]
        public Client Client { get; set; }

        [DataMember]
        public ClientRequestParameter[] Parameters { get; set; }

        [DataMember]
        public Service Service { get; set; }

        [DataMember]
        public Workplace[] Workplaces { get; set; }

        [DataMember]
        public bool HasPlanned { get; set; }

        [DataMember]
        public int Position { get; set; }

        [DataMember]
        public TimeSpan WaitingTime { get; set; }

        [DataMember]
        public bool IsToday { get; set; }

        [DataMember]
        public CouponSection Sections { get; set; }
    }
}