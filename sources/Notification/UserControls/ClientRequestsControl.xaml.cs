using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Queue.Notification.ViewModels;
using System.ComponentModel;
using System.Windows.Controls;

namespace Queue.Notification.UserControls
{
    public partial class ClientRequestsControl : UserControl
    {
        public ClientRequestsControl()
        {
            InitializeComponent();

            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                DataContext = ServiceLocator.Current.GetInstance<UnityContainer>().Resolve<ClientRequestsControlViewModel>();
            }
        }
    }
}