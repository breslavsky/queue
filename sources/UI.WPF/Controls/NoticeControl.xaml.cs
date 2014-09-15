using System;
using System.Windows;
using System.Windows.Controls;

namespace Queue.UI.WPF
{
    public partial class NoticeControl : UserControl
    {
        private Action onClose;

        public NoticeControl()
        {
            InitializeComponent();
        }

        public void Show(object message)
        {
            Show(message, null);
        }

        public NoticeControl Show(object message, Action onClose)
        {
            noticeTextBlock.Text = message.ToString();
            this.onClose = onClose;
            Visibility = Visibility.Visible;

            foreach (FrameworkElement child in ((Panel)Parent).Children)
            {
                if (!child.Equals(this))
                {
                    child.Blur();
                }
            }

            hideButton.Focus();

            return this;
        }

        public void Hide()
        {
            Visibility = Visibility.Hidden;

            foreach (FrameworkElement child in ((Panel)Parent).Children)
            {
                if (!child.Equals(this))
                {
                    child.UnBlur();
                }
            }

            if (onClose != null)
            {
                onClose();
            }
        }

        private void hideButton_Click(object sender, RoutedEventArgs e)
        {
            Hide();
        }
    }
}