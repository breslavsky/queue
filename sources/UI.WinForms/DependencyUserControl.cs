using Junte.UI.WinForms;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using System.ComponentModel;

namespace Queue.UI.WinForms
{
    public class DependencyUserControl : RichUserControl
    {
        public DependencyUserControl()
            : base()
        {
            if (LicenseManager.UsageMode != LicenseUsageMode.Designtime)
            {
                ServiceLocator.Current.GetInstance<IUnityContainer>().BuildUp(this.GetType(), this);
            }
        }
    }
}