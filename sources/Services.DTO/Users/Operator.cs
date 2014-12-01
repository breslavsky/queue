﻿using System;
using System.Runtime.Serialization;

namespace Queue.Services.DTO
{
    public class Operator : User
    {
        [DataMember]
        public Workplace Workplace { get; set; }

        [DataMember]
        public virtual bool IsInterruption { get; set; }

        [DataMember]
        public TimeSpan InterruptionStartTime { get; set; }

        [DataMember]
        public TimeSpan InterruptionFinishTime { get; set; }
    }
}