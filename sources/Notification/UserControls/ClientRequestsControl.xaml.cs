using Microsoft.Practices.ServiceLocation;
using Queue.Notification.ViewModels;
using Queue.UI.WPF;
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
                Content = ServiceLocator.Current.GetInstance<ITemplateManager>().GetTemplate("client-requests.xaml");
                DataContext = new ClientRequestsControlViewModel();
            }
        }
    }
}