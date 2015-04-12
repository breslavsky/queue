using Queue.Common;
using System.Configuration;
using System.Globalization;

namespace Queue.Administrator
{
    public class AdministratorSettings : ConfigurationSection
    {
        public const string SectionKey = "administrator";

        [ConfigurationProperty("couponPrinter")]
        public string CouponPrinter
        {
            get { return (string)this["couponPrinter"]; }
            set { this["couponPrinter"] = value; }
        }

        public override bool IsReadOnly()
        {
            return false;
        }
    }
}