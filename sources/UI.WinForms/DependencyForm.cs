﻿using Junte.UI.WinForms;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using System.ComponentModel;

namespace Queue.UI.WinForms
{
    public class DependencyForm : RichForm
    {
        protected bool designtime;

        public DependencyForm()
            : base()
        {
            designtime = LicenseManager.UsageMode == LicenseUsageMode.Designtime;
            if (designtime)
            {
                return;
            }

            ServiceLocator.Current.GetInstance<UnityContainer>()
                .BuildUp(this.GetType(), this);
        }
    }
}