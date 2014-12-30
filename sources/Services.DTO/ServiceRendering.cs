using Queue.Model.Common;
using System;
using System.Resources;
using System.Runtime.Serialization;

namespace Queue.Services.DTO
{
    [DataContract]
    public class ServiceRendering : IdentifiedEntity
    {
        public ServiceRendering()
        {
            Mode = ServiceRenderingMode.AllRequests;
        }

        [DataMember]
        public Schedule Schedule { get; set; }

        [DataMember]
        public Operator Operator { get; set; }

        [DataMember]
        public ServiceRenderingMode Mode { get; set; }

        [DataMember]
        public ServiceStep ServiceStep { get; set; }

        [DataMember]
        public int Priority { get; set; }

        public override string ToString()
        {
            return string.Format("[{0}] {1}", Operator, Mode.Translate());
        }
    }
}