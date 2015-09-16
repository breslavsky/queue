using Queue.Common;
using System.Configuration;
using System.Globalization;

namespace Queue.UI.WinForms
{
    public class LoginFormSettings : ConfigurationSection
    {
        public const string SectionKey = "loginForm";

        [ConfigurationProperty("isRemember")]
        public bool IsRemember
        {
            get { return (bool)this["isRemember"]; }
            set { this["isRemember"] = value; }
        }

        public override bool IsReadOnly()
        {
            return false;
        }

        public void Reset()
        {
            IsRemember = false;
        }
    }
}