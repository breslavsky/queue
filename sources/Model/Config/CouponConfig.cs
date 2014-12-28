using Junte.Data.Common;
using NHibernate.Mapping.Attributes;
using Queue.Model.Common;

namespace Queue.Model
{
    [JoinedSubclass(Table = "config_coupon", ExtendsType = typeof(Config), Lazy = false, DynamicUpdate = true)]
    [Key(Column = "ConfigId", ForeignKey = "CouponConfigToConfigReference")]
    public class CouponConfig : Config
    {
        private const int TemplateLength = 1024 * 1024;

        public CouponConfig()
        {
            Type = ConfigType.Coupon;
        }

        #region properties

        [Property(Length = TemplateLength)]
        public virtual string Template { get; set; }

        #endregion properties
    }
}