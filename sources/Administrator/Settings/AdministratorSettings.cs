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

        public override bool IsReadOnly()
        {
            return false;
        }
    }
}