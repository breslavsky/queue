﻿using Junte.Data.NHibernate;
using NHibernate.Mapping.Attributes;
using NHibernate.Validator.Constraints;
using Queue.Model.Common;
using System;
using System.Resources;

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
        [Length(Min = 0, Max = 1000, Message = "Номер рабочего места должен быть больше 1 и меньше 1000")]
        public virtual int Number { get; set; }

        [Property]
        public virtual WorkplaceModificator Modificator { get; set; }

        [Property(Length = 1000)]
        public virtual string Comment { get; set; }

        [Property]
        public virtual byte Display { get; set; }

        [Property]
        public virtual byte Segments { get; set; }

        #endregion properties

        public override string ToString()
        {
            return string.Format("{0} {1}{2}", Type.Translate(), Number, Modificator.Translate());
        }
    }
}