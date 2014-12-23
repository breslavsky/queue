using Junte.Data.Common;
using Junte.Data.NHibernate;
using NHibernate.Mapping.Attributes;
using NHibernate.Validator.Constraints;
using Queue.Model.Common;
using System;

namespace Queue.Model
{
    [Class(Table = "event", DynamicUpdate = true, Lazy = false)]
    [Cache(Usage = CacheUsage.ReadWrite)]
    public abstract class Event : IdentifiedEntity
    {
        public Event()
        {
            CreateDate = DateTime.Now;
        }

        #region properties

        [Property]
        public virtual DateTime CreateDate { get; set; }

        [Property]
        public virtual EventType Type { get; set; }

        [NotNullNotEmpty(Message = "Для события не указано сообщение")]
        [Property(Length = DataLength._1M)]
        public virtual string Message { get; set; }

        #endregion properties

        public override string ToString()
        {
            return Message;
        }
    }
}