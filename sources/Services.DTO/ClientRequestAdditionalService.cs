using Queue.Model.Common;
using System;
using System.Runtime.Serialization;

namespace Queue.Services.DTO
{
    [DataContract]
    public class ClientRequestAdditionalService : IdentifiedEntity
    {
        [DataMember]
        public AdditionalService AdditionalService { get; set; }

        [DataMember]
        public float Quantity { get; set; }

        public override string ToString()
        {
            return string.Format("{0} {1} {2}", AdditionalService, Quantity, AdditionalService.Measure);
        }
    }
}