using System.Runtime.Serialization;

namespace Queue.Services.DTO
{
    [DataContract]
    public class ServiceGroup : IdentifiedEntity
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
        public int Columns { get; set; }

        [DataMember]
        public int Rows { get; set; }

        [DataMember]
        public string Color { get; set; }

        [DataMember]
        public ServiceGroup ParentGroup { get; set; }

        [DataMember]
        public bool IsActive { get; set; }

        public override string ToString()
        {
            return string.Format("{0} {1}", Code, Name);
        }
    }
}