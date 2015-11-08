using Queue.Common;
using System.Configuration;

namespace Queue.Administrator.Settings
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

        [ConfigurationProperty("theme")]
        public string Theme
        {
            get { return (string)this["theme"]; }
            set { this["theme"] = value; }
        }

        public AdministratorSettings()
        {
            Theme = Templates.Themes.Default;
        }

        public override bool IsReadOnly()
        {
            return false;
        }
    }
}