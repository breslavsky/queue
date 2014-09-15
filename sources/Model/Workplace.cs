﻿using Junte.Data.NHibernate;
using NHibernate.Mapping.Attributes;
using NHibernate.Validator.Constraints;
using Queue.Model.Common;
using System;
using System.Resources;
using Translation = Queue.Model.Common.Translation;

namespace Queue.Model
{
    [Class(Table = "workplace", DynamicUpdate = true, Lazy = false)]
    [Cache(Usage = CacheUsage.ReadWrite)]
    public class Workplace : IdentifiedEntity
    {
        public Workplace()
        {
            Type = WorkplaceType.Window;
            Number = 1;
            Modificator = WorkplaceModificator.None;
            Display = 1;
            Segments = 3;
        }

        #region properties

        [Property]
        public virtual WorkplaceType Type { get; set; }

        [Property]
        [Min(Value = 0, Message = "Номер рабочего места должен быть больше 1")]
        [Max(Value = 10000, Message = "Номер рабочего места должен быть меньше 10000")]
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
            ResourceManager workplaceTypeTranslation = Translation.WorkplaceType.ResourceManager;
            ResourceManager workplaceModificatorTranslation = Translation.WorkplaceModificator.ResourceManager;

            return string.Format("{0} {1}{2}", workplaceTypeTranslation.GetString(Type.ToString()), Number, workplaceModificatorTranslation.GetString(Modificator.ToString()));
        }
    }
}