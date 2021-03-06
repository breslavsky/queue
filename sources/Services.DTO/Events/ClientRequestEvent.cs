﻿using Queue.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Queue.Services.DTO
{
    [DataContract]
    public class ClientRequestEvent : Event
    {
        [DataMember]
        public ClientRequest ClientRequest { get; set; }
    }
}
