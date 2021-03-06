﻿using Junte.Data.NHibernate;
using Junte.Translation;
using NHibernate.Mapping.Attributes;
using NHibernate.Validator.Constraints;
using Queue.Model.Common;
using System.Collections.Generic;

namespace Queue.Model
{
    [Class(Table = "workplace", DynamicUpdate = true, Lazy = false)]
    [Cache(Usage = CacheUsage.ReadWrite)]
    public class Workplace : IdentifiedEntity
    {
        #region properties

        [Property]
        public virtual WorkplaceType Type { get; set; }

        [Property]
        [Min(Value = 0, Message = "Номер рабочего места должен быть больше 0")]
        public virtual int Number { get; set; }

        [Property]
        public virtual WorkplaceModificator Modificator { get; set; }

        [Property(Length = 1000)]
        public virtual string Comment { get; set; }

        [Property]
        public virtual byte DisplayDeviceId { get; set; }

        [Property]
        public virtual byte QualityPanelDeviceId { get; set; }

        #endregion properties

        public override string ToString()
        {
            var chunks = new List<string>() { Translater.Enum(Type) };
            if (Number > 0)
            {
                chunks.Add(Number.ToString());

                var modificator = Translater.Enum(Modificator);
                if (!string.IsNullOrEmpty(modificator))
                {
                    chunks.Add(modificator);
                }
            }
            return string.Join(" ", chunks);
        }
    }
}