using Queue.Common;
using System.Configuration;

namespace Queue.UI.WinForms
{
    public class LoginFormSettings : AbstractSettings
    {
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
    }
}