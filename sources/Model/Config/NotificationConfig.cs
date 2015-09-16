using NHibernate.Mapping.Attributes;
using NHibernate.Validator.Constraints;
using Queue.Model.Common;
using System.Collections.Generic;

namespace Queue.Model
{
    [JoinedSubclass(Table = "config_notification", ExtendsType = typeof(Config), Lazy = false, DynamicUpdate = true)]
    [Key(Column = "ConfigId", ForeignKey = "NotificationConfigToConfigReference")]
    public class NotificationConfig : Config
    {
        public NotificationConfig()
        {
            Type = ConfigType.Notification;
        }

        #region properties

        [Range(Min = 5, Max = 30, Message = "Длина списка запросов должна быть от 5 до 30")]
        [Property]
        public virtual int ClientRequestsLength { get; set; }

        #endregion properties
    }
}