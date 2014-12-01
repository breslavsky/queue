using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Queue.Services.DTO
{
    [DataContract]
    public class ScheduleLink : EntityLink
    {
    }

    [DataContract]
    public class ServiceRenderingLink : EntityLink
    {
    }

    [DataContract]
    public class UserLink : EntityLink
    {
    }

    [DataContract]
    public abstract class EntityLink
    {
        [DataMember]
        public Guid Id { get; set; }

        [DataMember]
        public string Presentation { get; set; }

        public override string ToString()
        {
            return Presentation;
        }
    }
}