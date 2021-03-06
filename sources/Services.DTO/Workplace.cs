﻿using Junte.Translation;
using Queue.Model.Common;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Queue.Services.DTO
{
    [DataContract]
    public class Workplace : IdentifiedEntity
    {
        [DataMember]
        public string Comment { get; set; }

        [DataMember]
        public byte DisplayDeviceId { get; set; }

        [DataMember]
        public byte QualityPanelDeviceId { get; set; }

        [DataMember]
        public WorkplaceModificator Modificator { get; set; }

        [DataMember]
        public int Number { get; set; }

        [DataMember]
        public WorkplaceType Type { get; set; }

        public override string ToString()
        {
            var chunks = new List<string>() { Translater.Enum(Type) };
            if (Number > 0)
            {
                chunks.Add(Number.ToString());

                var modificator = Translater.Enum(Modificator);
                if (!String.IsNullOrEmpty(modificator))
                {
                    chunks.Add(modificator);
                }
            }
            return String.Join(" ", chunks);
        }
    }
}