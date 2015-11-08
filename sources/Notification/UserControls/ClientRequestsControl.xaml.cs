using Microsoft.Practices.Unity;
using Queue.Notification.ViewModels;
using Queue.UI.WPF;
using System.ComponentModel;

namespace Queue.Notification.UserControls
{
    public partial class ClientRequestsControl : RichUserControl
    {
        [Dependency]
        public ITemplateManager TemplateManager { get; set; }

        public ClientRequestsControl()
        {
            InitializeComponent();

            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                Content = TemplateManager.GetTemplate("client-requests.xaml");
                DataContext = new ClientRequestsControlViewModel();
            }
        }
    }
}