using NHibernate.Mapping.Attributes;
using Queue.Model.Common;

namespace Queue.Model
{
    [JoinedSubclass(Table = "config_smtp", ExtendsType = typeof(Config), Lazy = false, DynamicUpdate = true)]
    [Key(Column = "ConfigId", ForeignKey = "SMTPConfigToConfigReference")]
    public class SMTPConfig : Config
    {
        public SMTPConfig()
        {
            Type = ConfigType.SMTP;
        }

        #region properties

        [Property]
        public virtual string Server { get; set; }

        [Property]
        public virtual bool EnableSsl { get; set; }

        [Property(Column = "_User")]
        public virtual string User { get; set; }

        [Property]
        public virtual string Password { get; set; }

        [Property(Column = "_From")]
        public virtual string From { get; set; }

        #endregion properties
    }
}