using System.Runtime.Serialization;

namespace Queue.Services.DTO
{
    [DataContract]
    public class AdditionalService : IdentifiedEntity
    {
        private string name;
        private decimal price;
        private string measure;

        [DataMember]
        public string Name
        {
            get { return name; }
            set { SetProperty(ref name, value); }
        }

        [DataMember]
        public decimal Price
        {
            get { return price; }
            set { SetProperty(ref price, value); }
        }

        [DataMember]
        public string Measure
        {
            get { return measure; }
            set { SetProperty(ref measure, value); }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}