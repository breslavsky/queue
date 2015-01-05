using System.Runtime.Serialization;

namespace Queue.Services.DTO
{
    [DataContract]
    public class CouponDataParam
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Value { get; set; }
    }
}