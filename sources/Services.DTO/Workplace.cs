using Junte.Translation;
using Queue.Model.Common;
using System.Runtime.Serialization;

namespace Queue.Services.DTO
{
    [DataContract]
    public class Workplace : IdentifiedEntity
    {
        [DataMember]
        public string Comment { get; set; }

        [DataMember]
        public byte Display { get; set; }

        [DataMember]
        public WorkplaceModificator Modificator { get; set; }

        [DataMember]
        public int Number { get; set; }

        [DataMember]
        public byte Segments { get; set; }

        [DataMember]
        public WorkplaceType Type { get; set; }

        public override string ToString()
        {
            return string.Format("{0} {1}{2}", Translater.Enum(Type), Number, Translater.Enum(Modificator)).Trim();
        }
    }
}