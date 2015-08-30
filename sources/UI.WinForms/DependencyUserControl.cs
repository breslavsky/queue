using Junte.UI.WinForms;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using System.ComponentModel;

namespace Queue.UI.WinForms
{
    public class DependencyUserControl : RichUserControl
    {
        protected bool designtime;

        public DependencyUserControl()
            : base()
        {
            designtime = LicenseManager.UsageMode == LicenseUsageMode.Designtime;
            if (designtime)
            {
                return;
            }

            ServiceLocator.Current.GetInstance<IUnityContainer>().BuildUp(this.GetType(), this);
        }
    }
}