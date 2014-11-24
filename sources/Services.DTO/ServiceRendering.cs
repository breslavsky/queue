using Queue.Model.Common;
using System;
using System.Resources;
using System.Runtime.Serialization;
using Translation = Queue.Model.Common.Translation;

namespace Queue.Services.DTO
{
    [DataContract]
    public class ServiceRendering : IdentifiedEntity
    {
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
            var translation = Translation.ServiceRenderingMode.ResourceManager;

            return string.Format("[{0}] {1}", Operator, translation.GetString(Mode.ToString()));
        }
    }
}