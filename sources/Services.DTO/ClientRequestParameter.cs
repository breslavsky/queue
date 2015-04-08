using System.Runtime.Serialization;

namespace Queue.Services.DTO
{
    [DataContract]
    public class ClientRequestParameter : IdentifiedEntity
    {
        private string name;
        private string value;

        [DataMember]
        public string Name
        {
            get { return name; }
            set { SetProperty(ref name, value); }
        }

        [DataMember]
        public string Value
        {
            get { return value; }
            set { SetProperty(ref this.value, value); }
        }

        public override string ToString()
        {
            return string.Format("{0} {1}", name, value);
        }
    }
}