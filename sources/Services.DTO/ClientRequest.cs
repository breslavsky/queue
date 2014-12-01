using Queue.Model.Common;
using System;
using System.Runtime.Serialization;

namespace Queue.Services.DTO
{
    [DataContract]
    public class ClientRequest : IdentifiedEntity
    {
        [DataMember]
        public DateTime CreateDate { get; set; }

        [DataMember]
        public int Number { get; set; }

        [DataMember]
        public DateTime RequestDate { get; set; }

        [DataMember]
        public TimeSpan RequestTime { get; set; }

        [DataMember]
        public ClientRequestType Type { get; set; }

        [DataMember]
        public Client Client { get; set; }

        [DataMember]
        public Service Service { get; set; }

        [DataMember]
        public ServiceType ServiceType { get; set; }

        [DataMember]
        public ServiceStep ServiceStep { get; set; }

        [DataMember]
        public Operator Operator { get; set; }

        [DataMember]
        public float Productivity { get; set; }

        [DataMember]
        public bool IsPriority { get; set; }

        [DataMember]
        public int Subjects { get; set; }

        [DataMember]
        public TimeSpan WaitingStartTime { get; set; }

        [DataMember]
        public TimeSpan CallingStartTime { get; set; }

        [DataMember]
        public TimeSpan CallingFinishTime { get; set; }

        [DataMember]
        public TimeSpan RenderStartTime { get; set; }

        [DataMember]
        public TimeSpan RenderFinishTime { get; set; }

        [DataMember]
        public ClientRequestState State { get; set; }

        [DataMember]
        public bool IsClosed { get; set; }

        [DataMember]
        public int Version { get; set; }

        [DataMember]
        public string Color { get; set; }

        [DataMember]
        public bool IsEditable { get; set; }

        [DataMember]
        public bool IsRestorable { get; set; }

        [DataMember]
        public ClientRequestParameter[] Parameters { get; set; }

        public bool IsRecent(ClientRequest target)
        {
            return Equals(target) && Version > target.Version;
        }

        public override string ToString()
        {
            return string.Format("{0} {1} - {2}", Number, Client != null ? Client.ToString() : string.Empty, Service);
        }
    }
}