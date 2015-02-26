using Queue.Notification.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace Queue.Notification.UserControls
{
    public partial class TickerUserControl : UserControl
    {
        private TickerUserControlVM model;

        public TickerUserControl()
        {
            model = new TickerUserControlVM();

            DataContext = model;

            InitializeComponent();
        }

        private void UserControl_Unloaded(object sender, System.Windows.RoutedEventArgs e)
        {
            model.Stop();
        }

        internal void Initialize(FrameworkElement container)
        {
            model.Initialize(container, TickerItem);
        }
    }
}