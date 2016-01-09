using Queue.Model.Common;
using System;
using System.Runtime.Serialization;

namespace Queue.Services.DTO
{
    [DataContract]
    public class LifeSituation : IdentifiedEntity
    {
        [DataMember]
        public string Code { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Comment { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string Color { get; set; }

        [DataMember]
        public float FontSize { get; set; }

        [DataMember]
        public long SortId { get; set; }

        [DataMember]
        public bool IsActive { get; set; }

        [DataMember]
        public LifeSituationGroup LifeSituationGroup { get; set; }

        [DataMember]
        public Service Service { get; set; }

        public override string ToString()
        {
            return string.Format("{0} {1} для [{2}]", Code, Name, Service != null ? Service.Code : string.Empty);
        }
    }
}