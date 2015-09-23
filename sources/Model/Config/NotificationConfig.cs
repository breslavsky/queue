using NHibernate.Mapping.Attributes;
using NHibernate.Validator.Constraints;
using Queue.Model.Common;

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

        [Range(Min = 1, Max = 30, Message = "Длина списка запросов должна быть от 1 до 30")]
        [Property]
        public virtual int ClientRequestsLength { get; set; }

        #endregion properties
    }
}