using Queue.Model.Common;
using System;
using System.Runtime.Serialization;

namespace Queue.Services.DTO
{
    public partial class QueuePlan
    {
        //TODO: think!
        [DataContract]
        public class ClientRequest : IdentifiedEntity
        {
            [DataMember]
            public int Number { get; set; }

            [DataMember]
            public TimeSpan RequestTime { get; set; }

            [DataMember]
            public ClientRequestType Type { get; set; }

            [DataMember]
            public int Subjects { get; set; }

            [DataMember]
            public string Client { get; set; }

            [DataMember]
            public string Service { get; set; }

            [DataMember]
            public bool IsPriority { get; set; }

            [DataMember]
            public ClientRequestState State { get; set; }

            [DataMember]
            public bool IsClosed { get; set; }

            [DataMember]
            public string Color { get; set; }

            [DataMember]
            public bool IsEditable { get; set; }

            [DataMember]
            public bool IsRestorable { get; set; }

            public override string ToString()
            {
                return string.Format("{0} {1} - {2}", Number, Service, Client);
            }

            public override IdentifiedEntityLink GetLink()
            {
                return new ClientRequestLink
                {
                    Id = Id,
                    Presentation = ToString()
                };
            }
        }
    }
}