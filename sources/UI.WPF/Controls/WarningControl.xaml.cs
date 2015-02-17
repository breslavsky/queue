using System;
using System.Windows;
using System.Windows.Controls;

namespace Queue.UI.WPF
{
    public partial class WarningControl : UserControl
    {
        private Action closed;

        public WarningControl()
        {
            InitializeComponent();
        }

        public WarningControl Show(string message, Action closed)
        {
            warningTextBlock.Text = message;
            this.closed = closed;
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

        public void Hide(bool noAction = false)
        {
            Visibility = Visibility.Hidden;

            foreach (FrameworkElement child in ((Panel)Parent).Children)
            {
                if (!child.Equals(this))
                {
                    child.UnBlur();
                }
            }

            if (!noAction && (closed != null))
            {
                closed();
            }
        }

        private void hideButton_Click(object sender, RoutedEventArgs e)
        {
            Hide();
        }
    }
}