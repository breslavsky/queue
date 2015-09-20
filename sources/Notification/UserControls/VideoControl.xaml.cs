using Queue.Notification.ViewModels;
using System.ComponentModel;
using System.Windows.Controls;

namespace Queue.Notification.UserControls
{
    public partial class VideoControl : UserControl
    {
        public VideoControl()
        {
            InitializeComponent();

            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                DataContext = new VideoControlViewModel(this);
            }
        }
    }
}