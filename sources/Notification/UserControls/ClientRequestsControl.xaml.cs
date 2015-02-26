using Queue.Notification.ViewModels;
using System.Windows.Controls;

namespace Queue.Notification.UserControls
{
    public partial class ClientRequestsControl : UserControl
    {
        public ClientRequestsControlVM Model { get; private set; }

        public ClientRequestsControl()
        {
            InitializeComponent();

            Model = new ClientRequestsControlVM();
            Model.SetClientRequestsGrid(clientRequestsGrid);

            DataContext = Model;
        }

        private void UserControl_Unloaded(object sender, System.Windows.RoutedEventArgs e)
        {
            if (Model != null)
            {
                Model.Dispose();
            }
        }
    }
}