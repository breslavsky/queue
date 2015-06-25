using Junte.Data.Common;
using NHibernate.Mapping.Attributes;
using NHibernate.Validator.Constraints;
using Queue.Model.Common;
using System;

namespace Queue.Model
{
    [JoinedSubclass(Table = "config_default", ExtendsType = typeof(Config), Lazy = false, DynamicUpdate = true)]
    [Key(Column = "ConfigId", ForeignKey = "DefaultConfigToConfigReference")]
    public class DefaultConfig : Config
    {
        public DefaultConfig()
        {
            Type = ConfigType.Default;
        }

        #region properties

        [Property(Length = 255)]
        public virtual string QueueName { get; set; }

        [Property]
        public virtual TimeSpan WorkStartTime { get; set; }

        [Property]
        public virtual TimeSpan WorkFinishTime { get; set; }

        [Min(Value = 1, Message = "Максимальное количество запросов не может быть менее 1")]
        [Property]
        public virtual int MaxClientRequests { get; set; }

        [Min(Value = 1, Message = "Максимальное время обслуживания не может быть менее 1")]
        [Property]
        public virtual int MaxRenderingTime { get; set; }

        [Property]
        public virtual bool IsDebug { get; set; }

        #endregion properties
    }
}