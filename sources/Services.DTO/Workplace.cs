using Queue.Model.Common;
using System;
using System.Resources;
using System.Runtime.Serialization;
using Translation = Queue.Model.Common.Translation;

namespace Queue.Services.DTO
{
    [DataContract]
    public class WorkplaceLink : IdentifiedEntityLink { }

    [DataContract]
    public class Workplace : IdentifiedEntity
    {
        [DataMember]
        public WorkplaceType Type { get; set; }

        [DataMember]
        public int Number { get; set; }

        [DataMember]
        public WorkplaceModificator Modificator { get; set; }

        [DataMember]
        public string Comment { get; set; }

        [DataMember]
        public byte Display { get; set; }

        [DataMember]
        public byte Segments { get; set; }

        public override string ToString()
        {
            ResourceManager translation1 = Translation.WorkplaceType.ResourceManager;
            ResourceManager translation2 = Translation.WorkplaceType.ResourceManager;

            return string.Format("{0} {1}{2}", translation1.GetString(Type.ToString()), Number, translation2.GetString(Modificator.ToString())).Trim();
        }

        public override IdentifiedEntityLink GetLink()
        {
            return new WorkplaceLink
            {
                Id = Id,
                Presentation = ToString()
            };
        }
    }
}