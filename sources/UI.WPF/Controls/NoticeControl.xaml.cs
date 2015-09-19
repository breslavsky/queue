using Microsoft.Practices.ServiceLocation;
using Queue.UI.WPF.Types;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Queue.UI.WPF
{
    public partial class NoticeControl : UserControl
    {
        private readonly Action onClose;

        public NoticeControl(object message, Action onClose)
        {
            InitializeComponent();

            noticeTextBlock.Text = message.ToString();
            this.onClose = onClose;
        }

        public void Hide()
        {
            if (onClose != null)
            {
                onClose();
            }

            ServiceLocator.Current.GetInstance<IMainWindow>().HideMessageBox(this);
        }

        private void hideButton_Click(object sender, RoutedEventArgs e)
        {
            Hide();
        }
    }
}