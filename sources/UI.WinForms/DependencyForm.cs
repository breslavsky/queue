using Junte.UI.WinForms;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Queue.UI.WinForms
{
    public class DependencyForm : RichForm
    {
        public DependencyForm()
            : base()
        {
            if (LicenseManager.UsageMode != LicenseUsageMode.Designtime)
            {
                ServiceLocator.Current.GetInstance<IUnityContainer>().BuildUp(this.GetType(), this);
            }
        }
    }
}