﻿using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using System.ComponentModel;
using System.Windows.Controls;

namespace Queue.UI.WPF
{
    public class RichPage : Page
    {
        public RichPage()
        {
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                ServiceLocator.Current.GetInstance<IUnityContainer>().BuildUp(GetType(), this);
            }
        }
    }
}