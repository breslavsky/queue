using Queue.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Queue.Services.DTO
{
    public partial class QueuePlan
    {
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
            public bool IsEditable { get; set; }

            public virtual string Color
            {
                get
                {
                    if (IsClosed)
                    {
                        switch (State)
                        {
                            case ClientRequestState.Rendered:
                                return "GreenYellow";

                            case ClientRequestState.Absence:
                                return "LightPink";

                            case ClientRequestState.Canceled:
                                return "Silver";
                        }
                    }
                    else
                    {
                        switch (State)
                        {
                            case ClientRequestState.Waiting:
                                switch (Type)
                                {
                                    case ClientRequestType.Early:
                                        return "LightSeaGreen";

                                    case ClientRequestType.Live:
                                        return "BurlyWood";
                                }
                                break;

                            case ClientRequestState.Calling:
                                return "Yellow";

                            case ClientRequestState.Rendering:
                                return "LightBlue";

                            case ClientRequestState.Redirected:
                                return "Blue";
                        }
                    }

                    return "BurlyWood";
                }
            }

            public override string ToString()
            {
                return string.Format("{0} {1} - {2}", Number, Service, Client);
            }
        }
    }
}