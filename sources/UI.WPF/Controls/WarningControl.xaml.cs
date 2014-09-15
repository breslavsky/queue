using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Effects;

namespace Queue.UI.WPF
{
    public partial class WarningControl : UserControl
    {
        Action closed;

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

            if (closed != null)
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
