using Queue.Notification.ViewModels;
using System.ComponentModel;
using System.Windows.Controls;

namespace Queue.Notification.UserControls
{
    public partial class TickerControl : UserControl
    {
        public TickerControl()
        {
            InitializeComponent();

            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                DataContext = new TickerControlViewModel(this);
            }
        }
    }
}