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

namespace Queue.UI.Silverlight
{
    public partial class WarningControl : UserControl
    {
        public WarningControl()
        {
            InitializeComponent();
        }

        public void Show(object message)
        {
            warningTextBlock.Text = message.ToString();
            Visibility = Visibility.Visible;
        }

        public void Hide()
        {
            Visibility = Visibility.Collapsed;
        }

        private void hideButton_Click(object sender, RoutedEventArgs e)
        {
            Hide();
        }
    }
}
