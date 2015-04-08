using System.Runtime.Serialization;

namespace Queue.Services.DTO
{
    [DataContract]
    public class ClientRequestAdditionalService : IdentifiedEntity
    {
        private ClientRequest clientRequest;
        private AdditionalService additionalService;
        private float quantity;

        [DataMember]
        public ClientRequest ClientRequest
        {
            get { return clientRequest; }
            set { SetProperty(ref clientRequest, value); }
        }

        [DataMember]
        public AdditionalService AdditionalService
        {
            get { return additionalService; }
            set { SetProperty(ref additionalService, value); }
        }

        [DataMember]
        public float Quantity
        {
            get { return quantity; }
            set { SetProperty(ref quantity, value); }
        }

        public override string ToString()
        {
            return string.Format("{0} {1} {2}", additionalService, quantity, additionalService.Measure);
        }
    }
}