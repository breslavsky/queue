using Queue.Notification.ViewModels;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace Queue.Notification.UserControls
{
    public partial class TickerUserControl : UserControl
    {
        private TickerUserControlViewModel model;

        public TickerUserControl()
        {
            InitializeComponent();

            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                model = new TickerUserControlViewModel();
                DataContext = model;
            }
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